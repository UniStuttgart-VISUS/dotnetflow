// <copyright file="FlowRecord.cs" company="Universität Stuttgart">
// Copyright © 2020 - 2022 Institut für Visualisierung und Interaktive Systeme. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.Net;
using System.Net.Sockets;


namespace DotNetFlow.Netflow5 {

    /// <summary>
    /// Representation of a Netflow v5 flow record.
    /// </summary>
    /// <remarks>
    /// Cf. https://www.cisco.com/c/en/us/td/docs/net_mgmt/netflow_collection_engine/3-6/user/guide/format.html
    /// </remarks>
    [OnWirePadding(32)]
    public sealed class FlowRecord {

        #region Public constructors
        /// <summary>
        /// Initialises a new intance.
        /// </summary>
        public FlowRecord() {
            this._destinationAddress = IPAddress.Any;
            this._nextHop = IPAddress.Any;
            this._sourceAddress = IPAddress.Any;
        }
        #endregion

        #region Public properties
        /// <summary>
        /// Gets or sets the autonomous system number of the destination, either
        /// origin or peer.
        /// </summary>
        [OnWireOrder(16)]
        public ushort DestinationAutonomousSystem { get; set; }

        /// <summary>
        /// Gets or sets the destination IP address.
        /// </summary>
        [OnWireOrder(1)]
        public IPAddress DestinationAddress {
            get => this._destinationAddress;
            set {
                CheckIPAddress(value);
                this._destinationAddress = value;
            }
        }

        /// <summary>
        /// Gets or sets the destination address prefix mask bits.
        /// </summary>
        [OnWireOrder(18)]
        public byte DestinationMask { get; set; }

        /// <summary>
        /// Gets or sets the TCP/UDP destination port number of equivalent.
        /// </summary>
        [OnWireOrder(10)]
        public ushort DestinationPort { get; set; }

        /// <summary>
        /// Gets or sets the system uptime (measured in milliseconds) at time
        /// when the last packet of the flow was received.
        /// </summary>
        [OnWireOrder(8)]
        public uint End { get; set; }

        /// <summary>
        /// Gets or sets the SNMP index of the input interface.
        /// </summary>
        [OnWireOrder(3)]
        public ushort InputInterface { get; set; }

        /// <summary>
        /// Gets or sets the IP address of the next hop router.
        /// </summary>
        [OnWireOrder(2)]
        public IPAddress NextHop {
            get => this._nextHop;
            set {
                CheckIPAddress(value);
                this._nextHop = value;
            }
        }

        /// <summary>
        /// Gets or sets the total number of layer 3 bytes in the packets of
        /// the flow.
        /// </summary>
        [OnWireOrder(6)]
        public uint Octets { get; set; }

        /// <summary>
        /// Gets or sets the SNMP index of the output interface.
        /// </summary>
        [OnWireOrder(4)]
        public ushort OutputInterface { get; set; }

        /// <summary>
        /// Gets or sets the number of packets in the flwo.
        /// </summary>
        [OnWireOrder(5)]
        public uint Packets { get; set; }

        /// <summary>
        /// Gets the first padding bytes after the port numbers.
        /// </summary>
        [OnWireOrder(11)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance",
            "CA1822:Mark members as static",
            Justification = "Reflection producing on-wire representation does not consider static.")]
        public byte Padding1 => 0;

        /// <summary>
        /// Gets the first the protocol type (<see cref="ProtocolType" />).
        /// </summary>
        [OnWireOrder(13)]
        public byte Protocol { get; set; }

        /// <summary>
        /// Gets or sets the autonomous system number of the source, either
        /// origin or peer.
        /// </summary>
        [OnWireOrder(15)]
        public ushort SourceAutonomousSystem { get; set; }

        /// <summary>
        /// Gets or sets the source IP address.
        /// </summary>
        [OnWireOrder(0)]
        public IPAddress SourceAddress {
            get => this._sourceAddress;
            set {
                CheckIPAddress(value);
                this._sourceAddress = value;
            }
        }

        /// <summary>
        /// Gets or sets the source address prefix mask bits.
        /// </summary>
        [OnWireOrder(17)]
        public byte SourceMask { get; set; }

        /// <summary>
        /// Gets or sets the TCP/UDP source port number of equivalent.
        /// </summary>
        [OnWireOrder(9)]
        public ushort SourcePort { get; set; }

        /// <summary>
        /// Gets or sets the system uptime (measured in milliseconds) at the 
        /// start of the flow.
        /// </summary>
        [OnWireOrder(7)]
        public uint Start { get; set; }

        /// <summary>
        /// Gets the first the cumulative TCP flags.
        /// </summary>
        [OnWireOrder(12)]
        public byte TcpFlags { get; set; }

        /// <summary>
        /// Gets the first the IP type of service (ToS).
        /// </summary>
        [OnWireOrder(14)]
        public byte TypeOfService { get; set; }
        #endregion

        #region Private class methods
        /// <summary>
        /// Checks whether <paramref name="value"/> is a valid IPv4.
        /// </summary>
        /// <param name="value"></param>
        private static void CheckIPAddress(IPAddress value) {
            if (value == null) {
                throw new ArgumentNullException(nameof(value));
            }
            if (value.AddressFamily != AddressFamily.InterNetwork) {
                throw new ArgumentException(
                    Properties.Resources.ErrorNetflow5OnlyIPv4);
            }
        }
        #endregion

        #region Private fields
        private IPAddress _destinationAddress;
        private IPAddress _nextHop;
        private IPAddress _sourceAddress;
        #endregion
    }
}
