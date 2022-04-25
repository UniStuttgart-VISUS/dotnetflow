// <copyright file="NetworkOrderExtensions.cs" company="Universität Stuttgart">
// Copyright © 2020 SAPPAN Consortium. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using System.Net;


namespace Sappan.Netflow {

    /// <summary>
    /// Extension methods for conversion from and to network byte order.
    /// </summary>
    public static class NetworkOrderExtensions {

        #region ToHostByteOrder
        /// <summary>
        /// Converts <paramref name="that"/> to host byte order.
        /// </summary>
        /// <param name="that">The data in network byte order to be
        /// converted to network byte order.</param>
        /// <returns><paramref name="that"/> in host byte order.</returns>
        public static byte ToHostByteOrder(this byte that) {
            return that;
        }

        /// <summary>
        /// Converts <paramref name="that"/> to host byte order.
        /// </summary>
        /// <param name="that">The data in network byte order to be
        /// converted to network byte order.</param>
        /// <returns><paramref name="that"/> in host byte order.</returns>
        public static short ToHostByteOrder(this short that) {
            return IPAddress.HostToNetworkOrder(that);
        }

        /// <summary>
        /// Converts <paramref name="that"/> to host byte order.
        /// </summary>
        /// <param name="that">The data in network byte order to be
        /// converted to network byte order.</param>
        /// <returns><paramref name="that"/> in host byte order.</returns>
        public static ushort ToHostByteOrder(this ushort that) {
            var retval = IPAddress.HostToNetworkOrder((short) that);
            return (ushort) retval;
        }

        /// <summary>
        /// Converts <paramref name="that"/> to host byte order.
        /// </summary>
        /// <param name="that">The data in network byte order to be
        /// converted to network byte order.</param>
        /// <returns><paramref name="that"/> in host byte order.</returns>
        public static int ToHostByteOrder(this int that) {
            return IPAddress.HostToNetworkOrder(that);
        }

        /// <summary>
        /// Converts <paramref name="that"/> to host byte order.
        /// </summary>
        /// <param name="that">The data in network byte order to be
        /// converted to network byte order.</param>
        /// <returns><paramref name="that"/> in host byte order.</returns>
        public static uint ToHostByteOrder(this uint that) {
            var retval = IPAddress.HostToNetworkOrder((int) that);
            return (uint) retval;
        }

        /// <summary>
        /// Converts <paramref name="that"/> to host byte order.
        /// </summary>
        /// <param name="that">The data in network byte order to be
        /// converted to network byte order.</param>
        /// <returns><paramref name="that"/> in host byte order.</returns>
        public static long ToHostByteOrder(this long that) {
            return IPAddress.HostToNetworkOrder(that);
        }

        /// <summary>
        /// Converts <paramref name="that"/> to host byte order.
        /// </summary>
        /// <param name="that">The data in network byte order to be
        /// converted to network byte order.</param>
        /// <returns><paramref name="that"/> in host byte order.</returns>
        public static ulong ToHostByteOrder(this ulong that) {
            var retval = IPAddress.HostToNetworkOrder((long) that);
            return (ulong) retval;
        }
        #endregion

        #region ToNetworkByteOrder
        /// <summary>
        /// Converts <paramref name="that"/> to network byte order.
        /// </summary>
        /// <param name="that">The data in host byte order to be
        /// converted to network byte order.</param>
        /// <returns><paramref name="that"/> in network byte order.
        /// </returns>
        public static byte NetworkToHostOrder(this byte that) {
            return that;
        }

        /// <summary>
        /// Converts <paramref name="that"/> to network byte order.
        /// </summary>
        /// <param name="that">The data in host byte order to be
        /// converted to network byte order.</param>
        /// <returns><paramref name="that"/> in network byte order.
        /// </returns>
        public static short ToNetworkByteOrder(this short that) {
            return IPAddress.NetworkToHostOrder(that);
        }

        /// <summary>
        /// Converts <paramref name="that"/> to network byte order.
        /// </summary>
        /// <param name="that">The data in host byte order to be
        /// converted to network byte order.</param>
        /// <returns><paramref name="that"/> in network byte order.
        /// </returns>
        public static ushort ToNetworkByteOrder(this ushort that) {
            var retval = IPAddress.NetworkToHostOrder((short) that);
            return (ushort) retval;
        }

        /// <summary>
        /// Converts <paramref name="that"/> to network byte order.
        /// </summary>
        /// <param name="that">The data in host byte order to be
        /// converted to network byte order.</param>
        /// <returns><paramref name="that"/> in network byte order.
        /// </returns>
        public static int ToNetworkByteOrder(this int that) {
            return IPAddress.NetworkToHostOrder(that);
        }

        /// <summary>
        /// Converts <paramref name="that"/> to network byte order.
        /// </summary>
        /// <param name="that">The data in host byte order to be
        /// converted to network byte order.</param>
        /// <returns><paramref name="that"/> in network byte order.
        /// </returns>
        public static uint ToNetworkByteOrder(this uint that) {
            var retval = IPAddress.NetworkToHostOrder((int) that);
            return (uint) retval;
        }

        /// <summary>
        /// Converts <paramref name="that"/> to network byte order.
        /// </summary>
        /// <param name="that">The data in host byte order to be
        /// converted to network byte order.</param>
        /// <returns><paramref name="that"/> in network byte order.
        /// </returns>
        public static long ToNetworkByteOrder(this long that) {
            return IPAddress.NetworkToHostOrder(that);
        }

        /// <summary>
        /// Converts <paramref name="that"/> to network byte order.
        /// </summary>
        /// <param name="that">The data in host byte order to be
        /// converted to network byte order.</param>
        /// <returns><paramref name="that"/> in network byte order.
        /// </returns>
        public static ulong ToNetworkByteOrder(this ulong that) {
            var retval = IPAddress.NetworkToHostOrder((long) that);
            return (ulong) retval;
        }
        #endregion
    }
}
