// <copyright file="IFlowSet.cs" company="Universität Stuttgart">
// Copyright © 2020 - 2022 Institut für Visualisierung und Interaktive Systeme. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>


namespace DotNetFlow.Netflow9 {

    /// <summary>
    /// Shared interface of template flow sets and data flow sets.
    /// </summary>
    public interface IFlowSet {

        /// <summary>
        /// Gets or sets the ID of the flow set.
        /// </summary>
        ushort ID {
            get;
            set;
        }

        /// <summary>
        /// Gets the length of the flow set in bytes.
        /// </summary>
        ushort Length {
            get;
        }
    }
}
