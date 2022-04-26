// <copyright file="ConversionExtensions.cs" company="Universität Stuttgart">
// Copyright © 2020 - 2022 Institut für Visualisierung und Interaktive Systeme. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;


namespace DotNetFlow {

    /// <summary>
    /// Extension methods for conversion to network byte order.
    /// </summary>
    internal static class OnWireExtensions {

        #region FromWire
        /// <summary>
        /// Converts <paramref name="data"/> into <paramref name="that"/>.
        /// </summary>
        /// <param name="that">The object to deserialise to.</param>
        /// <param name="data">The data, which must be enough to represent
        /// <paramref name="that"/> after <paramref name="offset"/>.</param>
        /// <param name="offset">The offset into <paramref name="data"/> where
        /// the input is located. On exit, the offset will be incremented by
        /// the amount of data that have been consumed for
        /// <paramref name="that"/>.</param>
        /// <param name="length">On exit, this is the length of the data that
        /// have not been consumed for <paramref name="that"/>.</param>
        /// <returns>The number of bytes that have been consumed to reconstruct
        /// <paramref name="that"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="data"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="offset"/>
        /// is negative.</exception>
        /// <exception cref="ArgumentException">If <paramref name="data"/>
        /// is too small to reconstruct <typeparamref name="that"/>.
        /// </exception>
        public static int FromWire(ref byte that, byte[] data,
                ref int offset, ref int length) {
            var retval = FromWire(ref that, data, offset);
            offset += retval;
            length = data.Length - offset;
            return retval;
        }

        /// <summary>
        /// Converts <paramref name="data"/> into <paramref name="that"/>.
        /// </summary>
        /// <param name="that">The object to deserialise to.</param>
        /// <param name="data">The data, which must be enough to represent
        /// <paramref name="that"/> after <paramref name="offset"/>.</param>
        /// <param name="offset">The offset into <paramref name="data"/> where
        /// the input is located.</param>
        /// <returns>The number of bytes that have been consumed to reconstruct
        /// <paramref name="that"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="data"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="offset"/>
        /// is negative.</exception>
        /// <exception cref="ArgumentException">If <paramref name="data"/>
        /// is too small to reconstruct <typeparamref name="that"/>.
        /// </exception>
        public static int FromWire(ref byte that, byte[] data,
                int offset = 0) {
            var retval = CheckData(that.GetType(), data, offset);
            that = data[offset];
            return retval;
        }

        /// <summary>
        /// Converts <paramref name="data"/> into <paramref name="that"/>.
        /// </summary>
        /// <param name="that">The object to deserialise to.</param>
        /// <param name="data">The data, which must be enough to represent
        /// <paramref name="that"/> after <paramref name="offset"/>.</param>
        /// <param name="offset">The offset into <paramref name="data"/> where
        /// the input is located. On exit, the offset will be incremented by
        /// the amount of data that have been consumed for
        /// <paramref name="that"/>.</param>
        /// <param name="length">On exit, this is the length of the data that
        /// have not been consumed for <paramref name="that"/>.</param>
        /// <returns>The number of bytes that have been consumed to reconstruct
        /// <paramref name="that"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="data"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="offset"/>
        /// is negative.</exception>
        /// <exception cref="ArgumentException">If <paramref name="data"/>
        /// is too small to reconstruct <typeparamref name="that"/>.
        /// </exception>
        public static int FromWire(ref short that, byte[] data,
                ref int offset, ref int length) {
            var retval = FromWire(ref that, data, offset);
            offset += retval;
            length = data.Length - offset;
            return retval;
        }

        /// <summary>
        /// Converts <paramref name="data"/> into <paramref name="that"/>.
        /// </summary>
        /// <param name="that">The object to deserialise to.</param>
        /// <param name="data">The data, which must be enough to represent
        /// <paramref name="that"/> after <paramref name="offset"/>.</param>
        /// <param name="offset">The offset into <paramref name="data"/> where
        /// the input is located.</param>
        /// <returns>The number of bytes that have been consumed to reconstruct
        /// <paramref name="that"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="data"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="offset"/>
        /// is negative.</exception>
        /// <exception cref="ArgumentException">If <paramref name="data"/>
        /// is too small to reconstruct <typeparamref name="that"/>.
        /// </exception>
        public static int FromWire(ref short that, byte[] data,
                int offset = 0) {
            var retval = CheckData(that.GetType(), data, offset);
            that = BitConverter.ToInt16(data, offset).ToHostByteOrder();
            return retval;
        }

