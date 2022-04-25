// <copyright file="NetFlowReader.cs" company="Universität Stuttgart">
// Copyright © 2020 SAPPAN Consortium. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using static Sappan.Netflow.OnWireExtensions;


namespace Sappan.Netflow.Netflow9 {

    /// <summary>
    /// Reader for NetFlow v9 streams.
    /// </summary>
    public sealed class NetflowReader : NetflowReaderBase {

        #region Class constructor
        /// <summary>
        /// Initialises the class.
        /// </summary>
        static NetflowReader() {
            var flags = BindingFlags.Public | BindingFlags.Static;
            var types = from f in typeof(FieldType).GetFields(flags)
                        where f.GetValue(null) is FieldType
                        select f;

            foreach (var t in types) {
                var atts = t.GetCustomAttributes<ClrTypeAttribute>();
                if (atts != null) {
                    ClrTypeMappings[(FieldType) t.GetValue(null)]
                        = atts.Select(a => a.Type).ToArray();
                }
            }
        }
        #endregion

        #region Public constructors
        /// <summary>
        /// Initialises a new instance.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="leaveOpen">Indicates that the stream should not be
        /// closed if the reader is disposed.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="stream"/>
        /// is <c>null</c>.</exception>
        public NetflowReader(Stream stream, bool leaveOpen = false)
            : base(stream, leaveOpen) { }

        /// <summary>
        /// Initialises a new instance.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="sourceID">The source ID the templates to follow
        /// belong to.</param>
        /// <param name="templates">Optionally specifies known templates the
        /// reader can use to decode data records.</param>
        /// <param name="leaveOpen">Indicates that the stream should not be
        /// closed if the reader is disposed.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="stream"/>
        /// is <c>null</c>.</exception>
        public NetflowReader(Stream stream, uint sourceID,
                IEnumerable<TemplateRecord> templates, bool leaveOpen = false)
            : this(new BinaryReader(stream, Encoding.UTF8, leaveOpen), sourceID,
                templates) { }

        /// <summary>
        /// Initialises a new instance.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="templates">The known templates per source ID the
        /// reader can use to decode data records.</param>
        /// <param name="leaveOpen">Indicates that the stream should not be
        /// closed if the reader is disposed.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="stream"/>
        /// is <c>null</c>.</exception>
        public NetflowReader(Stream stream,
                IDictionary<uint, IEnumerable<TemplateRecord>> templates,
                bool leaveOpen = false)
            : this(new BinaryReader(stream, Encoding.UTF8, leaveOpen),
                templates) { }

        /// <summary>
        /// Initialises a new instance.
        /// </summary>
        /// <param name="reader">The <see cref="BinaryReader"/> providing the
        /// data.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="reader"/>
        /// is <c>null</c>.</exception>
        public NetflowReader(BinaryReader reader) : base(reader) { }

        /// <summary>
        /// Initialises a new instance.
        /// </summary>
        /// <param name="reader">The <see cref="BinaryReader"/> providing the
        /// data.</param>
        /// <param name="sourceID">The source ID the templates to follow
        /// belong to.</param>
        /// <param name="templates">Optionally specifies known templates the
        /// reader can use to decode data records.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="reader"/>
        /// is <c>null</c>.</exception>
        public NetflowReader(BinaryReader reader, uint sourceID,
                IEnumerable<TemplateRecord> templates) : this(reader) {
            if (templates != null) {
                this._templates[sourceID] = new TemplateSet();
                var ts = this._templates[sourceID].DataTemplates;
                foreach (var t in templates) {
                    ts[t.ID] = t;
                }
            }
        }

        /// <summary>
        /// Initialises a new instance.
        /// </summary>
        /// <param name="reader">The <see cref="BinaryReader"/> providing the
        /// data.</param>
        /// <param name="templates">The known templates per source ID the
        /// reader can use to decode data records.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="reader"/>
        /// is <c>null</c>.</exception>
        public NetflowReader(BinaryReader reader,
                IDictionary<uint, IEnumerable<TemplateRecord>> templates)
                : this(reader) {
            if (templates != null) {
                foreach (var t in templates) {
                    var ts = this.GetTemplates(t.Key);
                    foreach (var u in t.Value) {
                        ts.DataTemplates[u.ID] = u;
                    }
                }
            }
        }
        #endregion

        #region Public properties
        /// <summary>
        /// Gets the templates that are valid for the current observation
        /// domain.
        /// </summary>
        /// <remarks>
        /// This member is only valid if the reader is currently processing a
        /// packet. If no packet header, which defined the observation domain,
        /// has been read, the property will be <c>null</c>.
        /// </remarks>
        public IDictionary<ushort, TemplateRecord> CurrentDataTemplates {
            get {
                return (this._packetHeader != null)
                    ? this.GetDataTemplates(this._packetHeader.SourceID)
                    : null;
            }
        }

