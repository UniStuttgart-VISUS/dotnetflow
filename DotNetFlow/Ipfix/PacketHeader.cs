// <copyright file="PacketHeader.cs" company="Universität Stuttgart">
// Copyright © 2020 - 2022 Institut für Visualisierung und Interaktive Systeme. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using System;


namespace DotNetFlow.Ipfix {

    /// <summary>
    /// The definition of an IPFIX packet header.
    /// </summary>
    /// <remarks>
    /// <para>For the implementation of the serialiser, we cannot use
    /// <see cref="LayoutKind.Sequential"/> and convert it directly, because the
    /// on-wire representation is in network byte order, wherefore all fields
    /// need to be swapped separately. The <see cref="OnWireOrderAttribute"/>s
    /// define the order of the fields in the on-wire representation.</para>
    /// </remarks>
    public class PacketHeader : IEquatable<PacketHeader> {

        #region Public constructors
        /// <summary>
        /// Initialises a new instance.
        /// </summary>
        public PacketHeader() : this(0, 0, 0) { }

        /// <summary>
        /// Initialises a new instance.
        /// </summary>
        /// <param name="length">The length of the total message in bytes.
        /// </param>
        /// <param name="sequenceNumber">The sequence number of the packet.
        /// </param>
        /// <param name="observationDomainID">The observation domain the packet
        /// is for.</param>
        public PacketHeader(ushort length, ushort sequenceNumber,
                ushort observationDomainID) {
            this.Version = 0xa;
            this.Length = length;
            this.ExportTime = (uint) DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            this.SequenceNumber = sequenceNumber;
            this.ObservationDomainID = observationDomainID;
        }
        #endregion

        #region Public properties
        /// <summary>
        /// Gets or sets the time at which the IPFIX message header leaves the
        /// exporter, expressed in seconds since the UNIX epoch of
        /// 1st January 1970 at 00:00 UTC, encoded as an unsigned 32-bit
        /// integer.
        /// </summary>
        [OnWireOrder(2)]
        public uint ExportTime { get; set; }

        /// <summary>
        /// Gets or sets the total length of the IPFIX message, measured in
        /// octets, including message header and set(s).
        /// </summary>
        [OnWireOrder(1)]
        public ushort Length { get; set; }

        /// <summary>
        /// Gets or sets the 32-bit identifier of the observation domain that is
        /// locally unique to the exporting process.
        /// </summary>
        /// <remarks>
        /// <para>The exporting process uses the observation domain ID to
        /// uniquely identify to the collecting process the observation domain
        /// that metered the flows. It is RECOMMENDED that this identifier also
        /// be unique per IPFIX device. Collecting processes SHOULD use the
        /// transport session and the observation domain ID field to separate
        /// different export streams originating from the same exporter.The
        /// observation domain ID SHOULD be 0 when no specific observation
        /// domain ID is relevant for the entire IPFIX message, for example when
        /// exporting the exporting process statistics, or in the case of a
        /// hierarchy of collectors when aggregated data records are exported.
        /// </para>
        /// </remarks>
        [OnWireOrder(4)]
        public uint ObservationDomainID { get; set; }

        /// <summary>
        /// Gets or sets the incremental sequence counter modulo 2^32 of all
        /// IPFIX data records sent in the current stream from the current
        /// observation domain by the exporting process.
        /// </summary>
        /// <remarks>
        /// <para>Each SCTP stream counts sequence numbers separately, while
        /// all messages in a TCP connection or UDP session are considered to
        /// be part of the same stream. This value can beused by the collecting
        /// process to identify whether any IPFIX data records have been missed.
        /// Template and options template records do not increase the sequence
        /// number.</para>
        /// </remarks>
        [OnWireOrder(3)]
        public uint SequenceNumber { get; set; }

        /// <summary>
        /// Gets or sets the version of the NetFlow records exported in this
        /// packet.
        /// </summary>
        /// <remarks>
        /// For IPFIX, this property is 0x000a.
        /// </remarks>
        [OnWireOrder(0)]
        public ushort Version { get; set; }
        #endregion

        #region Public methods
        /// <summary>
        /// Test for equality.
        /// </summary>
        /// <param name="rhs">The object to be compared.</param>
        /// <returns><c>true</c> if this object an <paramref name="rhs"/> are
        /// equal.</returns>
        public bool Equals(PacketHeader rhs) {
            return object.ReferenceEquals(this, rhs) || ((rhs != null)
                && (this.ExportTime == rhs.ExportTime)
                && (this.Length == rhs.Length)
                && (this.ObservationDomainID == rhs.ObservationDomainID)
                && (this.SequenceNumber == rhs.SequenceNumber)
                && (this.Version == rhs.Version));
        }

        /// <inheritdoc />
        public override bool Equals(object rhs) {
            return this.Equals(rhs as PacketHeader);
        }

        /// <inheritdoc />
        public override int GetHashCode() {
            var retval = 0u;
            Func<uint, uint> combine = (n) => ((retval << 5) + retval) ^ n;
            retval = combine((uint) this.Length << 2 | this.Version);
            retval = combine(this.SequenceNumber);
            retval = combine(this.ObservationDomainID);
            retval = combine(this.ExportTime);
            return (int) retval;
        }
        #endregion
    }

}