        /// <summary>
        /// Converts <paramref name="data"/> into <paramref name="that"/>.
        /// </summary>
        /// <param name="that">The object to deserialise to.</param>
        /// <param name="data">The data, which must be enough to represent
        /// <paramref name="that"/> after <paramref name="offset"/>.</param>
        /// <param name="offset">The offset into <paramref name="data"/> where
        /// the input is located. On exit, the offset will be incremented by
        /// the amount of data that have been consumed for
        /// <paramref name="that"/>.</param>
        /// <param name="length">On exit, this is the length of the data that
        /// have not been consumed for <paramref name="that"/>.</param>
        /// <returns>The number of bytes that have been consumed to reconstruct
        /// <paramref name="that"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="data"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="offset"/>
        /// is negative.</exception>
        /// <exception cref="ArgumentException">If <paramref name="data"/>
        /// is too small to reconstruct <typeparamref name="that"/>.
        /// </exception>
        public static int FromWire(ref ushort that, byte[] data,
                ref int offset, ref int length) {
            var retval = FromWire(ref that, data, offset);
            offset += retval;
            length = data.Length - offset;
            return retval;
        }

        /// <summary>
        /// Converts <paramref name="data"/> into <paramref name="that"/>.
        /// </summary>
        /// <param name="that">The object to deserialise to.</param>
        /// <param name="data">The data, which must be enough to represent
        /// <paramref name="that"/> after <paramref name="offset"/>.</param>
        /// <param name="offset">The offset into <paramref name="data"/> where
        /// the input is located.</param>
        /// <returns>The number of bytes that have been consumed to reconstruct
        /// <paramref name="that"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="data"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="offset"/>
        /// is negative.</exception>
        /// <exception cref="ArgumentException">If <paramref name="data"/>
        /// is too small to reconstruct <typeparamref name="that"/>.
        /// </exception>
        public static int FromWire(ref ushort that, byte[] data,
                int offset = 0) {
            var retval = CheckData(that.GetType(), data, offset);
            that = BitConverter.ToUInt16(data, offset).ToHostByteOrder();
            return retval;
        }

        /// <summary>
        /// Converts <paramref name="data"/> into <paramref name="that"/>.
        /// </summary>
        /// <param name="that">The object to deserialise to.</param>
        /// <param name="data">The data, which must be enough to represent
        /// <paramref name="that"/> after <paramref name="offset"/>.</param>
        /// <param name="offset">The offset into <paramref name="data"/> where
        /// the input is located. On exit, the offset will be incremented by
        /// the amount of data that have been consumed for
        /// <paramref name="that"/>.</param>
        /// <param name="length">On exit, this is the length of the data that
        /// have not been consumed for <paramref name="that"/>.</param>
        /// <returns>The number of bytes that have been consumed to reconstruct
        /// <paramref name="that"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="data"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="offset"/>
        /// is negative.</exception>
        /// <exception cref="ArgumentException">If <paramref name="data"/>
        /// is too small to reconstruct <typeparamref name="that"/>.
        /// </exception>
        public static int FromWire(ref int that, byte[] data,
                ref int offset, ref int length) {
            var retval = FromWire(ref that, data, offset);
            offset += retval;
            length = data.Length - offset;
            return retval;
        }

        /// <summary>
        /// Converts <paramref name="data"/> into <paramref name="that"/>.
        /// </summary>
        /// <param name="that">The object to deserialise to.</param>
        /// <param name="data">The data, which must be enough to represent
        /// <paramref name="that"/> after <paramref name="offset"/>.</param>
        /// <param name="offset">The offset into <paramref name="data"/> where
        /// the input is located.</param>
        /// <returns>The number of bytes that have been consumed to reconstruct
        /// <paramref name="that"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="data"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="offset"/>
        /// is negative.</exception>
        /// <exception cref="ArgumentException">If <paramref name="data"/>
        /// is too small to reconstruct <typeparamref name="that"/>.
        /// </exception>
        public static int FromWire(ref int that, byte[] data,
                int offset = 0) {
            var retval = CheckData(that.GetType(), data, offset);
            that = BitConverter.ToInt32(data, offset).ToHostByteOrder();
            return retval;
        }

        /// <summary>
        /// Converts <paramref name="data"/> into <paramref name="that"/>.
        /// </summary>
        /// <param name="that">The object to deserialise to.</param>
        /// <param name="data">The data, which must be enough to represent
        /// <paramref name="that"/> after <paramref name="offset"/>.</param>
        /// <param name="offset">The offset into <paramref name="data"/> where
        /// the input is located. On exit, the offset will be incremented by
        /// the amount of data that have been consumed for
        /// <paramref name="that"/>.</param>
        /// <param name="length">On exit, this is the length of the data that
        /// have not been consumed for <paramref name="that"/>.</param>
        /// <returns>The number of bytes that have been consumed to reconstruct
        /// <paramref name="that"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="data"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="offset"/>
        /// is negative.</exception>
        /// <exception cref="ArgumentException">If <paramref name="data"/>
        /// is too small to reconstruct <typeparamref name="that"/>.
        /// </exception>
        public static int FromWire(ref uint that, byte[] data,
                ref int offset, ref int length) {
            var retval = FromWire(ref that, data, offset);
            offset += retval;
            length = data.Length - offset;
            return retval;
        }

