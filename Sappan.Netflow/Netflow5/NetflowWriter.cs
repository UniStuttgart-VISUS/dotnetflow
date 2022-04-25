// <copyright file="NetFlowWriter.cs" company="Universität Stuttgart">
// Copyright © 2020 SAPPAN Consortium. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.IO;


namespace Sappan.Netflow.Netflow5 {

    /// <summary>
    /// Writes NetFlow v5 data into a stream.
    /// </summary>
    public sealed class NetflowWriter : NetflowWriterBase {

        #region Public constructors
        /// <summary>
        /// Initialises a new instance.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="leaveOpen">Indicates whether the writer should
        /// keep the stream open if it is disposed.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="stream"/>
        /// is <c>null</c>.</exception>
        public NetflowWriter(Stream stream, bool leaveOpen = false)
            : base(stream, leaveOpen) { }

        /// <summary>
        /// Initialises a new instance.
        /// </summary>
        /// <param name="writer">The <see cref="BinaryWriter"/> used to write to
        /// the target stream.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="writer"/>
        /// is <c>null</c>.</exception>
        public NetflowWriter(BinaryWriter writer) : base(writer) { }
        #endregion

        #region Public methods
        /// <summary>
        /// Writes a <see cref="FlowRecord"/> into the stream.
        /// </summary>
        /// <param name="record">The flow record to be written to the underlying
        /// stream.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="record"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">If the packet header was
        /// not yet written.</exception>
        /// <exception cref="ObjectDisposedException">If the underlying writer
        /// has already been disposed.</exception>
        public void Write(FlowRecord record) {
            if (record == null) {
                throw new ArgumentNullException(nameof(record));
            }
            this.CheckState(State.Flows);
            var data = record.ToWire();
            var written = this.Write(data);
            this.WritePadding(record, written);
            this.CheckEndOfPacket();
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
            this.Write(packetHeader.ToWire());
            this.BeginFlows(packetHeader.Count);
        }
        #endregion
    }
}
