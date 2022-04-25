// <copyright file="NetFlowReader.cs" company="Universität Stuttgart">
// Copyright © 2020 SAPPAN Consortium. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace Sappan.Netflow.Netflow5 {

    /// <summary>
    /// Reader for NetFlow v5 streams.
    /// </summary>
    public sealed class NetflowReader : NetflowReaderBase {

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
        /// <param name="reader">The <see cref="BinaryReader"/> providing the
        /// data.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="reader"/>
        /// is <c>null</c>.</exception>
        public NetflowReader(BinaryReader reader) : base(reader) { }
        #endregion

        #region Public methods
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

            return retval;
        }

        /// <summary>
        /// Reads a flow record from the current location in the underlying
        /// stream.
        /// </summary>
        /// <returns>The record that has been read.</returns>
        /// <exception cref="InvalidOperationException">If the reader is not in
        /// a stated when it expectes a flow set. This is typically the case if
        /// the packet header has not yet been read.</exception>
        /// <exception cref="ObjectDisposedException">If the reader has already
        /// been disposed.</exception>
        /// <exception cref="KeyNotFoundException">If a data flow set was read
        /// which for we do not know a template.</exception>
        public FlowRecord ReadFlowRecord() {
            this.CheckState(State.Flows);
            Debug.Assert(this.Reader != null);
            var retval = new FlowRecord();

            var data = this.Reader.ReadBytes(retval.GetOnWireSize());
            this.CheckEndOfPacket();    // Even if parse fails, data are consumed.

            // Unpack the IP addresses at the begin manually.
            retval.SourceAddress = new IPAddress(data.Extract(0 * 4, 4));
            retval.DestinationAddress = new IPAddress(data.Extract(1 * 4, 4));
            retval.NextHop = new IPAddress(data.Extract(2 * 4, 4));

            // Unpack the trivial members automatically.
            var offset = 3 * sizeof(uint);
            var length = data.Length - offset;
            ReadMembers(ref retval, data, ref offset, ref length, 3);

            return retval;
        }
        #endregion
    }
}
