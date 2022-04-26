// <copyright file="NatEventType.cs" company="Universität Stuttgart">
// Copyright © 2020 - 2022 Institut für Visualisierung und Interaktive Systeme. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>


namespace DotNetFlow.Ipfix {

    /// <summary>
    /// Possible NAT events types as defined on
    /// https://www.iana.org/assignments/ipfix/ipfix.xhtml.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design",
        "CA1028:Enum Storage should be Int32",
        Justification = "Underlying type must match the protocol size.")]
    public enum NatEventType : byte {

        //Reserved = 0

        /// <summary>
        /// NAT translation create (Historic)
        /// </summary>
        NatTranslationCreate = 1,

        /// <summary>
        /// NAT translation delete (Historic)
        /// </summary>
        NatTranslationDelete = 2,

        /// <summary>
        /// NAT Addresses exhausted
        /// </summary>
        NatAddressesExhausted = 3,

        /// <summary>
        /// NAT44 session create
        /// </summary>
        Nat44SessionCreate = 4,

        /// <summary>
        /// NAT44 session delete
        /// </summary>
        Nat44SessionDelete = 5,

        /// <summary>
        /// NAT64 session create
        /// </summary>
        Nat64SessionCreate = 6,

        /// <summary>
        /// NAT64 session delete
        /// </summary>
        Nat64SessionDelete = 7,

        /// <summary>
        /// NAT44 BIB create
        /// </summary>
        Nat44BibCreate = 8,

        /// <summary>
        /// NAT44 BIB delete
        /// </summary>
        Nat44BibDelete = 9,

        /// <summary>
        /// NAT64 BIB create
        /// </summary>
        Nat64BibCreate = 10,

        /// <summary>
        /// NAT64 BIB delete
        /// </summary>
        Nat64BibDelete = 11,

        /// <summary>
        /// NAT ports exhausted
        /// </summary>
        NatPortsExhausted = 12,

        /// <summary>
        /// Quota Exceeded
        /// </summary>
        QuotaExceeded = 13,

        /// <summary>
        /// Address binding create
        /// </summary>
        AddressBindingCreate = 14,

        /// <summary>
        /// Address binding delete
        /// </summary>
        AddressBindingDelete = 15,

        /// <summary>
        /// Port block allocation
        /// </summary>
        PortBlockAllocation = 16,

        /// <summary>
        /// Port block de-allocation
        /// </summary>
        PortBlockDeallocation = 17,

        /// <summary>
        /// Threshold Reached
        /// </summary>
        ThresholdReached = 18

    }
}
