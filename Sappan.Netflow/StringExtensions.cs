// <copyright file="StringExtensions.cs" company="Universität Stuttgart">
// Copyright © 2020 SAPPAN Consortium. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using Sappan.Netflow.Netflow9;
using System;
using System.Linq;


namespace Sappan.Netflow {

    /// <summary>
    /// Provides extensions for <see cref="String"/> that facilitate handling of
    /// strings as elements of flow sets.
    /// </summary>
    public static class StringExtensions {

        /// <summary>
        /// Gets a string that is padded or truncated to fit into the specified
        /// length of a NetFlow field.
        /// </summary>
        /// <remarks>
        /// <para>Note that the length is specified in bytes and we assume
        /// ASCII-encoded strings. Unexpected results might be returned when
        /// using characters outside the set of valid ASCII characters.</para>
        /// </remarks>
        /// <param name="that">The string to be adjusted.</param>
        /// <param name="fieldLength">The expected length of the NetFlow field
        /// in bytes.</param>
        /// <param name="paddingChar">The padding character that is used to
        /// fill the the field if the string is too short.</param>
        /// <returns>A string that will have exactly
        /// <paramref name="fieldLength"/> bytes in its on-wire representation.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <paramref name="that"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If
        /// <paramref name="fieldLength"/> is less than 1.</exception>
        public static byte[] AdjustToNetFlow(this string that,
                int fieldLength, byte paddingChar = 0) {
            if (that == null) {
                throw new ArgumentNullException(nameof(that));
            }
            if (fieldLength <= 0) {
                throw new ArgumentException(
                    Properties.Resources.ErrorNegativeNetFlowFieldLength,
                    nameof(fieldLength));
            }

            var retval = that.ToWire();

            if (retval.Length > fieldLength) {
                return retval.Take(fieldLength).ToArray();

            } else if (retval.Length < fieldLength) {
                var diff = fieldLength - retval.Length;
                return retval.Concat(Enumerable.Repeat(paddingChar, diff))
                    .ToArray();

            } else {
                return retval;
            }
        }

        /// <summary>
        /// Gets a string that is padded or truncated to fit into the specified
        /// length of a NetFlow field.
        /// </summary>
        /// <remarks>
        /// <param name="that">The string to be adjusted.</param>
        /// <param name="field">The field the string should be stored in.
        /// </param>
        /// <param name="paddingChar">The padding character that is used to
        /// fill the the field if the string is too short.</param>
        /// <returns>A string that will have exactly
        /// <paramref name="fieldLength"/> bytes in its on-wire representation.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <paramref name="that"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="field"/>
        /// is <c>null</c>.</exception>
        public static byte[] AdjustToNetFlow(this string that,
                Field field, byte paddingChar = 0) {
            if (field == null) {
                throw new ArgumentNullException(nameof(field));
            }
            return that.AdjustToNetFlow(field.Length, paddingChar);
        }
    }
}
