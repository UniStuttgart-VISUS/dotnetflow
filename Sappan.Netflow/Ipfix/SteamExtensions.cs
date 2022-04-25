// <copyright file="StreamExtensions.cs" company="Universität Stuttgart">
// Copyright © 2020 SAPPAN Consortium. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.Diagnostics;
using System.IO;
using static Sappan.Netflow.StreamExtensions;


namespace Sappan.Netflow.Ipfix {

    /// <summary>
    /// Provides extensions for <see cref="Stream"/> that allow for efficiently
    /// moving IPFIX packets between streams.
    /// </summary>
    public static class SteamExtensions {

        /// <summary>
        /// Copy a single IPFIX packet from <paramref name="that"/> to
        /// <paramref name="dst"/>.
        /// </summary>
        /// <remarks>
        /// <para>It is assumed that the file pointer in <paramref name="that"/>
        /// stands at the begin of a IPFIX packet. No checks are performed to
        /// ensure that. The method might rampage if the assumption does not
        /// hold and random data are interpreted as packet size.</para>
        /// <para>This method tries to inspect as few content of the packet
        /// as possible in order to prevent costly byte order conversions. It
        /// simply retrieves the packet header to obtain the size of the whole
        /// packet and then copies data without inspecting it.</para>
        /// </remarks>
        /// <param name="that">The stream to copy the packet from.</param>
        /// <param name="dst">The strem to copy the packet to.</param>
        /// <param name="bufferSize">The maximum size of the copy buffer to be
        /// used for transferring the data.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="that"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="dst"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="bufferSize"/>
        /// is less than 4 (the minimum fraction of the header we need to read).
        /// </exception>
        /// <exception cref="EndOfStreamException">If the input stream ended
        /// before the expected end of the packet.</exception>
        public static void CopyIpfixPacket(this Stream that, Stream dst,
                int bufferSize = DefaultCopyBufferSize) {
            if (that == null) {
                throw new ArgumentNullException(nameof(that));
            }
            if (dst == null) {
                throw new ArgumentNullException(nameof(that));
            }

            var cntHeader = (ushort) (2 * sizeof(ushort));
            if (bufferSize < cntHeader) {
                throw new ArgumentException(
                    Properties.Resources.ErrorCopyBufferSize,
                    nameof(bufferSize));
            }

            // The minmum to read from the header are two ushorts, the version
            // number and the packet size in bytes.
            var buffer = new byte[bufferSize];
            that.ReadBytesTo(buffer, 0, cntHeader);

            // Copy the start of the header we just read.
            dst.Write(buffer, 0, cntHeader);

            // Determine the (remaining) length of the packet.
            var length = BitConverter.ToUInt16(buffer, 2).ToHostByteOrder();
            Debug.Assert(length > cntHeader);
            length -= cntHeader;

            // Copy the rest of the packet.
            that.CopyBytesTo(dst, length, buffer);
        }
    }
}
