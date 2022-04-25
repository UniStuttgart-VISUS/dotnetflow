// <copyright file="IpfixWriter.cs" company="Universität Stuttgart">
// Copyright © 2020 SAPPAN Consortium. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.Diagnostics;
using System.IO;


namespace Sappan.Netflow.Ipfix {


    /// <summary>
    /// Writes IFPIX data into a stream.
    /// </summary>
    public sealed class IpfixWriter : NetflowWriterBase {

        #region Public constructors
        /// <summary>
        /// Initialises a new instance.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="leaveOpen">Indicates whether the writer should
        /// keep the stream open if it is disposed.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="stream"/>
        /// is <c>null</c>.</exception>
        public IpfixWriter(Stream stream, bool leaveOpen = false)
            : base(stream, leaveOpen) { }

        /// <summary>
        /// Initialises a new instance.
        /// </summary>
        /// <param name="writer">The <see cref="BinaryWriter"/> used to write to
        /// the target stream.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="writer"/>
        /// is <c>null</c>.</exception>
        public IpfixWriter(BinaryWriter writer) : base(writer) { }
        #endregion

        #region Public methods
        /// <summary>
        /// Writes a <see cref="DataSet"/> into the stream.
        /// </summary>
        /// <remarks>
        /// <para>The writer assumes that the set passed here is correct and
        /// matches the indicated template. It does not perform any checks;
        /// most importantly, it does not pad any variably sized elements like
        /// strings. Use
        /// <see cref="StringExtensions.AdjustToNetFlow(string, Field, byte)" />
        /// to adjust strings to the expected size of a field.</para>
        /// </remarks>
        /// <param name="set">The flow set to be written to the underlying
        /// stream.</param>
        /// <exception cref="ArgumentNullException">If
        /// <paramref name="set"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">If the writer is not in
        /// a state where it expects a packet header.</exception>
        /// <exception cref="ObjectDisposedException">If the writer has already
        /// been disposed.</exception>
        public void Write(DataSet set) {
            if (set == null) {
                throw new ArgumentNullException(nameof(set));
            }
            this.CheckState(State.Flows);
            var data = set.ToWire();
            var written = this.Write(data);
            written += this.WritePadding(set, written);
            Debug.Assert(written <= ushort.MaxValue);
            this.CheckEndOfPacket((ushort) written);
        }

        /// <summary>
        /// Writes a <see cref="OptionsTemplateSet"/> into the stream.
        /// </summary>
        /// <param name="set">The flow set to be written to the underlying
        /// stream.</param>
        /// <exception cref="ArgumentNullException">If
        /// <paramref name="set"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">If the writer is not in
        /// a state where it expects a packet header.</exception>
        /// <exception cref="ObjectDisposedException">If the writer has already
        /// been disposed.</exception>
        public void Write(OptionsTemplateSet set) {
            if (set == null) {
                throw new ArgumentNullException(nameof(set));
            }
            this.CheckState(State.Flows);
            var data = set.ToWire();
            var written = this.Write(data);
            written += this.WritePadding(set, written);
            Debug.Assert(written <= ushort.MaxValue);
            this.CheckEndOfPacket((ushort) written);
        }

        /// <summary>
        /// Writes a <see cref="PacketHeader"/> into the stream.
        /// </summary>
        /// <param name="packetHeader">The packet header to be written.</param>
        /// <exception cref="ArgumentNullException">If
        /// <paramref name="packetHeader"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">If the writer is not in
        /// a state where it expects a packet header.</exception>
        /// <exception cref="ObjectDisposedException">If the writer has already
        /// been disposed.</exception>
        public void Write(PacketHeader packetHeader) {
            if (packetHeader == null) {
                throw new ArgumentNullException(nameof(packetHeader));
            }
            this.CheckState(State.PacketHeader);
            var written = this.Write(packetHeader.ToWire());
            this.BeginFlows((ushort) (packetHeader.Length - written));
        }

        /// <summary>
        /// Writes a <see cref="TemplateFlowSet"/> into the stream.
        /// </summary>
        /// <param name="set">The flow set to be written to the underlying
        /// stream.</param>
        /// <exception cref="ArgumentNullException">If
        /// <paramref name="set"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">If the writer is not in
        /// a state where it expects a packet header.</exception>
        /// <exception cref="ObjectDisposedException">If the writer has already
        /// been disposed.</exception>
        public void Write(TemplateSet set) {
            if (set == null) {
                throw new ArgumentNullException(nameof(set));
            }
            this.CheckState(State.Flows);
            var data = set.ToWire();
            var written = this.Write(data);
            written += this.WritePadding(set, written);
            Debug.Assert(written <= ushort.MaxValue);
            this.CheckEndOfPacket((ushort) written);
        }
        #endregion
    }
}
