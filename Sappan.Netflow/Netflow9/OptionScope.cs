// <copyright file="OptionScope.cs" company="Universität Stuttgart">
// Copyright © 2020 SAPPAN Consortium. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>


namespace Sappan.Netflow.Netflow9 {

    /// <summary>
    /// Possible values for open scopes.
    /// </summary>
    /// <remarks>
    /// This enumeration lists the currently defined scopes by Cisco described
    /// at https://www.cisco.com/en/US/technologies/tk648/tk362/technologies_white_paper09186a00800a3db9.html
    /// </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design",
        "CA1028:Enum Storage should be Int32",
        Justification = "The size of the type must match the NetFlow protocol.")]
    public enum OptionScope : ushort {

        /// <summary>
        /// Represents a whole system.
        /// </summary>
        System = 0x0001,

        /// <summary>
        /// Represents a single interface of the switch.
        /// </summary>
        Interface = 0x0002,

        /// <summary>
        /// Represents a line card of the switch.
        /// </summary>
        LineCard = 0x0003,

        NetFlowCache = 0x0004,

        Template = 0x0005
    }
}
