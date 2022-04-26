// <copyright file="StreamExtensions.cs" company="Universität Stuttgart">
// Copyright © 2020 - 2022 Institut für Visualisierung und Interaktive Systeme. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.IO;


namespace DotNetFlow.Netflow5 {

    /// <summary>
    /// Provides extensions for <see cref="Stream"/> that allow for efficiently
    /// moving NetFlow v5 packets between streams.
    /// </summary>
    public static class StreamExtensions {

        /// <summary>
        /// Copy a single NetFlow v5 packet from <paramref name="that"/> to
        /// <paramref name="dst"/>.
        /// </summary>
        /// <remarks>
        /// <para>It is assumed that the file pointer in <paramref name="that"/>
        /// stands at the begin of a flow packet. No checks are performed to
        /// ensure that.</para>
        /// <para>As NetFlow v5 packets have a fixed size, this copy operation
        /// is not using any information from the content of the stream.</para>
        /// </remarks>
        /// <param name="that">The stream to copy the packet from.</param>
        /// <param name="dst">The strem to copy the packet to.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="that"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="dst"/>
        /// is <c>null</c>.</exception>
        public static void CopyNetflow5Packet(this Stream that, Stream dst) {
            if (that == null) {
                throw new ArgumentNullException(nameof(that));
            }
            if (dst == null) {
                throw new ArgumentNullException(nameof(dst));
            }

            var size = 24 + 48;
            that.CopyBytesTo(dst, size, new byte[size]);
        }

    }
}
