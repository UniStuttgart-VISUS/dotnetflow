// <copyright file="PacketHeader.cs" company="Universität Stuttgart">
// Copyright © 2020 SAPPAN Consortium. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.Diagnostics;

namespace Sappan.Netflow.Netflow5 {

    /// <summary>
    /// The definition of a NetFlow 4 packet header.
    /// </summary>
    public class PacketHeader : IEquatable<PacketHeader> {

        #region Public constructors
        /// <summary>
        /// Initialises a new instance.
        /// </summary>
        public PacketHeader() : this(0, 0) { }

        /// <summary>
        /// Initialises a new instance.
        /// </summary>
        /// <param name="count">The number of flow sets in the packet.</param>
        /// <param name="sequenceNumber">The sequence number of the packet.
        /// </param>
        public PacketHeader(ushort count, ushort sequenceNumber) {
            this.Version = 5;
            this.Count = count;
            this.SystemUptime = (uint) (Environment.TickCount / 1000);

            {
                var now = DateTimeOffset.UtcNow;
                var seconds = now.ToUnixTimeSeconds();
                var rem = now - DateTimeOffset.FromUnixTimeSeconds(seconds);
                Debug.Assert(rem < TimeSpan.FromSeconds(1.0));
                this.UnixSeconds = (uint) seconds;
                this.UnixNanoseconds = (uint) (rem.TotalMilliseconds * 1e6);
            }

            this.SequenceNumber = sequenceNumber;
            this.EngineType = 0;
            this.EngineID = 0;
            this.SamplingInterval = 0;
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
        /// Gets or sets the slot number of the flow-switching engine.
        /// </summary>
        [OnWireOrder(7)]
        public byte EngineID { get; set; }

        /// <summary>
        /// Gets or sets the type of the flow-switching engine.
        /// </summary>
        [OnWireOrder(6)]
        public byte EngineType { get; set; }

        /// <summary>
        /// Gets or sets the sampling mode and the sampling interval.
        /// </summary>
        /// <remarks>
        /// First two bits hold the sampling mode; remaining 14 bits hold value
        /// of sampling interval.
        /// </remarks>
        [OnWireOrder(8)]
        public ushort SamplingInterval { get; set; }

        /// <summary>
        /// Gets or sets the sequence counter of total flows seen.
        /// </summary>
        [OnWireOrder(5)]
        public uint SequenceNumber { get; set; }

        /// <summary>
        /// Gets or sets the time in milliseconds since the device was first
        /// booted.
        /// </summary>
        [OnWireOrder(2)]
        public uint SystemUptime { get; set; }

        /// <summary>
        /// Gets the residual nanoseconds for <see cref="UnixSeconds"/>.
        /// </summary>
        [OnWireOrder(4)]
        public uint UnixNanoseconds { get; set; }

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
                && (this.EngineID == rhs.EngineID)
                && (this.EngineType == rhs.EngineType)
                && (this.SamplingInterval== rhs.SamplingInterval)
                && (this.SequenceNumber == rhs.SequenceNumber)
                && (this.SystemUptime == rhs.SystemUptime)
                && (this.UnixSeconds == rhs.UnixSeconds)
                && (this.UnixNanoseconds == rhs.UnixNanoseconds)
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
            retval = combine((uint) this.EngineID << 2 | this.EngineType);
            retval = combine(this.SamplingInterval);
            retval = combine(this.SequenceNumber);
            retval = combine(this.SystemUptime);
            retval = combine(this.UnixSeconds);
            retval = combine(this.UnixNanoseconds);
            return (int) retval;
        }
        #endregion
    }
}