        /// <summary>
        /// Converts <paramref name="data"/> into <paramref name="that"/>.
        /// </summary>
        /// <param name="that">The object to deserialise to.</param>
        /// <param name="data">The data, which must be enough to represent
        /// <paramref name="that"/> after <paramref name="offset"/>.</param>
        /// <param name="offset">The offset into <paramref name="data"/> where
        /// the input is located.</param>
        /// <returns>The number of bytes that have been consumed to reconstruct
        /// <paramref name="that"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="data"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="offset"/>
        /// is negative.</exception>
        /// <exception cref="ArgumentException">If <paramref name="data"/>
        /// is too small to reconstruct <typeparamref name="that"/>.
        /// </exception>
        public static int FromWire(ref uint that, byte[] data,
                int offset = 0) {
            var retval = CheckData(that.GetType(), data, offset);
            that = BitConverter.ToUInt32(data, offset).ToHostByteOrder();
            return retval;
        }

        /// <summary>
        /// Converts <paramref name="data"/> into <paramref name="that"/>.
        /// </summary>
        /// <param name="that">The object to deserialise to.</param>
        /// <param name="data">The data, which must be enough to represent
        /// <paramref name="that"/> after <paramref name="offset"/>.</param>
        /// <param name="offset">The offset into <paramref name="data"/> where
        /// the input is located. On exit, the offset will be incremented by
        /// the amount of data that have been consumed for
        /// <paramref name="that"/>.</param>
        /// <param name="length">On exit, this is the length of the data that
        /// have not been consumed for <paramref name="that"/>.</param>
        /// <returns>The number of bytes that have been consumed to reconstruct
        /// <paramref name="that"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="data"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="offset"/>
        /// is negative.</exception>
        /// <exception cref="ArgumentException">If <paramref name="data"/>
        /// is too small to reconstruct <typeparamref name="that"/>.
        /// </exception>
        public static int FromWire(ref long that, byte[] data,
                ref int offset, ref int length) {
            var retval = FromWire(ref that, data, offset);
            offset += retval;
            length = data.Length - offset;
            return retval;
        }

        /// <summary>
        /// Converts <paramref name="data"/> into <paramref name="that"/>.
        /// </summary>
        /// <param name="that">The object to deserialise to.</param>
        /// <param name="data">The data, which must be enough to represent
        /// <paramref name="that"/> after <paramref name="offset"/>.</param>
        /// <param name="offset">The offset into <paramref name="data"/> where
        /// the input is located.</param>
        /// <returns>The number of bytes that have been consumed to reconstruct
        /// <paramref name="that"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="data"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="offset"/>
        /// is negative.</exception>
        /// <exception cref="ArgumentException">If <paramref name="data"/>
        /// is too small to reconstruct <typeparamref name="that"/>.
        /// </exception>
        public static int FromWire(ref long that, byte[] data,
                int offset = 0) {
            var retval = CheckData(that.GetType(), data, offset);
            that = BitConverter.ToInt64(data, offset).ToHostByteOrder();
            return retval;
        }

        /// <summary>
        /// Converts <paramref name="data"/> into <paramref name="that"/>.
        /// </summary>
        /// <param name="that">The object to deserialise to.</param>
        /// <param name="data">The data, which must be enough to represent
        /// <paramref name="that"/> after <paramref name="offset"/>.</param>
        /// <param name="offset">The offset into <paramref name="data"/> where
        /// the input is located. On exit, the offset will be incremented by
        /// the amount of data that have been consumed for
        /// <paramref name="that"/>.</param>
        /// <param name="length">On exit, this is the length of the data that
        /// have not been consumed for <paramref name="that"/>.</param>
        /// <returns>The number of bytes that have been consumed to reconstruct
        /// <paramref name="that"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="data"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="offset"/>
        /// is negative.</exception>
        /// <exception cref="ArgumentException">If <paramref name="data"/>
        /// is too small to reconstruct <typeparamref name="that"/>.
        /// </exception>
        public static int FromWire(ref ulong that, byte[] data,
                ref int offset, ref int length) {
            var retval = FromWire(ref that, data, offset);
            offset += retval;
            length = data.Length - offset;
            return retval;
        }

