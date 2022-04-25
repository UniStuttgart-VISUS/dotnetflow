// <copyright file="PacketHeader.cs" company="Universität Stuttgart">
// Copyright © 2020 SAPPAN Consortium. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.Runtime.InteropServices;


namespace Sappan.Netflow.Netflow9 {

    /// <summary>
    /// The definition of a NetFlow 9 packet header.
    /// </summary>
    /// <remarks>
    /// <para>The format of the NetFlow version 9 packet header remains
    /// relatively unchanged from previous versions. It is based on the NetFlow
    /// version 5 packet header.</para>
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
        /// <param name="count">The number of flow sets in the packet.</param>
        /// <param name="sequenceNumber">The sequence number of the packet.
        /// </param>
        /// <param name="sourceID">The source ID of the packet.</param>
        public PacketHeader(ushort count, ushort sequenceNumber,
                ushort sourceID) {
            this.Version = 9;
            this.Count = count;
            this.SystemUptime = (uint) (Environment.TickCount / 1000);
            this.UnixSeconds = (uint) DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            this.SequenceNumber = sequenceNumber;
            this.SourceID = sourceID;
        }
        #endregion

        #region Public properties
        /// <summary>
        /// Gets or sets the number of FlowSet records (both template and data)
        /// contained within this packet.
        /// </summary>
        [OnWireOrder(1)]
        public ushort Count { get; set; }

        /// <summary>
        /// Gets or sets the incremental sequence counter of all export packets
        /// sent by a single device.
        /// </summary>
        /// <remarks>
        /// <para>This value is cumulative, and it can be used to identify
        /// whether any export packets have been missed.</para>
        /// <para>Note: This is a change from the NetFlow version 5 and version
        /// 8 headers, where this number represented &quot;total flows&quot;.</para>
        /// </remarks>
        [OnWireOrder(4)]
        public uint SequenceNumber { get; set; }

        /// <summary>
        /// Gets or sets the source ID.
        /// </summary>
        /// <remarks>
        /// <para>The <see cref="SourceID"/> property is a 32-bit value that is
        /// used to guarantee uniqueness for all flows exported from a
        /// particular device. (The <see cref="SourceID"/> is the equivalent of
        /// the engine type and engine ID fields found in the NetFlow version 5
        /// and version 8 headers). The format of this field is vendor-specific.
        /// </para>
        /// <para>In the Cisco implementation, the first two bytes are reserved
        /// for future expansion, and will always be zero. Byte 3 provides
        /// uniqueness with respect to the routing engine on the exporting
        /// device. Byte 4 provides uniqueness with respect to the particular
        /// line card or Versatile Interface Processor on the exporting device.
        /// Collector devices should use the combination of the source IP
        /// address plus the Source ID field to associate an incoming
        /// NetFlow export packet with a unique instance of NetFlow on a
        /// particular device.</para>
        /// </remarks>
        [OnWireOrder(5)]
        public uint SourceID { get; set; }

        /// <summary>
        /// Gets or sets the time in milliseconds since the device was first
        /// booted.
        /// </summary>
        [OnWireOrder(2)]
        public uint SystemUptime { get; set; }

        /// <summary>
        /// Gets or sets the seconds since 0000 Coordinated Universal Time (UTC)
        /// 1970.
        /// </summary>
        [OnWireOrder(3)]
        public uint UnixSeconds { get; set; }

        /// <summary>
        /// Gets or sets the version of the NetFlow records exported in this
        /// packet.
        /// </summary>
        /// <remarks>
        /// For NetFlow version 9, this property is 0x0009.
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
                && (this.Count == rhs.Count)
                && (this.SequenceNumber == rhs.SequenceNumber)
                && (this.SourceID == rhs.SourceID)
                && (this.SystemUptime == rhs.SystemUptime)
                && (this.UnixSeconds == rhs.UnixSeconds)
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
            retval = combine((uint) this.Count << 2 | this.Version);
            retval = combine(this.SequenceNumber);
            retval = combine(this.SourceID);
            retval = combine(this.SystemUptime);
            retval = combine(this.UnixSeconds);
            return (int) retval;
        }
        #endregion
    }
}
