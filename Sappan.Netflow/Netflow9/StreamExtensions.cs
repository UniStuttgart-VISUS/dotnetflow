// <copyright file="StreamExtensions.cs" company="Universität Stuttgart">
// Copyright © 2020 SAPPAN Consortium. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.IO;
using static Sappan.Netflow.StreamExtensions;


namespace Sappan.Netflow.Netflow9 {

    /// <summary>
    /// Provides extensions for <see cref="Stream"/> that allow for efficiently
    /// moving NetFlow v9 packets between streams.
    /// </summary>
    public static class StreamExtensions {

        /// <summary>
        /// Copy a single NetFlow v9 packet from <paramref name="that"/> to
        /// <paramref name="dst"/>.
        /// </summary>
        /// <remarks>
        /// <para>It is assumed that the file pointer in <paramref name="that"/>
        /// stands at the begin of a IPFIX packet. No checks are performed to
        /// ensure that. The method might rampage if the assumption does not
        /// hold and random data are interpreted as packet size.</para>
        /// <para>This method tries to inspect as few content of the packet
        /// as possible in order to prevent costly byte order conversions. It
        /// retrieves the packet header to obtain the number of flow sets and
        /// peeks the headers of the flow sets for their size. Unfortunately,
        /// the packet therefore cannot be copied at once, but needs to be
        /// copied on a per-flow set basis.</para>
        /// </remarks>
        /// <param name="that">The stream to copy the packet from.</param>
        /// <param name="dst">The strem to copy the packet to.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="that"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="dst"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="bufferSize"/>
        /// is less than 4 (the minimum fraction of the header we need to read).
        /// </exception>
        /// <exception cref="EndOfStreamException">If the input stream ended
        /// before the expected end of the packet.</exception>
        public static void CopyNetflow9Packet(this Stream that, Stream dst,
                int bufferSize = DefaultCopyBufferSize) {
            if (that == null) {
                throw new ArgumentNullException(nameof(that));
            }
            if (dst == null) {
                throw new ArgumentNullException(nameof(that));
            }

            var cntHeader = (ushort) (2 * sizeof(ushort) + 4 * sizeof(uint));
            if (bufferSize < cntHeader) {
                throw new ArgumentException(
                    Properties.Resources.ErrorCopyBufferSize,
                    nameof(bufferSize));
            }

            // Read the whole packet header.
            var buffer = new byte[bufferSize];
            that.ReadBytesTo(buffer, 0, cntHeader);

            // Copy the header we just read.
            dst.Write(buffer, 0, cntHeader);

            // Determine number of flow sets we need to read.
            var cntFlowSets = BitConverter.ToUInt16(buffer, 2).ToHostByteOrder();

            // From now on, we are only interested in the first two ushorts in 
            // the flow set header.
            cntHeader = 2 * sizeof(ushort);

            // Process one flow set after the other, because we cannot compute
            // the packet size from the packet header, but only from the headers
            // of the individual flow sets.
            for (int i = 0; i < cntFlowSets; ++i) {
                // Copy the minimum header bytes.
                that.ReadBytesTo(buffer, 0, cntHeader);
                dst.Write(buffer, 0, cntHeader);

                // Compute the number of remaining bytes in the flow set.
                var length = BitConverter.ToUInt16(buffer, 2).ToHostByteOrder();
                length -= cntHeader;

                // Copy the rest of the flow sets.
                that.CopyBytesTo(dst, length, buffer);
            }
        }
    }
}