        /// <summary>
        /// Converts <paramref name="data"/> into <paramref name="that"/>.
        /// </summary>
        /// <param name="that">The object to deserialise to.</param>
        /// <param name="data">The data, which must be enough to represent
        /// <paramref name="that"/> after <paramref name="offset"/>.</param>
        /// <param name="offset">The offset into <paramref name="data"/> where
        /// the input is located.</param>
        /// <returns>The number of bytes that have been consumed to reconstruct
        /// <paramref name="that"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="data"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="offset"/>
        /// is negative.</exception>
        /// <exception cref="ArgumentException">If <paramref name="data"/>
        /// is too small to reconstruct <typeparamref name="that"/>.
        /// </exception>
        public static int FromWire(ref ulong that, byte[] data,
                int offset = 0) {
            var retval = CheckData(that.GetType(), data, offset);
            that = BitConverter.ToUInt64(data, offset).ToHostByteOrder();
            return retval;
        }

        /// <summary>
        /// Converts <paramref name="data"/> into <paramref name="that"/>.
        /// </summary>
        /// <param name="that">The object to deserialise to.</param>
        /// <param name="data">The data, which must be enough to represent
        /// <paramref name="that"/> after <paramref name="offset"/>.</param>
        /// <param name="offset">The offset into <paramref name="data"/> where
        /// the input is located. On exit, the offset will be incremented by
        /// the amount of data that have been consumed for
        /// <paramref name="that"/>.</param>
        /// <param name="length">On exit, this is the length of the data that
        /// have not been consumed for <paramref name="that"/>.</param>
        /// <returns>The number of bytes that have been consumed to reconstruct
        /// <paramref name="that"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="data"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="offset"/>
        /// is negative.</exception>
        /// <exception cref="ArgumentException">If <paramref name="data"/>
        /// is too small to reconstruct <typeparamref name="that"/>.
        /// </exception>
        public static int FromWire(ref float that, byte[] data,
                ref int offset, ref int length) {
            var retval = FromWire(ref that, data, offset);
            offset += retval;
            length = data.Length - offset;
            return retval;
        }

        /// <summary>
        /// Converts <paramref name="data"/> into <paramref name="that"/>.
        /// </summary>
        /// <param name="that">The object to deserialise to.</param>
        /// <param name="data">The data, which must be enough to represent
        /// <paramref name="that"/> after <paramref name="offset"/>.</param>
        /// <param name="offset">The offset into <paramref name="data"/> where
        /// the input is located.</param>
        /// <returns>The number of bytes that have been consumed to reconstruct
        /// <paramref name="that"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="data"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="offset"/>
        /// is negative.</exception>
        /// <exception cref="ArgumentException">If <paramref name="data"/>
        /// is too small to reconstruct <typeparamref name="that"/>.
        /// </exception>
        public static int FromWire(ref float that, byte[] data,
                int offset = 0) {
            var retval = CheckData(that.GetType(), data, offset);
            that = BitConverter.ToSingle(data, offset);
            return retval;
        }

        /// <summary>
        /// Converts <paramref name="data"/> into <paramref name="that"/>.
        /// </summary>
        /// <param name="that">The object to deserialise to.</param>
        /// <param name="data">The data, which must be enough to represent
        /// <paramref name="that"/> after <paramref name="offset"/>.</param>
        /// <param name="offset">The offset into <paramref name="data"/> where
        /// the input is located. On exit, the offset will be incremented by
        /// the amount of data that have been consumed for
        /// <paramref name="that"/>.</param>
        /// <param name="length">On exit, this is the length of the data that
        /// have not been consumed for <paramref name="that"/>.</param>
        /// <returns>The number of bytes that have been consumed to reconstruct
        /// <paramref name="that"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="data"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="offset"/>
        /// is negative.</exception>
        /// <exception cref="ArgumentException">If <paramref name="data"/>
        /// is too small to reconstruct <typeparamref name="that"/>.
        /// </exception>
        public static int FromWire(ref double that, byte[] data,
                ref int offset, ref int length) {
            var retval = FromWire(ref that, data, offset);
            offset += retval;
            length = data.Length - offset;
            return retval;
        }

        /// <summary>
        /// Converts <paramref name="data"/> into <paramref name="that"/>.
        /// </summary>
        /// <param name="that">The object to deserialise to.</param>
        /// <param name="data">The data, which must be enough to represent
        /// <paramref name="that"/> after <paramref name="offset"/>.</param>
        /// <param name="offset">The offset into <paramref name="data"/> where
        /// the input is located.</param>
        /// <returns>The number of bytes that have been consumed to reconstruct
        /// <paramref name="that"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="data"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="offset"/>
        /// is negative.</exception>
        /// <exception cref="ArgumentException">If <paramref name="data"/>
        /// is too small to reconstruct <typeparamref name="that"/>.
        /// </exception>
        public static int FromWire(ref double that, byte[] data,
                int offset = 0) {
            var retval = CheckData(that.GetType(), data, offset);
            that = BitConverter.ToDouble(data, offset);
            return retval;
        }

