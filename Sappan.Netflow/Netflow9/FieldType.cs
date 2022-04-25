// <copyright file="FieldType.cs" company="Universität Stuttgart">
// Copyright © 2020 SAPPAN Consortium. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>


using System.Net;

namespace Sappan.Netflow.Netflow9 {

    /// <summary>
    /// NetFlow v9 field types.
    /// </summary>
    /// <remarks>
    /// The constants are described on https://www.cisco.com/en/US/technologies/tk648/tk362/technologies_white_paper09186a00800a3db9.html
    /// </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design",
        "CA1028:Enum Storage should be Int32",
        Justification = "The size must match the one used in the NetFlow protocol.")]
    public enum FieldType : ushort {

        /// <summary>
        /// Incoming counter with length N x 8 bits for number of bytes
        /// associated with an IP Flow.
        /// </summary>
        [FieldLength(Default = 4)]
        [ClrType(typeof(long))]
        [ClrType(typeof(int))]
        [ClrType(typeof(short))]
        IncomingBytes = 1,

        /// <summary>
        /// Incoming counter with length N x 8 bits for the number of packets
        /// associated with an IP Flow.
        /// </summary>
        [FieldLength(Default = 4)]
        [ClrType(typeof(long))]
        [ClrType(typeof(int))]
        [ClrType(typeof(short))]
        IncomingPackets = 2,

        /// <summary>
        /// Number of flows that were aggregated.
        /// </summary>
        [FieldLength]
        [ClrType(typeof(long))]
        [ClrType(typeof(int))]
        [ClrType(typeof(short))]
        Flows = 3,

        /// <summary>
        /// IP protocol.
        /// </summary>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        Protocol = 4,

        /// <summary>
        /// Type of Service byte setting when entering incoming interface.
        /// </summary>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        SourceServiceType = 5,

        /// <summary>
        /// Cumulative of all the TCP flags seen for this flow.
        /// </summary>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        TcpFlags = 6,

        /// <summary>
        /// TCP/UDP source port number, i.e. FTP, Telnet, or equivalent.
        /// </summary>
        [FieldLength(2)]
        [ClrType(typeof(short))]
        Layer4SourcePort = 7,

        /// <summary>
        /// IPv4 source address.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(IPAddress))]
        IPv4SourceAddress = 8,

        /// <summary>
        /// The number of contiguous bits in the source address subnet mask,
        /// i.e. the submask in slash notation.
        /// </summary>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        IPv4SourceMask = 9,

        /// <summary>
        /// Input interface index.
        /// </summary>
        [FieldLength]
        [ClrType(typeof(long))]
        [ClrType(typeof(int))]
        [ClrType(typeof(short))]
        InputSnmp = 10,

        /// <summary>
        /// TCP/UDP destination port number, i.e. FTP, Telnet, or equivalent.
        /// </summary>
        [FieldLength(2)]
        [ClrType(typeof(short))]
        Layer4DestinationPort = 11,

        /// <summary>
        /// IPv4 destination address.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(IPAddress))]
        IPv4DestinationAddress = 12,

        /// <summary>
        /// The number of contiguous bits in the destination address subnet
        /// mask, i.e. the submask in slash notation.
        /// </summary>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        IPv4DestinationMask = 13,

        /// <summary>
        /// Output interface index.
        /// </summary>
        [FieldLength]
        [ClrType(typeof(long))]
        [ClrType(typeof(int))]
        [ClrType(typeof(short))]
        OutputSnmp = 14,

        /// <summary>
        /// IPv4 address of next-hop router.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(IPAddress))]
        IPv4NextHop = 15,

        /// <summary>
        /// Source BGP autonomous system number where the field length could be
        /// 2 or 4.
        /// </summary>
        [FieldLength(Default = 2)]
        [ClrType(typeof(short))]
        SourceAutonomousSystem = 16,

        /// <summary>
        /// Destination BGP autonomous system number where the field length
        /// could be 2 or 4.
        /// </summary>
        [FieldLength(Default = 2)]
        [ClrType(typeof(short))]
        DestinationAutonomousSystem = 17,

        /// <summary>
        /// Next-hop router's IP in the BGP domain.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(int))]
        BgpIPv4NextHop = 18,

        /// <summary>
        /// IP multicast outgoing packet counter with length N x 8 bits for packets
        /// associated with the IP Flow.
        /// </summary>
        [FieldLength(Default = 4)]
        [ClrType(typeof(long))]
        [ClrType(typeof(int))]
        [ClrType(typeof(short))]
        MulticastDestinationPackets = 19,

        /// <summary>
        /// IP multicast outgoing byte counter with length N x 8 bits for bytes
        /// associated with the IP Flow.
        /// </summary>
        [FieldLength(Default = 4)]
        [ClrType(typeof(long))]
        [ClrType(typeof(int))]
        [ClrType(typeof(short))]
        MulticastDestintationBytes = 20,

        /// <summary>
        /// System uptime at which the last packet of this flow was switched.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(int))]
        LastSwitched = 21,

        /// <summary>
        /// System uptime at which the first packet of this flow was switched.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(int))]
        FirstSwitched = 22,

        /// <summary>
        /// Outgoing counter with length N x 8 bits for the number of bytes
        /// associated with an IP Flow.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(int))]
        OutgoingBytes = 23,

        /// <summary>
        /// Outgoing counter with length N x 8 bits for the number of packets
        /// associated with an IP Flow.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(int))]
        OutgoingPackets = 24,

        /// <summary>
        /// Minimum IP packet length on incoming packets of the flow.
        /// </summary>
        [FieldLength(2)]
        [ClrType(typeof(short))]
        MinimumPacketLength = 25,

        /// <summary>
        /// Maximum IP packet length on incoming packets of the flow.
        /// </summary>
        [FieldLength(2)]
        [ClrType(typeof(short))]
        MaximumPacketLength = 26,

        /// <summary>
        /// IPv6 source address.
        /// </summary>
        [FieldLength(16)]
        [ClrType(typeof(IPAddress))]
        IPv6SourceAddress = 27,

        /// <summary>
        /// IPv6 destination address.
        /// </summary>
        [FieldLength(16)]
        [ClrType(typeof(IPAddress))]
        IPv6DestinationAddress = 28,

        /// <summary>
        /// Length of the IPv6 source mask in contiguous bits.
        /// </summary>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        IPv6SourceMask = 29,

        /// <summary>
        /// Length of the IPv6 destination mask in contiguous bits.
        /// </summary>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        IPv6DestinationMask = 30,

        /// <summary>
        /// IPv6 flow label as per RFC 2460 definition.
        /// </summary>
        [FieldLength(3)]
        IPv6FlowLabel = 31,

        /// <summary>
        /// Internet Control Message Protocol (ICMP) packet type; reported as
        /// ((ICMP Type * 256) + ICMP code).
        /// </summary>
        [FieldLength(2)]
        [ClrType(typeof(short))]
        IcmpType = 32,

        /// <summary>
        /// Internet Group Management Protocol (IGMP) packet type.
        /// </summary>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        MulticastIgmpType = 33,

        /// <summary>
        /// When using sampled NetFlow, the rate at which packets are sampled,
        /// i.e. a value of 100 indicates that one of every 100 packets is
        /// sampled.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(int))]
        SamplingInterval = 34,

        /// <summary>
        /// The type of algorithm used for sampled NetFlow: 0x01 for
        /// deterministic sampling, 0x02 for random sampling.
        /// </summary>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        SamplingAlgorithm = 35,

        /// <summary>
        /// Timeout value (in seconds) for active flow entries in the NetFlow
        /// cache.
        /// </summary>
        [FieldLength(2)]
        [ClrType(typeof(short))]
        FlowActiveTimeout = 36,

        /// <summary>
        /// Timeout value (in seconds) for inactive flow entries in the NetFlow
        /// cache.
        /// </summary>
        [FieldLength(2)]
        [ClrType(typeof(short))]
        FlowInactiveTimeout = 37,

        /// <summary>
        /// Type of flow switching engine: RP = 0, VIP/Linecard = 1.
        /// </summary>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        EngineType = 38,

        /// <summary>
        /// ID number of the flow switching engine.
        /// </summary>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        EngineID = 39,

        /// <summary>
        /// Counter with length N x 8 bits for bytes for the number of bytes
        /// exported by the observation domain.
        /// </summary>
        [FieldLength(Default = 4)]
        [ClrType(typeof(long))]
        [ClrType(typeof(int))]
        [ClrType(typeof(short))]
        TotalBytesExported = 40,

        /// <summary>
        /// Counter with length N x 8 bits for bytes for the number of packets
        /// exported by the observation domain.
        /// </summary>
        [FieldLength(Default = 4)]
        [ClrType(typeof(long))]
        [ClrType(typeof(int))]
        [ClrType(typeof(short))]
        TotalPacketsExported = 41,

        /// <summary>
        /// Counter with length N x 8 bits for bytes for the number of flows
        /// exported by the observation domain.
        /// </summary>
        [FieldLength(Default = 4)]
        [ClrType(typeof(long))]
        [ClrType(typeof(int))]
        [ClrType(typeof(short))]
        TotalFlowsExported = 42,

        // 43 is vendor proprietary.

        /// <summary>
        /// IPv4 source address prefix (specific for Catalyst architecture).
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(IPAddress))]
        IPv4SourcePrefix = 44,

        /// <summary>
        /// IPv4 destination address prefix (specific for Catalyst architecture).
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(IPAddress))]
        IPv4DestinationPrefix = 45,

        /// <summary>
        /// MPLS Top Label Type.
        /// </summary>
        /// <remarks>
        /// 0x00: UNKNOWN, 0x01: TE-MIDPT, 0x02: ATOM, 0x03: VPN, 0x04: BGP,
        /// 0x05: LDP.
        /// </remarks>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        MplsTopLabelType = 46,

        /// <summary>
        /// Forwarding Equivalent Class corresponding to the MPLS Top Label.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(IPAddress))]
        MplsTopLabelIPAddress = 47,

        /// <summary>
        /// Identifier shown in &quot;show flow-sampler&quot;.
        /// </summary>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        FlowSamplerID = 48,

        /// <summary>
        /// The type of algorithm used for sampling data.
        /// </summary>
        /// <remarks>
        /// 0x02: random sampling. Use in connection with
        /// <see cref="FlowSamplerRandomInterval"/>.
        /// </remarks>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        FlowSamplerMode = 49,

        /// <summary>
        /// Packet interval at which to sample.
        /// </summary>
        /// <remarks>
        /// Use in connection with <see cref="FlowSamplerMode"/>
        /// </remarks>
        [FieldLength(4)]
        [ClrType(typeof(int))]
        FlowSamplerRandomInterval = 50,

        // 51 is vendor proprietary.

        /// <summary>
        /// Minimum TTL on incoming packets of the flow.
        /// </summary>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        MinimumTimeToLive = 52,

        /// <summary>
        /// Maximum TTL on incoming packets of the flow
        /// </summary>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        MaximumTimeToLive = 53,

        /// <summary>
        /// The IP v4 identification field.
        /// </summary>
        [FieldLength(2)]
        [ClrType(typeof(short))]
        IPv4Identification = 54,

        /// <summary>
        /// Type of Service byte setting when exiting outgoing interface.
        /// </summary>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        DestinationServiceType = 55,

        /// <summary>
        /// Incoming source MAC address.
        /// </summary>
        [FieldLength(6)]
        IncomingSourceMac = 56,

        /// <summary>
        /// Outgoing destination MAC address.
        /// </summary>
        [FieldLength(6)]
        OutgoingDestinationMac = 57,

        /// <summary>
        /// Virtual LAN identifier associated with ingress interface.
        /// </summary>
        [FieldLength(2)]
        [ClrType(typeof(short))]
        SourceVlan = 58,

        /// <summary>
        /// Virtual LAN identifier associated with egress interface.
        /// </summary>
        [FieldLength(2)]
        [ClrType(typeof(short))]
        DestinationVlan = 59,

        /// <summary>
        /// Internet Protocol Version Set to 4 for IPv4, set to 6 for IPv6.
        /// </summary>
        /// <remarks>
        /// If not present in the template, then version 4 is assumed.
        /// </remarks>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        IpProtocolVersion = 60,

        /// <summary>
        /// Flow direction: 0 - ingress flow, 1 - egress flow
        /// </summary>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        Direction = 61,

        /// <summary>
        /// IPv6 address of the next-hop router.
        /// </summary>
        [FieldLength(16)]
        [ClrType(typeof(IPAddress))]
        IPv6NextHop = 62,

        /// <summary>
        /// Next-hop router in the BGP domain.
        /// </summary>
        [FieldLength(16)]
        [ClrType(typeof(IPAddress))]
        BgpIPv6NextHop = 63,

        /// <summary>
        /// Bit-encoded field identifying IPv6 option headers found in the flow.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(int))]
        IPv6OptionHeaders = 64,

        // 65 - 69 are vendor proprietary.

        /// <summary>
        /// MPLS label at position 1 in the stack.
        /// </summary>
        /// <remarks>
        /// This comprises 20 bits of MPLS label, 3 EXP (experimental) bits and
        /// 1 S (end-of-stack) bit.
        /// </remarks>
        [FieldLength(3)]
        MplsLabel1 = 70,

        /// <summary>
        /// MPLS label at position 2 in the stack.
        /// </summary>
        /// <remarks>
        /// This comprises 20 bits of MPLS label, 3 EXP (experimental) bits and
        /// 1 S (end-of-stack) bit.
        /// </remarks>
        [FieldLength(3)]
        MplsLabel2 = 71,

        /// <summary>
        /// MPLS label at position 3 in the stack.
        /// </summary>
        /// <remarks>
        /// This comprises 20 bits of MPLS label, 3 EXP (experimental) bits and
        /// 1 S (end-of-stack) bit.
        /// </remarks>
        [FieldLength(3)]
        MplsLabel3 = 72,

        /// <summary>
        /// MPLS label at position 4 in the stack.
        /// </summary>
        /// <remarks>
        /// This comprises 20 bits of MPLS label, 3 EXP (experimental) bits and
        /// 1 S (end-of-stack) bit.
        /// </remarks>
        [FieldLength(3)]
        MplsLabel4 = 73,

        /// <summary>
        /// MPLS label at position 5 in the stack.
        /// </summary>
        /// <remarks>
        /// This comprises 20 bits of MPLS label, 3 EXP (experimental) bits and
        /// 1 S (end-of-stack) bit.
        /// </remarks>
        [FieldLength(3)]
        MplsLabel5 = 74,

        /// <summary>
        /// MPLS label at position 6 in the stack.
        /// </summary>
        /// <remarks>
        /// This comprises 20 bits of MPLS label, 3 EXP (experimental) bits and
        /// 1 S (end-of-stack) bit.
        /// </remarks>
        [FieldLength(3)]
        MplsLabel6 = 75,

        /// <summary>
        /// MPLS label at position 7 in the stack.
        /// </summary>
        /// <remarks>
        /// This comprises 20 bits of MPLS label, 3 EXP (experimental) bits and
        /// 1 S (end-of-stack) bit.
        /// </remarks>
        [FieldLength(3)]
        MplsLabel7 = 76,

        /// <summary>
        /// MPLS label at position 8 in the stack.
        /// </summary>
        /// <remarks>
        /// This comprises 20 bits of MPLS label, 3 EXP (experimental) bits and
        /// 1 S (end-of-stack) bit.
        /// </remarks>
        [FieldLength(3)]
        MplsLabel8 = 77,

        /// <summary>
        /// MPLS label at position 9 in the stack.
        /// </summary>
        /// <remarks>
        /// This comprises 20 bits of MPLS label, 3 EXP (experimental) bits and
        /// 1 S (end-of-stack) bit.
        /// </remarks>
        [FieldLength(3)]
        MplsLabel9 = 78,

        /// <summary>
        /// MPLS label at position 10 in the stack.
        /// </summary>
        /// <remarks>
        /// This comprises 20 bits of MPLS label, 3 EXP (experimental) bits and
        /// 1 S (end-of-stack) bit.
        /// </remarks>
        [FieldLength(3)]
        MplsLabel10 = 79,

        /// <summary>
        /// Incoming destination MAC address.
        /// </summary>
        [FieldLength(6)]
        IncomingDestinationMac = 80,

        /// <summary>
        /// Outgoing source MAC address
        /// </summary>
        [FieldLength(6)]
        OutgoingSourceMac = 81,

        /// <summary>
        /// Shortened interface name, i.e. &quot;FE1/0&quot.
        /// </summary>
        [FieldLength]
        [ClrType(typeof(string))]
        InterfaceName = 82,

        /// <summary>
        /// Full interface name, i.e. &quot;FastEthernet 1/0&quot.
        /// </summary>
        [FieldLength]
        [ClrType(typeof(string))]
        InterfaceDescription = 83,

        /// <summary>
        /// Name of the flow sampler.
        /// </summary>
        [FieldLength]
        [ClrType(typeof(string))]
        SamplerName = 84,

        /// <summary>
        /// Running byte counter for a permanent flow.
        /// </summary>
        [FieldLength(Default = 4)]
        [ClrType(typeof(long))]
        [ClrType(typeof(int))]
        [ClrType(typeof(short))]
        IncomingPermanentBytes = 85,

        /// <summary>
        /// Running packet counter for a permanent flow.
        /// </summary>
        [FieldLength(Default = 4)]
        [ClrType(typeof(long))]
        [ClrType(typeof(int))]
        [ClrType(typeof(short))]
        IncomingPermanentPackets = 86,

        // 87 is vendor proprietary

        /// <summary>
        /// The fragment-offset value from fragmented IP packets.
        /// </summary>
        [FieldLength(2)]
        [ClrType(typeof(short))]
        FragmentOffset = 88,

        /// <summary>
        /// Forwarding status is encoded on 1 byte with the 2 left bits giving
        /// the status and the 6 remaining bits giving the reason code.
        /// </summary>
        /// <remarks>
        /// Status is either unknown (00), forwarded (10), dropped (10) or
        /// consumed (11). Below is the list of forwarding status values with
        /// their meanings:
        /// <para>Unknown
        /// <list type="bullet">
        /// <item><term>0</term><description></description></item>
        /// </list>
        /// </para>
        /// <para>Forwarded
        /// <list type="bullet">
        /// <item><term>64</term><description>Unknown</description></item>
        /// <item><term>65</term><description>Forwarded fragmented</description></item>
        /// <item><term>66</term><description>Forwarded not fragmented</description></item>
        /// </list>
        /// </para>
        /// <para>Dropped
        /// <list type="bullet">
        /// <item><term>128</term><description>Unknown</description></item>
        /// <item><term>129</term><description>Drop ACL Deny</description></item>
        /// <item><term>130</term><description>Drop ACL drop</description></item>
        /// <item><term>131</term><description>Drop Unroutable </description></item>
        /// <item><term>132</term><description>Drop Adjacency</description></item>
        /// <item><term>133</term><description>Drop Fragmentation & DF set</description></item>
        /// <item><term>134</term><description>Drop Bad header checksum</description></item>
        /// <item><term>135</term><description></description>Drop Bad total Length</item>
        /// <item><term>136</term><description></description>Drop Bad Header Length</item>
        /// <item><term>137</term><description></description>Drop bad TTL</item>
        /// <item><term>138</term><description></description>Drop Policer</item>
        /// <item><term>139</term><description></description>Drop WRED</item>
        /// <item><term>140</term><description></description>Drop RPF</item>
        /// <item><term>141</term><description></description>Drop For us</item>
        /// <item><term>142</term><description></description>Drop Bad output interface</item>
        /// <item><term>143</term><description></description>Drop Hardware</item>
        /// </list>
        /// </para>
        /// <para>Consumed
        /// <list type="bullet">
        /// <item><term>192</term><description>Unknown</description></item>
        /// <item><term>193</term><description>Terminate Punt Adjacency</description></item>
        /// <item><term>194</term><description>Terminate Incomplete Adjacency</description></item>
        /// <item><term>195</term><description>Terminate For us</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        ForwardingStatus = 89,

        /// <summary>
        /// MPLS PAL Route Distinguisher.
        /// </summary>
        [FieldLength(8)]
        [ClrType(typeof(long))]
        MplsPalRouteDistinguisher = 90,

        /// <summary>
        /// Number of consecutive bits in the MPLS prefix length.
        /// </summary>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        MplsPrefixLength = 91,

        /// <summary>
        /// BGP Policy Accounting Source Traffic Index.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(int))]
        SourceTrafficIndex = 92,

        /// <summary>
        /// BGP Policy Accounting Destination Traffic Index.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(int))]
        DestinationTrafficIndex = 93,

        /// <summary>
        /// Application description.
        /// </summary>
        [FieldLength]
        [ClrType(typeof(string))]
        ApplicationDescription = 94,

        /// <summary>
        /// 8 bits of engine ID, followed by N bits of classification.
        /// </summary>
        [FieldLength]
        [ClrType(typeof(string))]
        ApplicationTag = 95,

        /// <summary>
        /// Name associated with a classification.
        /// </summary>
        [FieldLength]
        [ClrType(typeof(string))]
        ApplicationName = 96,

        /// <summary>
        /// The value of a Differentiated Services Code Point (DSCP) encoded in
        /// the Differentiated Services Field, after modification.
        /// </summary>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        PostIPDifferentiatedServicesCodePoint = 98,

        /// <summary>
        /// Multicast replication factor.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(int))]
        ReplicationFactor = 99,

        // 100 is deprecated.

        // 101 is missing in the specification.

        /// <summary>
        /// Layer 2 packet section offset, potentially a generic offset.
        /// </summary>
        Layer2PacketSectionOffset = 102,

        /// <summary>
        /// Layer 2 packet section size, potentially a generic size.
        /// </summary>
        Layer2PacketSectionSize = 103,

        /// <summary>
        /// Layer 2 packet section data.
        /// </summary>
        Layer2PacketSectionData = 104

        // 105 - 127 are reserved for future use.
    }
}
