// <copyright file="ClassificationEngineID.cs" company="Universität Stuttgart">
// Copyright © 2020 SAPPAN Consortium. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>


namespace Sappan.Netflow.Ipfix {

    /// <summary>
    /// Possible classification engines as defined on
    /// https://www.iana.org/assignments/ipfix/ipfix.xhtml.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design",
        "CA1028:Enum Storage should be Int32",
        Justification = "Underlying type must match the protocol size.")]
    public enum ClassificationEngineID : byte {

        /// <summary>
        /// Invalid.
        /// </summary>
        Invalid = 0,

        /// <summary>
        /// The Assigned Internet Protocol Number (layer 3 (L3)) is exported in
        /// the Selector ID.
        /// </summary>
        IanaL3 = 1,

        /// <summary>
        /// Proprietary layer 3 definition. An enterprise can export its own
        /// layer 3 protocol numbers. The Selector ID has a global significance
        /// for all devices from the same enterprise.
        /// </summary>
        PanaL3 = 2,

        /// <summary>
        /// The IANA layer 4 (L4) well-known port number is exported in the
        /// Selector ID.
        /// </summary>
        /// <remarks>
        /// Note: as an IPFIX flow is unidirectional, it contains the
        /// destination port in a flow from the client to the server.
        /// </remarks>
        IanaL4 = 3,

        /// <summary>
        /// Proprietary layer 4 definition. An enterprise can export its own
        /// layer 4 port numbers. The Selector ID has global significance for
        /// devices from the same enterprise.
        /// </summary>
        /// <remarks>
        /// Example: IPFIX had the port 4739 pre-assigned in the IETF draft for
        /// years. While waiting for the RFC and its associated IANA
        /// registration, the Selector ID 4739 was used with this PANA-L4.
        /// </remarks>
        PanaL4 = 4,

        // Reserved = 5

        /// <summary>
        /// The Selector ID represents applications defined by the user (using
        /// CLI, GUI, etc.) based on the methods described in section 2. The
        /// Selector ID has a local significance per device.
        /// </summary>
        UserDefined = 6,

        // Reserved = 7
        // Reserved = 8
        // Reserved = 9
        // Reserved = 10
        // Reserved = 11

        /// <summary>
        /// Proprietary layer 2 (L2) definition. An enterprise can export its
        /// own layer 2 identifiers. The Selector ID represents the enterprise's
        /// unique global layer 2 applications. The Selector ID has a global
        /// significance for all devices from the same enterprise. Examples
        /// include Cisco Subnetwork Access Protocol (SNAP).
        /// </summary>
        PanaL2 = 12,

        /// <summary>
        /// Proprietary layer 7 definition. The Selector ID represents the
        /// enterprise's unique global ID for the layer 7 applications. The
        /// Selector ID has a global significance for all devices from the same
        /// enterprise. This Classification Engine Id is used when the
        /// application registry is owned by the Exporter manufacturer (referred
        /// to as the "enterprise" in this document).
        /// </summary>
        PanaL7 = 13,

        // Reserved = 14
        // Reserved = 15
        // Reserved = 16
        // Reserved = 17

        /// <summary>
        /// The Selector ID represents the well- known Ethertype.
        /// </summary>
        /// <remarks>
        /// See http://standards.ieee.org/develop/regauth/ethertype/eth.txt.
        /// Note that the Ethertype is usually expressed in hexadecimal.
        /// However, the corresponding decimal value is used in this Selector
        /// ID.
        /// </remarks>
        Ethertype = 18,

        /// <summary>
        /// The Selector ID represents the well-known IEEE 802.2 Link Layer
        /// Control (LLC) Destination Service Access Point (DSAP).
        /// </summary>
        /// <remarks>
        ///  See http://standards.ieee.org/develop/regauth/llc/public.html. Note
        ///  that LLC DSAP is usually expressed in hexadecimal. However, the
        ///  corresponding decimal value is used in this Selector ID.
        /// </remarks>
        Llc = 19,

        /// <summary>
        /// Proprietary layer 7 definition, including a Private Enterprise
        /// Number (PEN) [IANA registry enterprise-numbers] to identify that
        /// the application registry being used is not owned by the Exporter
        /// manufacturer or to identify the original enterprise in the case of
        /// a mediator or 3rd party device. The Selector ID represents the
        /// enterprise unique global ID for the layer 7 applications. The
        /// Selector ID has a global significance for all devices from the same
        /// enterprise.
        /// </summary>
        PanaL7Pen = 20,

        /// <summary>
        /// The Selector ID contains an application ID from the Qosmos ixEngine.
        /// </summary>
        QosmosIxEngine = 21,

        /// <summary>
        /// The Selector ID contains a protocol from the ntop nDPI engine.
        /// </summary>
        NtopNDpi = 22

    }
}
