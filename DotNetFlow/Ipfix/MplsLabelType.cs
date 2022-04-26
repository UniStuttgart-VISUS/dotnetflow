// <copyright file="MplsLabelType.cs" company="Universität Stuttgart">
// Copyright © 2020 - 2022 Institut für Visualisierung und Interaktive Systeme. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>


namespace DotNetFlow.Ipfix {

    /// <summary>
    /// Possible MPLS label types used for the
    /// <see cref="InformationElement.MplsTopLabelType"/> information element
    /// as defined on https://www.iana.org/assignments/ipfix/ipfix.xhtml.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design",
        "CA1028:Enum Storage should be Int32",
        Justification = "Underlying type must match the protocol size.")]
    public enum MplsLabelType : byte {

        /// <summary>
        /// The MPLS label type is not known.
        /// </summary>
        Unknown = 0x00,

        /// <summary>
        /// Any TE tunnel mid-point or tail label
        /// </summary>
        TeMidpt = 0x01,

        /// <summary>
        /// Any PWE3 or Cisco AToM based label.
        /// </summary>
        Pseudowire = 0x02,

        /// <summary>
        /// Any label associated with VPN.
        /// </summary>
        Vpn = 0x03,

        /// <summary>
        ///  Any label associated with BGP or BGP routing.
        /// </summary>
        Bgp = 0x04,

        /// <summary>
        /// Any label associated with dynamically assigned labels using LDP.
        /// </summary>
        Ldp = 0x05
    }
}
