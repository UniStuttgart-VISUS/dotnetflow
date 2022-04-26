// <copyright file="NatQuotaExceededEventType.cs" company="Universität Stuttgart">
// Copyright © 2020 - 2022 Institut für Visualisierung und Interaktive Systeme. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>


namespace DotNetFlow.Ipfix {

    /// <summary>
    /// Possible NAT quota event types as defined on
    /// https://www.iana.org/assignments/ipfix/ipfix.xhtml.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design",
        "CA1028:Enum Storage should be Int32",
        Justification = "Underlying type must match the protocol size.")]
    public enum NatQuotaExceededEventType : uint {

        //Reserved = 0

        /// <summary>
        /// Maximum session entries.
        /// </summary>
        SessionEntries = 1,

        /// <summary>
        /// Maximum BIB entries.
        /// </summary>
        BibEntries = 2,

        /// <summary>
        /// Maximum entries per user.
        /// </summary>
        EntriesPerUser = 3,

        /// <summary>
        /// Maximum active hosts or subscribers.
        /// </summary>
        ActiveHosts = 4,

        /// <summary>
        /// Maximum fragments pending reassembly.
        /// </summary>
        FragmentsPendingReassembly = 5

    }
}