        /// <summary>
        /// Converts <paramref name="data"/> into <paramref name="that"/>.
        /// </summary>
        /// <remarks>
        /// We assume that the input data are ASCII-encoded strings.
        /// </remarks>
        /// <param name="that">The object to deserialise to.</param>
        /// <param name="data">The data, which must be enough to represent
        /// <paramref name="that"/> after <paramref name="offset"/>.</param>
        /// <param name="offset">The offset into <paramref name="data"/> where
        /// the input is located. On exit, the offset will be incremented by
        /// the amount of data that have been consumed for
        /// <paramref name="that"/>.</param>
        /// <param name="length">The length of the data to be converted. If
        /// zero or negative, the whole content of <paramref name="data"/> is
        /// being consumed. On exit, this is the length of the data that
        /// have not been consumed for <paramref name="that"/>.</param>
        /// <returns>The number of bytes that have been consumed to reconstruct
        /// <paramref name="that"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="data"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="offset"/>
        /// is negative.</exception>
        /// <exception cref="ArgumentException">If <paramref name="data"/>
        /// is too small to reconstruct <typeparamref name="that"/>.
        /// </exception>
        public static int FromWire(ref string that, byte[] data,
                ref int offset, ref int length) {
            CheckData(data, offset, ref length);
            var retval = length;
            that = Encoding.ASCII.GetString(data, offset, length);
            offset += retval;
            length = data.Length - offset;
            return retval;
        }

        /// <summary>
        /// Converts <paramref name="data"/> into <paramref name="that"/>.
        /// </summary>
        /// <remarks>
        /// We assume that the input data are ASCII-encoded strings.
        /// </remarks>
        /// <param name="that">The object to deserialise to.</param>
        /// <param name="data">The data, which must be enough to represent
        /// <paramref name="that"/> after <paramref name="offset"/>.</param>
        /// <param name="offset">The offset into <paramref name="data"/> where
        /// the input is located.</param>
        /// <param name="length">The length of the data to be converted. If
        /// zero or negative, the whole content of <paramref name="data"/> is
        /// being consumed.</param>
        /// <returns>The number of bytes that have been consumed to reconstruct
        /// <paramref name="that"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="data"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="offset"/>
        /// is negative.</exception>
        /// <exception cref="ArgumentException">If <paramref name="data"/>
        /// is too small to reconstruct <typeparamref name="that"/>.
        /// </exception>
        public static int FromWire(ref string that, byte[] data,
                int offset = 0, int length = 0) {
            CheckData(data, offset, ref length);
            var retval = length;
            that = Encoding.ASCII.GetString(data, offset, length);
            return retval;
        }

        /// <summary>
        /// Converts <paramref name="data"/> into <paramref name="that"/>.
        /// </summary>
        /// <remarks>
        /// We assume that the input data are ASCII-encoded strings.
        /// </remarks>
        /// <param name="that">The object to deserialise to.</param>
        /// <param name="data">The data, which must be enough to represent
        /// <paramref name="that"/> after <paramref name="offset"/>.</param>
        /// <param name="offset">The offset into <paramref name="data"/> where
        /// the input is located. On exit, the offset will be incremented by
        /// the amount of data that have been consumed for
        /// <paramref name="that"/>.</param>
        /// <param name="length">The length of the data to be converted. If
        /// zero or negative, the whole content of <paramref name="data"/> is
        /// being consumed. On exit, this is the length of the data that
        /// have not been consumed for <paramref name="that"/>.</param>
        /// <returns>The number of bytes that have been consumed to reconstruct
        /// <paramref name="that"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="data"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="offset"/>
        /// is negative.</exception>
        /// <exception cref="ArgumentException">If <paramref name="data"/>
        /// is too small to reconstruct <typeparamref name="that"/>.
        /// </exception>
        public static int FromWire(ref IPAddress that, byte[] data,
                ref int offset, ref int length) {
            var retval = FromWire(ref that, data, offset, length);
            offset += retval;
            length = data.Length - offset;
            return retval;
        }

