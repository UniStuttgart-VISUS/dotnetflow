// <copyright file="NetFlowWriter.cs" company="Universität Stuttgart">
// Copyright © 2020 - 2022 Institut für Visualisierung und Interaktive Systeme. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.IO;


namespace DotNetFlow.Netflow9 {

    /// <summary>
    /// Writes NetFlow v9 data into a stream.
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
        /// Writes a <see cref="DataFlowSet"/> into the stream.
        /// </summary>
        /// <remarks>
        /// <para>The writer assumes that the flow set passed here is correct
        /// and matches the indicated template. It does not perform any checks;
        /// most importantly, it does not pad any variably sized elements like
        /// strings. Use
        /// <see cref="StringExtensions.AdjustToNetFlow(string, Field, byte)" />
        /// to adjust strings to the expected size of a field.</para>
        /// </remarks>
        /// <param name="flowSet">The flow set to be written to the underlying
        /// stream.</param>
        /// <exception cref="ArgumentNullException">If
        /// <paramref name="flowSet"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">If the writer is not in
        /// a state where it expects a packet header.</exception>
        /// <exception cref="ObjectDisposedException">If the writer has already
        /// been disposed.</exception>
        public void Write(DataFlowSet flowSet) {
            if (flowSet == null) {
                throw new ArgumentNullException(nameof(flowSet));
            }
            this.CheckState(State.Flows);
            var data = flowSet.ToWire();
            var written = this.Write(data);
            this.WritePadding(flowSet, written);
            this.CheckEndOfPacket();
        }

        /// <summary>
        /// Writes a <see cref="OptionsDataFlowSet"/> into the stream.
        /// </summary>
        /// <param name="flowSet">The flow set to be written to the underlying
        /// stream.</param>
        /// <exception cref="ArgumentNullException">If
        /// <paramref name="flowSet"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">If the writer is not in
        /// a state where it expects a packet header.</exception>
        /// <exception cref="ObjectDisposedException">If the writer has already
        /// been disposed.</exception>
        public void Write(OptionsDataFlowSet flowSet) {
            if (flowSet == null) {
                throw new ArgumentNullException(nameof(flowSet));
            }
            this.CheckState(State.Flows);
            var data = flowSet.ToWire();
            var written = this.Write(data);
            this.WritePadding(flowSet, written);
            this.CheckEndOfPacket();
        }

        /// <summary>
        /// Writes a <see cref="OptionsTemplateFlowSet"/> into the stream.
        /// </summary>
        /// <param name="flowSet">The flow set to be written to the underlying
        /// stream.</param>
        /// <exception cref="ArgumentNullException">If
        /// <paramref name="flowSet"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">If the writer is not in
        /// a state where it expects a packet header.</exception>
        /// <exception cref="ObjectDisposedException">If the writer has already
        /// been disposed.</exception>
        public void Write(OptionsTemplateFlowSet flowSet) {
            if (flowSet == null) {
                throw new ArgumentNullException(nameof(flowSet));
            }
            this.CheckState(State.Flows);
            var data = flowSet.ToWire();
            var written = this.Write(data);
            this.WritePadding(flowSet, written);
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

        /// <summary>
        /// Writes a <see cref="TemplateFlowSet"/> into the stream.
        /// </summary>
        /// <param name="flowSet">The flow set to be written to the underlying
        /// stream.</param>
        /// <exception cref="ArgumentNullException">If
        /// <paramref name="flowSet"/> is <c>null</c>.</exception>
        /// <exception cref="InvalidOperationException">If the writer is not in
        /// a state where it expects a packet header.</exception>
        /// <exception cref="ObjectDisposedException">If the writer has already
        /// been disposed.</exception>
        public void Write(TemplateFlowSet flowSet) {
            if (flowSet == null) {
                throw new ArgumentNullException(nameof(flowSet));
            }
            this.CheckState(State.Flows);
            var data = flowSet.ToWire();
            var written = this.Write(data);
            this.WritePadding(flowSet, written);
            this.CheckEndOfPacket();
        }
        #endregion
    }
}
