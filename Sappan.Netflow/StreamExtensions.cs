// <copyright file="StreamExtensions.cs" company="Universität Stuttgart">
// Copyright © 2020 SAPPAN Consortium. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.Diagnostics;
using System.IO;


namespace Sappan.Netflow {

    /// <summary>
    /// Extension methods for <see cref="Stream"/>.
    /// </summary>
    internal static class StreamExtensions {

        /// <summary>
        /// Default copy buffer size.
        /// </summary>
        /// <remarks>
        /// The value is taken from the .NET reference implementation at
        /// https://github.com/microsoft/referencesource/blob/master/mscorlib/system/io/stream.cs
        /// and comes only into play if the packet should be larger than
        /// this value.
        /// </remarks>
        internal const int DefaultCopyBufferSize = 81920;

        /// <summary>
        /// Copies <paramref name="length"/> bytes from <paramref name="that"/>
        /// to <paramref name="dst"/>.
        /// </summary>
        /// <param name="that">The stream to copy from.</param>
        /// <param name="dst">The stream to copy to.</param>
        /// <param name="length">The number of bytes to be copied. If this value
        /// is zero or negative, the method behaves like
        /// <see cref="Stream.CopyTo(Stream, int)"/>.</param>
        /// <param name="buffer">The buffer used to copy the data.</param>
        /// <exception cref="EndOfStreamException">If <paramref name="that"/>
        /// ended before <paramref name="length"/> bytes could be copied.
        /// </exception>
        internal static void CopyBytesTo(this Stream that, Stream dst,
                int length, byte[] buffer) {
            Debug.Assert(that != null);
            Debug.Assert(that.CanRead);
            Debug.Assert(dst != null);
            Debug.Assert(dst.CanWrite);
            Debug.Assert(buffer != null);
            Debug.Assert(buffer.Length > 0);

            if (length <= 0) {
                // Special case: no length limit was given, copy everything.
                that.CopyTo(dst);

            } else {
                var remaining = length;

                while (remaining > 0) {
                    var count = Math.Min(remaining, buffer.Length);
                    var read = that.Read(buffer, 0, count);
                    if (read == 0) {
                        throw new EndOfStreamException();
                    }

                    dst.Write(buffer, 0, read);
                    Debug.Assert(remaining >= read);
                    remaining -= read;
                }
            }
        }

        /// <summary>
        /// Reads <paramref name="count"/> bytes from <paramref name="that"/>
        /// into <see cref="buffer"/> or fails if this is not possible.
        /// </summary>
        /// <param name="that">The stream to read from.</param>
        /// <param name="dst">The buffer to read to.</param>
        /// <param name="offset">The index where to start writing the read
        /// data in <paramref name="dst"/>.</param>
        /// <param name="count">The number of bytes to read.</param>
        /// <exception cref="EndOfStreamException">If the stream ended before
        /// the requested number of bytes could be read.</exception>
        internal static void ReadBytesTo(this Stream that, byte[] dst,
                int offset, int count) {
            Debug.Assert(that != null);
            Debug.Assert(that.CanRead);
            Debug.Assert(dst != null);
            Debug.Assert(dst.Length >= count + offset);
            Debug.Assert(offset >= 0);
            Debug.Assert(count > 0);

            while (count > 0) {
                var read = that.Read(dst, offset, count);
                if (read == 0) {
                    throw new EndOfStreamException();
                }

                offset += read;
                count -= read;
            }
        }

    }
}
