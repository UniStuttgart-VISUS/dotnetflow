// <copyright file="ISet.cs" company="Universität Stuttgart">
// Copyright © 2020 - 2022 Institut für Visualisierung und Interaktive Systeme. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>


namespace DotNetFlow.Ipfix {

    /// <summary>
    /// Shared interface of all sets.
    /// </summary>
    public interface ISet {

        /// <summary>
        /// Gets or sets the ID of the set.
        /// </summary>
        ushort ID {
            get;
            set;
        }

        /// <summary>
        /// Gets the total length of the set in bytes.
        /// </summary>
        /// <remarks>
        /// <para>Total length of the set, in octets, including the set header,
        /// all records and the optional padding. Because an individual set MAY
        /// contain multiple records, the length value MUST be used to determine
        /// the position of the next set.</para>
        /// </remarks>
        ushort Length {
            get;
        }
    }
}
