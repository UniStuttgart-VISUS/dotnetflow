// <copyright file="FlowSelectorAlgorithm.cs" company="Universität Stuttgart">
// Copyright © 2020 - 2022 Institut für Visualisierung und Interaktive Systeme. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>


namespace DotNetFlow.Ipfix {

    /// <summary>
    /// Possible flow selection algorithms as defined on
    /// https://www.iana.org/assignments/ipfix/ipfix.xhtml.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design",
        "CA1028:Enum Storage should be Int32",
        Justification = "Underlying type must match the protocol size.")]
    public enum FlowSelectorAlgorithm : ushort {

        //Unassigned = 0

        /// <summary>
        /// Systematic count-based sampling.
        /// </summary>
        CountBasedSampling = 1,

        /// <summary>
        /// Systematic time-based sampling.
        /// </summary>
        TimeBasedSampling = 2,

        /// <summary>
        /// Random n-out-of-N sampling.
        /// </summary>
        RandomSampling  = 3,

        /// <summary>
        /// Uniform probabilistic sampling.
        /// </summary>
        ProbabilisticSampling = 4,

        /// <summary>
        /// Property Match Filtering.
        /// </summary>
        PropertyMatchFiltering = 5,

        /// <summary>
        /// Hash-based Filtering using BOB.
        /// </summary>
        HashFilteringBob = 6,

        /// <summary>
        /// Hash-based Filtering using IPSX.
        /// </summary>
        HashFilteringIpsx = 7,

        /// <summary>
        /// Hash-based Filtering using CRC.
        /// </summary>
        HasFilteringCrc = 8,

        /// <summary>
        /// Flow-state Dependent Intermediate Flow Selection Process.
        /// </summary>
        StateDependentSelection = 9

    }
}
