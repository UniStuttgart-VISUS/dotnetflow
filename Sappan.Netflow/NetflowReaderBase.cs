// <copyright file="NetFlowReaderBase.cs" company="Universität Stuttgart">
// Copyright © 2020 SAPPAN Consortium. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;


namespace Sappan.Netflow {

    /// <summary>
    /// Base class for Netflow readers.
    /// </summary>
    public abstract class NetflowReaderBase : IDisposable {

        #region Finaliser
        /// <summary>
        /// Finalises the instance.
        /// </summary>
        ~NetflowReaderBase() {
            this.Dispose(false);
        }
        #endregion

        #region Public properties
        /// <summary>
        /// Gets the underlying stream of the reader.
        /// </summary>
        public Stream BaseStream => this.Reader?.BaseStream;
        #endregion

        #region Public methods
        /// <inheritdoc />
        public void Dispose() {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Internal constructors
        /// <summary>
        /// Initialises a new instance.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="leaveOpen">Indicates that the stream should not be
        /// closed if the reader is disposed.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="stream"/>
        /// is <c>null</c>.</exception>
        internal NetflowReaderBase(Stream stream, bool leaveOpen = false)
            : this(new BinaryReader(stream, Encoding.UTF8, leaveOpen)) { }

        /// <summary>
        /// Initialsies a new instance.
        /// </summary>
        /// <param name="reader">The <see cref="BinaryReader"/> providing the
        /// data.</param>
        /// <param name="templates">Optionally specifies known templates the
        /// reader can use to decode data records.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="reader"/>
        /// is <c>null</c>.</exception>
        internal NetflowReaderBase(BinaryReader reader) {
            this._cntFlows = 0;
            this.Reader = reader
                ?? throw new ArgumentNullException(nameof(reader));
            this._state = State.PacketHeader;
        }
        #endregion

        #region Nested enum State
        /// <summary>
        /// Possible states of the reader.
        /// </summary>
        protected enum State {
            /// <summary>
            /// The reader expects the packet header next.
            /// </summary>
            PacketHeader,

            /// <summary>
            /// The reader expects a flow set or record next.
            /// </summary>
            Flows
        }
        #endregion

        #region Protected class methods
        /// <summary>
        /// Reads the members (starting with ordinal <paramref name="from"/> and
        /// ending at ordinal <paramref name="to"/>) from
        /// <paramref name="data"/>.
        /// </summary>
        /// <typeparam name="TObject">The type of the object to read the
        /// properties of.</typeparam>
        /// <param name="target">The target object to assign the properties to.
        /// </param>
        /// <param name="data">The raw data to read the properties from.</param>
        /// <param name="offset">The offset in bytes where to start reading.
        /// </param>
        /// <param name="length">The nunmber of bytes that can be consumed for
        /// the members.</param>
        /// <param name="from">The ordinal of the first member that should be
        /// read.</param>
        /// <param name="to">The ordinal of the last member that should be read.
        /// </param>
        protected static void ReadMembers<TObject>(ref TObject target,
                byte[] data, ref int offset, ref int length,
                int from = int.MinValue, int to = int.MaxValue) {
            Contract.Assert(target != null);
            Contract.Assert(data != null);
            // Note: structs must be boxed by us when calling SetValue.
            // Cf. https://stackoverflow.com/questions/6280506/is-there-a-way-to-set-properties-on-struct-instances-using-reflection
            var boxed = (object) target;

            if (target is IHandleOnWire handler) {
                var read = handler.FromWire(data, offset);
                offset += read;
                length -= offset;

            } else {
                var members = target.GetType().GetOnWireMembers(from, to);
                if (!members.Any()) {
                    throw new ArgumentException(
                        Properties.Resources.ErrorNoOnWireMembers,
                        nameof(target));
                }

                foreach (var m in members) {
                    if (m is FieldInfo f) {
                        var v = f.FieldType.FromWire(data, ref offset,
                            ref length);
                        f.SetValue(boxed, v);

                    } else if (m is PropertyInfo p) {
                        var v = p.PropertyType.FromWire(data, ref offset,
                            ref length);
                        if (p.CanWrite) {   // Skip read-only stuff like 'Length'.
                            p.SetValue(boxed, v);
                        }

                    } else {
                        throw new InvalidOperationException(
                            Properties.Resources.ErrorOnWireMemberType);
                    }
                }
            }

            target = (TObject) boxed;  // Write back boxed struct to output.
        }
        #endregion

        #region Protected properties
        /// <summary>
        /// Gets the reader obtaining the data from the underlying stream.
        /// </summary>
        protected BinaryReader Reader { get; private set; }
        #endregion

        #region Protected methods
        /// <summary>
        /// Begins a new packet that contains the specified number of flow sets
        /// or flow records.
        /// </summary>
        /// <param name="flows">The expected number of flows as read from the
        /// packet header.</param>
        protected void BeginFlows(ushort flows) {
            Debug.Assert(this._cntFlows == 0);
            Debug.Assert(this._state == State.PacketHeader);
            this._cntFlows = flows;
            this._state = State.Flows;
        }

        /// <summary>
        /// Decrements the counter <see cref="_cntFlows"/> and checks whether it
        /// reached zero. If so, the state is reset to the begin of a new packet.
        /// </summary>
        /// <remarks>
        /// The <paramref name="read"/> parameter is for IPFIX, which uses bytes
        /// instead of number of flows in its header.
        /// </remarks>
        /// <param name="read">The number of flows that have been read. This
        /// parameter defaults to 1.</param>
        protected void CheckEndOfPacket(ushort read = 1) {
            Debug.Assert(this._cntFlows >= read);
            this._cntFlows -= read;
            if (this._cntFlows == 0) {
                this._state = State.PacketHeader;
            }
        }

        /// <summary>
        /// Checks whether the reader is not disposed and in the given state or
        /// throws an exception.
        /// </summary>
        /// <param name="expected">The expected state of the reader.</param>
        /// <exception cref="InvalidOperationException">If the current state of
        /// the reader is not <paramref name="expected"/>.</exception>
        /// <exception cref="ObjectDisposedException">If the reader has already
        /// been disposed.</exception>
        protected void CheckState(State expected) {
            if (this._state != expected) {
                throw new InvalidOperationException(
                    Properties.Resources.ErrorReaderState);
            }
            if (this.Reader == null) {
                throw new ObjectDisposedException(nameof(this.Reader));
            }
        }

        /// <summary>
        /// Implements the actual disposal of the object.
        /// </summary>
        /// <param name="disposing">Indicates whether the method was called
        /// from <see cref="Dispose"/> or from the finaliser.</param>
        protected virtual void Dispose(bool disposing) {
            try {
                if (disposing) {
                    this.Reader.Dispose();
                }
            } finally {
                this.Reader = null;
            }
        }
        #endregion

        #region Private fields
        /// <summary>
        /// The number of remaining flows.
        /// </summary>
        /// <remarks>
        /// This member will be initialised if the packet header is read and is
        /// decremented for each flow set that is retrieved.
        /// </remarks>
        private int _cntFlows;

        /// <summary>
        /// Tracks the state of the reader.
        /// </summary>
        private State _state;
        #endregion
    }
}