        /// <summary>
        /// Gets the option templates that are valid for the current observation
        /// domain.
        /// </summary>
        /// <remarks>
        /// This member is only valid if the reader is currently processing a
        /// packet. If no packet header, which defined the observation domain,
        /// has been read, the property will be <c>null</c>.
        /// </remarks>
        public IDictionary<ushort, OptionsTemplateRecord> CurrentOptionTemplates {
            get {
                return (this._packetHeader != null)
                    ? this.GetOptionTemplates(this._packetHeader.SourceID)
                    : null;
            }
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Gets the templates the reader uses for the observation domain with
        /// the specified ID.
        /// </summary>
        /// <param name="sourceID">The ID of the observation domain.</param>
        /// <returns>The templates for the specified observation domain.
        /// </returns>
        public IDictionary<ushort, TemplateRecord> GetDataTemplates(
                uint sourceID) {
            return this.GetTemplates(sourceID).DataTemplates;
        }

        /// <summary>
        /// Gets the option templates the reader uses for the observation domain
        /// with the specified ID.
        /// </summary>
        /// <param name="sourceID">The ID of the observation domain.</param>
        /// <returns>The templates for the specified observation domain.
        /// </returns>
        public IDictionary<ushort, OptionsTemplateRecord> GetOptionTemplates(
                uint sourceID) {
            return this.GetTemplates(sourceID).OptionTemplates;
        }

        /// <summary>
        /// Reads a packet header from the current location in the underlying
        /// stream.
        /// </summary>
        /// <returns>The packet header.</returns>
        /// <exception cref="InvalidOperationException">If the packet header has
        /// already been read.</exception>
        /// <exception cref="ObjectDisposedException">If the reader has already
        /// been disposed.</exception>
        public PacketHeader ReadPacketHeader() {
            this.CheckState(State.PacketHeader);
            Debug.Assert(this.Reader != null);

            var retval = new PacketHeader();

            var data = this.Reader.ReadBytes(retval.GetOnWireSize());
            var offset = 0;
            var length = data.Length;
            ReadMembers(ref retval, data, ref offset, ref length);
            this.BeginFlows(retval.Count);
            this._packetHeader = retval;

            return retval;
        }

        /// <summary>
        /// Reads a template flow set or data flow set from the current location
        /// in the underlying stream.
        /// </summary>
        /// <remarks>
        /// <para>This method is implemented in such a way that the flow set is
        /// read in one piece, ie if the data are corrupted, but the flow set ID
        /// and the length are still correct, only one flow set is lost (if an
        /// exception is thrown), but the reader can continue processing the
        /// next flow set.</para>
        /// </remarks>
        /// <returns>The flow set that has been read.</returns>
        /// <exception cref="InvalidOperationException">If the reader is not in
        /// a stated when it expectes a flow set. This is typically the case if
        /// the packet header has not yet been read.</exception>
        /// <exception cref="ObjectDisposedException">If the reader has already
        /// been disposed.</exception>
        /// <exception cref="KeyNotFoundException">If a data flow set was read
        /// which for we do not know a template.</exception>
        /// <exception cref="FormatException">If the input data are corrupt and
        /// cannot be processed. The flow set will have been skipped in thi case
        /// and the next one might be readable if it is OK.</exception>
        public IFlowSet ReadFlowSet() {
            this.CheckState(State.Flows);
            Debug.Assert(this.Reader != null);

            var id = this.Reader.ReadUInt16().ToHostByteOrder();
            var length = (int) this.Reader.ReadUInt16().ToHostByteOrder();
            length -= 2 * sizeof(ushort);
            var data = this.Reader.ReadBytes(length);

            // For the underlying stream, the flow has been consumed at this
            // point, regardless of whether any problem occurs when parsing it
            // later on. Therefore, we check whether the end of the packet has
            // been reached right after reading the contents of the packet.
            this.CheckEndOfPacket();

            if (id == 0) {
                // This is a template flow set.
                var retval = this.ReadTemplateFlowSet(data);
                Debug.Assert(retval.ID == id);
                return retval;

            } else if (id == 1) {
                // This is a option template flow set
                var retval = this.ReadOptionsTemplateFlowSet(data);
                Debug.Assert(retval.ID == id);
                return retval;

            } else if (id >= 256) {
                // This is a (option) data flow set.
                try {
                    var template = this.CurrentDataTemplates[id];
                    var retval = ReadDataFlowSet(data, template);
                    Debug.Assert(retval.ID == id);
                    return retval;

                } catch (KeyNotFoundException) {
                    var template = this.CurrentOptionTemplates[id];
                    var retval = ReadOptionDataFlowSet(data, template);
                    Debug.Assert(retval.ID == id);
                    return retval;

                }

            } else {
                // This is something we do not understand.
                Debug.WriteLine($"Skipping {length} byte(s) of data the reader "
                    + "does not understand.");
                return null;
            }
        }
        #endregion

        #region Nested class TemplateSet
        /// <summary>
        /// Container for per observation domain templates.
        /// </summary>
        private class TemplateSet {
            public readonly IDictionary<ushort, TemplateRecord> DataTemplates
                = new Dictionary<ushort, TemplateRecord>();
            public readonly IDictionary<ushort, OptionsTemplateRecord> OptionTemplates
                = new Dictionary<ushort, OptionsTemplateRecord>();
        }
        #endregion

        #region Private class methods
        /// <summary>
        /// Gets the CLR type to represent the field.
        /// </summary>
        /// <param name="fieldType">The field to determine the CLR object for.</param>
        /// <returns>An type used to represent <paramref name="field"/> or
        /// <c>null</c> if no type could be inferred from the input.</returns>
        private static Type GetClrType(Field field) {
            Debug.Assert(field != null);
            if (ClrTypeMappings.TryGetValue(field.Type, out var types)) {
                if (types.Length == 1) {
                    // This was explicitly annotated as the only solution.
                    return types[0];
                }

                foreach (var t in types) {
                    if (Marshal.SizeOf(t) == field.Length) {
                        return t;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Reads <paramref name="data"/> as a <see cref="DataFlowSet" /> with a
        /// structured described by <paramref name="template"/>.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        private static DataFlowSet ReadDataFlowSet(byte[] data,
                TemplateRecord template) {
            Debug.Assert(data != null);
            Debug.Assert(template != null);

            var offset = 0;
            var length = data.Length;
            var retval = new DataFlowSet(template.ID);

            while (length > 0) {
                ReadFields(retval.Records, data, ref offset, ref length,
                    template.Fields);
            }

            return retval;
        }

        /// <summary>
        /// Reads the fields described by <paramref name="fields"/> from
        /// <paramref name="data"/> and appends them to
        /// <paramref name="target"/>.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <param name="fields"></param>
        private static void ReadFields(IList<object> target, byte[] data,
                ref int offset, ref int length, IEnumerable<Field> fields) {
            Debug.Assert(target != null);
            Debug.Assert(data != null);
            Debug.Assert(fields != null);

            foreach (var f in fields) {
                var l = (int) f.Length;
                var t = GetClrType(f);

                if (t == typeof(IPAddress)) {
                    // IP addresses require a special conversion.
                    var v = new IPAddress(IPAddress.Any.GetAddressBytes());
                    FromWire(ref v, data, ref offset, ref l);
                    target.Add(v);

                } else if ((t != null) && (t.GetOnWireSize() == l)) {
                    // This should be an automatically mappable type.
                    var v = t.FromWire(data, ref offset, ref l);
                    target.Add(v);

                } else {
                    // Cannot be handled automatically, just copy the
                    // bytes.
                    var v = data.Extract(offset, f.Length);
                    target.Add(v);
                    offset += f.Length;
                }

                length -= f.Length;
            }
        }

        /// <summary>
        /// Reads <paramref name="data"/> as a <see cref="OptionsDataFlowSet" />
        /// with a structured described by <paramref name="template"/>.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        private static OptionsDataFlowSet ReadOptionDataFlowSet(byte[] data,
                OptionsTemplateRecord template) {
            Debug.Assert(data != null);
            Debug.Assert(template != null);

            var offset = 0;
            var length = data.Length;
            var cnt = length / template.ActualRecordSize;

            var retval = new OptionsDataFlowSet(template.ID);

            for (var i = 0; i < cnt; ++i) {
                foreach (var f in template.Scopes) {
                    var v = data.Extract(offset, f.Length);
                    retval.Scopes.Add(v);
                    offset += f.Length;
                    length -= f.Length;
                }

                ReadFields(retval.Options, data, ref offset, ref length,
                        template.Options);
            }

            return retval;
        }
        #endregion

        #region Private class fields
        /// <summary>
        /// Stores the mapping from <see cref="FieldType"/>s to CLR types.
        /// </summary>
        private static readonly Dictionary<FieldType, Type[]> ClrTypeMappings
            = new Dictionary<FieldType, Type[]>();
        #endregion

        #region Private methods
        /// <summary>
        /// Gets the templates for the given source ID or creates a new template
        /// set if none was found.
        /// </summary>
        /// <param name="sourceID">The source to retrieve the template for.
        /// </param>
        /// <returns>The template set for the specified source.</returns>
        private TemplateSet GetTemplates(uint sourceID) {
            TemplateSet retval;

            if (!this._templates.TryGetValue(sourceID, out retval)) {
                this._templates[sourceID] = retval = new TemplateSet();
            }

            return retval;
        }

        /// <summary>
        /// Processes <paramref name="data"/> as
        /// <see cref="OptionsTemplateFlowSet"/>.
        /// </summary>
        /// <param name="data">The raw data of the flow set.</param>
        /// <returns>The decoded flow set.</returns>
        private OptionsTemplateFlowSet ReadOptionsTemplateFlowSet(byte[] data) {
            Debug.Assert(data != null);
            var offset = 0;
            var length = data.Length;
            var optsLen = (ushort) 0;
            var scopeLen = (ushort) 0;
            var tid = (ushort) 0;

            var retval = new OptionsTemplateFlowSet();

            do {
                // Template ID is first.
                FromWire(ref tid, data, ref offset, ref length);
                Debug.Assert(tid >= 256);

                // Length of option scopes in bytes.
                FromWire(ref scopeLen, data, ref offset, ref length);

                // Length of option values in bytes.
                FromWire(ref optsLen, data, ref offset, ref length);

                // Compute how many option scopes to expect.
                {
                    var single = 4;
                    if (scopeLen % single != 0) {
                        var msg = Properties.Resources.ErrorMalformedScopes;
                        msg = string.Format(CultureInfo.CurrentCulture,
                            msg, scopeLen, single);
                        throw new FormatException(msg);
                    }
                    scopeLen /= (ushort) single;
                }

                // Compute how many options to expect.
                {
                    var single = 4;
                    if (optsLen % single != 0) {
                        var msg = Properties.Resources.ErrorMalformedScopes;
                        msg = string.Format(CultureInfo.CurrentCulture,
                            msg, optsLen, single);
                        throw new FormatException(msg);
                    }
                    optsLen /= (ushort) single;
                }

                var record = new OptionsTemplateRecord(tid);

                // Read the scopes.
                for (ushort i = 0; i < scopeLen; ++i) {
                    var s = new Scope();
                    ReadMembers(ref s, data, ref offset, ref length);
                    record.Scopes.Add(s);
                }

                // Read the options.
                for (ushort i = 0; i < optsLen; ++i) {
                    var f = new Field();
                    ReadMembers(ref f, data, ref offset, ref length);
                    record.Options.Add(f);
                }

                // Remember the template to decode future data flow sets.
                {
                    var ts = this.CurrentOptionTemplates;
                    Debug.Assert(ts != null);
                    ts[tid] = record;
                }

                // Add the record to the flow set.
                retval.Records.Add(record);
            } while (length > 3 * sizeof(uint));

            return retval;
        }

        /// <summary>
        /// Processes <paramref name="data"/> as <see cref="TemplateFlowSet"/>.
        /// </summary>
        /// <param name="data">The raw data of the flow set.</param>
        /// <returns>The decoded flow set.</returns>
        private TemplateFlowSet ReadTemplateFlowSet(byte[] data) {
            Debug.Assert(data != null);
            var offset = 0;
            var length = data.Length;
            var retval = new TemplateFlowSet();

            while (length > 0) {
                var cntFields = (ushort) 0;
                var tid = (ushort) 0;

                // Read the template ID.
                FromWire(ref tid, data, ref offset, ref length);
                Debug.Assert(offset == 2);

                // Read the number of fields to follow.
                FromWire(ref cntFields, data, ref offset, ref length);
                Debug.Assert(offset == 4);

                // Create the record once we know the ID.
                var record = new TemplateRecord(tid);

                // Read the fields.
                for (ushort i = 0; i < cntFields; ++i) {
                    var field = new Field();
                    ReadMembers(ref field, data, ref offset, ref length);
                    record.Fields.Add(field);
                }
                Debug.Assert(record.FieldCount == cntFields);

                // Remember the template to decode future data flow sets.
                {
                    var ts = this.CurrentDataTemplates;
                    Debug.Assert(ts != null);
                    ts[tid] = record;
                }

                // Add the record to the flow set.
                retval.Records.Add(record);
            }

            return retval;
        }
        #endregion

        #region Private fields
        /// <summary>
        /// The header of the currently processed packed, which is required to
        /// determine the observation domain the templates belong to.
        /// </summary>
        private PacketHeader _packetHeader;

        /// <summary>
        /// Tracks the (data and option) templates we have already read.
        /// </summary>
        private readonly IDictionary<uint, TemplateSet> _templates
            = new Dictionary<uint, TemplateSet>();
        #endregion
    }
}
