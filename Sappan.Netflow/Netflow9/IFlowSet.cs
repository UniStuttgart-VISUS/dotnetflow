// <copyright file="IFlowSet.cs" company="Universität Stuttgart">
// Copyright © 2020 SAPPAN Consortium. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>


namespace Sappan.Netflow.Netflow9 {

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