        /// <summary>
        /// Converts <paramref name="data"/> into <paramref name="that"/>.
        /// </summary>
        /// <remarks>
        /// We assume that the input data are ASCII-encoded strings.
        /// </remarks>
        /// <param name="that">The object to deserialise to.</param>
        /// <param name="data">The data, which must be enough to represent
        /// <paramref name="that"/> after <paramref name="offset"/>.</param>
        /// <param name="offset">The offset into <paramref name="data"/> where
        /// the input is located.</param>
        /// <param name="length">The length of the data to be converted. If
        /// zero or negative, the whole content of <paramref name="data"/> is
        /// being consumed.</param>
        /// <returns>The number of bytes that have been consumed to reconstruct
        /// <paramref name="that"/>.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="data"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="offset"/>
        /// is negative.</exception>
        /// <exception cref="ArgumentException">If <paramref name="data"/>
        /// is too small to reconstruct <typeparamref name="that"/>.
        /// </exception>
        public static int FromWire(ref IPAddress that, byte[] data,
                int offset = 0, int length = 0) {
            CheckData(data, offset, ref length);
            Debug.Assert(length >= 4);
            that = new IPAddress(data.Skip(offset).Take(length).ToArray());
            return length;
        }

        /// <summary>
        /// Restore a trivial object of the given type <paramref name="that"/>
        /// from <paramref name="data"/>, possibly limiting the data that can
        /// be used to the range from <paramref name="offset"/> of at most
        /// <paramref name="length"/> bytes.
        /// </summary>
        /// <remarks>
        /// This method only dispatches to the overloads of
        /// <see cref="FromWire(Type, byte[], ref int, ref int)"/>, but does
        /// not unpack complex types. The reason for that is that unpacking data
        /// flow sets required knowledge of the templates and therefore can only
        /// be performed by <see cref="Netflow9.NetflowReader"/>.
        /// </remarks>
        /// <param name="that">The type to be restored.</param>
        /// <param name="data">The raw data to restore from.</param>
        /// <param name="offset">The offset into <paramref name="data"/> where
        /// the object of type <paramref name="that"/> starts. On exit, the
        /// offset will be incremented by the amount of data that have been
        /// consumed for <paramref name="that"/>.</param>
        /// <param name="length">The length of the data to be converted. If
        /// zero or negative, the whole content of <paramref name="data"/> is
        /// being consumed. On exit, this is the length of the data that
        /// have not been consumed for <paramref name="that"/>.</param>
        /// <returns>The deserialised object or <c>null</c> if the type could
        /// not be processed using one of the overloads in this method.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="data"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="offset"/>
        /// is negative.</exception>
        /// <exception cref="ArgumentException">If <paramref name="data"/>
        /// is too small to reconstruct <typeparamref name="that"/>.
        /// </exception>
        public static object FromWire(this Type that, byte[] data,
                ref int offset, ref int length) {
            if (that == null) {
                throw new ArgumentNullException(nameof(that));
            }
            CheckData(data, offset, ref length);

            // Make sure that we use the underlying type if we read an enum.
            var type = that.IsEnum ? that.GetEnumUnderlyingType() : that;

            // Check whether the type implements custom handling.
            if (type.GetInterfaces().Contains(typeof(IHandleOnWire))) {
                var retval = Activator.CreateInstance(type) as IHandleOnWire;
                var cnt = retval.FromWire(data, offset);
                offset += cnt;
                length -= cnt;
                return retval;
            }

            // Dispatch to the right overload using reflection.
            var fromWire = typeof(OnWireExtensions).GetMethod(nameof(FromWire),
                new[] { type.MakeByRefType(), data.GetType(),
                    offset.GetType().MakeByRefType(),
                    length.GetType().MakeByRefType() });

            if (fromWire != null) {
                // Invoke the overload if available.
                try {
                    var args = new[] { Activator.CreateInstance(type), data,
                        offset, length };
                    fromWire.Invoke(null, args);
                    offset = (int) args[2];
                    length = (int) args[3];
                    return args[0];
                } catch (TargetInvocationException ex) {
                    // Unpack TargetInvocationException to make this call behave
                    // like calling the original method directly.
                    throw ex.InnerException ?? ex;
                }

            } else {
                // Signal that the type could not be processed automatically.
                return null;
            }
        }
        #endregion

        #region ToWire
        /// <summary>
        /// Converts <paramref name="that"/> to its on-wire representation.
        /// </summary>
        /// <param name="that">The data to be converted.</param>
        /// <returns>The on-wire representation of <paramref name="that"/>.
        /// </returns>
        public static byte[] ToWire(this byte that) {
            return new[] { that };
        }

        /// <summary>
        /// Converts <paramref name="that"/> to its on-wire representation.
        /// </summary>
        /// <param name="that">The data to be converted.</param>
        /// <returns>The on-wire representation of <paramref name="that"/>.
        /// </returns>
        public static byte[] ToWire(this short that) {
            // TODO: We could speed up conversion using the LayoutKind.Explicit hack
            return BitConverter.GetBytes(that.ToNetworkByteOrder());
        }

        /// <summary>
        /// Converts <paramref name="that"/> to its on-wire representation.
        /// </summary>
        /// <param name="that">The data to be converted.</param>
        /// <returns>The on-wire representation of <paramref name="that"/>.
        /// </returns>
        public static byte[] ToWire(this ushort that) {
            // TODO: We could speed up conversion using the LayoutKind.Explicit hack
            return BitConverter.GetBytes(that.ToNetworkByteOrder());
        }

