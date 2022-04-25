// <copyright file="ArrayExtensions.cs" company="Universität Stuttgart">
// Copyright © 2020 SAPPAN Consortium. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using System;


namespace Sappan.Netflow {

    /// <summary>
    /// Extension methods for arrays.
    /// </summary>
    internal static class ArrayExtensions {

        /// <summary>
        /// Extract <paramref name="length"/> elements from
        /// <paramref name="that"/>, starting at <paramref name="offset"/>.
        /// </summary>
        /// <typeparam name="TElement">The type of the array elements.
        /// </typeparam>
        /// <param name="that">The array to extract the elements from.</param>
        /// <param name="offset">The offset of the first element.</param>
        /// <param name="length">The number of elements to be copied.</param>
        /// <returns>The subarray with the specified bounds.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="that"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If
        /// <paramref name="offset"/> is outside the range of
        /// <paramref name="that"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If
        /// <paramref name="length"/> is negative.</exception>
        /// <exception cref="ArgumentException">If <paramref name="that"/>
        /// has too few elements.</exception>
        public static TElement[] Extract<TElement>(this TElement[] that,
                int offset, int length) {
            var retval = new TElement[length];
            Array.Copy(that, offset, retval, 0, length);
            return retval;
        }
    }
}
