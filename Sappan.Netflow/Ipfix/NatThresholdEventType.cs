// <copyright file="NatThresholdEventType.cs" company="Universität Stuttgart">
// Copyright © 2020 SAPPAN Consortium. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>


namespace Sappan.Netflow.Ipfix {

    /// <summary>
    /// Possible values for <see cref="InformationElement.NatThresholdEvent"/>.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design",
        "CA1028:Enum Storage should be Int32",
        Justification = "Underlying type must match the protocol size.")]
    public enum NatThresholdEventType : uint {

        //Reserved = 0,

        /// <summary>
        /// Address pool high threshold event
        /// </summary>
        AddressPoolHigh = 1,

        /// <summary>
        /// Address pool low threshold event
        /// </summary>
        AddressPoolLow = 2,

        /// <summary>
        /// Address and port mapping high threshold event
        /// </summary>
        AddressMappingHigh = 3,

        /// <summary>
        /// Address and port mapping per user high threshold event
        /// </summary>
        AddressMappingLow = 4,

        /// <summary>
        /// Global Address mapping high threshold event
        /// </summary>
        GlobalAddressMappingHigh = 5

    }
}