        /// <summary>
        /// Converts <paramref name="that"/> to its on-wire representation.
        /// </summary>
        /// <param name="that">The data to be converted.</param>
        /// <returns>The on-wire representation of <paramref name="that"/>.
        /// </returns>
        public static byte[] ToWire(this int that) {
            // TODO: We could speed up conversion using the LayoutKind.Explicit hack
            return BitConverter.GetBytes(that.ToNetworkByteOrder());
        }

        /// <summary>
        /// Converts <paramref name="that"/> to its on-wire representation.
        /// </summary>
        /// <param name="that">The data to be converted.</param>
        /// <returns>The on-wire representation of <paramref name="that"/>.
        /// </returns>
        public static byte[] ToWire(this uint that) {
            // TODO: We could speed up conversion using the LayoutKind.Explicit hack
            return BitConverter.GetBytes(that.ToNetworkByteOrder());
        }

        /// <summary>
        /// Converts <paramref name="that"/> to its on-wire representation.
        /// </summary>
        /// <param name="that">The data to be converted.</param>
        /// <returns>The on-wire representation of <paramref name="that"/>.
        /// </returns>
        public static byte[] ToWire(this long that) {
            // TODO: We could speed up conversion using the LayoutKind.Explicit hack
            return BitConverter.GetBytes(that.ToNetworkByteOrder());
        }

        /// <summary>
        /// Converts <paramref name="that"/> to its on-wire representation.
        /// </summary>
        /// <param name="that">The data to be converted.</param>
        /// <returns>The on-wire representation of <paramref name="that"/>.
        /// </returns>
        public static byte[] ToWire(this ulong that) {
            // TODO: We could speed up conversion using the LayoutKind.Explicit hack
            return BitConverter.GetBytes(that.ToNetworkByteOrder());
        }

        /// <summary>
        /// Converts <paramref name="that"/> to its on-wire representation.
        /// </summary>
        /// <param name="that">The data to be converted.</param>
        /// <returns>The on-wire representation of <paramref name="that"/>.
        /// </returns>
        public static byte[] ToWire(this float that) {
            var retval = BitConverter.GetBytes(that);
            return retval;
        }

        /// <summary>
        /// Converts <paramref name="that"/> to its on-wire representation.
        /// </summary>
        /// <param name="that">The data to be converted.</param>
        /// <returns>The on-wire representation of <paramref name="that"/>.
        /// </returns>
        public static byte[] ToWire(this double that) {
            var retval = BitConverter.GetBytes(that);
            return retval;
        }

        /// <summary>
        /// Converts <paramref name="that"/> to its on-wire representation.
        /// </summary>
        /// <remarks>
        /// We assume that only ASCII strings are used in NetFlow, so this
        /// method uses the ASCII encoding to obtain the byte representation.
        /// </remarks>
        /// <param name="that">The data to be converted.</param>
        /// <returns>The on-wire representation of <paramref name="that"/>.
        /// </returns>
        public static byte[] ToWire(this string that) {
            if (that == null) {
                throw new ArgumentNullException(nameof(that));
            }
            var retval = Encoding.ASCII.GetBytes(that);
            return retval;
        }

        /// <summary>
        /// Converts <paramref name="that"/> to its on-wire representation.
        /// </summary>
        /// <remarks>
        /// Note that we use the non-generic <see cref="IEnumerable"/> because
        /// this way, the method is considered &quot;trivial&quot; in the
        /// implementation for <see cref="Object"/>.
        /// </remarks>
        /// <param name="that">The data to be converted.</param>
        /// <returns>The on-wire representation of <paramref name="that"/>.
        /// </returns>
        public static byte[] ToWire(this IEnumerable that) {
            if (that == null) {
                throw new ArgumentNullException(nameof(that));
            }

            // TODO: This is a performance-off implementation.
            var retval = new List<byte>();
            foreach (var e in that) {
                retval.AddRange(ToWire(e));
            }

            return retval.ToArray();
        }

        /// <summary>
        /// Converts <paramref name="that"/> to its on-wire representation.
        /// </summary>
        /// <param name="that">The data to be converted.</param>
        /// <returns>The on-wire representation of <paramref name="that"/>.
        /// </returns>
        public static byte[] ToWire(this IPAddress that) {
            if (that == null) {
                throw new ArgumentNullException(nameof(that));
            }
            return that.GetAddressBytes();
        }

