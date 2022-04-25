// <copyright file="SetIDs.cs" company="Universität Stuttgart">
// Copyright © 2020 SAPPAN Consortium. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>


namespace Sappan.Netflow.Ipfix {

    /// <summary>
    /// Provides constants for well-known IPFIX set IDs.
    /// </summary>
    public static class SetIDs {

        /// <summary>
        /// Answer whether the specified ID is from the range of IDs reserved
        /// for data sets.
        /// </summary>
        /// <param name="id">The ID to be tested.</param>
        /// <returns><c>true</c> if <paramref name="id"/> is a data set ID,
        /// <c>false</c> otherwise.</returns>
        public static bool IsDataSet(ushort id) {
            return (id >= 256);
        }

        /// <summary>
        /// IPIX option templates.
        /// </summary>
        public const ushort OptionsTemplateSet = 3;

        /// <summary>
        /// IPFIX data templates.
        /// </summary>
        public const ushort TemplateSet = 2;

    }
}