        /// <summary>
        /// Converts <paramref name="that"/> to its on-wire representation.
        /// </summary>
        /// <remarks>
        /// This method also accepts all of the primitive conversions done by
        /// the methods above. It checks whether they are applicable and treats
        /// <paramref name="that"/> only as a complex object, if no trivial
        /// conversion is available.
        /// </remarks>
        /// <param name="that">The data to be converted.</param>
        /// <returns>The on-wire representation of <paramref name="that"/>.
        /// </returns>
        public static byte[] ToWire(this object that) {
            if (that == null) {
                throw new ArgumentNullException(nameof(that));
            }

            // First, check whether the object implements custom conversion.
            if (that is IHandleOnWire handler) {
                return handler.ToWire();
            }

            // Next, we need to check whether 'that' can be directly converted,
            // because dynamic dispatching depends on the static type of 'that'
            // and therefore might send primitive types into this method as
            // well.
            {
                var toWire = typeof(OnWireExtensions).GetMethod(nameof(ToWire),
                    new[] { that.GetType() });

                if ((toWire != null) && (toWire != ObjectToWire)) {
                    return (byte[]) toWire.Invoke(null, new[] { that });
                }
            }

            // If we did not find a better implementation, tread 'that' as a
            // complex type.
            // TODO: This is a performance-off implementation.
            var retval = new List<byte>();

            var members = that.GetType().GetOnWireMembers();
            if (!members.Any()) {
                throw new ArgumentException(
                    Properties.Resources.ErrorNoOnWireMembers,
                    nameof(that));
            }

            foreach (var m in members) {
                if (m is FieldInfo f) {
                    var v = f.GetValue(that);
                    retval.AddRange(ToWire(v));

                } else if (m is PropertyInfo p) {
                    var v = p.GetValue(that);
                    retval.AddRange(ToWire(v));

                } else {
                    throw new InvalidOperationException(
                        Properties.Resources.ErrorOnWireMemberType);
                }
            }

            return retval.ToArray();
        }
        #endregion

        #region Private methods
        /// <summary>
        /// Performs a sanity check on <paramref name="data"/> and
        /// <paramref name="offset"/> for conversion from on-wire to in-memory
        /// representations.
        /// </summary>
        /// <param name="target">The target type.</param>
        /// <param name="data">The array with the byte representation of the
        /// target type.</param>
        /// <param name="offset">The offset into <paramref name="data"/> where
        /// the target type starts.</param>
        /// <returns>The number of bytes required to reconstruct the target
        /// type.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="data"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="offset"/>
        /// is negative.</exception>
        /// <exception cref="ArgumentException">If <paramref name="data"/>
        /// is too small for <typeparamref name="target"/>.</exception>
        private static int CheckData(Type target, byte[] data, int offset) {
            Debug.Assert(target != null);
            if (data == null) {
                throw new ArgumentNullException(nameof(data));
            }
            if (offset < 0) {
                throw new ArgumentException(
                    Properties.Resources.ErrorNegativeOffset,
                    nameof(offset));
            }

            var retval = Marshal.SizeOf(target);

            if (data.Length < retval + offset) {
                throw new ArgumentException(
                    Properties.Resources.ErrorTooFewDataToConvert,
                    nameof(data));
            }

            return retval;
        }

        /// <summary>
        /// Performs a sanity check on <paramref name="data"/> and
        /// <paramref name="offset"/> for conversion from on-wire to in-memory
        /// representations.
        /// </summary>
        /// <remarks>
        /// On successful exit, the <paramref name="length"/> will be either a
        /// valid user provided amount of data that can safely be read from
        /// <paramref name="data"/> or it will the size of the array.
        /// </remarks>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <exception cref="ArgumentNullException">If <paramref name="data"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="offset"/>
        /// is negative.</exception>
        /// <exception cref="ArgumentException">If <paramref name="data"/>
        /// is too small to retrieve <paramref name="length"/> elements after
        /// <paramref name="offset"/>.</exception>
        private static void CheckData(byte[] data, int offset, ref int length) {
            if (data == null) {
                throw new ArgumentNullException(nameof(data));
            }
            if (offset < 0) {
                throw new ArgumentException(
                    Properties.Resources.ErrorNegativeOffset,
                    nameof(offset));
            }

            if (length <= 0) {
                // If no user-specified length is given, consume everything.
                length = data.Length - offset;
            }

            if (data.Length < offset + length) {
                throw new ArgumentException(
                    Properties.Resources.ErrorTooFewDataToConvert,
                    nameof(data));
            }
        }
        #endregion

        #region Private fields
        /// <summary>
        /// The generic <see cref="ToWire(object)"/> that is called as a
        /// fallback solution.
        /// </summary>
        private static readonly MethodInfo ObjectToWire = typeof(OnWireExtensions)
            .GetMethod(nameof(ToWire), new[] { typeof(object) });
        #endregion
    }
}
