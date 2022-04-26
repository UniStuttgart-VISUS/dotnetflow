// <copyright file="InformationElement.cs" company="Universität Stuttgart">
// Copyright © 2020 - 2022 Institut für Visualisierung und Interaktive Systeme. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.Net;


namespace DotNetFlow.Ipfix {

    /// <summary>
    /// List of supported IPFIX information elements.
    /// </summary>
    /// <remarks>
    /// <para>The information element is the equivalent to the
    /// <see cref="Netflow9.FieldType"/> in NetFlow v9.</para>
    /// <para>The enumeration members have been compiled from
    /// https://www.iana.org/assignments/ipfix/ipfix.xhtml.
    /// </para>
    /// </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design",
        "CA1028:Enum Storage should be Int32",
        Justification = "Underlying type must match the protocol size.")]
    public enum InformationElement : ushort {

        // Reserved = 0

        /// <summary>
        /// The number of octets since the previous report (if any) in incoming
        /// packets for this Flow at the Observation Point.
        /// </summary>
        /// <remarks>
        /// The number of octets includes IP header(s) and IP payload.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        OctetDeltaCount = 1,

        /// <summary>
        /// The number of incoming packets since the previous report (if any)
        /// for this Flow at the Observation Point.
        /// </summary>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        PacketDeltaCount = 2,

        /// <summary>
        /// The conservative count of Original Flows contributing to this
        /// Aggregated Flow.
        /// </summary>
        /// <remarks>
        /// May be distributed via any of the methods expressed by the
        /// <see cref="ValueDistributionMethod"/> Information Element.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        DeltaFlowCount = 3,

        /// <summary>
        /// The value of the protocol number in the IP packet header.
        /// </summary>
        /// <remarks>
        /// <para>The protocol number identifies the IP packet payload type.
        /// Protocol numbers are defined in the IANA Protocol Numbers registry.
        /// </para>
        /// <para>In Internet Protocol version 4 (IPv4), this is carried in the
        /// Protocol field. In Internet Protocol version 6 (IPv6), this is
        /// carried in the Next Header field in the last extension header of the
        /// packet.</para>
        /// </remarks>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        ProtocolIdentifier = 4,

        /// <summary>
        /// For IPv4 packets, this is the value of the TOS field in the IPv4
        /// packet header.
        /// </summary>
        /// <remarks>
        /// For IPv6 packets, this is the value of the Traffic Class field in
        /// the IPv6 packet header.
        /// </remarks>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        IPClassOfService = 5,

        /// <summary>
        /// TCP control bits observed for the packets of this Flow.
        /// </summary>
        /// <remarks><para>This information is encoded as a bit field; for each
        /// TCP control bit, there is a bit in this set. The bit is set to 1 if
        /// any observed packet of this Flow has the corresponding TCP control
        /// bit set to 1. The bit is cleared to 0 otherwise.</para>
        /// <para>The values of each bit are shown below, per the definition of
        /// the bits in the TCP header [RFC793] [RFC3168]:</para>
        /// <c>MSb                                                         LSb
        ///       0   1   2   3   4   5   6   7   8   9  10  11  12  13  14  15
        /// +---+---+---+---+---+---+---+---+---+---+---+---+---+---+---+---+
        /// |               |           | N | C | E | U | A | P | R | S | F |
        /// |     Zero      |   Future  | S | W | C | R | C | S | S | Y | I |
        /// | (Data Offset) |    Use    |   | R | E | G | K | H | T | N | N |
        /// +---+---+---+---+---+---+---+---+---+---+---+---+---+---+---+---+
        /// 
        /// bit    flag
        /// value  name  description
        /// ------+-----+-------------------------------------
        /// 0x8000       Zero (see tcpHeaderLength)
        /// 0x4000       Zero (see tcpHeaderLength)
        /// 0x2000       Zero (see tcpHeaderLength)
        /// 0x1000       Zero (see tcpHeaderLength)
        /// 0x0800       Future Use
        /// 0x0400       Future Use
        /// 0x0200       Future Use
        /// 0x0100   NS  ECN Nonce Sum
        /// 0x0080  CWR  Congestion Window Reduced
        /// 0x0040  ECE  ECN Echo
        /// 0x0020  URG  Urgent Pointer field significant
        /// 0x0010  ACK  Acknowledgment field significant
        /// 0x0008  PSH  Push Function
        /// 0x0004  RST  Reset the connection
        /// 0x0002  SYN  Synchronise sequence numbers
        /// 0x0001  FIN  No more data from sender</c>
        /// <para>As the most significant 4 bits of octets 12 and 13 (counting
        /// from zero) of the TCP header [RFC793] are used to encode the TCP
        /// data offset (header length), the corresponding bits in this
        /// Information Element MUST be exported as zero and MUST be ignored by
        /// the collector. Use the tcpHeaderLength Information Element to encode
        /// this value.</para>
        /// <para>Each of the 3 bits (0x800, 0x400, and 0x200), which are
        /// reserved for future use in [RFC793], SHOULD be exported as observed
        /// in the TCP headers of the packets of this Flow.</para>
        /// <para>If exported as a single octet with reduced-size encoding, this
        /// Information Element covers the low-order octet of this field (i.e.
        /// bits 0x80 to 0x01), omitting the ECN Nonce Sum and the three Future
        /// Use bits. A collector receiving this Information Element with
        /// reduced-size encoding must not assume anything about the content of
        /// these four bits.</para>
        /// <para>Exporting Processes exporting this Information Element on
        /// behalf of a Metering Process that is not capable of observing any of
        /// the ECN Nonce Sum or Future Use bits SHOULD use reduced-size
        /// encoding, and only export the least significant 8 bits of this
        /// Information Element.</para>
        /// <para>Note that previous revisions of this Information Element's
        /// definition specified that the CWR and ECE bits must be exported as
        /// zero, even if observed.  Collectors should therefore not assume that
        /// a value of zero for these bits in this Information Element indicates
        /// the bits were never set in the observed traffic, especially if these
        /// bits are zero in every Flow Record sent by a given exporter.</para>
        /// </remarks>
        [FieldLength(2)]
        [ClrType(typeof(ushort))]
        TcpControlBits = 6, // TODO

        /// <summary>
        /// The source port identifier in the transport header.
        /// </summary>
        /// <remarks>
        /// For the transport protocols UDP, TCP, and SCTP, this is the source
        /// port number given in the respective header. This field MAY also be
        /// used for future transport protocols that have 16-bit source port
        /// identifiers.
        /// </remarks>
        [FieldLength(2)]
        [ClrType(typeof(ushort))]
        SourceTransportPort = 7,

        /// <summary>
        /// The IPv4 source address in the IP packet header.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(IPAddress))]
        SourceIPv4Address = 8,

        /// <summary>
        /// The number of contiguous bits that are relevant in the
        /// <see cref="SourceIPv4Prefix" /> Information Element.
        /// </summary>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        SourceIPv4PrefixLength = 9,

        /// <summary>
        /// The index of the IP interface where packets of this Flow are being
        /// received.
        /// </summary>
        /// <remarks>
        /// The value matches the value of managed object 'ifIndex' as defined
        /// in [RFC2863]. Note that ifIndex values are not assigned statically
        /// to an interface and that the interfaces may be renumbered every
        /// time the device's management system is re-initialised, as specified
        /// in [RFC2863].
        /// </remarks>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        IngressInterface = 10,

        /// <summary>
        /// The destination port identifier in the transport header.
        /// </summary>
        /// <remarks>
        /// For the transport protocols UDP, TCP, and SCTP, this is the
        /// destination port number given in the respective header. This field
        /// MAY also be used for future transport protocols that have 16-bit
        /// destination port identifiers.
        /// </remarks>
        [FieldLength(2)]
        [ClrType(typeof(ushort))]
        DestinationTransportPort = 11,

        /// <summary>
        /// The IPv4 destination address in the IP packet header.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(IPAddress))]
        DestinationIPv4Address = 12,

        /// <summary>
        /// The number of contiguous bits that are relevant in the
        /// <see cref="DestinationIPv4Prefix"/> Information Element.
        /// </summary>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        DestinationIPv4PrefixLength = 13,

        /// <summary>
        /// The index of the IP interface where packets of this Flow are being
        /// sent.
        /// </summary>
        /// <remarks>
        /// The value matches the value of managed object 'ifIndex' as
        /// defined in [RFC2863]. Note that ifIndex values are not assigned
        /// statically to an interface and that the interfaces may be renumbered
        /// every time the device's management system is re-initialised, as
        /// specified in [RFC2863].
        /// </remarks>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        EgressInterface = 14,

        /// <summary>
        /// The IPv4 address of the next IPv4 hop.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(IPAddress))]
        IPNextHopIPv4Address = 15,

        /// <summary>
        /// The autonomous system (AS) number of the source IP address.
        /// </summary>
        /// <remarks>
        /// If AS path information for this Flow is only available as an
        /// unordered AS set (and not as an ordered AS sequence), then the value
        /// of this Information Element is 0.
        /// <remarks>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        BgpSourceAutonomousSystemNumber = 16,

        /// <summary>
        /// The autonomous system (AS) number of the destination IP address.
        /// </summary>
        /// <remarks>
        /// If AS path information for this Flow is only available as an
        /// unordered AS set (and not as an ordered AS sequence), then the value
        /// of this Information Element is 0.
        /// </remarks>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        BgpDestinationAutonomousSystemNumber = 17,

        /// <summary>
        /// The IPv4 address of the next (adjacent) BGP hop.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(IPAddress))]
        BgpNextHopIPv4Address = 18,

        /// <summary>
        /// The number of outgoing multicast packets since the previous report
        /// (if any) sent for packets of this Flow by a multicast daemon within
        /// the Observation Domain.
        /// </summary>
        /// <remarks>
        /// This property cannot necessarily be observed at the Observation
        /// Point, but may be retrieved by other means.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        PostMulticastPacketDeltaCount = 19,

        /// <summary>
        /// The number of octets since the previous report (if any) in outgoing
        /// multicast packets sent for packets of this Flow by a multicast
        /// daemon within the Observation Domain.
        /// </summary>
        /// <remarks>
        /// This property cannot necessarily be observed at the Observation Point,
        /// but may be retrieved by other means. The number of octets includes IP
        /// header(s) and IP payload.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        PostMulticastOctetDeltaCount = 20,

        /// <summary>
        /// The relative timestamp of the last packet of this Flow.
        /// </summary>
        /// <remarks>
        /// It indicates the number of milliseconds since the last
        /// (re-) initialisation of the IPFIX Device (sysUpTime). sysUpTime can
        /// be calculated from systemInitTimeMilliseconds.
        /// </remarks>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        FlowEndSysUpTime = 21,

        /// <summary>
        /// The relative timestamp of the first packet of this Flow.
        /// </summary>
        /// <remarks>
        /// It indicates the number of milliseconds since the last
        /// (re-)initialisation of the IPFIX Device (sysUpTime). sysUpTime can
        /// be calculated from systemInitTimeMilliseconds.
        /// </remarks>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        FlowStartSysUpTime = 22,

        /// <summary>
        /// The definition of this Information Element is identical to the
        /// definition of Information Element <see cref="OctetDeltaCount"/>,
        /// except that it reports a potentially modified value caused by a
        /// middlebox function after the packet passed the Observation Point.
        /// </summary>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        PostOctetDeltaCount = 23,

        /// <summary>
        /// The definition of this Information Element is identical to the
        /// definition of Information Element <see cref="PacketDeltaCount"/>,
        /// except that it reports a potentially modified value caused by a
        /// middlebox function after the packet passed the Observation Point.
        /// </summary>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        PostPacketDeltaCount = 24,

        /// <summary>
        /// Length of the smallest packet observed for this Flow.
        /// </summary>
        /// <remarks>
        /// The packet length includes the IP header(s) length and the IP
        /// payload length.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        MinimumIPTotalLength = 25,

        /// <summary>
        /// Length of the largest packet observed for this Flow.
        /// </summary>
        /// <remarks>
        /// The packet length includes the IP header(s) length and the IP
        /// payload length.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        MaximumIPTotalLength = 26,

        /// <summary>
        /// The IPv6 source address in the IP packet header.
        /// </summary>
        [FieldLength(16)]
        [ClrType(typeof(IPAddress))]
        SourceIPv6Address = 27,

        /// <summary>
        /// The IPv6 destination address in the IP packet header.
        /// </summary>
        [FieldLength(16)]
        [ClrType(typeof(IPAddress))]
        DestinationIPv6Address = 28,

        /// <summary>
        /// The number of contiguous bits that are relevant in the 
        /// sourceIPv6Prefix Information Element.
        /// </summary>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        SourceIPv6PrefixLength = 29,

        /// <summary>
        /// The number of contiguous bits that are relevant in the
        /// destinationIPv6Prefix Information Element.
        /// </summary>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        DestinationIPv6PrefixLength = 30,

        /// <summary>
        /// The value of the IPv6 Flow Label field in the IP packet header.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        FlowLabelIPv6 = 31,

        /// <summary>
        /// Type and Code of the IPv4 ICMP message.
        /// </summary>
        /// <remarks>
        /// The combination of both values is reported as 
        /// <c>(ICMP type * 256) + ICMP code</c>.
        /// </remarks>
        [FieldLength(2)]
        [ClrType(typeof(ushort))]
        IcmpTypeCodeIPv4 = 32,

        /// <summary>
        /// The type field of the IGMP message.
        /// </summary>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        IgmpType = 33,

        /// <summary>
        /// When using sampled NetFlow, the rate at which packets are sampled,
        /// e.g. a value of 100 indicates that one of every 100 packets is
        /// sampled.
        /// </summary>
        /// <remarks>
        /// Deprecated in favour of <see cref="SamplingPacketInterval"/>.
        /// </remarks>
        [Obsolete("Deprecated in favour of SamplingPacketInterval.")]
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        SamplingInterval = 34,

        /// <summary>
        /// The type of algorithm used for sampled NetFlow.
        /// </summary>
        /// <remarks>
        /// <para>Semantics are: 1 - Deterministic Sampling,
        /// 2 - Random Sampling.</para>
        /// <para>The values are not compatible with the selectorAlgorithm IE,
        /// where Deterministic has been replaced by Systematic count-based (1)
        /// or Systematic time-based (2), and Random is (3). Conversion is
        /// required.</para>
        /// <para>
        /// Deprecated in favour of <see cref="SelectorAlgorithm"/>.
        /// </para>
        /// </remarks>
        [Obsolete("Deprecated in favour of SelectorAlgorithm.")]
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        SamplingAlgorithm = 35,

        /// <summary>
        /// The number of seconds after which an active Flow is timed out
        /// anyway, even if there is still a continuous flow of packets.
        /// </summary>
        [FieldLength(2)]
        [ClrType(typeof(ushort))]
        FlowActiveTimeout = 36,

        /// <summary>
        /// A Flow is considered to be timed out if no packets belonging to the
        /// Flow have been observed for the number of seconds specified by this
        /// field.
        /// </summary>
        [FieldLength(2)]
        [ClrType(typeof(ushort))]
        FlowIdleTimeout = 37,

        /// <summary>
        /// Type of flow switching engine in a router/switch.
        /// </summary>
        /// <remarks>
        /// <para>Possible values are: RP = 0, VIP/Line card = 1, PFC/DFC = 2.
        /// Reserved for internal use on the Collector.</para>
        /// </remarks>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        EngineType = 38,

        /// <summary>
        /// Versatile Interface Processor (VIP) or line card slot number of the
        /// flow switching engine in a router/switch.
        /// </summary>
        /// <remarks>
        /// Reserved for internal use on the Collector.
        /// </remarks>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        EngineID = 39,

        /// <summary>
        /// The total number of octets that the Exporting Process has sent since
        /// the Exporting Process (re-)initialisation to a particular Collecting
        /// Process.
        /// </summary>
        /// <remarks>
        /// The value of this Information Element is calculated by summing up
        /// the IPFIX Message Header length values of all IPFIX Messages that
        /// were successfully sent to the Collecting Process. The reported
        /// number excludes octets in the IPFIX Message that carries the counter
        /// value. If this Information Element is sent to a particular
        /// Collecting Process, then by default it specifies the number of
        /// octets sent to this Collecting Process.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        ExportedOctetTotalCount = 40,

        /// <summary>
        /// The total number of IPFIX Messages that the Exporting Process has
        /// sent since the Exporting Process (re-)initialisation to a particular
        /// Collecting Process.
        /// </summary>
        /// <remarks>
        /// The reported number excludes the IPFIX Message that carries the
        /// counter value. If this Information Element is sent to a particular
        /// Collecting Process, then by default it specifies the number of IPFIX
        /// Messages sent to this Collecting Process.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        ExportedMessageTotalCount = 41,

        /// <summary>
        /// The total number of Flow Records that the Exporting Process has sent
        /// as Data Records since the Exporting Process (re-)initialisation to a
        /// particular Collecting Process.
        /// </summary>
        /// <remarks>
        /// The reported number excludes Flow Records in the IPFIX Message that
        /// carries the counter value. If this Information Element is sent to a
        /// particular Collecting Process, then by default it specifies the
        /// number of Flow Records sent to this process.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        ExportedFlowRecordTotalCount = 42,

        /// <summary>
        /// This is a platform-specific field for the Catalyst 5000 and
        /// Catalyst 6000 family. It is used to store the address of a router
        /// that is being shortcut when performing MultiLayer Switching.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(IPAddress))]
        IPv4RouterShortcut = 43,

        /// <summary>
        /// IPv4 source address prefix.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(IPAddress))]
        SourceIPv4Prefix = 44,

        /// <summary>
        /// IPv4 destination address prefix.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(IPAddress))]
        DestinationIPv4Prefix = 45,

        /// <summary>
        /// This field identifies the control protocol that allocated the
        /// top-of-stack label.
        /// </summary>
        /// <remarks>
        /// Values for this field are listed in the MPLS label type registry.
        /// See http://www.iana.org/assignments/ipfix/ipfix.xhtml#ipfix-mpls-label-type.
        /// </remarks>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        MplsTopLabelType = 46,

        /// <summary>
        /// The IPv4 address of the system that the MPLS top label will cause
        /// this Flow to be forwarded to.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(IPAddress))]
        MplsTopLabelIPv4Address = 47,

        /// <summary>
        /// The unique identifier associated with samplerName.
        /// </summary>
        /// <remarks>
        /// Deprecated in favour of <see cref="SelectorID"/>.
        /// </remarks>
        [Obsolete(" Deprecated in favour of SelectorID.")]
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        SamplerID = 48,

        /// <summary>
        /// Deprecated in favour of <see cref="SelectorAlgorithm"/>.
        /// </summary>
        /// <remarks>
        /// The values are not compatible: selectorAlgorithm = 3 is random
        /// sampling. The type of algorithm used for sampling data:
        /// 1 - Deterministic, 2 - Random Sampling. Use with 
        /// samplerRandomInterval.
        /// </remarks>
        [Obsolete("Deprecated in favour of SelectorAlgorithm.")]
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        SamplerMode = 49,

        /// <summary>
        /// Packet interval at which to sample in case of random sampling.
        /// </summary>
        /// <remarks>
        /// <para>Used in connection with the samplerMode 0x02 (random sampling)
        /// value.</para>
        /// <para>Deprecated in favour of <see cref="SamplingPacketInterval"/>.
        /// </para>
        /// </remarks>
        [Obsolete("Deprecated in favour of SamplingPacketInterval.")]
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        SamplerRandomInterval = 50,

        /// <summary>
        /// Characterises the traffic class, i.e. QoS treatment.
        /// </summary>
        /// <remarks>
        /// Deprecated in favour of <see cref="SelectorID"/>.
        /// </remarks>
        [Obsolete("Deprecated in favour of SelectorID.")]
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        ClassID = 51,

        /// <summary>
        /// Minimum TTL value observed for any packet in this Flow.
        /// </summary>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        MinimumTtl = 52,

        /// <summary>
        /// Maximum TTL value observed for any packet in this Flow.
        /// </summary>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        MaximumTtl = 53,

        /// <summary>
        /// The value of the Identification field in the IPv4 packet header or
        /// in the IPv6 Fragment header, respectively.
        /// </summary>
        /// <remarks>
        /// The value is 0 for IPv6 if there is no fragment header.
        /// </remarks>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        FragmentIdentification = 54,

        /// <summary>
        /// The definition of this Information Element is identical to the
        /// definition of Information Element <see cref="IPClassOfService"/>,
        /// except that it reports a potentially modified value caused by a
        /// middlebox function after the packet passed the Observation Point.
        /// </summary>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        PostIPClassOfService = 55,

        /// <summary>
        /// The IEEE 802 source MAC address field.
        /// </summary>
        [FieldLength(6)]
        SourceMacAddress = 56,

        /// <summary>
        /// The definition of this Information Element is identical to the
        /// definition of Information Element
        /// <see cref="DestinationMacAddress"/>, except that it reports a
        /// potentially modified value caused by a middlebox function after the
        /// packet passed the Observation Point.
        /// /// </summary>
        [FieldLength(6)]
        PostDestinationMacAddress = 57,

        /// <summary>
        /// Virtual LAN identifier associated with ingress interface.
        /// </summary>
        /// <remarks>
        /// For dot1q vlans, see 243 <see cref="Dot1QVlanID"/>.
        /// </remarks>

        [FieldLength(2)]
        [ClrType(typeof(ushort))]
        VlanID = 58,

        /// <summary>
        /// Virtual LAN identifier associated with egress interface.
        /// </summary>
        /// <remarks>
        /// For postdot1q vlans, see 254, <see cref="PostDot1QVlanID"/>.
        /// </remarks>
        [FieldLength(2)]
        [ClrType(typeof(ushort))]
        PostVlanID = 59,

        /// <summary>
        /// The IP version field in the IP packet header.
        /// </summary>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        IPVersion = 60,

        /// <summary>
        /// The direction of the Flow observed at the Observation Point.
        /// </summary>
        /// <remarks>
        /// There are only two values defined. 0x00: ingress flow and 
        /// 0x01: egress flow.
        /// </remarks>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        FlowDirection = 61,

        /// <summary>
        /// The IPv6 address of the next IPv6 hop.
        /// </summary>
        [FieldLength(16)]
        [ClrType(typeof(IPAddress))]
        IPNextHopIPv6Address = 62,

        /// <summary>
        /// The IPv6 address of the next (adjacent) BGP hop.
        /// </summary>
        [FieldLength(16)]
        [ClrType(typeof(IPAddress))]
        BgpNextHopIPv6Address = 63,

        /// <summary>
        /// IPv6 extension headers observed in packets of this Flow.
        /// /// </summary>
        /// <remarks>
        /// <para>The information is encoded in a set of bit fields. For each
        /// IPv6 option header, there is a bit in this set. The bit is set to 1
        /// if any observed packet of this Flow contains the corresponding IPv6
        /// extension header. Otherwise, if no observed packet of this Flow
        /// contained the respective IPv6 extension header, the value of the
        /// corresponding bit is 0.</para>
        /// <c>
        /// 0     1     2     3     4     5     6     7
        /// +-----+-----+-----+-----+-----+-----+-----+-----+
        /// | DST | HOP | Res | UNK |FRA0 | RH  |FRA1 | Res |  ...
        /// +-----+-----+-----+-----+-----+-----+-----+-----+
        /// 
        /// 8     9    10    11    12    13    14    15
        /// +-----+-----+-----+-----+-----+-----+-----+-----+
        /// ... |           Reserved    | MOB | ESP | AH  | PAY | ...
        /// +-----+-----+-----+-----+-----+-----+-----+-----+
        /// 
        /// 16    17    18    19    20    21    22    23
        /// +-----+-----+-----+-----+-----+-----+-----+-----+
        /// ... |                  Reserved                     | ...
        /// +-----+-----+-----+-----+-----+-----+-----+-----+
        /// 
        /// 24    25    26    27    28    29    30    31
        /// +-----+-----+-----+-----+-----+-----+-----+-----+
        /// ... |                  Reserved                     |
        /// +-----+-----+-----+-----+-----+-----+-----+-----+
        /// 
        /// Bit    IPv6 Option   Description
        /// 
        /// 0, DST      60       Destination option header
        /// 1, HOP       0       Hop-by-hop option header
        /// 2, Res               Reserved
        /// 3, UNK               Unknown Layer 4 header
        ///                      (compressed, encrypted, not supported)
        /// 4, FRA0     44       Fragment header - first fragment
        /// 5, RH       43       Routing header
        /// 6, FRA1     44       Fragmentation header - not first fragment
        /// 7, Res               Reserved
        /// 8 to 11              Reserved
        /// 12, MOB     135      IPv6 mobility [RFC3775]
        /// 13, ESP      50      Encrypted security payload
        /// 14, AH       51      Authentication Header
        /// 15, PAY     108      Payload compression header
        /// 16 to 31             Reserved
        /// </c>
        /// </remarks>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        IPv6ExtensionHeaders = 64,

        /// <summary>
        /// The Label, Exp, and S fields from the top MPLS label stack entry,
        /// i.e. from the last label that was pushed.
        /// </summary>
        /// <remarks>
        /// <para>The size of this Information Element is 3 octets.</para>
        /// <c>
        /// 0                   1                   2
        /// 0 1 2 3 4 5 6 7 8 9 0 1 2 3 4 5 6 7 8 9 0 1 2 3
        /// +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+
        /// |                Label                  | Exp |S|
        /// +-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+</c>
        /// <para>Label: Label Value, 20 bits; Exp: Experimental Use, 3 bits;
        /// S: Bottom of Stack, 1 bit</para>
        [FieldLength(3)]
        MplsTopLabelStackSection = 70,

        /// <summary>
        /// The Label, Exp, and S fields from the label stack entry that was
        /// pushed immediately before the label stack entry that would be
        /// reported by <see cref="MplsTopLabelStackSection"/>.
        /// </summary>
        /// <remarks>
        /// See the definition of <see cref="MplsTopLabelStackSection"/> for
        /// further details. The size of this Information Element is 3 octets.
        /// </remarks>
        [FieldLength(3)]
        MplsLabelStackSection2 = 71,

        /// <summary>
        /// The Label, Exp, and S fields from the label stack entry that was
        /// pushed immediately before the label stack entry that would be
        /// reported by <see cref="MplsTopLabelStackSection2"/>.
        /// </summary>
        /// <remarks>
        /// See the definition of <see cref="MplsTopLabelStackSection"/> for
        /// further details. The size of this Information Element is 3 octets.
        /// </remarks>
        [FieldLength(3)]
        MplsLabelStackSection3 = 72,

        /// <summary>
        /// The Label, Exp, and S fields from the label stack entry that was
        /// pushed immediately before the label stack entry that would be
        /// reported by <see cref="MplsTopLabelStackSection3"/>.
        /// </summary>
        /// <remarks>
        /// See the definition of <see cref="MplsTopLabelStackSection"/> for
        /// further details. The size of this Information Element is 3 octets.
        /// </remarks>
        [FieldLength(3)]
        MplsLabelStackSection4 = 73,

        /// <summary>
        /// The Label, Exp, and S fields from the label stack entry that was
        /// pushed immediately before the label stack entry that would be
        /// reported by <see cref="MplsTopLabelStackSection4"/>.
        /// </summary>
        /// <remarks>
        /// See the definition of <see cref="MplsTopLabelStackSection"/> for
        /// further details. The size of this Information Element is 3 octets.
        /// </remarks>
        [FieldLength(3)]
        MplsLabelStackSection5 = 74,

        /// <summary>
        /// The Label, Exp, and S fields from the label stack entry that was
        /// pushed immediately before the label stack entry that would be
        /// reported by <see cref="MplsTopLabelStackSection5"/>.
        /// </summary>
        /// <remarks>
        /// See the definition of <see cref="MplsTopLabelStackSection"/> for
        /// further details. The size of this Information Element is 3 octets.
        /// </remarks>
        [FieldLength(3)]
        MplsLabelStackSection6 = 75,

        /// <summary>
        /// The Label, Exp, and S fields from the label stack entry that was
        /// pushed immediately before the label stack entry that would be
        /// reported by <see cref="MplsTopLabelStackSection6"/>.
        /// </summary>
        /// <remarks>
        /// See the definition of <see cref="MplsTopLabelStackSection"/> for
        /// further details. The size of this Information Element is 3 octets.
        /// </remarks>
        [FieldLength(3)]
        MplsLabelStackSection7 = 76,

        /// <summary>
        /// The Label, Exp, and S fields from the label stack entry that was
        /// pushed immediately before the label stack entry that would be
        /// reported by <see cref="MplsTopLabelStackSection7"/>.
        /// </summary>
        /// <remarks>
        /// See the definition of <see cref="MplsTopLabelStackSection"/> for
        /// further details. The size of this Information Element is 3 octets.
        /// </remarks>
        [FieldLength(3)]
        MplsLabelStackSection8 = 77,

        /// <summary>
        /// The Label, Exp, and S fields from the label stack entry that was
        /// pushed immediately before the label stack entry that would be
        /// reported by <see cref="MplsTopLabelStackSection8"/>.
        /// </summary>
        /// <remarks>
        /// See the definition of <see cref="MplsTopLabelStackSection"/> for
        /// further details. The size of this Information Element is 3 octets.
        /// </remarks>
        [FieldLength(3)]
        MplsLabelStackSection9 = 78,

        /// <summary>
        /// The Label, Exp, and S fields from the label stack entry that was
        /// pushed immediately before the label stack entry that would be
        /// reported by <see cref="MplsTopLabelStackSection9"/>.
        /// </summary>
        /// <remarks>
        /// See the definition of <see cref="MplsTopLabelStackSection"/> for
        /// further details. The size of this Information Element is 3 octets.
        /// </remarks>
        [FieldLength(3)]
        MplsLabelStackSection10 = 79,

        /// <summary>
        /// The IEEE 802 destination MAC address field.
        /// </summary>
        [FieldLength(6)]
        DestinationMacAddress = 80,

        /// <summary>
        /// The definition of this Information Element is identical to the
        /// definition of Information Element <see cref="SourceMacAddress"/>,
        /// except that it reports a potentially modified value caused by a
        /// middlebox function after the packet passed the Observation Point.
        /// </summary>
        [FieldLength(6)]
        PostSourceMacAddress = 81,

        /// <summary>
        /// A short name uniquely describing an interface, e.g.
        /// &quot;Eth1/0&quot;.
        /// </summary>
        [FieldLength]
        [ClrType(typeof(string))]
        InterfaceName = 82,

        /// <summary>
        /// The description of an interface, e.g. &quot;FastEthernet 1/0&quot;
        /// or &quot;ISP connection&quot;.
        /// </summary>
        [FieldLength]
        [ClrType(typeof(string))]
        InterfaceDescription = 83,

        /// <summary>
        /// Name of the flow sampler.
        /// </summary>
        /// <remarks>
        /// Deprecated in favour of <see cref="SelectorName"/>.
        /// </remarks>
        [Obsolete("Deprecated in favour of SelectorName.")]
        [FieldLength]
        [ClrType(typeof(string))]
        SamplerName = 84,

        /// <summary>
        /// The total number of octets in incoming packets for this Flow at the
        /// Observation Point since the Metering Process (re-)initialisation for
        /// this Observation Point.
        /// </summary>
        /// <remarks>
        /// The number of octets includes IP header(s) and IP payload.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        OctetTotalCount = 85,

        /// <summary>
        /// /// The total number of incoming packets for this Flow at the
        /// Observation Point since the Metering Process (re-)initialisation for
        /// this Observation Point.
        /// </summary>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        PacketTotalCount = 86,

        /// <summary>
        /// Flow flags and the value of the sampler ID (<see cref="SamplerID"/>)
        /// combined in one bitmapped field.
        /// </summary>
        /// <remarks>
        /// Reserved for internal use on the Collector.
        /// </remarks>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        FlagsAndSamplerID = 87,

        /// <summary>
        /// The value of the IP fragment offset field in the IPv4 packet header
        /// or the IPv6 Fragment header, respectively.
        /// </summary>
        /// <remarks>
        /// The value is 0 for IPv6 if there is no fragment header.
        /// </remarks>
        [FieldLength(2)]
        [ClrType(typeof(ushort))]
        FragmentOffset = 88,

        /// <summary>
        /// This Information Element describes the forwarding status of the flow
        /// and any attached reasons.
        /// </summary>
        /// <remarks>
        /// <para>The layout of the encoding is as follows:
        /// <c>
        /// MSB  -  0   1   2   3   4   5   6   7  -  LSB
        ///      +---+---+---+---+---+---+---+---+
        ///      | Status|  Reason code or flags |
        ///      +---+---+---+---+---+---+---+---+
        /// </c></para>
        /// <para>See the Forwarding Status sub-registries at 
        /// http://www.iana.org/assignments/ipfix/ipfix.xhtml#forwarding-status.
        /// </para>
        /// </remarks>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        ForwardingStatus = 89,

        /// <summary>
        /// The value of the VPN route distinguisher of a corresponding entry in
        /// a VPN routing and forwarding table.
        /// </summary>
        /// Route distinguisher ensures that the same address can be used in
        /// several different MPLS VPNs and that it is possible for BGP to carry
        /// several completely different routes to that address, one for each
        /// VPN. According to [RFC4364], the size of 
        /// <see cref="MplsVpnRouteDistinguisher"/> is 8 octets. However, in
        /// [RFC4382] an octet string with flexible length was chosen for
        /// representing a VPN route distinguisher by object
        /// MplsL3VpnRouteDistinguisher. This choice was made in order to be
        /// open to future changes of the size. This idea was adopted when
        /// choosing octetArray as abstract data type for this Information
        /// Element. The maximum length of this Information Element is 256
        /// octets.
        [FieldLength(MaximumLength = 256)]
        MplsVpnRouteDistinguisher = 90,

        /// <summary>
        /// The prefix length of the subnet of the
        /// <see cref="MplsTopLabelIPv4Address"/> that the MPLS top label will
        /// cause the Flow to be forwarded to.
        /// </summary>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        MplsTopLabelPrefixLength = 91,

        /// <summary>
        /// BGP Policy Accounting Source Traffic Index.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        SourceTrafficIndex = 92,

        /// <summary>
        /// BGP Policy Accounting Destination Traffic Index.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        DestinationTrafficIndex = 93,

        /// <summary>
        /// Specifies the description of an application.
        /// </summary>
        [FieldLength]
        [ClrType(typeof(string))]
        ApplicationDescription = 94,

        /// <summary>
        /// Specifies an Application ID per RFC6759.
        /// </summary>
        [FieldLength]
        ApplicationID = 95,

        /// <summary>
        /// Specifies the name of an application.
        /// </summary>
        [FieldLength]
        [ClrType(typeof(string))]
        ApplicationName = 96,

        ///// <summary>
        /////
        ///// </summary>
        //[ClrType(typeof())]
        //Assigned for NetFlow v9 compatibility = 97,

        /// <summary>
        /// The definition of this Information Element is identical to the
        /// definition of Information Element
        /// <see cref="IpDiffServCodePoint"/>, except that it reports a
        /// potentially modified value caused by a middlebox function after
        /// the packet passed the Observation Point.
        /// </summary>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        PostIPDiffServCodePoint = 98,   // TODO

        /// <summary>
        /// The amount of multicast replication that is applied to a traffic
        /// stream.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        MulticastReplicationFactor = 99,

        /// <summary>
        /// Traffic Class Name, associated with the <see cref="ClassID"/>
        /// Information Element.
        /// </summary>
        /// <remarks>
        /// Deprecated in favour of <see cref="SelectorName"/>.
        /// </remarks>
        [Obsolete("Deprecated in favour of SelectorName.")]
        [FieldLength]
        [ClrType(typeof(string))]
        ClassName = 100,

        /// <summary>
        /// A unique identifier for the engine that determined the Selector ID.
        /// </summary>
        /// <remarks>
        /// Thus, the Classification Engine ID defines the context for the
        /// Selector ID. The Classification Engine can be considered a specific
        /// registry for application assignments. Values for this field are
        /// listed in the Classification Engine IDs registry. See
        /// http://www.iana.org/assignments/ipfix/ipfix.xhtml#classification-engine-ids.
        /// </remarks>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        ClassificationEngineID = 101,

        /// <summary>
        /// Layer 2 packet section offset. Potentially a generic packet section
        /// offset.
        /// </summary>
        /// <remarks>
        /// Deprecated in favour of <see cref="SectionOffset"/>.
        /// </remarks>
        [Obsolete("Deprecated in favour of SectionOffset.")]
        [FieldLength(2)]
        [ClrType(typeof(ushort))]
        Layer2PacketSectionOffset = 102,

        /// <summary>
        /// Layer 2 packet section size. Potentially a generic packet section
        /// size.
        /// </summary>
        /// <remarks>
        /// Deprecated in favour of <see cref="DataLinkFrameSize"/>.
        /// </remarks>
        [Obsolete("Deprecated in favour of DataLinkFrameSize.")]
        [FieldLength(2)]
        [ClrType(typeof(ushort))]
        Layer2PacketSectionSize = 103,

        /// <summary>
        /// Deprecated in favour of 315 dataLinkFrameSection. Layer 2 packet
        /// section data.
        /// </summary>
        [FieldLength]
        Layer2PacketSectionData = 104,

        /// <summary>
        /// The autonomous system (AS) number of the first AS in the AS path to
        /// the destination IP address.
        /// </summary>
        /// <remarks></remarks>
        /// The path is deduced by looking up the destination IP address of the
        /// Flow in the BGP routing information base. If AS path information for
        /// this Flow is only available as an unordered AS set (and not as an
        /// ordered AS sequence), then the value of this Information Element is
        /// 0.
        /// </remarks>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        BgpNextAdjacentAutonomousSystemNumber = 128,

        /// <summary>
        /// The autonomous system (AS) number of the last AS in the AS path from
        /// the source IP address.
        /// </summary>
        /// <remarks>
        /// The path is deduced by looking up the source IP address of the Flow
        /// in the BGP routing information base. If AS path information for this
        /// Flow is only available as an unordered AS set (and not as an ordered
        /// AS sequence), then the value of this Information Element is 0. In
        /// case of BGP asymmetry, the
        /// <see cref="BgpPreviousAdjacentAutonomousSystemNumber"/> might not be
        /// able to report the correct value.
        /// </remarks>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        BgpPreviousAdjacentAutonomousSystemNumber = 129,

        /// <summary>
        /// The IPv4 address used by the Exporting Process.
        /// </summary>
        /// <remarks>
        /// This is used by the Collector to identify the Exporter in cases
        /// where the identity of the Exporter may have been obscured by the use
        /// of a proxy.
        /// </remarks>
        [FieldLength(4)]
        [ClrType(typeof(IPAddress))]
        ExporterIPv4Address = 130,

        /// <summary>
        /// The IPv6 address used by the Exporting Process.
        /// </summary>
        /// <remarks>
        /// This is used by the Collector to identify the Exporter in cases
        /// where the identity of the Exporter may have been obscured by the use
        /// of a proxy.
        /// </remarks>
        [FieldLength(16)]
        [ClrType(typeof(IPAddress))]
        ExporterIPv6Address = 131,

        /// <summary>
        /// The number of octets since the previous report (if any) in packets
        /// of this Flow dropped by packet treatment.
        /// </summary>
        /// <remarks>
        /// The number of octets includes IP header(s) and IP payload.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        DroppedOctetDeltaCount = 132,

        /// <summary>
        /// The number of packets since the previous report (if any) of this
        /// Flow dropped by packet treatment.
        /// /// </summary>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        DroppedPacketDeltaCount = 133,

        /// <summary>
        /// The total number of octets in packets of this Flow dropped by packet
        /// treatment since the Metering Process (re -)initialisation for this
        /// Observation Point.
        /// </summary>
        /// <remarks>
        /// The number of octets includes IP header(s) and IP payload.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        DroppedOctetTotalCount = 134,

        /// <summary>
        /// The number of packets of this Flow dropped by packet treatment since
        /// the Metering Process (re -)initialisation for this Observation
        /// Point.
        /// </summary>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        DroppedPacketTotalCount = 135,

        /// <summary>
        /// The reason for Flow termination.
        /// </summary>
        /// <remarks>
        /// The range of values includes the following: 0x01: idle timeout – The
        /// Flow was terminated because it was considered to be idle.
        /// 0x02: active timeout – The Flow was terminated for reporting
        /// purposes while it was still active, for example, after the maximum
        /// lifetime of unreported Flows was reached. 0x03: end of Flow 
        /// detected – The Flow was terminated because the Metering Process
        /// detected signals indicating the end of the Flow, for example, the
        /// TCP FIN flag. 0x04: forced end – The Flow was terminated because of
        /// some external event, for example, a shutdown of the Metering Process
        /// initiated by a network management application. 0x05: lack of
        /// resources – The Flow was terminated because of lack of resource
        /// available to the Metering Process and/or the Exporting Process.
        /// </remarks>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        FlowEndReason = 136,

        /// <summary>
        /// An identifier of a set of common properties that is unique per
        /// Observation Domain and Transport Session.
        /// </summary>
        /// <remarks>
        /// Typically, this Information Element is used to link to information
        /// reported in separate Data Records.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        CommonPropertiesID = 137,

        /// <summary>
        /// An identifier of an Observation Point that is unique per Observation
        /// Domain.
        /// </summary>
        /// <remarks>
        /// It is RECOMMENDED that this identifier is also unique per IPFIX
        /// Device. Typically, this Information Element is used for limiting the
        /// scope of other Information Elements.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        ObservationPointID = 138,

        /// <summary>
        /// Type and Code of the IPv6 ICMP message.
        /// </summary>
        /// <remarks>
        /// The combination of both values is reported as 
        /// <c>ICMP type * 256) +ICMP code</c>.
        /// </remarks>
        [FieldLength(2)]
        [ClrType(typeof(ushort))]
        IcmpTypeCodeIPv6 = 139,

        /// <summary>
        /// The IPv6 address of the system that the MPLS top label will cause
        /// this Flow to be forwarded to.
        /// </summary>
        [FieldLength(16)]
        [ClrType(typeof(IPAddress))]
        MplsTopLabelIPv6Address = 140,

        /// <summary>
        /// An identifier of a line card that is unique per IPFIX Device hosting
        /// an Observation Point.
        /// </summary>
        /// <remarks>
        /// Typically, this Information Element is used for limiting the scope
        /// of other Information Elements.
        /// </remarks>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        LineCardID = 141,

        /// <summary>
        /// An identifier of a line port that is unique per IPFIX Device hosting
        /// an Observation Point.
        /// </summary>
        /// <remarks>
        /// Typically, this Information Element is used for limiting the scope
        /// of other Information Elements.
        /// </remarks>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        PortID = 142,

        /// <summary>
        /// An identifier of a Metering Process that is unique per IPFIX Device.
        /// </summary>
        /// <remarks>
        /// Typically, this Information Element is used for limiting the scope
        /// of other Information Elements. Note that process identifiers are
        /// typically assigned dynamically. The Metering Process may be
        /// re-started with a different ID.
        /// </remarks>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        MeteringProcessID = 143,

        /// <summary>
        /// An identifier of an Exporting Process that is unique per IPFIX
        /// Device.
        /// </summary>
        /// <remarks>
        /// Typically, this Information Element is used for limiting the scope
        /// of other Information Elements.Note that process identifiers are
        /// typically assigned dynamically. The Exporting Process may be
        /// re-started with a different ID.
        /// </remarks>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        ExportingProcessID = 144,

        /// <summary>
        /// An identifier of a Template that is locally unique within a
        /// combination of a Transport session and an Observation Domain.
        /// </summary>
        /// <remarks>
        /// Template IDs 0 - 255 are reserved for Template Sets, Options
        /// Template Sets and other reserved Sets yet to be created. Template
        /// IDs of Data Sets are numbered from 256 to 65535. Typically, this
        /// Information Element is used for limiting the scope of other
        /// Information Elements. Note that after a re-start of the Exporting
        /// Process Template identifiers may be re-assigned.
        /// </remarks>
        [FieldLength(2)]
        [ClrType(typeof(ushort))]
        TemplateID = 145,

        /// <summary>
        /// The identifier of the 802.11 (Wi-Fi) channel used.
        /// </summary>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        WlanChannelID = 146,

        /// <summary>
        /// The Service Set IDentifier (SSID) identifying an 802.11 (Wi-Fi)
        /// network used.
        /// </summary>
        /// <remarks>
        /// According to IEEE.802-11.1999, the SSID is encoded into a string
        /// of up to 32 characters.
        /// </remarks>
        [FieldLength(MaximumLength = 32)]
        [ClrType(typeof(string))]
        WlanSSID = 147,

        /// <summary>
        /// An identifier of a Flow that is unique within an Observation Domain.
        /// </summary>
        /// <remarks>
        /// This Information Element can be used to distinguish between
        /// different Flows if Flow Keys such as IP addresses and port numbers
        /// are not reported or are reported in separate records.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        FlowID = 148,

        /// <summary>
        /// An identifier of an Observation Domain that is locally unique to an
        /// Exporting Process.
        /// </summary>
        /// <remarks>
        /// The Exporting Process uses the Observation Domain ID to uniquely
        /// identify to the Collecting Process the Observation Domain where
        /// Flows were metered. It is RECOMMENDED that this identifier is also
        /// unique per IPFIX Device. A value of 0 indicates that no specific
        /// Observation Domain is identified by this Information Element.
        /// Typically, this Information Element is used for limiting the scope
        /// of other Information Elements.
        /// </remarks>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        ObservationDomainID = 149,

        /// <summary>
        /// The absolute timestamp of the first packet of this Flow.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        FlowStartSeconds = 150,

        /// <summary>
        /// The absolute timestamp of the last packet of this Flow.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        FlowEndSeconds = 151,

        /// <summary>
        /// The absolute timestamp of the first packet of this Flow.
        /// </summary>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        FlowStartMilliseconds = 152,

        /// <summary>
        /// The absolute timestamp of the last packet of this Flow.
        /// </summary>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        FlowEndMilliseconds = 153,

        /// <summary>
        /// The absolute timestamp of the first packet of this Flow.
        /// </summary>
        /// <remarks>
        /// The data type is an unsigned 32-bit integer in network byte order
        /// containing the number of seconds since the UNIX epoch, 1 January
        /// 1970 at 00:00 UTC, as defined in POSIX.1. It is encoded
        /// identically to the IPFIX Message Header Export Time field. It
        /// can represent dates between 1 January 1970 and 7 February 2106
        /// without wraparound; see Section 5.2 for wraparound considerations.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        FlowStartMicroseconds = 154,

        /// <summary>
        /// The absolute timestamp of the last packet of this Flow.
        /// </summary>
        /// <remarks>
        /// See <see cref="FlowStartMicroseconds"/> for data format information.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        FlowEndMicroseconds = 155,

        /// <summary>
        /// The absolute timestamp of the first packet of this Flow.
        /// </summary>
        /// <remarks>
        /// <para> The data type is a 64-bit field encoded according to the NTP
        /// Timestamp format as defined in Section 6 of RFC5905. This field is
        /// made up of two unsigned 32-bit integers in network byte order:
        /// Seconds and Fraction. The Seconds field is the number of seconds
        /// since the NTP epoch, 1st January 1900 at 00:00 UTC. The Fraction field
        /// is the fractional number of seconds in units of 1/(2^32) seconds
        /// (approximately 233 picoseconds). It can represent dates between
        /// 1st January 1900 and 8th February 2036 in the current NTP era; see
        /// Section 5.2 for wraparound considerations.</para>
        /// <para> Note that microseconds and nanoseconds share an identical
        /// encoding.  There is no restriction on the interpretation the
        /// Fraction field for the nanoseconds data type.</para>
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        FlowStartNanoseconds = 156,

        /// <summary>
        /// The absolute timestamp of the last packet of this Flow.
        /// </summary>
        /// <remarks>
        /// See <see cref="FlowStartNanoseconds"/> for data format information.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        FlowEndNanoseconds = 157,

        /// <summary>
        /// This is a relative timestamp only valid within the scope of a single
        /// IPFIX Message.
        /// </summary>
        /// <remarks>It contains the negative time offset of the first observed
        /// packet of this Flow relative to the export time specified in the
        /// IPFIX Message Header.
        /// </remarks>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        FlowStartDeltaMicroseconds = 158,

        /// <summary>
        /// This is a relative timestamp only valid within the scope of a single
        /// IPFIX Message.
        /// </summary>
        /// <remarks>
        /// It contains the negative time offset of the last observed packet of
        /// this Flow relative to the export time specified in the IPFIX Message
        /// Header.
        /// </remarks>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        FlowEndDeltaMicroseconds = 159,

        /// <summary>
        /// The absolute timestamp of the last (re-)initialisation of the IPFIX
        /// Device.
        /// </summary>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        SystemInitialisaionTimeMilliseconds = 160,

        /// <summary>
        /// The difference in time between the first observed packet of this
        /// Flow and the last observed packet of this Flow.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        FlowDurationMilliseconds = 161,

        /// <summary>
        /// The difference in time between the first observed packet of this
        /// Flow and the last observed packet of this Flow.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        FlowDurationMicroseconds = 162,

        /// <summary>
        /// The total number of Flows observed in the Observation Domain since
        /// the Metering Process (re-) initialisation for this Observation
        /// Point.
        /// </summary>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        ObservedFlowTotalCount = 163,

        /// <summary>
        /// The total number of observed IP packets that the Metering Process
        /// did not process since the (re-) initialisation of the Metering
        /// Process.
        /// </summary>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        IgnoredPacketTotalCount = 164,

        /// <summary>
        /// The total number of octets in observed IP packets (including the IP
        /// header) that the Metering Process did not process since the
        /// (re-) initialisation of the Metering Process.
        /// </summary>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        IgnoredOctetTotalCount = 165,

        /// <summary>
        /// The total number of Flow Records that were generated by the Metering
        /// Process and dropped by the Metering Process or by the Exporting
        /// Process instead of being sent to the Collecting Process.
        /// </summary>
        /// <remarks>
        /// There are several potential reasons for this including resource
        /// shortage and special Flow export policies.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        NotSentFlowTotalCount = 166,

        /// <summary>
        /// The total number of packets in Flow Records that were generated by
        /// the Metering Process and dropped by the Metering Process or by the
        /// Exporting Process instead of being sent to the Collecting Process.
        /// </summary>
        /// <remarks>
        /// There are several potential reasons for this including resource
        /// shortage and special Flow export policies.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        NotSentPacketTotalCount = 167,

        /// <summary>
        /// The total number of octets in packets in Flow Records that were
        /// generated by the Metering Process and dropped by the Metering
        /// Process or by the Exporting Process instead of being sent to the
        /// Collecting Process.
        /// </summary>
        /// <remarks>
        /// There are several potential reasons for this including resource
        /// shortage and special Flow export policies.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        NotSentOctetTotalCount = 168,

        /// <summary>
        /// IPv6 destination address prefix.
        /// </summary>
        [FieldLength(16)]
        [ClrType(typeof(IPAddress))]
        DestinationIPv6Prefix = 169,

        /// <summary>
        /// IPv6 source address prefix.
        /// </summary>
        [FieldLength(16)]
        [ClrType(typeof(IPAddress))]
        SourceIPv6Prefix = 170,

        /// <summary>
        /// The definition of this Information Element is identical to the
        /// definition of Information Element <see cref="OctetTotalCount"/>,
        /// except that it reports a potentially modified value caused by a
        /// middlebox function after the packet passed the Observation Point.
        /// </summary>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        PostOctetTotalCount = 171,

        /// <summary>
        /// The definition of this Information Element is identical to the 
        /// definition of Information Element <see cref="PacketTotalCount"/>,
        /// except that it reports a potentially modified value caused by a
        /// middlebox function after the packet passed the Observation Point.
        /// </summary>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        PostPacketTotalCount = 172,

        /// <summary>
        /// This set of bit fields is used for marking the Information Elements
        /// of a Data Record that serve as Flow Key.
        /// </summary>
        /// <remarks>
        /// Each bit represents an Information Element in the Data Record, with
        /// the n-th least significant bit representing the n-th Information
        /// Element. A bit set to value 1 indicates that the corresponding
        /// Information Element is a Flow Key of the reported Flow. A bit set to
        /// value 0 indicates that this is not the case. If the Data Record
        /// contains more than 64 Information Elements, the corresponding
        /// Template SHOULD be designed such that all Flow Keys are among the
        /// first 64 Information Elements, because the flowKeyIndicator only
        /// contains 64 bits.If the Data Record contains less than 64
        /// Information Elements, then the bits in the flowKeyIndicator for
        /// which no corresponding Information Element exists MUST have the
        /// value 0.
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        FlowKeyIndicator = 173,

        /// <summary>
        /// The total number of outgoing multicast packets sent for packets of
        /// this Flow by a multicast daemon within the Observation Domain since
        /// the Metering Process (re-) initialisation.
        /// </summary>
        /// <remarks>
        /// This property cannot necessarily be observed at the Observation
        /// Point, but may be retrieved by other means.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        PostMulticastPacketTotalCount = 174,

        /// <summary>
        /// The total number of octets in outgoing multicast packets sent for
        /// packets of this Flow by a multicast daemon in the Observation Domain
        /// since the Metering Process (re-) initialisation.
        /// </summary>
        /// <remarks>
        /// <para>This property cannot necessarily be observed at the
        /// Observation Point, but may be retrieved by other means.</para>
        /// <para>The number of octets includes IP header(s) and IP payload.
        /// </para>
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        PostMulticastOctetTotalCount = 175,

        /// <summary>
        /// Type of the IPv4 ICMP message.
        /// </summary>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        IcmpTypeIPv4 = 176,

        /// <summary>
        /// Code of the IPv4 ICMP message.
        /// </summary>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        IcmpCodeIPv4 = 177,

        /// <summary>
        /// Type of the IPv6 ICMP message.
        /// </summary>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        IcmpTypeIPv6 = 178,

        /// <summary>
        /// Code of the IPv6 ICMP message.
        /// </summary>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        IcmpCodeIPv6 = 179,

        /// <summary>
        /// The source port identifier in the UDP header.
        /// </summary>
        [FieldLength(2)]
        [ClrType(typeof(ushort))]
        UdpSourcePort = 180,

        /// <summary>
        /// The destination port identifier in the UDP header.
        /// </summary>
        [FieldLength(2)]
        [ClrType(typeof(ushort))]
        UdpDestinationPort = 181,

        /// <summary>
        /// The source port identifier in the TCP header.
        /// </summary>
        [FieldLength(2)]
        [ClrType(typeof(ushort))]
        TcpSourcePort = 182,

        /// <summary>
        /// The destination port identifier in the TCP header.
        /// </summary>
        [FieldLength(2)]
        [ClrType(typeof(ushort))]
        TcpDestinationPort = 183,

        /// <summary>
        /// The sequence number in the TCP header.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        TcpSequenceNumber = 184,

        /// <summary>
        /// The acknowledgement number in the TCP header.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        TcpAcknowledgementNumber = 185,

        /// <summary>
        /// The window field in the TCP header.
        /// </summary>
        /// <remarks>
        /// If the TCP window scale is supported, then TCP window scale must be
        /// known to fully interpret the value of this information.
        /// </remarks>
        [FieldLength(2)]
        [ClrType(typeof(ushort))]
        TcpWindowSize = 186,

        /// <summary>
        /// The urgent pointer in the TCP header.
        /// </summary>
        [FieldLength(2)]
        [ClrType(typeof(ushort))]
        TcpUrgentPointer = 187,

        /// <summary>
        /// The length of the TCP header.
        /// </summary>
        /// <remarks>Note that the value of this Information Element is
        /// different from the value of the Data Offset field in the TCP
        /// header.The Data Offset field indicates the length of the TCP
        /// header in units of 4 octets. This Information Elements specifies
        /// the length of the TCP header in units of octets.
        /// </remarks>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        TcpHeaderLength = 188,

        /// <summary>
        /// The length of the IP header.
        /// </summary>
        /// <remarks>
        /// For IPv6, the value of this Information Element is 40.
        /// </remarks>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        IPHeaderLength = 189,

        /// <summary>
        /// The total length of the IPv4 packet.
        /// </summary>
        [FieldLength(2)]
        [ClrType(typeof(ushort))]
        TotalLengthIPv4 = 190,

        /// <summary>
        /// This Information Element reports the value of the Payload Length
        /// field in the IPv6 header.
        /// </summary>
        /// <remarks>
        /// Note that IPv6 extension headers belong to the payload. Also note
        /// that in case of a jumbo payload option the value of the Payload
        /// Length field in the IPv6 header is zero and so will be the value
        /// reported by this Information Element.
        /// </remarks>
        [FieldLength(2)]
        [ClrType(typeof(ushort))]
        PayloadLengthIPv6 = 191,

        /// <summary>
        /// For IPv4, the value of the Information Element matches the value of
        /// the Time to Live (TTL) field in the IPv4 packet header. For IPv6,
        /// the value of the Information Element matches the value of the Hop
        /// Limit field in the IPv6 packet header.
        /// </summary>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        IPTtl = 192,

        /// <summary>
        /// The value of the Next Header field of the IPv6 header. 
        /// </summary>
        /// <remarks>
        /// The value identifies the type of the following IPv6 extension header
        /// or of the following IP payload. Valid values are defined in the IANA
        /// Protocol Numbers registry.
        /// </remarks>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        NextHeaderIPv6 = 193,

        /// <summary>
        /// The size of the MPLS packet without the label stack.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        MplsPayloadLength = 194,

        /// <summary>
        /// The value of a Differentiated Services Code Point (DSCP)encoded in
        /// the Differentiated Services field.
        /// </summary>
        /// The Differentiated Services field spans the most significant 6 bits
        /// of the IPv4 TOS field or the IPv6 Traffic Class field, respectively.
        /// This Information Element encodes only the 6 bits of the
        /// Differentiated Services field.Therefore, its value may range from 0
        /// to 63.
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        IPDifferentiatedServicesCodePoint = 195,

        /// <summary>
        /// The value of the IP Precedence.
        /// </summary>
        /// <remarks>
        /// The IP Precedence value is encoded in the first 3 bits of the IPv4
        /// TOS field or the IPv6 Traffic Class field, respectively. This
        /// Information Element encodes only these 3 bits. Therefore, its value
        /// may range from 0 to 7.
        /// </remarks>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        IPPrecedence = 196,

        /// <summary>
        /// Fragmentation properties indicated by flags in the IPv 4packet
        /// header or the IPv6 Fragment header, respectively.
        /// </summary>
        /// <remarks>
        /// Bit 0: (RS) Reserved. The value of this bit MUST be 0 until
        /// specified otherwise. Bit 1: (DF) 0 = May Fragment, 1 = Don't
        /// Fragment. Corresponds to the value of the DF flag in the IPv4
        /// header. Will always be 0 for IPv6 unless a don't fragment feature
        /// is introduced to IPv6. Bit 2: (MF) 0 = Last Fragment, 1 = More
        /// Fragments. Corresponds to the MF flag in the IPv4 header or to the
        /// M flag in the IPv6 Fragment header, respectively.The value is 0 for
        /// IPv6 if there is no fragment header. Bits 3-7: (DC) Don't Care. The
        /// values of these bits are irrelevant.
        /// <c>
        /// 0   1   2   3   4   5   6   7
        /// +---+---+---+---+---+---+---+---+
        /// | R | D | M | D | D | D | D | D |
        /// | S | F | F | C | C | C | C | C |
        /// +---+---+---+---+---+---+---+---+
        /// </c>
        /// </remarks>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        FragmentFlags = 197,

        /// <summary>
        /// The sum of the squared numbers of octets per incoming packet since
        /// the previous report (if any) for this Flow at the Observation Point.
        /// </summary>
        /// <remarks>
        /// The number of octets includes IP header(s) and IP payload.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        OctetDeltaSumOfSquares = 198,

        /// <summary>
        /// The total sum of the squared numbers of octets in incoming packets
        /// for this Flow at the Observation Point since the Metering Process
        /// (re-) initialisation for this Observation Point. 
        /// </summary>
        /// <remarks>
        /// The number of octets includes IP header(s) and IP payload.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        OctetTotalSumOfSquares = 199,

        /// <summary>
        /// The TTL field from the top MPLS label stack entry, i.e.  the last
        /// label that was pushed.
        /// /// </summary>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        MplsTopLabelTTL = 200,

        /// <summary>
        /// The length of the MPLS label stack in units of octets.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        MplsLabelStackLength = 201,

        /// <summary>
        /// The number of labels in the MPLS label stack.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        MplsLabelStackDepth = 202,

        /// <summary>
        /// The Exp field from the top MPLS label stack entry, i.e. the last
        /// label that was pushed.
        /// </summary>
        /// <remarks>
        /// Bits 0-4: Don't Care, value is irrelevant. Bits 5-7: MPLS Exp field.
        /// <c>
        /// 0   1   2   3   4   5   6   7
        /// +---+---+---+---+---+---+---+---+
        /// |     don't care    |    Exp    |
        /// +---+---+---+---+---+---+---+---+
        /// </c>
        /// </remarks>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        MplsTopLabelExp = 203,

        /// <summary>
        /// The effective length of the IP payload.
        /// </summary>
        /// <remarks>
        /// For IPv4 packets, the value of this Information Element is the
        /// difference between the total length of the IPv4 packet (as reported
        /// by Information Element totalLengthIPv4) and the length of the IPv4
        /// header(as reported by Information Element headerLengthIPv4). For
        /// IPv6, the value of the Payload Length field in the IPv6 header is
        /// reported except in the case that the value of this field is zero and
        /// that there is a valid jumbo payload option.In this case, the value
        /// of the Jumbo Payload Length field in the jumbo payload option is
        /// reported.
        /// </remarks>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        IPPayloadLength = 204,

        /// <summary>
        /// The value of the Length field in the UDP header.
        /// </summary>
        [FieldLength(2)]
        [ClrType(typeof(ushort))]
        UdpMessageLength = 205,

        /// <summary>
        /// If the IP destination address is not a reserved multicast address,
        /// then the value of all bits of the octet (including the reserved
        /// ones) is zero. The first bit of this octet is set to 1 if the
        /// Version field of the IP header has the value 4 and if the
        /// Destination Address field contains a reserved multicast address in
        /// the range from 224.0.0.0 to 239.255.255.255. Otherwise, this bit is
        /// set to 0. The second and third bits of this octet are reserved for
        /// future use. The remaining bits of the octet are only set to values
        /// other than zero if the IP Destination Address is a reserved IPv6
        /// multicast address.Then the fourth bit of the octet is set to the
        /// value of the T flag in the IPv6 multicast address and the remaining
        /// four bits are set to the value of the scope field in the IPv6
        /// multicast address.
        /// </summary>
        /// <remarks>
        /// <c>
        /// 0      1      2      3      4      5      6      7
        /// +------+------+------+------+------+------+------+------+
        /// |   IPv6 multicast scope    |  T   | RES. | RES. | MCv4 |
        /// +------+------+------+------+------+------+------+------+</c>
        /// </c>
        /// Bits 0 - 3: set to value of multicast scope if IPv6 multicast.
        /// Bit 4: set to value of T flag, if IPv6 multicast. Bits 5-6: reserved
        /// for future use. Bit 7: set to 1 if IPv4 multicast.
        /// </remarks>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        IsMulticast = 206,

        /// <summary>
        /// The value of the Internet Header Length (IHL) field in the IPv4
        /// header.
        /// </summary>
        /// <remarks>
        /// It specifies the length of the header in units of 4 octets. Please
        /// note that its unit is different from most of the other Information
        /// Elements reporting length values.
        /// </remarks>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        IPv4InternetHeaderLength = 207,

        /// <summary>
        /// IPv4 options in packets of this Flow.
        /// </summary>
        /// <remarks>
        /// <para>The information is encoded in a set of bit fields. For each
        /// valid IPv4 option type, there is a bit in this set. The bit is set
        /// to 1 if any observed packet of this Flow contains the corresponding
        /// IPv4 option type. Otherwise, if no observed packet of this Flow
        /// contained the respective IPv4 option type, the value of the
        /// corresponding bit is 0. The list of valid IPv4 options is maintained
        /// by IANA. Note that for identifying an option not just the 5-bit
        /// Option Number, but all 8 bits of the Option Type need to match one
        /// of the IPv4 options specified at
        /// http://www.iana.org/assignments/ip-parameters.</para>
        /// <para>Options are mapped to bits according to their option numbers.
        /// Option number X is mapped to bit X. The mapping is illustrated by
        /// the figure below.
        /// <c>
        ///         0      1      2      3      4      5      6      7
        ///     +------+------+------+------+------+------+------+------+
        /// ... |  RR  |CIPSO |E-SEC |  TS  | LSR  |  SEC | NOP  | EOOL |
        ///     +------+------+------+------+------+------+------+------+
        ///         8      9     10     11     12     13     14     15
        ///     +------+------+------+------+------+------+------+------+
        /// ... |ENCODE| VISA | FINN | MTUR | MTUP | ZSU  | SSR  | SID  | ...
        ///     +------+------+------+------+------+------+------+------+
        ///        16     17     18     19     20     21     22     23
        ///     +------+------+------+------+------+------+------+------+
        /// ... | DPS  |NSAPA | SDB  |RTRALT|ADDEXT|  TR  | EIP  |IMITD | ...
        ///     +------+------+------+------+------+------+------+------+
        ///
        ///        24     25     26     27     28     29     30     31
        ///     +------+------+------+------+------+------+------+------+
        /// ... |      | EXP  |   to be assigned by IANA  |  QS  | UMP  |
        ///     +------+------+------+------+------+------+------+------+
        /// 
        ///     Type   Option
        /// Bit Value  Name    Reference
        /// ---+-----+-------+------------------------------------
        /// 0      7   RR      Record Route, RFC 791
        /// 1    134   CIPSO   Commercial Security
        /// 2    133   E-SEC   Extended Security, RFC 1108
        /// 3     68   TS      Time Stamp, RFC 791
        /// 4    131   LSR     Loose Source Route, RFC791
        /// 5    130   SEC     Security, RFC 1108
        /// 6      1   NOP     No Operation, RFC 791
        /// 7      0   EOOL    End of Options List, RFC 791
        /// 8     15   ENCODE
        /// 9    142   VISA    Experimental Access Control
        /// 10   205   FINN    Experimental Flow Control
        /// 11    12   MTUR    (obsoleted) MTU Reply, RFC 1191
        /// 12    11   MTUP    (obsoleted) MTU Probe, RFC 1191
        /// 13    10   ZSU     Experimental Measurement
        /// 14   137   SSR     Strict Source Route, RFC 791
        /// 15   136   SID     Stream ID, RFC 791
        /// 16   151   DPS     Dynamic Packet State
        /// 17   150   NSAPA   NSAP Address
        /// 18   149   SDB     Selective Directed Broadcast
        /// 19   147   ADDEXT  Address Extension
        /// 20   148   RTRALT  Router Alert, RFC 2113
        /// 21    82   TR      Traceroute, RFC 3193
        /// 22   145   EIP     Extended Internet Protocol, RFC 1385
        /// 23   144   IMITD   IMI Traffic Descriptor
        /// 25    30   EXP     RFC3692-style Experiment
        /// 25    94   EXP     RFC3692-style Experiment
        /// 25   158   EXP     RFC3692-style Experiment
        /// 25   222   EXP     RFC3692-style Experiment
        /// 30    25   QS      Quick-Start
        /// 31   152   UMP     Upstream Multicast Pkt.
        /// ...  ...   ...     Further options numbers may be assigned by IANA
        /// </c>
        /// </para>
        /// </remarks>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        IPv4Options = 208,

        /// <summary>
        /// TCP options in packets of this Flow.
        /// </summary>
        /// <remarks>
        /// <para>The information is encoded in a set of bit fields. For each
        /// TCP option, there is a bit in this set. The bit is set to 1 if any
        /// observed packet of this Flow contains the corresponding TCP option.
        /// Otherwise, if no observed packet of this Flow contained the
        /// respective TCP option, the value of the corresponding bit is 0.
        /// </para>
        /// <para>
        /// Options are mapped to bits according to their option numbers.
        /// Option number X is mapped to bit X. TCP option numbers are
        /// maintained by IANA.
        /// <c>
        ///     0     1     2     3     4     5     6     7
        /// +-----+-----+-----+-----+-----+-----+-----+-----+
        /// |   7 |   6 |   5 |   4 |   3 |   2 |   1 |   0 |  ...
        /// +-----+-----+-----+-----+-----+-----+-----+-----+
        /// 
        ///         8     9    10    11    12    13    14    15
        ///     +-----+-----+-----+-----+-----+-----+-----+-----+
        /// ... |  15 |  14 |  13 |  12 |  11 |  10 |   9 |   8 |...
        ///     +-----+-----+-----+-----+-----+-----+-----+-----+
        ///
        ///        16    17    18    19    20    21    22    23
        ///     +-----+-----+-----+-----+-----+-----+-----+-----+
        /// ... |  23 |  22 |  21 |  20 |  19 |  18 |  17 |  16 |...
        ///     +-----+-----+-----+-----+-----+-----+-----+-----+
        ///
        ///                 . . .
        ///
        ///        56    57    58    59    60    61    62    63
        ///     +-----+-----+-----+-----+-----+-----+-----+-----+
        /// ... |  63 |  62 |  61 |  60 |  59 |  58 |  57 |  56 |
        ///     +-----+-----+-----+-----+-----+-----+-----+-----+
        /// </c>
        /// </para>
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        TcpOptions = 209,

        /// <summary>
        /// The value of this Information Element is always a sequence of 0x00
        /// values.
        /// </summary>
        [FieldLength]
        PaddingOctets = 210,

        /// <summary>
        /// An IPv4 address to which the Exporting Process sends Flow
        /// information.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(IPAddress))]
        CollectorIPv4Address = 211,

        /// <summary>
        /// An IPv6 address to which the Exporting Process sends Flow
        /// information.
        /// </summary>
        [FieldLength(16)]
        [ClrType(typeof(IPAddress))]
        CollectorIPv6Address = 212,

        /// <summary>
        /// The index of the interface from which IPFIX Messages sent by the 
        /// Exporting Process to a Collector leave the IPFIX Device.
        /// </summary>
        /// <remarks>
        /// The value matches the value of managed object 'ifIndex' as defined
        /// in RFC2863. Note that ifIndex values are not assigned statically to
        /// an interface and that the interfaces may be renumbered every time
        /// the device's management system is re-initialised, as specified in
        /// RFC2863.
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        ExportInterface = 213,

        /// <summary>
        /// The protocol version used by the Exporting Process for sending Flow
        /// information.
        /// </summary>
        /// <remarks>
        /// The protocol version is given by the value of the Version Number
        /// field in the Message Header. The protocol version is 10 for IPFIX
        /// and 9 for NetFlow version 9. A value of 0 indicates that no export
        /// protocol is in use.
        /// </remarks>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        ExportProtocolVersion = 214,

        /// <summary>
        /// The value of the protocol number used by the Exporting Process for
        /// sending Flow information.
        /// </summary>
        /// <remarks>
        /// The protocol number identifies the IP packet payload type. Protocol
        /// numbers are defined in the IANA Protocol Numbers registry. In
        /// Internet Protocol version 4 (IPv4), this is carried in the Protocol
        /// field. In Internet Protocol version 6 (IPv6), this is carried in the
        /// Next Header field in the last extension header of the packet.
        /// </remarks>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        ExportTransportProtocol = 215,

        /// <summary>
        /// The destination port identifier to which the Exporting Process sends
        /// Flow information.
        /// </summary>
        /// <remarks>
        /// For the transport protocols UDP, TCP, and SCTP, this is the
        /// destination port number. This field MAY also be used for future
        /// transport protocols that have 16-bit source port identifiers.
        /// </remarks>
        [FieldLength(2)]
        [ClrType(typeof(ushort))]
        CollectorTransportPort = 216,

        /// <summary>
        /// The source port identifier from which the Exporting Process sends
        /// Flow information.
        /// </summary>
        /// <remarks>
        /// For the transport protocols UDP, TCP, and SCTP, this is the source
        /// port number. This field MAY also be used for future transport
        /// protocols that have 16-bit source port identifiers. This field may
        /// be useful for distinguishing multiple Exporting Processes that use
        /// the same IP address.
        /// </remarks>
        [FieldLength(2)]
        [ClrType(typeof(ushort))]
        ExporterTransportPort = 217,

        /// <summary>
        /// The total number of packets of this Flow with TCP Synchronise
        /// sequence numbers (SYN) flag set.
        /// </summary>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        TcpSynTotalCount = 218,

        /// <summary>
        /// The total number of packets of this Flow with TCP No more data from
        /// sender (FIN) flag set.
        /// </summary>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        TcpFinTotalCount = 219,

        /// <summary>
        /// The total number of packets of this Flow with TCP Reset the
        /// connection (RST) flag set.
        /// </summary>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        TcpRstTotalCount = 220,

        /// <summary>
        /// The total number of packets of this Flow with TCP Push Function
        /// (PSH) flag set.
        /// </summary>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        TcpPshTotalCount = 221,

        /// <summary>
        /// The total number of packets of this Flow with TCP Acknowledgment
        /// field significant (ACK) flag set.
        /// </summary>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        TcpAckTotalCount = 222,

        /// <summary>
        /// The total number of packets of this Flow with TCP Urgent Pointer
        /// field significant (URG) flag set.
        /// </summary>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        TcpUrgTotalCount = 223,

        /// <summary>
        /// The total length of the IP packet.
        /// </summary>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        IPTotalLength = 224,

        /// <summary>
        /// The definition of this Information Element is identical to the
        /// definition of Information Element <see cref="SourceIPv4Address"/>,
        /// except that it reports a modified value caused by a NAT middlebox
        /// function after the packet passed the Observation Point.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(IPAddress))]
        PostNatSourceIPv4Address = 225,

        /// <summary>
        /// The definition of this Information Element is identical to the
        /// definition of Information Element
        /// <see cref="DestinationIPv4Address"/>, except that it reports a
        /// modified value caused by a NAT middlebox function after the packet
        /// passed the Observation Point.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(IPAddress))]
        PostNatDestinationIPv4Address = 226,

        /// <summary>
        /// The definition of this Information Element is identical to the
        /// definition of Information Element <see cref="SourceTransportPort"/>,
        /// except that it reports a modified value caused by a Network Address
        /// Port Translation (NAPT) middlebox function after the packet passed
        /// the Observation Point.
        /// </summary>
        [FieldLength(2)]
        [ClrType(typeof(ushort))]
        PostNaptSourceTransportPort = 227,

        /// <summary>
        /// The definition of this Information Element is identical to the
        /// definition of Information Element
        /// <see cref="DestinationTransportPort"/>, except that it reports a
        /// modified value caused by a Network Address Port Translation (NAPT)
        /// middlebox function after the packet passed the Observation Point.
        /// </summary>
        [FieldLength(2)]
        [ClrType(typeof(ushort))]
        PostNaptDestinationTransportPort = 228,

        /// <summary>
        /// Indicates whether the session was created because traffic originated
        /// in the private or public address realm.
        /// </summary>
        /// <remarks>
        /// <see cref="PostNatSourceIPv4Address"/>,
        /// <see cref="PostNatDestinationIPv4Address"/>,
        /// <see cref="PostNaptSourceTransportPort"/> and 
        /// <see cref="PostNaptDestinationTransportPort"/>are qualified with the
        /// address realm in perspective. The allowed values are: Private: 1;
        /// Public: 2.
        /// </remarks>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        NatOriginatingAddressRealm = 229,

        /// <summary>
        /// This Information Element identifies a NAT event.
        /// </summary>
        /// <remarks>
        /// This IE identifies the type of a NAT event. Examples of NAT events
        /// include, but are not limited to, NAT translation create, NAT
        /// translation delete, Threshold Reached, or Threshold Exceeded, etc.
        /// Values for this Information Element are listed in the NAT Event
        /// Type registry, see
        /// http://www.iana.org/assignments/ipfix/ipfix.xhtml#ipfix-nat-event-type.
        /// New assignments of values will be administered by IANA and are
        /// subject to Expert Review [RFC8126]. Experts need to check 
        /// definitions of new values for completeness, accuracy, and
        /// redundancy.
        [FieldLength(1)]
        [ClrType(typeof(NatEventType))]
        NatEvent = 230,

        /// <summary>
        /// The total number of layer 4 payload bytes in a flow from the
        /// initiator since the previous report.
        /// </summary>
        /// <remarks>
        /// The initiator is the device which triggered the session creation and
        /// remains the same for the life of the session.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        InitiatorOctets = 231,

        /// <summary>
        /// The total number of layer 4 payload bytes in a flow from the
        /// responder since the previous report.
        /// </summary>
        /// <remarks>
        /// The responder is the device which replies to the initiator and
        /// remains the same for the life of the session.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        ResponderOctets = 232,

        /// <summary>
        /// Indicates a firewall event.
        /// </summary>
        /// <remarks>
        /// The allowed values are: 0 - Ignore (invalid); 1 - Flow Created;
        /// 2 - Flow Deleted; 3 - Flow Denied; 4 - Flow Alert; 5 - Flow Update.
        /// </remarks>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        FirewallEvent = 233,

        /// <summary>
        /// An unique identifier of the VRFname where the packets of this flow
        /// are being received.
        /// </summary>
        /// <remarks>
        /// This identifier is unique per Metering Process.
        /// </remarks>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        IngressVrfID = 234,

        /// <summary>
        /// An unique identifier of the VRFname where the packets of this flow
        /// are being sent.
        /// </summary>
        /// <remarks>
        /// This identifier is unique per Metering Process.
        /// </remarks>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        EgressVrfID = 235,

        /// <summary>
        /// The name of a VPN Routing and Forwarding table (VRF).
        /// </summary>
        [ClrType(typeof(string))]
        VrfName = 236,

        /// <summary>
        /// The definition of this Information Element is identical to the
        /// definition of Information Element <see cref="MplsTopLabelExp"/>,
        /// except that it reports a potentially modified value caused by a
        /// middlebox function after the packet passed the Observation Point.
        /// </summary>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        PostMplsTopLabelExp = 237,

        /// <summary>
        /// The scale of the window field in the TCP header.
        /// </summary>
        [FieldLength(2)]
        [ClrType(typeof(ushort))]
        TcpWindowScale = 238,

        /// <summary>
        /// A description of the direction assignment method used to assign the
        /// Biflow Source and Destination.
        /// </summary>
        /// <remarks>
        /// <para>/// This Information Element MAY be present in a Flow Data
        /// Record, or applied to all flows exported from an Exporting Process
        /// or Observation Domain using IPFIX Options. If this Information
        /// Element is not present in a Flow Record or associated with a Biflow
        /// via scope, it is assumed that the configuration of the direction
        /// assignment method is done out-of-band.</para>
        /// <para>Note that when using IPFIX Options to apply this Information
        /// Element to all flows within an Observation Domain or from an
        /// Exporting Process, the Option SHOULD be sent reliably. If reliable
        /// transport is not available (i.e. when using UDP), this Information
        /// Element SHOULD appear in each Flow Record.</para>
        /// <para>This field may take the following values:
        /// <c>
        /// +-------+------------------+----------------------------------------+
        /// | Value | Name             | Description                            |
        /// +-------+------------------+----------------------------------------+
        /// | 0x00  | arbitrary        | Direction was assigned arbitrarily.    |
        /// | 0x01  | initiator        | The Biflow Source is the flow          |
        /// |       |                  | initiator, as determined by the        |
        /// |       |                  | Metering Process' best effort to       |
        /// |       |                  | detect the initiator.                  |
        /// | 0x02  | reverseInitiator | The Biflow Destination is the flow     |
        /// |       |                  | initiator, as determined by the        |
        /// |       |                  | Metering Process' best effort to       |
        /// |       |                  | detect the initiator.  This value is   |
        /// |       |                  | provided for the convenience of        |
        /// |       |                  | Exporting Processes to revise an       |
        /// |       |                  | initiator estimate without re-encoding |
        /// |       |                  | the Biflow Record.                     |
        /// | 0x03  | perimeter        | The Biflow Source is the endpoint      |
        /// |       |                  | outside of a defined perimeter.  The   |
        /// |       |                  | perimeter's definition is implicit in  |
        /// |       |                  | the set of Biflow Source and Biflow    |
        /// |       |                  | Destination addresses exported in the  |
        /// |       |                  | Biflow Records.                        |
        /// +-------+------------------+----------------------------------------+
        /// </c>
        ///</para>
        ///</remarks>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        BiflowDirection = 239,

        /// <summary>
        /// The difference between the length of an Ethernet frame (minus the
        /// FCS) and the length of its MAC Client Data section (including any
        /// padding) as defined in section 3.1 of [IEEE.802-3.2005].
        /// </summary>
        /// <remarks>
        /// It does not include the Preamble, SFD and Extension field lengths.
        /// </remarks>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        EthernetHeaderLength = 240,

        /// <summary>
        /// The length of the MAC Client Data section (including any padding) of
        /// a frame as defined in section 3.1 of [IEEE.802-3.2005].
        /// /// </summary>
        [FieldLength(2)]
        [ClrType(typeof(ushort))]
        EthernetPayloadLength = 241,

        /// <summary>
        /// The total length of the Ethernet frame (excluding the Preamble, SFD,
        /// Extension and FCS fields) as described in section 3.1 of
        /// [IEEE.802-3.2005].
        /// </summary>
        [FieldLength(2)]
        [ClrType(typeof(ushort))]
        EthernetTotalLength = 242,

        /// <summary>
        /// The value of the 12-bit VLAN Identifier portion of the Tag Control
        /// Information field of an Ethernet frame.
        /// </summary>
        /// <remarks>
        /// The structure and semantics within the Tag Control Information field
        /// are defined in [IEEE802.1Q]. In Provider Bridged Networks, it
        /// represents the Service VLAN identifier in the Service VLAN Tag
        /// (S-TAG) Tag Control Information (TCI) field or the Customer VLAN
        /// identifier in the Customer VLAN Tag (C-TAG) Tag Control Information
        /// (TCI) field as described in [IEEE802.1Q]. In Provider Backbone
        /// Bridged Networks, it represents the Backbone VLAN identifier in the
        /// Backbone VLAN Tag (B-TAG) Tag Control Information (TCI) field as
        /// described in [IEEE802.1Q]. In a virtual link between a host system
        /// and EVB bridge, it represents the Service VLAN identifier indicating
        /// S-channel as described in [IEEE802.1Qbg]. In the case of a
        /// multi-tagged frame, it represents the outer tag's VLAN identifier,
        /// except for I-TAG.
        [FieldLength(2)]
        [ClrType(typeof(ushort))]
        Dot1QVlanID = 243,

        /// <summary>
        /// The value of the 3-bit User Priority portion of the Tag Control
        /// Information field of an Ethernet frame.
        /// </summary>
        /// <remarks>
        /// The structure and semantics within the Tag Control Information field
        /// are defined in [IEEE802.1Q]. In the case of multi-tagged frame, it
        /// represents the 3-bit Priority Code Point (PCP) portion of the outer
        /// tag's Tag Control Information (TCI) field as described in
        /// [IEEE802.1Q], except for I-TAG.
        /// </remarks>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        Dot1QPriority = 244,

        /// <summary>
        /// The value represents the Customer VLAN identifier in the Customer
        /// VLAN Tag (C-TAG) Tag Control Information (TCI) field as described in
        /// [IEEE802.1Q].
        /// </summary>
        [FieldLength(2)]
        [ClrType(typeof(ushort))]
        Dot1QCustomerVlanID = 245,

        /// <summary>
        /// The value represents the 3-bit Priority Code Point (PCP) portion of
        /// the Customer VLAN Tag (C-TAG) Tag Control Information (TCI) field as
        /// described in [IEEE802.1Q].
        /// </summary>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        Dot1QCustomerPriority = 246,

        /// <summary>
        /// The EVC Service Attribute which uniquely identifies the Ethernet
        /// Virtual Connection (EVC) within a Metro Ethernet Network, as defined
        /// in section 6.2 of MEF 10.1.
        /// </summary>
        /// <remarks>
        /// The MetroEVCID is encoded in a string of up to 100 characters.
        /// </remarks>
        [FieldLength(MaximumLength = 100)]
        [ClrType(typeof(string))]
        MetroEvcID = 247,

        /// <summary>
        /// The 3-bit EVC Service Attribute which identifies the type of service
        /// provided by an EVC.
        /// </summary>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        MetroEvcType = 248,

        /// <summary>
        /// A 32-bit non-zero connection identifier, which together with the
        /// <see cref="PseudoWireType"/>, identifies the Pseudo Wire (PW) as
        /// defined in [RFC8077].
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        PseudoWireID = 249,

        /// <summary>
        /// The value of this information element identifies the type of MPLS
        /// Pseudo Wire (PW) as defined in [RFC4446].
        /// /// </summary>
        [FieldLength(2)]
        [ClrType(typeof(ushort))]
        PseudoWireType = 250,

        /// <summary>
        /// The 32-bit Preferred Pseudo Wire (PW) MPLS Control Word as defined
        /// in Section 3 of [RFC4385].
        /// /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        PseudoWireControlWord = 251,

        /// <summary>
        /// The index of a networking device's physical interface (e.g. a switch
        /// port) where packets of this flow are being received.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        IngressPhysicalInterface = 252,

        /// <summary>
        /// The index of a networking device's physical interface (e.g. a switch
        /// port) where packets of this flow are being sent.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        EgressPhysicalInterface = 253,

        /// <summary>
        /// The definition of this Information Element is identical to the
        /// definition of Information Element <see cref="Dot1QVlanID"/>, except
        /// that it reports a potentially modified value caused by a middlebox
        /// function after the packet passed the Observation Point.
        /// </summary>
        [FieldLength(2)]
        [ClrType(typeof(ushort))]
        PostDot1QVlanID = 254,

        /// <summary>
        /// The definition of this Information Element is identical to the
        /// definition of Information Element <see cref="Dot1QCustomerVlanID"/>,
        /// except that it reports a potentially modified value caused by a
        /// middlebox function after the packet passed the Observation Point.
        /// </summary>
        [FieldLength(2)]
        [ClrType(typeof(ushort))]
        PostDot1QCustomerVlanID = 255,

        /// <summary>
        /// The Ethernet type field of an Ethernet frame that identifies the
        /// MAC client protocol carried in the payload as defined in paragraph
        /// 1.4.349 of [IEEE.802-3.2005].
        /// </summary>
        [FieldLength(2)]
        [ClrType(typeof(ushort))]
        EthernetType = 256,

        /// <summary>
        /// The definition of this Information Element is identical to the
        /// definition of Information Element <see cref="IPPrecedence"/>, except
        /// that it reports a potentially modified value caused by a middlebox
        /// function after the packet passed the Observation Point.
        /// </summary>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        PostIPPrecedence = 257,

        /// <summary>
        /// The absolute timestamp at which the data within the scope containing
        /// this Information Element was received by a Collecting Process.
        /// </summary>
        /// <remarks>
        /// This Information Element SHOULD be bound to its containing IPFIX
        /// Message via IPFIX Options and the messageScope Information Element,
        /// as defined below.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        CollectionTimeMilliseconds = 258,

        /// <summary>
        /// The value of the SCTP Stream Identifier used by the Exporting
        /// Process for exporting IPFIX Message data.
        /// </summary>
        /// <remarks>
        /// This is carried in the Stream Identifier field of the header of the
        /// SCTP DATA chunk containing the IPFIX Message(s).
        /// </remarks>
        [FieldLength(2)]
        [ClrType(typeof(ushort))]
        ExportSctpStreamID = 259,

        /// <summary>
        /// The absolute Export Time of the latest IPFIX Message within the
        /// scope containing this Information Element.
        /// </summary>
        /// <remarks>
        /// This Information Element SHOULD be bound to its containing IPFIX
        /// Transport Session via IPFIX Options and the
        /// <see cref="SessionScope"/> Information Element.
        /// </remarks>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        MaximumExportSeconds = 260,

        /// <summary>
        /// The latest absolute timestamp of the last packet within any Flow
        /// within the scope containing this Information Element, rounded up to
        /// the second if necessary.
        /// </summary>
        /// <remarks>
        /// This Information Element SHOULD be bound to its containing IPFIX
        /// Transport Session via IPFIX Options and the
        /// <see cref="SessionScope"/> Information Element.
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        MaximumFlowEndSeconds = 261,

        /// <summary>
        /// The MD5 checksum of the IPFIX Message containing this record.
        /// </summary>
        /// <remarks>
        /// This Information Element SHOULD be bound to its containing IPFIX
        /// Message via an options record and the <see cref="MessageScope"/>
        /// Information Element, as defined below, and SHOULD appear only once
        /// in a given IPFIX Message. To calculate the value of this Information
        /// Element, first buffer the containing IPFIX Message, setting the
        /// value of this Information Element to all zeroes. Then calculate the
        /// MD5 checksum of the resulting buffer as defined in [RFC1321], place
        /// the resulting value in this Information Element, and export the
        /// buffered message. This Information Element is intended as a simple
        /// checksum only; therefore collision resistance and algorithm agility
        /// are not required, and MD5 is an appropriate message digest. This
        /// Information Element has a fixed length of 16 octets.
        /// </remarks>
        [FieldLength(16)]
        MessageMD5Checksum = 262,

        /// <summary>
        /// The presence of this Information Element as scope in an Options
        /// Template signifies that the options described by the Template apply
        /// to the IPFIX Message that contains them.
        /// </summary>
        /// <remarks>
        /// It is defined for general purpose message scoping of options, and
        /// proposed specifically to allow the attachment a checksum to a
        /// message via IPFIX Options. The value of this Information Element
        /// MUST be written as 0 by the File Writer or Exporting Process. The
        /// value of this Information Element MUST be ignored by the File Reader
        /// or the Collecting Process.
        /// </remarks>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        MessageScope = 263,

        /// <summary>
        /// The absolute Export Time of the earliest IPFIX Message within the
        /// scope containing this Information Element.
        /// </summary>
        /// <remarks>
        /// This Information Element SHOULD be bound to its containing IPFIX
        /// Transport Session via an options record and the sessionScope
        /// Information Element.
        /// </remarks>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        MinimumExportSeconds = 264,

        /// <summary>
        /// The earliest absolute timestamp of the first packet within any Flow
        /// within the scope containing this Information Element, rounded down
        /// to the second if necessary.
        /// </summary>
        /// <remarks>
        /// This Information Element SHOULD be bound to its containing IPFIX
        /// Transport Session via an options record and the sessionScope
        /// Information Element.
        /// </remarks>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        MinimumFlowStartSeconds = 265,

        /// <summary>
        /// This Information Element is used to encapsulate non-IPFIX data into
        /// an IPFIX Message stream, for the purpose of allowing a non-IPFIX
        /// data processor to store a data stream inline within an IPFIX File.
        /// </summary>
        /// <remarks>
        /// A Collecting Process or File Writer MUST NOT try to interpret this
        /// binary data. This Information Element differs from
        /// <see cref="PaddingOctets"/> as its contents are meaningful in some
        /// non-IPFIX context, while the contents of paddingOctets MUST be 0x00
        /// and are intended only for Information Element alignment.
        /// </remarks>
        [FieldLength]
        OpaqueOctets = 266,

        /// <summary>
        /// The presence of this Information Element as scope in an Options
        /// Template signifies that the options described by the Template apply
        /// to the IPFIX Transport Session that contains them.
        /// </summary>
        /// <remarks>
        /// Note that as all options are implicitly scoped to Transport Session
        /// and Observation Domain, this Information Element is equivalent to a
        /// null scope. It is defined for general purpose session scoping of
        /// options, and proposed specifically to allow the attachment of time
        /// window to an IPFIX File via IPFIX Options. The value of this
        /// Information Element MUST be written as 0 by the File Writer or
        /// Exporting Process. The value of this Information Element MUST be
        /// ignored by the File Reader or the Collecting Process.
        /// </remarks>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        SessionScope = 267,

        /// <summary>
        /// The latest absolute timestamp of the last packet within any Flow
        /// within the scope containing this Information Element, rounded up to
        /// the microsecond if necessary.
        /// </summary>
        /// <remarks>
        /// This Information Element SHOULD be bound to its containing IPFIX
        /// Transport Session via IPFIX Options and the
        /// <see cref="SessionScope"/> Information Element. This Information
        /// Element SHOULD be used only in Transport Sessions containing Flow
        /// Records with microsecond-precision (or better) timestamp Information
        /// Elements.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        MaximumFlowEndMicroseconds = 268,

        /// <summary>
        /// The latest absolute timestamp of the last packet within any Flow
        /// within the scope containing this Information Element, rounded up to
        /// the millisecond if necessary.
        /// </summary>
        /// <remarks>
        /// This Information Element SHOULD be bound to its containing IPFIX
        /// Transport Session via IPFIX Options and the sessionScope Information
        /// Element. This Information Element SHOULD be used only in Transport
        /// Sessions containing Flow Records with millisecond- precision (or
        /// better) timestamp Information Elements.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        MaximumFlowEndMilliseconds = 269,

        /// <summary>
        /// The latest absolute timestamp of the last packet within any Flow
        /// within the scope containing this Information Element.
        /// </summary>
        /// <remarks>
        /// This Information Element SHOULD be bound to its containing IPFIX
        /// Transport Session via IPFIX Options and the
        /// <see cref="SessionScope"/> Information Element. This Information
        /// Element SHOULD be used only in Transport Sessions containing Flow
        /// Records with nanosecond-precision timestamp Information Elements.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        MaximumFlowEndNanoseconds = 270,

        /// <summary>
        /// The earliest absolute timestamp of the first packet within any Flow
        /// within the scope containing this Information Element, rounded down
        /// to the microsecond if necessary.
        /// </summary>
        /// <remarks>
        /// This Information Element SHOULD be bound to its containing IPFIX
        /// Transport Session via an options record and the
        /// <see cref="SessionScope"/> Information Element. This Information
        /// Element SHOULD be used only in Transport Sessions containing Flow
        /// Records with microsecond-precision (or better) timestamp Information
        /// Elements.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        MinimumFlowStartMicroseconds = 271,

        /// <summary>
        /// The earliest absolute timestamp of the first packet within any Flow
        /// within the scope containing this Information Element, rounded down
        /// to the millisecond if necessary.
        /// </summary>
        /// <remarks>
        /// This Information Element SHOULD be bound to its containing IPFIX
        /// Transport Session via an options record and the
        /// <see cref="SessionScope"/> Information Element. This Information
        /// Element SHOULD be used only in Transport Sessions containing Flow
        /// Records with millisecond-precision (or better) timestamp Information
        /// Elements.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        MinimumFlowStartMilliseconds = 272,

        /// <summary>
        /// The earliest absolute timestamp of the first packet within any Flow
        /// within the scope containing this Information Element.
        /// </summary>
        /// <remarks>
        /// This Information Element SHOULD be bound to its containing IPFIX
        /// Transport Session via an options record and the
        /// <see cref="SessionScope"/> Information Element. This Information
        /// Element SHOULD be used only in Transport Sessions containing Flow
        /// Records with nanosecond-precision timestamp Information Elements.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        MinimumFlowStartNanoseconds = 273,

        /// <summary>
        /// The full X.509 certificate, encoded in ASN.1 DER format, used by the
        /// Collector when IPFIX Messages were transmitted using TLS or DTLS.
        /// </summary>
        /// <remarks>
        /// This Information Element SHOULD be bound to its containing IPFIX
        /// Transport Session via an options record and the
        /// <see cref="SessionScope"/> Information Element, or to its containing
        /// IPFIX Message via an options record and the
        /// <see cref="MessageScope"/> Information Element.
        [FieldLength]
        CollectorCertificate = 274,

        /// <summary>
        /// The full X.509 certificate, encoded in ASN.1 DER format, used by the
        /// Collector when IPFIX Messages were transmitted using TLS or DTLS.
        /// </summary>
        /// <remarks>
        /// This Information Element SHOULD be bound to its containing IPFIX
        /// Transport Session via an options record and the 
        /// <see cref="SessionScope"/> Information Element, or to its containing
        /// IPFIX Message via an options record and the
        /// <see cref="MessageScope"/> Information Element.
        /// </remarks>
        [FieldLength]
        ExporterCertificate = 275,

        /// <summary>
        /// The export reliability of Data Records, within this SCTP stream, for
        /// the element(s) in the Options Template scope.
        /// </summary>
        /// <remarks>
        /// A typical example of an element for which the export reliability
        /// will be reported is the <see cref="TemplateID", as specified in the
        /// Data Records Reliability Options Template. A value of <c>true</c>
        /// means that the Exporting Process MUST send any Data Records
        /// associated with the element(s) reliably within this SCTP stream. A
        /// value of <c>false</c> means that the Exporting Process MAY send any
        /// Data Records associated with the element(s) unreliably within this
        /// SCTP stream.
        /// </remarks>
        [FieldLength(1)]
        [ClrType(typeof(bool))]
        DataRecordsReliability = 276,

        /// <summary>
        /// Type of observation point.
        /// </summary>
        /// <remarks>
        /// Values assigned to date are: 1 = Physical port; 2 = Port channel;
        /// 3 = VLAN.
        /// </remarks>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        ObservationPointType = 277,

        /// <summary>
        /// This information element counts the number of TCP or UDP connections
        /// which were opened during the observation period.
        /// </summary>
        /// <remarks>
        /// The observation period may be specified by the flow start and end
        /// timestamps.
        /// </remarks>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        NewConnectionDeltaCount = 278,

        /// <summary>
        /// This information element aggregates the total time in seconds
        /// for all of the TCP or UDP connections which were in use during
        /// the observation period.
        /// </summary>
        /// <remarks>
        /// For example if there are five concurrent connections each for ten
        /// seconds, the value would be 50s.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        ConnectionSumDurationSeconds = 279,

        /// <summary>
        /// This information element identifies a transaction within a
        /// connection.
        /// </summary>
        /// <remarks>
        /// A transaction is a meaningful exchange of application data between
        /// two network devices or a client and server. A transactionId is
        /// assigned the first time a flow is reported, so that later reports
        /// for the same flow will have the same
        /// <see cref="ConnectionTransactionID"/>. A different 
        /// <see cref="ConnectionTransactionID"/> is used for each transaction
        /// within a TCP or UDP connection. The identifiers need not be
        /// sequential.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        ConnectionTransactionID = 280,

        /// <summary>
        /// The definition of this Information Element is identical to the
        /// definition of Information Element <see cref="SourceIPv6Address"/>,
        /// except that it reports a modified value caused by a NAT64 middlebox
        /// function after the packet passed the Observation Point.
        /// </summary>
        /// <remarks>
        /// See [RFC8200] for the definition of the Source Address field in the
        /// IPv6 header. See[RFC3234] for the definition of middleboxes. See
        /// [RFC6146] for nat64 specification.
        /// </remarks>
        [FieldLength(16)]
        [ClrType(typeof(IPAddress))]
        PostNatSourceIPv6Address = 281,

        /// <summary>
        /// The definition of this Information Element is identical to the
        /// definition of Information Element
        /// <see cref="DestinationIPv6Address"/>, except that it reports a
        /// modified value caused by a NAT64 middlebox function after the packet
        /// passed the Observation Point.
        /// </summary>
        /// <remarks>
        /// See [RFC8200] for the definition of the Destination Address field in
        /// the IPv6 header. See [RFC3234] for the definition of middleboxes. See
        /// [RFC6146] for nat64 specification.
        /// </remarks>
        [FieldLength(16)]
        [ClrType(typeof(IPAddress))]
        PostNatTDestinationIPv6Address = 282,

        /// <summary>
        /// Locally unique identifier of a NAT pool.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        NatPoolID = 283,

        /// <summary>
        /// The name of a NAT pool identified by a <see cref="NatPoolID"/>.
        /// </summary>
        [ClrType(typeof(string))]
        NatPoolName = 284,

        /// <summary>
        /// A flag word describing specialised modifications to the
        /// anonymisation policy in effect for the anonymisation technique
        /// applied to a referenced Information Element within a referenced
        /// Template.
        /// </summary>
        /// <remarks>
        /// <para>When flags are clear (0), the normal policy(as described by
        /// <see cref="AnonyomisationTechnique"/>) applies without modification.
        /// <c>
        /// MSB   14  13  12  11  10   9   8   7   6   5   4   3   2   1  LSB
        /// +---+---+---+---+---+---+---+---+---+---+---+---+---+---+---+---+
        /// |                Reserved                       |LOR|PmA|   SC  |
        /// +---+---+---+---+---+---+---+---+---+---+---+---+---+---+---+---+
        /// 
        /// <see cref="AnonymisationFlags"/> IE
        /// 
        /// +--------+----------+-----------------------------------------------+
        /// | bit(s) | name     | description                                   |
        /// | (LSB = |          |                                               |
        /// | 0)     |          |                                               |
        /// +--------+----------+-----------------------------------------------+
        /// | 0-1    | SC       | Stability Class: see the Stability Class      |
        /// |        |          | table below, and section Section 5.1.         | 
        /// | 2      | PmA      | Perimeter anonymisation: when set (1),        |
        /// |        |          | source- Information Elements as described in  |
        /// |        |          | [RFC5103] are interpreted as external         |
        /// |        |          | addresses, and destination- Information       |
        /// |        |          | Elements as described in [RFC5103] are        |
        /// |        |          | interpreted as internal addresses, for the    |
        /// |        |          | purposes of associating                       |
        /// |        |          | anonymisationTechnique to Information         |
        /// |        |          | Elements only; see Section 7.2.2 for details. |
        /// |        |          | This bit MUST NOT be set when associated with |
        /// |        |          | a non-endpoint(i.e., source- or               |
        /// |        |          | destination-) Information Element.SHOULD be   |
        /// |        |          | consistent within a record (i.e., if a        |
        /// |        |          | source- Information Element has this flag     |
        /// |        |          | set, the corresponding destination- element   |
        /// |        |          | SHOULD have this flag set, and vice-versa.)   |
        /// | 3      | LOR      | Low-Order Unchanged: when set(1), the         |
        /// |        |          | low-order bits of the anonymised Information  |
        /// |        |          | Element contain real data.This modification   |
        /// |        |          | is intended for the anonymisation of          |
        /// |        |          | network-level addresses while leaving         |
        /// |        |          | host-level addresses intact in order to       |
        /// |        |          | preserve host level-structure, which could    |
        /// |        |          | otherwise be used to reverse anonymisation.   |
        /// |        |          | MUST NOT be set when associated with a        |
        /// |        |          | truncation-based anonymisationTechnique.      |
        /// | 4-15   | Reserved | Reserved for future use: SHOULD be cleared    |
        /// |        |          | (0) by the Exporting Process and MUST be      |
        /// |        |          | ignored by the Collecting Process.            |
        /// +--------+----------+-----------------------------------------------+
        /// </c></para>
        /// <para>The Stability Class portion of this flags word describes the
        /// stability class of the anonymisation technique applied to a referenced
        /// Information Element within a referenced Template. Stability classes
        /// refer to the stability of the parameters of the anonymisation
        /// technique, and therefore the comparability of the mapping between the
        /// real and anonymised values over time.This determines which anonymised
        /// datasets may be compared with each other. Values are as follows:
        /// <c>
        /// +-----+-----+-------------------------------------------------------+
        /// | Bit | Bit | Description                                           |
        /// | 1   | 0   |                                                       |
        /// +-----+-----+-------------------------------------------------------+
        /// | 0   | 0   | Undefined: the Exporting Process makes no             |
        /// |     |     | representation as to how stable the mapping is, or    |
        /// |     |     | over what time period values of this field will       |
        /// |     |     | remain comparable; while the Collecting Process MAY   |
        /// |     |     | assume Session level stability, Session level         |
        /// |     |     | stability is not guaranteed.  Processes SHOULD assume |
        /// |     |     | this is the case in the absence of stability class    |
        /// |     |     | information; this is the default stability class.     |
        /// | 0   | 1   | Session: the Exporting Process will ensure that the   |
        /// |     |     | parameters of the anonymisation technique are stable  |
        /// |     |     | during the Transport Session.All the values of the    |
        /// |     |     | described Information Element for each Record         |
        /// |     |     | described by the referenced Template within the       |
        /// |     |     | Transport Session are comparable.  The Exporting      |
        /// |     |     | Process SHOULD endeavour to ensure at least this      |
        /// |     |     | stability class.                                      |
        /// | 1   | 0   | Exporter-Collector Pair: the Exporting Process will   |
        /// |     |     | ensure that the parameters of the anonymisation       |
        /// |     |     | technique are stable across Transport Sessions over   |
        /// |     |     | time with the given Collecting Process, but may use   |
        /// |     |     | different parameters for different Collecting         |
        /// |     |     | Processes.Data exported to different Collecting       |
        /// |     |     | Processes are not comparable.                         |
        /// | 1   | 1   | Stable: the Exporting Process will ensure that the    |
        /// |     |     | parameters of the anonymisation technique are stable  |
        /// |     |     | across Transport Sessions over time, regardless of    |
        /// |     |     | the Collecting Process to which it is sent.           |
        /// +-----+-----+-------------------------------------------------------+
        /// </c></para>
        /// </remarks>
        [FieldLength(2)]
        [ClrType(typeof(ushort))]
        AnonymisationFlags = 285,

        /// <summary>
        /// A description of the anonymisation technique applied to a referenced
        /// Information Element within a referenced Template.
        /// </summary>
        /// <remarks>
        /// <para>Each technique may be applicable only to certain Information
        /// Elements and recommended only for certain Infomation Elements; these
        /// restrictions are noted in the table below.
        /// <c>
        /// +-------+---------------------------+-----------------+-------------+
        /// | Value | Description               | Applicable to   | Recommended |
        /// |       |                           |                 | for         |
        /// +-------+---------------------------+-----------------+-------------+
        /// | 0     | Undefined: the Exporting  | all             | all         |
        /// |       | Process makes no          |                 |             |
        /// |       | representation as to      |                 |             |
        /// |       | whether the defined field |                 |             |
        /// |       | is anonymised or not.     |                 |             |
        /// |       | While the Collecting      |                 |             |
        /// |       | Process MAY assume that   |                 |             |
        /// |       | the field is not          |                 |             |
        /// |       | anonymised, it is not     |                 |             |
        /// |       | guaranteed not to be.     |                 |             |
        /// |       | This is the default       |                 |             |
        /// |       | anonymisation technique.  |                 |             |
        /// | 1     | None: the values exported | all             | all         |
        /// |       | are real.                 |                 |             |
        /// | 2     | Precision                 | all             | all         |
        /// |       | Degradation/Truncation:   |                 |             |
        /// |       | the values exported are   |                 |             |
        /// |       | anonymised using simple   |                 |             |
        /// |       | precision degradation or  |                 |             |
        /// |       | truncation.The new        |                 |             |
        /// |       | precision or number of    |                 |             |
        /// |       | truncated bits is         |                 |             |
        /// |       | implicit in the exported  |                 |             |
        /// |       | data, and can be deduced  |                 |             |
        /// |       | by the Collecting         |                 |             |
        /// |       | Process.                  |                 |             |
        /// | 3     | Binning: the values       | all             | all         |
        /// |       | exported are anonymised   |                 |             |
        /// |       | into bins.                |                 |             |
        /// | 4     | Enumeration: the values   | all             | timestamps  |
        /// |       | exported are anonymised   |                 |             |
        /// |       | by enumeration.           |                 |             |
        /// | 5     | Permutation: the values   | all             | identifiers |
        /// |       | exported are anonymised   |                 |             |
        /// |       | by permutation.           |                 |             |
        /// | 6     | Structured Permutation:   | addresses       |             |
        /// |       | the values exported are   |                 |             |
        /// |       | anonymised by             |                 |             |
        /// |       | permutation, preserving   |                 |             |
        /// |       | bit-level structure as    |                 |             |
        /// |       | appropriate; this         |                 |             |
        /// |       | represents                |                 |             |
        /// |       | prefix-preserving IP      |                 |             |
        /// |       | address anonymisation or  |                 |             |
        /// |       | structured MAC address    |                 |             |
        /// |       | anonymisation.            |                 |             |
        /// | 7     | Reverse Truncation: the   | addresses       |             |
        /// |       | values exported are       |                 |             |
        /// |       | anonymised using reverse  |                 |             |
        /// |       | truncation.The number     |                 |             |
        /// |       | of truncated bits is      |                 |             |
        /// |       | implicit in the exported  |                 |             |
        /// |       | data, and can be deduced  |                 |             |
        /// |       | by the Collecting         |                 |             |
        /// |       | Process.                  |                 |             |
        /// | 8     | Noise: the values         | non-identifiers | counters    |
        /// |       | exported are anonymised   |                 |             |
        /// |       | by adding random noise to |                 |             |
        /// |       | each value.               |                 |             |
        /// | 9     | Offset: the values        | all             | timestamps  |
        /// |       | exported are anonymised   |                 |             |
        /// |       | by adding a single offset |                 |             |
        /// |       | to all values.            |                 |             |
        /// +-------+---------------------------+-----------------+-------------+
        /// </c></para>
        /// </remarks>
        [FieldLength(2)]
        [ClrType(typeof(ushort))]
        AnonymisationTechnique = 286,

        /// <summary>
        /// A zero-based index of an Information Element referenced by 
        /// <see cref="InformationElementID"/> within a Template referenced by
        /// <see cref="TemplateID"/>
        /// </summary>
        /// <remarks>
        /// Used to disambiguate scope for templates containing multiple identical
        /// Information Elements.
        /// </remarks>
        [FieldLength(2)]
        [ClrType(typeof(ushort))]
        InformationElementIndex = 287,

        /// <summary>
        /// Specifies if the Application ID is based on peer-to-peer technology.
        /// </summary>
        /// <remarks>
        /// Possible values are: { yes, y, 1 }, { no, n, 2 } and
        /// { unassigned, u, 0 }.
        /// </remarks>
        [FieldLength(1)]
        [ClrType(typeof(string))]
        P2pTechnology = 288,

        /// <summary>
        /// Specifies if the Application ID is used as a tunnel technology.
        /// </summary>
        /// <remarks>
        /// Possible values are: { yes, y, 1 }, { no, n, 2 } and
        /// { unassigned, u, 0 }.
        /// </remarks>
        [FieldLength(1)]
        [ClrType(typeof(string))]
        TunnelTechnology = 289,

        /// <summary>
        /// Specifies if the Application ID is an encrypted networking protocol.
        /// </summary>
        /// <remarks>
        /// Possible values are: { yes, y, 1 }, { no, n, 2 } and
        /// { unassigned, u, 0 }.
        /// </remarks>
        [FieldLength(1)]
        [ClrType(typeof(string))]
        EncryptedTechnology = 290,

        /// <summary>
        /// Specifies a generic Information Element with a basicList abstract
        /// data type.
        /// </summary>
        /// <remarks>
        /// For example, a list of port numbers, a list of interface indexes, etc.
        /// </remarks>
        [FieldLength]
        //[ClrType(typeof(basicList))]
        BasicList = 291,    // TODO

        /// <summary>
        /// Specifies a generic Information Element with a subTemplateList
        /// abstract data type.
        /// </summary>
        [FieldLength]
        //[ClrType(typeof(subTemplateList))]
        SubTemplateList = 292,  // TODO

        /// <summary>
        /// Specifies a generic Information Element with a subTemplateMultiList
        /// abstract data type.
        /// </summary>
        [FieldLength]
        //[ClrType(typeof(subTemplateMultiList))]
        SubTemplateMultiList = 293, // TODO

        /// <summary>
        /// This element describes the validity state of the BGP route
        /// correspondent source or destination IP address.
        /// </summary>
        /// <remarks>
        /// If the validity state for this Flow is only available, then the
        /// value of this Information Element is 255.
        /// </remarks>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        BgpValidityState = 294,

        /// <summary>
        /// IPSec Security Parameters Index (SPI).
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        IPSecSpi = 295,

        /// <summary>
        /// GRE key, which is used for identifying an individual traffic
        /// flow within a tunnel.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        GreKey = 296,

        /// <summary>
        /// The type of NAT treatment.
        /// </summary>
        /// <remarks>
        /// 0 = unknown; 1 = NAT44 translated; 2 = NAT64 translated;
        /// 3 = NAT46 translated; 4 = IPv4 --> IPv4 (no NAT);
        /// 5 = NAT66 translated; 6 = IPv6 --> IPv6 (no NAT).
        /// </remarks>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        NatType = 297,  // TODO

        /// <summary>
        /// The total number of layer 4 packets in a flow from the initiator
        /// since the previous report.
        /// </summary>
        /// <remarks>
        /// The initiator is the device which triggered the session creation
        /// and remains the same for the life of the session.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        InitiatorPackets = 298,

        /// <summary>
        /// The total number of layer 4 packets in a flow from the responder
        /// since the previous report.
        /// </summary>
        /// <remarks>
        /// The responder is the device which replies to the initiator and
        /// remains the same for the life of the session.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        ResponderPackets = 299,

        /// <summary>
        /// The name of an observation domain identified by an 
        /// <see cref="ObservationDomainID"/>.
        /// </summary>
        [ClrType(typeof(string))]
        ObservationDomainName = 300,

        /// <summary>
        /// From all the packets observed at an Observation Point, a subset of
        /// the packets is selected by a sequence of one or more Selectors.
        /// </summary>
        /// <remarks>
        /// The <see cref="SelectionSequenceID"/> is a unique value per
        /// Observation Domain, specifying the Observation Point and the
        /// sequence of Selectors through which the packets are selected.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        SelectionSequenceID = 301,

        /// <summary>
        /// The Selector ID is the unique ID identifying a Primitive Selector.
        /// </summary>
        /// <remarks>
        /// Each Primitive Selector must have a unique ID in the Observation
        /// Domain.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        SelectorID = 302,

        /// <summary>
        /// This Information Element contains the ID of another Information
        /// Element.
        /// </summary>
        [FieldLength(2)]
        [ClrType(typeof(ushort))]
        InformationElementID = 303,

        /// <summary>
        /// This Information Element identifies the packet selection methods
        /// (e.g. Filtering, Sampling) that are applied by the Selection
        /// Process.
        /// </summary>
        /// <remarks>
        /// <para>Most of these methods have parameters. Further Information
        /// Elements are needed to fully specify packet selection with these
        /// methods and all their parameters. The methods listed below are
        /// defined in [RFC5475]. For their parameters, Information Elements
        /// are defined in the information model document. The names of these
        /// Information Elements are listed for each method identifier. Further
        /// method identifiers may be added to the list below. It might be
        /// necessary to define new Information Elements to specify their
        /// parameters.</para>
        /// <para>The <see cref="SelectorAlgorithm"/> registry is maintained
        /// by IANA. New assignments for the registry will be administered
        /// by IANA and are subject to Expert Review [RFC8126].
        /// The registry can be updated when specifications of the new
        /// method(s) and any new Information Elements are provided. The group
        /// of experts must double check the <see cref="SelectorAlgorithm"/>
        /// definitions and Information Elements with already defined 
        /// <see cref="SelectorAlgorithm"/> and Information Elements for
        /// completeness, accuracy, and redundancy. Those experts will initially
        /// be drawn from the Working Group Chairs and document editors of the
        /// IPFIX and PSAMP Working Groups. The following packet selection
        /// methods identifiers are defined here: [IANA registry 
        /// psamp-parameters] There is a broad variety of possible parameters
        /// that could be used for Property match Filtering (5) but currently
        /// there are no agreed parameters specified.</para>
        /// </remarks>
        [FieldLength(2)]
        [ClrType(typeof(ushort))]
        SelectorAlgorithm = 304,

        /// <summary>
        /// This Information Element specifies the number of packets that are
        /// consecutively sampled.
        /// </summary>
        /// <remarks>
        /// A value of 100 means that 100 consecutive packets are sampled. For
        /// example, this Information Element may be used to describe the
        /// configuration of a systematic count-based Sampling Selector.
        /// </remarks>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        SamplingPacketInterval = 305,

        /// <summary>
        /// This Information Element specifies the number of packets between two
        /// <see cref="SamplingPacketInterval"/>s.
        /// </summary>
        /// <remarks>
        /// A value of 100 means that the next interval starts 100 packets
        /// (which are not sampled) after the current
        /// <see cref="SamplingPacketInterval"/> is over. For example, this
        /// Information Element may be used to describe the configuration of a
        /// systematic count-based Sampling Selector.
        /// </remarks>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        SamplingPacketSpace = 306,

        /// <summary>
        /// This Information Element specifies the time interval in microseconds
        /// during which all arriving packets are sampled.
        /// </summary>
        /// <remarks>
        /// For example, this Information Element may be used to describe the
        /// configuration of a systematic time-based Sampling Selector.
        /// </remarks>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        SamplingTimeInterval = 307,

        /// <summary>
        /// This Information Element specifies the time interval in microseconds
        /// between two samplingTimeIntervals.
        /// </summary>
        /// <remarks>
        /// A value of 100 means that the next interval starts 100 microseconds
        /// (during which no packets are sampled) after the current
        /// <see cref="SamplingTimeInterval"/> is over. For example, this
        /// Information Element may used to describe the configuration of a
        /// systematic time-based Sampling Selector.
        /// </remarks>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        SamplingTimeSpace = 308,

        /// <summary>
        /// This Information Element specifies the number of elements taken from
        /// the parent Population for random Sampling methods.
        /// </summary>
        /// <remarks>
        /// For example, this Information Element may be used to describe the
        /// configuration of a random n-out-of-N Sampling Selector.
        /// </remarks>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        SamplingSize = 309,

        /// <summary>
        /// This Information Element specifies the number of elements in the
        /// parent Population for random Sampling methods.
        /// </summary>
        /// <remarks>
        /// For example, this Information Element may be used to describe the
        /// configuration of a random n-out-of-N Sampling Selector.
        /// </remarks>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        SamplingPopulation = 310,

        /// <summary>
        /// This Information Element specifies the probability that a packet is
        /// sampled, expressed as a value between 0 and 1.
        /// </summary>
        /// <remarks>
        /// The probability is equal for every packet. A value of 0 means no
        /// packet was sampled since the probability is 0. For example, this
        /// Information Element may be used to describe the configuration of a
        /// uniform probabilistic Sampling Selector.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(double))]
        SamplingProbability = 311,

        /// <summary>
        /// This Information Element specifies the length of the selected data
        /// link frame.
        /// </summary>
        /// <remarks>
        /// The data link layer is defined in [ISO/IEC.7498-1:1994].
        /// </remarks>
        [FieldLength(2)]
        [ClrType(typeof(ushort))]
        DataLinkFrameSize = 312,

        /// <summary>
        /// This Information Element carries a series of n octets from the IP
        /// header of a sampled packet, starting <see cref="SectionOffset"/>
        /// octets into the IP header. However, if no
        /// <see cref="SectionOffset"/> field corresponding to this Information
        /// Element is present, then a <see cref="SectionOffset"/> of zero
        /// applies and the octets MUST be from the start of the IP header.
        /// </summary>
        /// <remarks>
        /// With sufficient length, this element also reports octets from the IP
        /// payload. However, full packet capture of arbitrary packet streams is
        /// explicitly out of scope per the Security Considerations sections of
        /// [RFC5477] and [RFC2804]. The <see cref="SectionExportedOctets "/>
        /// expresses how much data was exported, while the remainder is
        /// padding. When the <see cref="SectionExportedOctets "/> field
        /// corresponding to this Information Element exists, this Information
        /// Element MAY have a fixed length and MAY be padded, or it MAY have a
        /// variable length. If the <see cref="SectionExportedOctets "/> field
        /// corresponding to this Information Element does not exist, this
        /// Information Element SHOULD have a variable length and MUST NOT be
        /// padded. In this case, the size of the exported section may be
        /// constrained due to limitations in the IPFIX protocol.
        [FieldLength]
        IPHeaderPacketSection = 313,

        /// <summary>
        /// This Information Element carries a series of n octets from the IP
        /// payload of a sampled packet, starting <see cref="SectionOffset"/>
        /// octets into the IP payload. However, if no
        /// <see cref="SectionOffset"/> field corresponding to this Information
        /// Element is present, then a <see cref="SectionOffset"/> of zero
        /// applies and the octets MUST be from the start of the IP payload.
        /// </summary>
        /// <remarks>
        /// The IPv4 payload is that part of the packet that follows the IPv4
        /// header and any options, which [RFC791] refers to as data or data
        /// octets. For example, see the examples in [RFC791], Appendix A.
        /// The IPv6 payload is the rest of the packet following the 40-octet
        /// IPv6 header. Note that any extension headers present are considered
        /// part of the payload. See [RFC8200] for the IPv6 specification. The
        /// <see cref="SectionExportedOctets"/> expresses how much data was
        /// observed, while the remainder is padding. When the 
        /// <see cref="SectionExportedOctets"/> field corresponding to this
        /// Information Element exists, this Information Element MAY have a
        /// fixed length and MAY be padded, or MAY have a variable length. When
        /// the <see cref="SectionExportedOctets"/> field corresponding to this
        /// Information Element does not exist, this Information Element SHOULD
        /// have a variable length and MUST NOT be padded. In this case, the
        /// size of the exported section may be constrained due to limitations
        /// in the IPFIX protocol.
        /// </remarks>
        [FieldLength]
        IPPayloadPacketSection = 314,

        /// <summary>
        /// This Information Element carries n octets from the data link frame
        /// of a selected frame, starting <see cref="SectionOffset"/> octets
        /// into the frame. However, if no <see cref="SectionOffset"/> field
        /// corresponding to this Information Element is present, then a
        /// <see cref="SectionOffset"/> of zero applies and the octets MUST
        /// be from the start of the data link frame.
        /// </summary>
        /// <remarks>
        /// The <see cref="SectionExportedOctets"/> expresses how much data was
        /// observed, while the remainder is padding. When the 
        /// <see cref="SectionExportedOctets"/> field corresponding to this
        /// Information Element exists, this Information Element MAY have a
        /// fixed length and MAY be padded, or MAY have a variable length.
        /// When the <see cref="SectionExportedOctets"/> field corresponding to
        /// this Information Element does not exist, this Information Element
        /// SHOULD have a variable length and MUST NOT be padded. In this case,
        /// the size of the exported section may be constrained due to
        /// limitations in the IPFIX protocol. Further Information Elements,
        /// i.e. <see cref="DataLinkFrameType "/> and
        /// <see cref="DataLinkFrameSize"/>, are needed to specify the data link
        /// type and the size of the data link frame of this Information
        /// Element. A set of these Information Elements MAY be contained in a
        /// structured data type, as expressed in [RFC6313]. Or a set of these
        /// Information Elements MAY be contained in one Flow Record as shown in
        /// Appendix B of[RFC7133]. The data link layer is defined in
        /// [ISO/IEC.7498-1:1994].
        /// </remarks>
        [FieldLength]
        DataLinkFrameSection = 315,

        /// <summary>
        /// This Information Element carries a series of n octets from the MPLS
        /// label stack of a sampled packet, starting
        /// <see cref="SectionOffset"/> octets into the MPLS label stack.
        /// However, if no <see cref="SectionOffset"/> field corresponding to
        /// this Information Element is present, then a
        /// <see cref="SectionOffset"/> of zero applies and the octets MUST be
        /// from the head of the MPLS label stack.
        /// </summary>
        /// <remarks>
        /// With sufficient length, this element also reports octets from the
        /// MPLS payload. However, full packet capture of arbitrary packet
        /// streams is explicitly out of scope per the Security Considerations
        /// sections of [RFC5477] and [RFC2804]. See [RFC3031] for the 
        /// specification of MPLS packets. See [RFC3032] for the specification
        /// of the MPLS label stack. The <see cref="SectionExportedOctets "/>
        /// expresses how much data was observed, while the remainder is
        /// padding. When the sectionExportedOctets field corresponding to this
        /// Information Element exists, this Information Element MAY have a
        /// fixed length and MAY be padded, or MAY have a variable length. When
        /// the <see cref="SectionExportedOctets "/> field corresponding to this
        /// Information Element does not exist, this Information Element SHOULD
        /// have a variable length and MUST NOT be padded. In this case, the
        /// size of the exported section may be constrained due to limitations
        /// in the IPFIX protocol.
        /// </remarks>
        [FieldLength]
        MplsLabelStackSection = 316,

        /// <summary>
        /// The <see cref="MplsPayloadPacketSection"/> carries a series of n
        /// octets from the MPLS payload of a sampled packet, starting
        /// <see cref="SectionOffset"/> octets into the MPLS payload, as it is
        /// data that follows immediately after the MPLS label stack. However,
        /// if no <see cref = "SectionOffset" /> field corresponding to this
        /// Information Element is present, then a
        /// <see cref = "SectionOffset" /> of zero applies, and the octets MUST
        /// be from the start of the MPLS payload.
        /// </summary>
        /// <remarks>
        /// See [RFC3031] for the specification of MPLS packets. See [RFC3032]
        /// for the specification of the MPLS label stack. The
        /// <see cref="SectionExportedOctets "/> expresses how much data was
        /// observed, while the remainder is padding. When the
        /// <see cref="SectionExportedOctets "/> field corresponding to this
        /// Information Element exists, this Information Element MAY have a
        /// fixed length and MAY be padded, or it MAY have a variable length.
        /// When the <see cref="SectionExportedOctets "/> field corresponding to
        /// this Information Element does not exist, this Information Element
        /// SHOULD have a variable length and MUST NOT be padded. In this case,
        /// the size of the exported section may be constrained due to
        /// limitations in the IPFIX protocol.
        /// </remarks>
        [FieldLength]
        MplsPayloadPacketSection = 317,

        /// <summary>
        /// This Information Element specifies the total number of packets
        /// observed by a Selector, for a specific value of
        /// <see cref="SelectorID"/>.
        /// </summary>
        /// <remarks>
        /// This Information Element should be used in an Options Template
        /// scoped to the observation to which it refers. See Section 3.4.2.1 of
        /// the IPFIX protocol document [RFC7011].
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        SelectorIDTotalPacketsObserved = 318,

        /// <summary>
        /// This Information Element specifies the total number of packets
        /// selected by a Selector, for a specific value of
        /// <see cref="SelectorID"/>.
        /// </summary>
        /// <remarks>
        /// This Information Element should be used in an Options Template
        /// scoped to the observation to which it refers. See Section 3.4.2.1
        /// of the IPFIX protocol document [RFC7011].
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        SelectorIDTotalPacketsSelected = 319,

        /// <summary>
        /// This Information Element specifies the maximum possible measurement
        /// error of the reported value for a given Information Element.
        /// </summary>
        /// <remarks>
        /// The <see cref="AbsoluteError"/> has the same unit as the Information
        /// Element with which it is associated. The real value of the metric
        /// can differ by <see cref="AbsoluteError"/> (positive or negative)
        /// from the measured value. This Information Element provides only the
        /// error for measured values. If an Information Element contains an
        /// estimated value (from Sampling), the confidence boundaries and
        /// confidence level have to be provided instead, using the 
        /// <see cref="UpperConfidenceIntervalLimit"/>, <see cref="LowerConfidenceIntervalLimit"/> and 
        /// <see cref="ConfidenceLevel "/>Information Elements. This Information
        /// Element should be used in an Options Template scoped to the
        /// observation to which it refers.See Section 3.4.2.1 of the IPFIX
        /// protocol document [RFC7011].
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(double))]
        AbsoluteError = 320,

        /// <summary>
        /// This Information Element specifies the maximum possible positive or
        /// negative error ratio for the reported value for a given Information
        /// Element as percentage of the measured value.
        /// </summary>
        /// <remarks>
        /// The real value of the metric can differ by relativeError percent
        /// (positive or negative) from the measured value. This Information
        /// Element provides only the error for measured values. If an
        /// Information Element contains an estimated value (from Sampling),
        /// the confidence boundaries and confidence level have to be provided
        /// instead, using the <see cref="UpperConfidenceIntervalLimit"/>,
        /// <see cref="LowerConfidenceIntervalLimit"/> and <see cref="ConfidenceLevel "/>
        /// Information Elements. This Information Element should be used in an
        /// Options Template scoped to the observation to which it refers. See
        /// Section 3.4.2.1 of the IPFIX protocol document [RFC7011].
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(double))]
        RelativeError = 321,

        /// <summary>
        /// This Information Element specifies the absolute time in seconds of
        /// an observation.
        /// /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        ObservationTimeSeconds = 322,

        /// <summary>
        /// This Information Element specifies the absolute time in milliseconds
        /// of an observation.
        /// </summary>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        ObservationTimeMilliseconds = 323,

        /// <summary>
        /// This Information Element specifies the absolute time in microseconds
        /// of an observation.
        /// </summary>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        ObservationTimeMicroseconds = 324,

        /// <summary>
        /// This Information Element specifies the absolute time in nanoseconds
        /// of an observation.
        /// </summary>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        ObservationTimeNanoseconds = 325,

        /// <summary>
        /// This Information Element specifies the value from the digest hash
        /// function.
        /// </summary>
        /// <remarks>
        /// See also Sections 6.2, 3.8 and 7.1 of [RFC5475].
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        DigestHashValue = 326,

        /// <summary>
        /// This Information Element specifies the IP payload offset used by a
        /// Hash-based Selection Selector.
        /// </summary>
        /// <remarks>
        /// See also Sections 6.2, 3.8 and 7.1 of [RFC5475].
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        HashIPPayloadOffset = 327,

        /// <summary>
        /// This Information Element specifies the IP payload size used by a
        /// Hash-based Selection Selector.
        /// </summary>
        /// <remarks>
        /// See also Sections 6.2, 3.8 and 7.1 of [RFC5475].
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        HashIPPayloadSize = 328,

        /// <summary>
        /// This Information Element specifies the value for the beginning of a
        /// hash function's potential output range.
        /// </summary>
        /// <remarks>
        /// See also Sections 6.2, 3.8 and 7.1 of [RFC5475].
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        HashOutputRangeMinimum = 329,

        /// <summary>
        /// This Information Element specifies the value for the end of a hash
        /// function's potential output range.
        /// </summary>
        /// <remarks>
        /// See also Sections 6.2, 3.8 and 7.1 of [RFC5475].
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        HashOutputRangeMaximum = 330,

        /// <summary>
        /// This Information Element specifies the value for the beginning of a
        /// hash function's selected range.
        /// </summary>
        /// <remarks>
        /// See also Sections 6.2, 3.8 and 7.1 of [RFC5475].
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        HashSelectedRangeMininum = 331,

        /// <summary>
        /// This Information Element specifies the value for the end of a hash
        /// function's selected range.
        /// </summary>
        /// <remarks>
        /// See also Sections 6.2, 3.8 and 7.1 of [RFC5475].
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        HashSelectedRangeMaximum = 332,

        /// <summary>
        /// This Information Element contains a boolean value that is
        /// <c>true</c> if the output from this hash Selector has been 
        /// configured to be included in the packet report as a packet digest,
        /// else <c>false</c>.
        /// </summary>
        /// <remarks>
        /// See also Sections 6.2, 3.8 and 7.1 of [RFC5475].
        /// </remarks>
        [FieldLength(1)]
        [ClrType(typeof(bool))]
        HashDigestOutput = 333,

        /// <summary>
        /// This Information Element specifies the initialiser value to the hash
        /// function.
        /// </summary>
        /// <remarks>
        /// See also Sections 6.2, 3.8 and 7.1 of [RFC5475].
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        HashInitialiserValue = 334,

        /// <summary>
        /// The name of a selector identified by a <see cref="SelectorID"/>.
        /// </summary>
        /// <remarks>
        /// Globally unique per Metering Process.
        /// </remarks>
        [FieldLength]
        [ClrType(typeof(string))]
        SelectorName = 335,

        /// <summary>
        /// This Information Element specifies the upper limit of a confidence
        /// interval.
        /// </summary>
        /// <remarks>
        /// It is used to provide an accuracy statement for an estimated value.
        /// The confidence limits define the range in which the real value is
        /// assumed to be with a certain probability p. Confidence limits always
        /// need to be associated with a confidence level that defines this
        /// probability p. Please note that a confidence interval only provides
        /// a probability that the real value lies within the limits. That means
        /// the real value can lie outside the confidence limits. The
        /// <see cref="UpperConfidenceIntervalLimit"/>,
        /// <see cref="LowerConfidenceIntervalLimit"/> and 
        /// <see cref="confidenceLevel"/> Information Elements should all be
        /// used in an Options Template scoped to the observation to which they
        /// refer. See Section 3.4.2.1 of the IPFIX protocol document [RFC7011].
        /// Note that the <see cref="UpperConfidenceIntervalLimit"/>,
        /// <see cref="LowerConfidenceIntervalLimit"/> and
        /// <see cref="ConfidenceLevel"/> are all required to specify
        /// confidence and should be disregarded unless all three are specified
        /// together.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(double))]
        UpperConfidenceIntervalLimit = 336,

        /// <summary>
        /// This Information Element specifies the lower limit of a confidence
        /// interval.
        /// </summary>
        /// <remarks>
        /// For further information, see the description of
        /// <see cref="UpperConfidenceIntervalLimit"/>. The
        /// <see cref="UpperConfidenceIntervalLimit"/>,
        /// <see cref="LowerConfidenceIntervalLimit"/> and
        /// <see cref="confidenceLevel "/> Information Elements should all be
        /// used in an Options Template scoped to the observation to which
        /// they refer. See Section 3.4.2.1 of the IPFIX protocol document
        /// [RFC7011]. Note that the 
        /// <see cref="UpperConfidenceIntervalLimit"/>,
        /// <see cref="LowerConfidenceIntervalLimit"/> and
        /// <see cref="ConfidenceLevel "/> are all required to specify
        /// confidence and should be disregarded unless all three are specified
        /// together.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(double))]
        LowerConfidenceIntervalLimit = 337,

        /// <summary>
        /// This Information Element specifies the confidence level.
        /// </summary>
        /// <remarks>
        /// It is used to provide an accuracy statement for estimated values.
        /// The confidence level provides the probability p with which the
        /// real value lies within a given range. A confidence level always
        /// needs to be associated with confidence limits that define the range
        /// in which the real value is assumed to be. The 
        /// <see cref="UpperConfidenceIntervalLimit"/>,
        /// <see cref="LowerConfidenceIntervalLimit"/> and
        /// <see cref="ConfidenceLevel "/>Information Elements should all be
        /// used in an Options Template scoped to the observation to which
        /// they refer. See Section 3.4.2.1 of the IPFIX protocol document
        /// [RFC7011]. Note that the <see cref="upperCILimit"/>,
        /// <see cref="LowerConfidenceIntervalLimit"/> and
        /// <see cref="ConfidenceLevel "/> are all required to specify
        /// confidence, and should be disregarded unless all three are
        /// specified together.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(double))]
        ConfidenceLevel = 338,

        /// <summary>
        /// A description of the abstract data type of an IPFIX information
        /// element.
        /// </summary>
        /// <remarks>
        /// These are taken from the abstract data types defined in section 3.1
        /// of the IPFIX Information Model [RFC5102]; see that section for more
        /// information on the types described in the 
        /// <see cref="InformationElementDataType "/> sub-registry.These types
        /// are registered in the IANA IPFIX Information Element Data Type
        /// subregistry. This subregistry is intended to assign numbers for type
        /// names, not to provide a mechanism for adding data types to the IPFIX
        /// Protocol, and as such requires a Standards Action [RFC8126] to
        /// modify.
        /// </remarks>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        InformationElementDataType = 339,

        /// <summary>
        /// A UTF-8 [RFC3629] encoded Unicode string containing a human-readable
        /// description of an Information Element.
        /// </summary>
        /// The content of the <see cref="InformationElementDescription"/> MAY
        /// be annotated with one or more language tags [RFC4646], encoded
        /// in-line [RFC2482] within the UTF-8 string, in order to specify the
        /// language in which the description is written. Description text in
        /// multiple languages MAY tag each section with its own language tag;
        /// in this case, the description information in each language SHOULD
        /// have equivalent meaning. In the absence of any language tag, the
        /// i-default [RFC2277] language SHOULD be assumed. See the Security
        /// Considerations section for notes on string handling for Information
        /// Element type records.
        [ClrType(typeof(string))]
        InformationElementDescription = 340,    // TODO: UTF-8

        /// <summary>
        /// A UTF-8 [RFC3629] encoded Unicode string containing the name of an
        /// Information Element, intended as a simple identifier.
        /// </summary>
        /// <remarks>
        /// See the Security Considerations section for notes on string handling
        /// for Information Element type records.
        /// </remarks>
        [ClrType(typeof(string))]
        InformationElementName = 341,

        /// <summary>
        /// Contains the inclusive low end of the range of acceptable values for
        /// an Information Element.
        /// /// </summary>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        InformationElementRangeBegin = 342,

        /// <summary>
        /// Contains the inclusive high end of the range of acceptable values
        /// for an Information Element.
        /// </summary>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        InformationElementRangeEnd = 343,

        /// <summary>
        /// A description of the semantics of an IPFIX Information Element.
        /// </summary>
        /// <remarks>
        /// These are taken from the data type semantics defined in section 3.2
        /// of the IPFIX Information Model [RFC5102]; see that section for more
        /// information on the types defined in the 
        /// <see cref="InformationElementSemantics"/> sub-registry. This field
        /// may take the values in Table; the special value 0x00 (default) is
        /// used to note that no semantics apply to the field; it cannot be
        /// manipulated by a Collecting Process or File Reader that does not
        /// understand it a priori. These semantics are registered in the IANA
        /// IPFIX Information Element Semantics subregistry. This subregistry
        /// is intended to assign numbers for semantics names, not to provide
        /// a mechanism for adding semantics to the IPFIX Protocol, and as such
        /// requires a Standards Action [RFC8126] to modify.
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        InformationElementSemantics = 344,

        /// <summary>
        /// A description of the units of an IPFIX Information Element.
        /// </summary>
        /// <remarks>
        /// These correspond to the units implicitly defined in the Information
        /// Element definitions in section 5 of the IPFIX Information Model
        /// [RFC5102]; see that section for more information on the types
        /// described in the informationElementsUnits sub-registry. This field
        /// may take the values in Table 3 below; the special value 0x00 (none)
        /// is used to note that field is unitless. These types are registered
        /// in the IANA IPFIX Information Element Units subregistry; new types
        /// may be added on a First Come First Served [RFC8126] basis.
        [FieldLength(2)]
        [ClrType(typeof(ushort))]
        InformationElementUnits = 345,

        /// <summary>
        /// A private enterprise number, as assigned by IANA.
        /// </summary>
        /// <remarks>
        /// Within the context of an Information Element Type record, this
        /// element can be used along with the
        /// <see cref="InformationElementID" /> element to scope properties to
        /// a specific Information Element. To export type information about an
        /// IANA-assigned Information Element, set the 
        /// <see cref="PrivateEnterpriseNumber "/> to 0, or do not export the
        /// <see cref="PrivateEnterpriseNumber "/> in the type record. To export
        /// type information about an enterprise-specific Information Element,
        /// export the enterprise number in
        /// <see cref="PrivateEnterpriseNumber "/>, and export the Information 
        /// Element number with the Enterprise bit cleared in 
        /// <see cref="InformationElementID"/>. The Enterprise bit in the
        /// associated <see cref="InformationElementID"/> Information Element
        /// MUST be ignored by the Collecting Process.
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        PrivateEnterpriseNumber = 346,

        /// <summary>
        /// Instance Identifier of the interface to a Virtual Station.
        /// </summary>
        /// <remarks>
        /// A Virtual Station is an end station instance: it can be a virtual
        /// machine or a physical host.
        /// </remarks>
        [FieldLength]
        VirtualStationInterfaceID = 347,

        /// <summary>
        /// Name of the interface to a Virtual Station.
        /// </summary>
        /// <remarks>
        /// A Virtual Station is an end station instance: it can be a virtual
        /// machine or a physical host.
        /// </remarks>
        [FieldLength]
        [ClrType(typeof(string))]
        VirtualStationInterfaceName = 348,

        /// <summary>
        /// Unique Identifier of a Virtual Station.
        /// </summary>
        /// <remarks>
        /// A Virtual Station is an end station instance: it can be a virtual
        /// machine or a physical host.
        /// </remarks>
        [FieldLength]
        VirtualStationUuid = 349,

        /// <summary>
        /// Name of a Virtual Station.
        /// </summary>
        /// <remarks>
        /// A Virtual Station is an end station instance: it can be a virtual
        /// machine or a physical host.
        /// </remarks>
        [FieldLength]
        [ClrType(typeof(string))]
        VirtualStationName = 350,

        /// <summary>
        /// Identifier of a layer 2 network segment in an overlay network.
        /// </summary>
        /// <remarks>
        /// The most significant byte identifies the layer 2 network overlay
        /// network encapsulation type: 0x00 - reserved; 0x01 - VxLAN;
        /// 0x02 - NVGRE The three lowest significant bytes hold the value of
        /// the layer 2 overlay network segment identifier. For example:
        /// a 24 bit segment ID VXLAN Network Identifier (VNI); a 24 bit Tenant
        /// Network Identifier (TNI) for NVGRE
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        Layer2SegmentID = 351,

        /// <summary>
        /// The number of layer 2 octets since the previous report (if any) in
        /// incoming packets for this Flow at the Observation Point.
        /// </summary>
        /// <remarks>
        /// The number of octets includes layer 2 header(s) and layer 2 payload.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        Layer2OctetDeltaCount = 352,

        /// <summary>
        /// The total number of layer 2 octets in incoming packets for this Flow
        /// at the Observation Point since the Metering Process (re-)
        /// initialisation for this Observation Point.
        /// </summary>
        /// <remarks>
        /// The number of octets includes layer 2 header(s) and layer 2 payload.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        Layer2OctetTotalCount = 353,

        /// <summary>
        /// The total number of incoming unicast packets metered at the
        /// Observation Point since the Metering Process (re-) initialisation
        /// for this Observation Point.
        /// </summary>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        IngressUnicastPacketTotalCount = 354,

        /// <summary>
        /// The total number of incoming multicast packets metered at the
        /// Observation Point since the Metering Process (re-) initialisation
        /// for this Observation Point.
        /// </summary>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        IngressMulticastPacketTotalCount = 355,

        /// <summary>
        /// The total number of incoming broadcast packets metered at the
        /// Observation Point since the Metering Process (re-) initialisation
        /// for this Observation Point.
        /// </summary>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        IngressBroadcastPacketTotalCount = 356,

        /// <summary>
        /// The total number of outgoing unicast packets metered at the
        /// Observation Point since the Metering Process (re-) initialisation
        /// for this Observation Point.
        /// </summary>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        EgressUnicastPacketTotalCount = 357,

        /// <summary>
        /// The total number of outgoing broadcast packets metered at the
        /// Observation Point since the Metering Process (re-) initialisation
        /// for this Observation Point.
        /// </summary>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        EgressBroadcastPacketTotalCount = 358,

        /// <summary>
        /// The absolute timestamp at which the monitoring interval started.
        /// </summary>
        /// <remarks>
        /// A Monitoring interval is the period of time during which the
        /// Metering Process is running.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        MonitoringIntervalStartMilliSeconds = 359,

        /// <summary>
        /// The absolute timestamp at which the monitoring interval ended.
        /// </summary>
        /// <remarks>
        /// A Monitoring interval is the period of time during which the
        /// Metering Process is running.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        MonitoringIntervalEndMilliSeconds = 360,

        /// <summary>
        /// The port number identifying the start of a range of ports.
        /// </summary>
        /// <remarks>
        /// A value of zero indicates that the range start is not specified,
        /// i.e. the range is defined in some other way. Additional information
        /// on defined TCP port numbers can be found at [IANA registry
        /// service-names-port-numbers].
        /// </remarks>
        [FieldLength(2)]
        [ClrType(typeof(ushort))]
        PortRangeStart = 361,

        /// <summary>
        /// The port number identifying the end of a range of ports.
        /// </summary>
        /// <remarks>
        /// A value of zero indicates that the range end is not specified, i.e.
        /// the range is defined in some other way. Additional information on 
        /// defined TCP port numbers can be found at [IANA registry
        /// service-names-port-numbers].
        /// </remarks>
        [FieldLength(2)]
        [ClrType(typeof(ushort))]
        PortRangeEnd = 362,

        /// <summary>
        /// The step size in a port range.
        /// </summary>
        /// <remarks>
        /// The default step size is 1, which indicates contiguous ports. A
        /// value of zero indicates that the step size is not specified, i.e.
        /// the range is defined in some other way.
        /// </remarks>
        [FieldLength(2)]
        [ClrType(typeof(ushort))]
        PortRangeStepSize = 363,

        /// <summary>
        /// The number of ports in a port range.
        /// </summary>
        /// <remarks>
        /// A value of zero indicates that the number of ports is not specified,
        /// i.e. the range is defined in some other way.
        /// </remarks>
        [FieldLength(2)]
        [ClrType(typeof(ushort))]
        PortRangeNumPorts = 364,

        /// <summary>
        /// The IEEE 802 MAC address of a wireless station (STA).
        /// </summary>
        [FieldLength(6)]
        StaMacAddress = 365,

        /// <summary>
        /// The IPv4 address of a wireless station (STA).
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(IPAddress))]
        StaIPv4Address = 366,

        /// <summary>
        /// The IEEE 802 MAC address of a wireless access point (WTP).
        /// </summary>
        [FieldLength(6)]
        WtpMacAddress = 367,

        /// <summary>
        /// The type of interface where packets of this Flow are being received.
        /// </summary>
        /// <remarks>
        /// The value matches the value of managed object 'ifType' as defined in
        /// [IANA registry ianaiftype-mib].
        /// </remarks>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        IngressInterfaceType = 368,

        /// <summary>
        /// The type of interface where packets of this Flow are being sent.
        /// </summary>
        /// <remarks>
        /// The value matches the value of managed object 'ifType' as defined in
        /// [IANA registry ianaiftype-mib].
        /// </remarks>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        EgressInterfaceType = 369,

        /// <summary>
        /// The RTP sequence number per [RFC3550].
        /// </summary>
        [FieldLength(2)]
        [ClrType(typeof(ushort))]
        RtpSequenceNumber = 370,

        /// <summary>
        /// User name associated with the flow.
        /// </summary>
        [FieldLength]
        [ClrType(typeof(string))]
        UserName = 371,

        /// <summary>
        /// An attribute that provides a first level categorisation for each
        /// Application ID.
        /// </summary>
        [FieldLength]
        [ClrType(typeof(string))]
        ApplicationCategoryName = 372,

        /// <summary>
        /// An attribute that provides a second level categorisation for each
        /// Application ID.
        /// </summary>
        [FieldLength]
        [ClrType(typeof(string))]
        ApplicationSubCategoryName = 373,

        /// <summary>
        /// An attribute that groups multiple Application IDs that belong to the
        /// same networking application.
        /// </summary>
        [FieldLength]
        [ClrType(typeof(string))]
        ApplicationGroupName = 374,

        /// <summary>
        /// The non-conservative count of Original Flows contributing to this
        /// Aggregated Flow.
        /// </summary>
        /// <remarks>
        /// Non-conservative counts need not sum to the original count on
        /// re-aggregation.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        OriginalFlowsPresent = 375,

        /// <summary>
        /// The conservative count of Original Flows whose first packet is
        /// represented within this Aggregated Flow.
        /// </summary>
        /// <remarks>
        /// Conservative counts must sum to the original count on
        /// re-aggregation.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        OriginalFlowsInitiated = 376,

        /// <summary>
        /// The conservative count of Original Flows whose last packet is
        /// represented within this Aggregated Flow.
        /// </summary>
        /// <remarks>
        /// Conservative counts must sum to the original count on
        /// re-aggregation.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        OriginalFlowsCompleted = 377,

        /// <summary>
        /// The count of distinct source IP address values for Original Flows
        /// contributing to this Aggregated Flow, without regard to IP version.
        /// </summary>
        /// <remarks>
        /// This Information Element is preferred to the IP-version-specific
        /// counters, unless it is important to separate the counts by version.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        DistinctCountOfSourceIPAddress = 378,

        /// <summary>
        /// The count of distinct destination IP address values for Original
        /// Flows contributing to this Aggregated Flow, without regard to IP
        /// version.
        /// </summary>
        /// <remarks>
        /// This Information Element is preferred to the version-specific
        /// counters below, unless it is important to separate the counts by
        /// version.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        DistinctCountOfDestinationIPAddress = 379,

        /// <summary>
        /// The count of distinct source IPv4 address values for Original Flows
        /// contributing to this Aggregated Flow.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        DistinctCountOfSourceIPv4Address = 380,

        /// <summary>
        /// The count of distinct destination IPv4 address values for Original
        /// Flows contributing to this Aggregated Flow.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        DistinctCountOfDestinationIPv4Address = 381,

        /// <summary>
        /// The count of distinct source IPv6 address values for Original Flows
        /// contributing to this Aggregated Flow.
        /// </summary>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        DistinctCountOfSourceIPv6Address = 382,

        /// <summary>
        /// The count of distinct destination IPv6 address values for Original
        /// Flows contributing to this Aggregated Flow.
        /// </summary>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        DistinctCountOfDestinationIPv6Address = 383,

        /// <summary>
        /// A description of the method used to distribute the counters from
        /// Contributing Flows into the Aggregated Flow records described by an
        /// associated scope, generally a Template.
        /// </summary>
        /// <remarks>
        /// The method is deemed to apply to all the non-key Information
        /// Elements in the referenced scope for which value distribution is a
        /// valid operation; if the <see cref="OriginalFlowsInitiated "/> and/or
        /// <see cref="OriginalFlowsCompleted "/> Information Elements appear in
        /// the Template, they are not subject to this distribution method, as
        /// they each infer their own distribution method. This is intended
        /// to be a complete set of possible value distribution methods; it is
        /// encoded as follows:
        /// <c>
        /// +-------+-----------------------------------------------------------+
        /// | Value | Description                                               |
        /// +-------+-----------------------------------------------------------+
        /// | 0     | Unspecified: The counters for an Original Flow are        |
        /// |       | explicitly not distributed according to any other method  |
        /// |       | defined for this Information Element; use for arbitrary   |
        /// |       | distribution, or distribution algorithms not described by |
        /// |       | any other codepoint.                                      |
        /// |       | --------------------------------------------------------- |
        /// |       |                                                           |
        /// | 1     | Start Interval: The counters for an Original Flow are     |
        /// |       | added to the counters of the appropriate Aggregated Flow  |
        /// |       | containing the start time of the Original Flow.  This     |
        /// |       | should be assumed the default if value distribution       |
        /// |       | information is not available at a Collecting Process for  |
        /// |       | an Aggregated Flow.                                       |
        /// |       | --------------------------------------------------------- |
        /// |       |                                                           |
        /// | 2     | End Interval: The counters for an Original Flow are added |
        /// |       | to the counters of the appropriate Aggregated Flow        |
        /// |       | containing the end time of the Original Flow.             |
        /// |       | --------------------------------------------------------- |
        /// |       |                                                           |
        /// | 3     | Mid Interval: The counters for an Original Flow are added |
        /// |       | to the counters of a single appropriate Aggregated Flow   |
        /// |       | containing some timestamp between start and end time of   |
        /// |       | the Original Flow.                                        |
        /// |       | --------------------------------------------------------- |
        /// |       |                                                           |
        /// | 4     | Simple Uniform Distribution: Each counter for an Original |
        /// |       | Flow is divided by the number of time intervals the       |
        /// |       | Original Flow covers (i.e., of appropriate Aggregated     |
        /// |       | Flows sharing the same Flow Key), and this number is      |
        /// |       | added to each corresponding counter in each Aggregated    |
        /// |       | Flow.                                                     |
        /// |       | --------------------------------------------------------- |
        /// |       |                                                           |
        /// | 5     | Proportional Uniform Distribution: Each counter for an    |
        /// |       | Original Flow is divided by the number of time units the  |
        /// |       | Original Flow covers, to derive a mean count rate.  This  |
        /// |       | mean count rate is then multiplied by the number of time  |
        /// |       | units in the intersection of the duration of the Original |
        /// |       | Flow and the time interval of each Aggregated Flow.  This |
        /// |       | is like simple uniform distribution, but accounts for the |
        /// |       | fractional portions of a time interval covered by an      |
        /// |       | Original Flow in the first and last time interval.        |
        /// |       | --------------------------------------------------------- |
        /// |       |                                                           |
        /// | 6     | Simulated Process: Each counter of the Original Flow is   |
        /// |       | distributed among the intervals of the Aggregated Flows   |
        /// |       | according to some function the Intermediate Aggregation   |
        /// |       | Process uses based upon properties of Flows presumed to   |
        /// |       | be like the Original Flow.  This is essentially an        |
        /// |       | assertion that the Intermediate Aggregation Process has   |
        /// |       | no direct packet timing information but is nevertheless   |
        /// |       | not using one of the other simpler distribution methods.  |
        /// |       | The Intermediate Aggregation Process specifically makes   |
        /// |       | no assertion as to the correctness of the simulation.     |
        /// |       | --------------------------------------------------------- |
        /// |       |                                                           |
        /// | 7     | Direct: The Intermediate Aggregation Process has access   |
        /// |       | to the original packet timings from the packets making up |
        /// |       | the Original Flow, and uses these to distribute or        |
        /// |       | recalculate the counters.                                 |
        /// +-------+-----------------------------------------------------------+
        /// </c>
        /// </remarks>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        ValueDistributionMethod = 384,


        /// <summary>
        /// Interarrival jitter as defined in section 6.4.1 of [RFC3550],
        /// measured in milliseconds.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        Rfc3550JitterMilliseconds = 385,

        /// <summary>
        /// Interarrival jitter as defined in section 6.4.1 of [RFC3550],
        /// measured in microseconds.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        Rfc3550JitterMicroseconds = 386,

        /// <summary>
        /// Interarrival jitter as defined in section 6.4.1 of [RFC3550],
        /// measured in nanoseconds.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        Rfc3550JitterNanoseconds = 387,

        /// <summary>
        /// The value of the 1-bit Drop Eligible Indicator (DEI) field of the 
        /// VLAN tag as described in 802.1Q-2011 subclause 9.6.
        /// </summary>
        /// <remarks>
        /// In case of a QinQ frame, it represents the outer tag's DEI field and
        /// in case of an IEEE 802.1ad frame it represents the DEI field of the
        /// S-TAG. Note: in earlier versions of 802.1Q the same bit field in the
        /// incoming packet is occupied by the Canonical Format Indicator (CFI)
        /// field, except for S-TAGs.
        /// </remarks>
        [FieldLength(1)]
        [ClrType(typeof(bool))]
        Dot1QDropEligibleIndicator = 388,

        /// <summary>
        /// In case of a QinQ frame, it represents the inner tag's Drop Eligible
        /// Indicator (DEI) field and in case of an IEEE 802.1ad frame it
        /// represents the DEI field of the C-TAG.
        /// </summary>
        [FieldLength(1)]
        [ClrType(typeof(bool))]
        Dot1QCustomerDropEligibleIndicator = 389,

        /// <summary>
        /// This Information Element identifies the Intermediate Flow Selection
        /// Process technique (e.g.  Filtering, Sampling) that is applied by the
        /// Intermediate Flow Selection Process.
        /// </summary>
        /// <remarks>
        /// Most of these techniques have parameters. Its configuration
        /// parameter(s) MUST be clearly specified. Further Information Elements
        /// are needed to fully specify packet selection with these methods and
        /// all their parameters. Further method identifiers may be added to the
        /// flowSelectorAlgorithm registry. It might be necessary to define new
        /// Information Elements to specify their parameters.The
        /// <see cref="FlowSelectorAlgorithm"/> registry is maintained by IANA.
        /// New assignments for the registry will be administered by IANA, on a
        /// First Come First Served basis [RFC8126], subject to Expert Review
        /// [RFC8126]. Please note that the purpose of the flow selection
        /// techniques described in this document is the improvement of 
        /// measurement functions as defined in the Scope (Section 1). Before
        /// adding new flow selector algorithms it should be checked what is
        /// their intended purpose and especially if those contradict with
        /// policies defined in [RFC2804]. The designated expert(s) should
        /// consult with the community if a request is received that runs
        /// counter to[RFC2804]. The registry can be updated when
        /// specifications of the new method(s) and any new Information Elements
        /// are provided.The group of experts must double check the
        /// <see cref="FlowSelectorAlgorithm"/> definitions and Information
        /// Elements with already defined <see cref="FlowSelectorAlgorithm"/>
        /// and Information Elements for completeness, accuracy and redundancy.
        /// Those experts will initially be drawn from the Working Group Chairs
        /// and document editors of the IPFIX and PSAMP Working Groups. The
        /// Intermediate Flow Selection Process Techniques identifiers are
        /// defined at
        /// [http://www.iana.org/assignments/ipfix/ipfix.xhtml#ipfix-flowselectoralgorithm].
        /// </remarks>
        [FieldLength(2)]
        [ClrType(typeof(FlowSelectorAlgorithm))]
        FlowSelectorAlgorithm = 390,

        /// <summary>
        /// This Information Element specifies the volume in octets of all Flows
        /// that are selected in the Intermediate Flow Selection Process since
        /// the previous report.
        /// </summary>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        FlowSelectedOctetDeltaCount = 391,

        /// <summary>
        /// This Information Element specifies the volume in packets of all
        /// Flows that were selected in the Intermediate Flow Selection Process
        /// since the previous report.
        /// /// </summary>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        FlowSelectedPacketDeltaCount = 392,

        /// <summary>
        /// This Information Element specifies the number of Flows that were
        /// selected in the Intermediate Flow Selection Process since the last
        /// report.
        /// </summary>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        FlowSelectedFlowDeltaCount = 393,

        /// <summary>
        /// This Information Element specifies the total number of Flows
        /// observed by a Selector, for a specific value of
        /// <see cref="SelectorID" />.
        /// </summary>
        /// <remarks>
        /// This Information Element should be used in an Options Template
        /// scoped to the observation to which it refers.See Section 3.4.2.1
        /// of the IPFIX protocol document [RFC7011].
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        SelectorIDTotalFlowsObserved = 394,

        /// <summary>
        /// This Information Element specifies the total number of Flows selected
        /// by a Selector, for a specific value of <see cref="SelectorID"/>.
        /// </summary>
        /// <remarks>
        /// This Information Element should be used in an Options Template
        /// scoped to the observation to which it refers. See Section 3.4.2.1 of
        /// the IPFIX protocol document [RFC7011].
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        SelectorIDTotalFlowsSelected = 395,

        /// <summary>
        /// This Information Element specifies the number of Flows that are
        /// consecutively sampled.
        /// </summary>
        /// <remarks>
        /// A value of 100 means that 100 consecutive Flows are sampled. For
        /// example, this Information Element may be used to describe the
        /// configuration of a systematic count-based Sampling Selector.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        SamplingFlowInterval = 396,

        /// <summary>
        /// This Information Element specifies the number of Flows between two
        /// <see cref="SamplingFlowInterval" />s.
        /// </summary>
        /// <remarks>
        /// A value of 100 means that the next interval starts 100 Flows (which
        /// are not sampled) after the current
        /// <see cref="SamplingFlowInterval "/> is over. For example, this
        /// Information Element may be used to describe the configuration of a
        /// systematic count-based Sampling Selector.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        SamplingFlowSpacing = 397,

        /// <summary>
        /// This Information Element specifies the time interval in microseconds
        /// during which all arriving Flows are sampled.
        /// </summary>
        /// <remarks>
        /// For example, this Information Element may be used to describe the
        /// configuration of a systematic time-based Sampling Selector.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        FlowSamplingTimeInterval = 398,

        /// <summary>
        /// This Information Element specifies the time interval in microseconds
        /// between two <see cref="FlowSamplingTimeIntervals"/>.
        /// </summary>
        /// <remarks>
        /// A value of 100 means that the next interval starts 100 microseconds
        /// (during which no Flows are sampled) after the current 
        /// <see cref="FlowsamplingTimeInterval "/>is over. For example, this
        /// Information Element may used to describe the configuration of a
        /// systematic time-based Sampling Selector.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        FlowSamplingTimeSpacing = 399,

        /// <summary>
        /// This Information Element specifies the Information Elements that are
        /// used by the Hash-based Flow Selector as the Hash Domain.
        /// </summary>
        [FieldLength(2)]
        [ClrType(typeof(ushort))]
        HashFlowDomain = 400,

        /// <summary>
        /// The number of octets, excluding IP header(s) and Layer 4 transport
        /// protocol header(s), observed for this Flow at the Observation Point
        /// since the previous report (if any).
        /// </summary>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        TransportOctetDeltaCount = 401,

        /// <summary>
        /// The number of packets containing at least one octet beyond the IP
        /// header(s) and Layer 4 transport protocol header(s), observed for
        /// this Flow at the Observation Point since the previous report
        /// (if any).
        /// </summary>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        TransportPacketDeltaCount = 402,

        /// <summary>
        /// The IPv4 address used by the Exporting Process on an Original
        /// Exporter, as seen by the Collecting Process on an IPFIX Mediator.
        /// </summary>
        /// <remarks>
        /// Used to provide information about the Original Observation Points
        /// to a downstream Collector.
        /// </remarks>
        [FieldLength(4)]
        [ClrType(typeof(IPAddress))]
        OriginalExporterIPv4Address = 403,

        /// <summary>
        /// The IPv6 address used by the Exporting Process on an Original
        /// Exporter, as seen by the Collecting Process on an IPFIX Mediator.
        /// </summary>
        /// <remarks></remarks>
        /// Used to provide information about the Original Observation Points
        /// to a downstream Collector.
        /// </remarks>
        [FieldLength(16)]
        [ClrType(typeof(IPAddress))]
        OriginalExporterIPv6Address = 404,

        /// <summary>
        /// The Observation Domain ID reported by the Exporting Process on an
        /// Original Exporter, as seen by the Collecting Process on an IPFIX
        /// Mediator.
        /// </summary>
        /// <remarks>
        /// Used to provide information about the Original Observation Domain
        /// to a downstream Collector. When cascading through multiple
        /// Mediators, this identifies the initial Observation Domain in the
        /// cascade.
        /// </remarks>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        OriginalObservationDomainID = 405,

        /// <summary>
        /// An identifier of an Intermediate Process that is unique per IPFIX
        /// Device.
        /// </summary>
        /// <remarks>
        /// Typically, this Information Element is used for limiting the scope
        /// of other Information Elements. Note that process identifiers may be
        /// assigned dynamically; that is, an Intermediate Process may be
        /// restarted with a different ID.
        /// </remarks>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        IntermediateProcessID = 406,

        /// <summary>
        /// The total number of received Data Records that the Intermediate
        /// Process did not process since the (re-) initialisation of the
        /// Intermediate Process; includes only Data Records not examined or
        /// otherwise handled by the Intermediate Process due to resource
        /// constraints, not Data Records that were examined or otherwise
        /// handled by the Intermediate Process but those that merely do not
        /// contribute to any exported Data Record due to the operations
        /// performed by the Intermediate Process.
        /// </summary>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        IgnoredDataRecordTotalCount = 407,

        /// <summary>
        /// This Information Element specifies the type of the selected data
        /// link frame.
        /// </summary>
        /// <remarks>
        /// <para>The following data link types are defined here:
        /// - 0x01 IEEE802.3 ETHERNET [IEEE802.3]
        /// - 0x02 IEEE802.11 MAC Frame format [IEEE802.11]</para>
        /// <para>Further values may be assigned by IANA. Note that the
        /// assigned values are bits so that multiple observations can be
        /// OR'd together. The data link layer is defined in
        /// [ISO/IEC.7498-1:1994].</para>
        /// </remarks>
        [FieldLength(2)]
        [ClrType(typeof(ushort))]
        DataLinkFrameType = 408,

        /// <summary>
        /// This Information Element specifies the offset of the packet section
        /// (e.g.  <see cref="DataLinkFrameSection"/>,
        /// <see cref="IPHeaderPacketSection"/>,
        /// <see cref="IPPayloadPacketSection"/>,
        /// <see cref="MplsLabelStackSection"/> and
        /// <see cref="MplsPayloadPacketSection"/>).
        /// </summary>
        /// <remarks>
        /// If this Information Element is omitted, it defaults to zero (i.e. no
        /// offset). If multiple sectionOffset Information Elements are
        /// specified within a single Template, then they apply to the packet
        /// section Information Elements in order: the first sectionOffset
        /// applies to the first packet section, the second to the second
        /// and so on. Note that the closest <see cref="SectionOffset"/> and
        /// packet section Information Elements within a given Template are
        /// not necessarily related. If there are fewer
        /// <see cref="SectionOffset"/> Information Elements than packet section
        /// Information Elements, then subsequent packet section Information
        /// Elements have no offset, i.e. a <see cref="SectionOffset"/> of zero
        /// applies to those packet section Information Elements. If there are
        /// more <see cref="SectionOffset"/> Information Elements than the
        /// number of packet section Information Elements, then the additional
        /// <see cref="SectionOffset"/>Information Elements are meaningless.
        /// </remarks>
        [FieldLength(2)]
        [ClrType(typeof(ushort))]
        SectionOffset = 409,

        /// <summary>
        /// This Information Element specifies the observed length of the packet
        /// section  (e.g.  <see cref="DataLinkFrameSection"/>,
        /// <see cref="IPHeaderPacketSection"/>,
        /// <see cref="IPPayloadPacketSection"/>,
        /// <see cref="MplsLabelStackSection"/> and
        /// <see cref="MplsPayloadPacketSection"/>) when padding is used.
        /// </summary>
        /// <remarks>
        /// The packet section may be of a fixed size larger than the 
        /// <see cref="SectionExportedOctets"/>. In this case, octets in the
        /// packet section beyond the <see cref="SectionExportedOctets"/> MUST
        /// follow the [RFC7011] rules for padding (i.e. be composed of zero
        /// (0) valued octets).
        /// </remarks>
        [FieldLength(2)]
        [ClrType(typeof(ushort))]
        SectionExportedOctets = 410,

        /// <summary>
        /// This Information Element, which is 16 octets long, represents the
        /// Backbone Service Instance Tag (I-TAG) Tag Control Information (TCI)
        /// field of an Ethernet frame as described in [IEEE802.1Q].
        /// </summary>
        /// <remarks>
        /// It encodes the Backbone Service Instance Priority Code Point
        /// (I-PCP), Backbone Service Instance Drop Eligible Indicator (I-DEI),
        /// Use Customer Addresses (UCAs), Backbone Service Instance Identifier
        /// (I-SID), Encapsulated Customer Destination Address (C-DA),
        /// Encapsulated Customer Source Address (C-SA) and reserved fields.
        /// The structure and semantics within the Tag Control Information field
        /// are defined in [IEEE802.1Q].
        /// </remarks>
        [FieldLength]
        Dot1QServiceInstanceTag = 411,

        /// <summary>
        /// The value of the 24-bit Backbone Service Instance Identifier (I-SID)
        /// portion of the Backbone Service Instance Tag (I-TAG) Tag Control
        /// Information (TCI) field of an Ethernet frame as described in
        /// [IEEE802.1Q].
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        Dot1QServiceInstanceID = 412,

        /// <summary>
        /// The value of the 3-bit Backbone Service Instance Priority Code Point
        /// (I-PCP) portion of the Backbone Service Instance Tag (I-TAG) Tag
        /// Control Information (TCI) field of an Ethernet frame as described
        /// in [IEEE802.1Q].
        /// </summary>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        Dot1QServiceInstancePriority = 413,

        /// <summary>
        /// The value of the Encapsulated Customer Source Address (C-SA) portion
        /// of the Backbone Service Instance Tag (I-TAG) Tag Control Information
        /// (TCI) field of an Ethernet frame as described in [IEEE802.1Q].
        /// </summary>
        [FieldLength(6)]
        Dot1QCustomerSourceMacAddress = 414,

        /// <summary>
        /// The value of the Encapsulated Customer Destination Address (C-DA)
        /// portion of the Backbone Service Instance Tag (I-TAG) Tag Control
        /// Information (TCI) field of an Ethernet frame as described in
        /// [IEEE802.1Q].
        /// </summary>
        [FieldLength(6)]
        Dot1qCustomerDestinationMacAddress = 415,

        /// <summary>
        /// Duplicate of Information Element ID 352,
        /// <see cref="Layer2OctetDeltaCount"/>.
        /// </summary>
        [Obsolete("This is a duplicate of Layer2OctetDeltaCount.")]
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        DuplicateOfLayer2OctetDeltaCount = 416,

        /// <summary>
        /// The definition of this Information Element is identical to the
        /// definition of the <see cref="Layer2OctetDeltaCount "/> Information
        /// Element, except that it reports a potentially modified value caused
        /// by a middlebox function after the packet passed the Observation
        /// Point.
        /// </summary>
        /// <remarks>
        /// This Information Element is the layer 2 version of
        /// <see cref="PostOctetDeltaCount"/>.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        PostLayer2OctetDeltaCount = 417,

        /// <summary>
        /// The number of layer 2 octets since the previous report (if any) in
        /// outgoing multicast packets sent for packets of this Flow by a
        /// multicast daemon within the Observation Domain.
        /// </summary>
        /// <remarks>
        /// This property cannot necessarily be observed at the Observation
        /// Point, but may be retrieved by other means. The number of octets
        /// includes layer 2 header(s) and layer 2 payload. This Information
        /// Element is the layer 2 version of
        /// <see cref="PostMulticastOctetDeltaCount"/>.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        PostMulticastLayer2OctetDeltaCount = 418,

        /// <summary>
        /// Duplicate of Information Element ID 353,
        /// <see cref="Layer2OctetTotalCount"/>.
        /// </summary>
        [Obsolete("This is a duplicate of Layer2OctetTotalCount.")]
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        DuplicateOfLayer2OctetTotalCount = 419,

        /// <summary>
        /// The definition of this Information Element is identical to the
        /// definition of the <see cref="Layer2OctetTotalCount "/> Information
        /// Element, except that it reports a potentially modified value caused
        /// by a middlebox function after the packet passed the Observation
        /// Point.
        /// </summary>
        /// <remarks>
        /// This Information Element is the layer 2 version of 
        /// <see cref="PostOctetDeltaCount"/>.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        PostLayer2OctetTotalCount = 420,

        /// <summary>
        /// The total number of layer 2 octets in outgoing multicast packets
        /// sent for packets of this Flow by a multicast daemon in the
        /// Observation Domain since the Metering Process (re-) initialisation.
        /// </summary>
        /// <remarks>
        /// This property cannot necessarily be observed at the Observation
        /// Point, but may be retrieved by other means. The number of octets
        /// includes layer 2 header(s) and layer 2 payload. This Information
        /// Element is the layer 2 version of
        /// <see cref="PostMulticastOctetTotalCount"/>.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        PostMulticastLayer2OctetTotalCount = 421,

        /// <summary>
        /// Layer 2 length of the smallest packet observed for this Flow.
        /// </summary>
        /// <remarks>
        /// The packet length includes the length of the layer 2 header(s)
        /// and the length of the layer 2 payload. This Information Element
        /// is the layer 2 version of <see cref="MinimumIPTotalLength"/>.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        MinimumLayer2TotalLength = 422,

        /// <summary>
        /// Layer 2 length of the largest packet observed for this Flow.
        /// </summary>
        /// <remarks>
        /// The packet length includes the length of the layer 2 header(s) and
        /// the length of the layer 2 payload. This Information Element is the
        /// layer 2 version of <see cref="MaximumIPTotalLength"/>.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        MaximumLayer2TotalLength = 423,

        /// <summary>
        /// The number of layer 2 octets since the previous report (if any) in
        /// packets of this Flow dropped by packet treatment.
        /// </summary>
        /// <remarks>
        /// The number of octets includes layer 2 header(s) and layer 2 payload.
        /// This Information Element is the layer 2 version of
        /// <see cref="DroppedOctetDeltaCount"/>.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        DroppedLayer2OctetDeltaCount = 424,

        /// <summary>
        /// The total number of octets in observed layer 2 packets (including
        /// the layer 2 header) that were dropped by packet treatment since the
        /// (re-) initialisation of the Metering Process.
        /// </summary>
        /// <remarks>
        /// This Information Element is the layer 2 version of 
        /// <see cref="DroppedOctetTotalCount"/>.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        DroppedLayer2OctetTotalCount = 425,

        /// <summary>
        /// The total number of octets in observed layer 2 packets (including
        /// the layer 2 header) that the Metering Process did not process since
        /// the (re-) initialisation of the Metering Process.
        /// </summary>
        /// <remarks>
        /// This Information Element is the layer 2 version of
        /// <see cref="IgnoredOctetTotalCount"/>.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        IgnoredLayer2OctetTotalCount = 426,

        /// <summary>
        /// The total number of octets in observed layer 2 packets (including
        /// the layer 2 header) that the Metering Process did not process since
        /// the (re-) initialisation of the Metering Process.
        /// </summary>
        /// <remarks>
        /// This Information Element is the layer 2 version of
        /// <see cref="NotSentOctetTotalCount"/>.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        NotSentLayer2OctetTotalCount = 427,

        /// <summary>
        /// The sum of the squared numbers of layer 2 octets per incoming packet
        /// since the previous report (if any) for this Flow at the Observation
        /// Point.
        /// </summary>
        /// <remarks>
        /// The number of octets includes layer 2 header(s) and layer 2 payload.
        /// This Information Element is the layer 2 version of
        /// <see cref="OctetDeltaSumOfSquares"/>.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        Layer2OctetDeltaSumOfSquares = 428,

        /// <summary>
        /// The total sum of the squared numbers of layer 2 octets in incoming
        /// packets for this Flow at the Observation Point since the Metering
        /// Process (re-) initialisation for this Observation Point.
        /// </summary>
        /// <remarks>
        /// The number of octets includes layer 2 header(s) and layer 2 payload.
        /// This Information Element is the layer 2 version of
        /// <see cref="OctetTotalSumOfSquares"/>.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        Layer2OctetTotalSumOfSquares = 429,

        /// <summary>
        /// The number of incoming layer 2 frames since the previous report (if
        /// any) for this Flow at the Observation Point.
        /// </summary>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        Layer2FrameDeltaCount = 430,

        /// <summary>
        /// The total number of incoming layer 2 frames for this Flow at the
        /// Observation Point since the Metering Process (re-) initialisation
        /// for this Observation Point.
        /// </summary>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        Layer2FrameTotalCount = 431,

        /// <summary>
        /// The destination IPv4 address of the PSN tunnel carrying the
        /// pseudowire.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(IPAddress))]
        PseudoWireDestinationIPv4Address = 432,

        /// <summary>
        /// The total number of observed layer 2 frames that the Metering
        /// Process did not process since the (re-) initialisation of the
        /// Metering Process.
        /// </summary>
        /// <remarks>
        /// This Information Element is the layer 2 version of 
        /// <see cref="IgnoredPacketTotalCount"/>.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        IgnoredLayer2FrameTotalCount = 433,

        /// <summary>
        /// An IPFIX Information Element that denotes that the integer value
        /// of a MIB object will be exported.
        /// </summary>
        /// <remarks>
        /// The MIB Object Identifier (<see cref="MibObjectIdentifier"/>) for
        /// this field MUST be exported in a MIB Field Option or via another
        /// means. This Information Element is used for MIB objects with the
        /// Base syntax of Integer32 and INTEGER with IPFIX reduced-size
        /// encoding used as required. The value is encoded as per the standard
        /// IPFIX Abstract Data Type of signed32.
        /// </remarks>
        [FieldLength(4)]
        [ClrType(typeof(int))]
        MibObjectValueInteger = 434,

        /// <summary>
        /// An IPFIX Information Element that denotes that an Octet String or
        /// Opaque value of a MIB object will be exported.
        /// </summary>
        /// <remarks>
        /// The MIB Object Identifier (<see cref="MibObjectIdentifier"/>) for
        /// this field MUST be exported in a MIB Field Option or via another
        /// means. This Information Element is used for MIB objects with the
        /// Base syntax of OCTET STRING and Opaque. The value is encoded as
        /// per the standard IPFIX Abstract Data Type of octetArray.
        [FieldLength]
        MibObjectValueOctetString = 435,

        /// <summary>
        /// An IPFIX Information Element that denotes that an Object Identifier
        /// or OID value of a MIB object will be exported.
        /// </summary>
        /// <remarks>
        /// The MIB Object Identifier (<see cref="MibObjectIdentifier"/>) for
        /// this field MUST be exported in a MIB Field Option or via another
        /// means. This Information Element is used for MIB objects with the
        /// Base syntax of OBJECT IDENTIFIER. Note: In this case, the
        /// <see cref="MibObjectIdentifier"/> defines which MIB object is being
        /// exported, and the <see cref="MibObjectValueOid"/> field will contain
        /// the OID value of that MIB object. The
        /// <see cref="MibObjectValueOid"/> Information Element is encoded as
        /// ASN.1/BER [X.690] in an octetArray.
        [FieldLength]
        MibObjectValueOid = 436,

        /// <summary>
        /// An IPFIX Information Element that denotes that a set of Enumerated
        /// flags or bits from a MIB object will be exported.
        /// </summary>
        /// <remarks>
        /// The MIB Object Identifier (<see cref="MibObjectIdentifier"/>) for
        /// this field MUST be exported in a MIB Field Option or via another
        /// means. This Information Element is used for MIB objects with the
        /// Base syntax of BITS. The flags or bits are encoded as per the
        /// standard IPFIX Abstract Data Type of octetArray, with sufficient
        /// length to accommodate the required number of bits. If the number
        /// of bits is not an integer multiple of octets, then the most
        /// significant bits at the end of the octetArray MUST be set to 0.
        [FieldLength]
        MibObjectValueBits = 437,

        /// <summary>
        /// An IPFIX Information Element that denotes that the IPv4 address
        /// value of a MIB object will be exported.
        /// </summary>
        /// <remarks>
        /// The MIB Object Identifier(<see cref="MibObjectIdentifier"/>) for
        /// this field MUST be exported in a MIB Field Option or via another
        /// means.This Information Element is used for MIB objects with the
        /// Base syntax of ipAddress.The value is encoded as per the standard
        /// IPFIX Abstract Data Type of ipv4Address.
        /// </remarks>
        [FieldLength(4)]
        [ClrType(typeof(IPAddress))]
        MibObjectValueIPAddress = 438,

        /// <summary>
        /// An IPFIX Information Element that denotes that the counter value of
        /// a MIB object will be exported.
        /// </summary>
        /// <remarks>
        /// The MIB Object Identifier (<see cref="MibObjectIdentifier"/>) for
        /// this field MUST be exported in a MIB Field Option or via another
        /// means. This Information Element is used for MIB objects with the
        /// Base syntax of Counter32 or Counter64 with IPFIX reduced-size
        /// encoding used as required. The value is encoded as per the standard
        /// IPFIX Abstract Data Type of unsigned64.
        /// </remarks>
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        MibObjectValueCounter = 439,

        /// <summary>
        /// An IPFIX Information Element that denotes that the Gauge value of a
        /// MIB object will be exported.
        /// </summary>
        /// <remarks>
        /// The MIB Object Identifier (<see cref="MibObjectIdentifier"/>) for
        /// this field MUST be exported in a MIB Field Option or via another
        /// means. This Information Element is used for MIB objects with the
        /// Base syntax of Gauge32. The value is encoded as per the standard
        /// IPFIX Abstract Data Type of unsigned32. This value represents a
        /// non-negative integer that may increase or decrease but that shall
        /// never exceed a maximum value or fall below a minimum value.
        /// </remarks>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        MibObjectValueGauge = 440,

        /// <summary>
        /// An IPFIX Information Element that denotes that the TimeTicks value of
        /// a MIB object will be exported.
        /// </summary>
        /// <remarks>
        /// The MIB Object Identifier (<see cref="MibObjectIdentifier"/>) for
        /// this field MUST be exported in a MIB Field Option or via another
        /// means. This Information Element is used for MIB objects with the
        /// Base syntax of TimeTicks. The value is encoded as per the standard
        /// IPFIX Abstract Data Type of unsigned32.
        /// </remarks>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        MibObjectValueTimeTicks = 441,

        /// <summary>
        /// An IPFIX Information Element that denotes that an unsigned integer
        /// value of a MIB object will be exported.
        /// </summary>
        /// <remarks>
        /// The MIB Object Identifier (<see cref="MibObjectIdentifier"/>) for
        /// this field MUST be exported in a MIB Field Option or via another
        /// means. This Information Element is used for MIB objects with the
        /// Base syntax of unsigned32 with IPFIX reduced-size encoding used as
        /// required. The value is encoded as per the standard IPFIX Abstract
        /// Data Type of unsigned32.
        /// </remarks>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        MibObjectValueUnsigned = 442,

        /// <summary>
        /// An IPFIX Information Element that denotes that a complete or partial
        /// conceptual table will be exported.
        /// </summary>
        /// <remarks>
        /// The MIB Object Identifier (<see cref="MibObjectIdentifier"/>) for
        /// this field MUST be exported in a MIB Field Option or via another
        /// means. This Information Element is used for MIB objects with a
        /// syntax of SEQUENCE OF.This is encoded as a subTemplateList of
        /// <see cref="mibObjectValue"/> Information Elements. The Template
        /// specified in the subTemplateList MUST be an Options Template and
        /// MUST include all the objects listed in the INDEX clause as Scope
        /// Fields.
        /// </remarks>
        [FieldLength]
        //[ClrType(typeof(subTemplateList))] TODO
        MibObjectValueTable = 443,

        /// <summary>
        /// An IPFIX Information Element that denotes that a single row of a
        /// conceptual table will be exported.
        /// </summary>
        /// <remarks>
        /// The MIB Object Identifier (<see cref="MibObjectIdentifier"/>) for
        /// this field MUST be exported in a MIB Field Option or via another
        /// means. This Information Element is used for MIB objects with a
        /// syntax of SEQUENCE. This is encoded as a subTemplateList of
        /// <see cref="MibObjectValue"/> Information Elements. The
        /// subTemplateList exported MUST contain exactly one row (i.e. one
        /// instance of the subTemplate). The Template specified in the
        /// subTemplateList MUST be an Options Template and MUST include all
        /// the objects listed in the INDEX clause as Scope Fields.
        /// </remarks>
        [FieldLength]
        //[ClrType(typeof(subTemplateList))] TODO
        MibObjectValueRow = 444,

        /// <summary>
        /// An IPFIX Information Element that denotes that a MIB Object
        /// Identifier (MIB OID) is exported in the (Options) Template Record.
        /// </summary>
        /// <remarks>
        /// The <see cref="MibObjectIdentifier"/> Information Element contains
        /// the OID assigned to the MIB object type definition encoded as
        /// ASN.1/BER [X.690].
        /// </remarks>
        [FieldLength]
        MibObjectIdentifier = 445,

        /// <summary>
        /// A non-negative sub-identifier of an Object Identifier (OID).
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        MibSubIdentifier = 446,

        /// <summary>
        /// A set of bit fields that is used for marking the Information
        /// Elements of a Data Record that serve as INDEX MIB objects for an
        /// indexed columnar MIB object.
        /// </summary>
        /// <remarks>
        /// Each bit represents an Information Element in the Data Record, with
        /// the n-th least significant bit representing the n-th Information
        /// Element. A bit set to 1 indicates that the corresponding Information
        /// Element is an index of the columnar object represented by the
        /// <see cref="MibObjectValue"/>. A bit set to 0 indicates that this is
        /// not the case. If the Data Record contains more than 64 Information
        /// Elements, the corresponding Template SHOULD be designed such that
        /// all index fields are among the first 64 Information Elements,
        /// because the mibIndexIndicator only contains 64 bits.If the Data
        /// Record contains less than 64 Information Elements, then the extra
        /// bits in the mibIndexIndicator for which no corresponding
        /// Information Element exists MUST have the value 0 and must be
        /// disregarded by the Collector. This Information Element may be
        /// exported with IPFIX reduced-size encoding.
        [FieldLength(8)]
        [ClrType(typeof(ulong))]
        MibIndexIndicator = 447,

        /// <summary>
        /// Indicates when in the lifetime of the Flow the MIB value was
        /// retrieved from the MIB for a <see cref="MibObjectIdentifier"/>.
        /// </summary>
        /// <remarks>
        /// This is used to indicate if the value exported was collected from
        /// the MIB closer to Flow creation or Flow export time and refers to
        /// the Timestamp fields included in the same Data Record. This field
        /// SHOULD be used when exporting a mibObjectValue that specifies
        /// counters or statistics. If the MIB value was sampled by SNMP prior
        /// to the IPFIX Metering Process or Exporting Process retrieving the
        /// value (i.e. the data is already stale) and it is important to know
        /// the exact sampling time, then an additional
        /// observationTime * element should be paired with the OID using IPFIX
        /// Structured Data [RFC6313]. Similarly, if different MIB capture times
        /// apply to different mibObjectValue elements within the Data Record,
        /// then individual mibCaptureTimeSemantics Information Elements should
        /// be paired with each OID using IPFIX Structured Data. Values:
        /// 0: undefined; 1: begin - The value for the MIB object is captured
        /// from the MIB when the Flow is first observed; 2: end - The value for
        /// the MIB object is captured from the MIB when the Flow ends;
        /// 3: export - The value for the MIB object is captured from the MIB at
        /// export time; 4: average - The value for the MIB object is an average
        /// of multiple captures from the MIB over the observed life of the Flow.
        /// </remarks>
        [FieldLength(1)]
        [ClrType(typeof(byte))]
        MibCaptureTimeSemantics = 448,

        /// <summary>
        /// A <see cref="MibContextEngineID"/> that specifies the SNMP engine ID
        /// for a MIB field being exported over IPFIX.Definition as per
        /// [RFC3411], Section 3.3.
        /// </summary>
        [FieldLength]
        MibContextEngineID = 449,

        /// <summary>
        /// An Information Element that denotes that a MIB context name is
        /// specified for a MIB field being exported over IPFIX.
        /// </summary>
        /// <remarks>
        /// Reference [RFC3411], Section 3.3.
        /// </remarks>
        [FieldLength]
        [ClrType(typeof(string))]
        MibContextName = 450,

        /// <summary>
        /// The name (called a descriptor in [RFC2578]) of an object type
        /// definition.
        /// </summary>
        [FieldLength]
        [ClrType(typeof(string))]
        MibObjectName = 451,

        /// <summary>
        /// The value of the DESCRIPTION clause of a MIB object type definition.
        /// </summary>
        [FieldLength]
        [ClrType(typeof(string))]
        MibObjectDescription = 452,

        /// <summary>
        /// The value of the SYNTAX clause of a MIB object type definition,
        /// which may include a textual convention or sub-typing.
        /// </summary>
        /// <remarks>
        /// See [RFC2578].
        /// </remarks>
        [FieldLength]
        [ClrType(typeof(string))]
        MibObjectSyntax = 453,

        /// <summary>
        /// The textual name of the MIB module that defines a MIB object.
        /// </summary>
        [FieldLength]
        [ClrType(typeof(string))]
        MibModuleName = 454,

        /// <summary>
        /// The International Mobile Subscription Identity (IMSI).
        /// </summary>
        /// <remarks>
        /// The IMSI is a decimal digit string with up to a maximum of 15
        /// ASCII/UTF-8 encoded digits (0x30 - 0x39).
        /// </remarks>
        [FieldLength(MaximumLength = 15)]
        [ClrType(typeof(string))]
        MobileImsi = 455,

        /// <summary>
        /// The Mobile Station International Subscriber Directory Number
        /// (MSISDN).
        /// </summary>
        /// <remarks>
        /// The MSISDN is a decimal digit string with up to a maximum of 15
        /// ASCII/UTF-8 encoded digits (0x30 - 0x39).
        /// </remarks>
        [FieldLength(MaximumLength = 15)]
        [ClrType(typeof(string))]
        MobileMsisdn = 456,

        /// <summary>
        /// The HTTP Response Status Code, as defined in section 6 of [RFC7231],
        /// associated with a flow.
        /// </summary>
        /// <remarks>
        /// Implies that the flow record represents a flow containing an HTTP
        /// Response.
        /// </remarks>
        [FieldLength(2)]
        [ClrType(typeof(ushort))]
        HttpStatusCode = 457,

        /// <summary>
        /// This Information Element contains the maximum number of IP source
        /// transport ports that can be used by an end user when sending IP
        /// packets; each user is associated with one or more(source) IPv4 or
        /// IPv6 addresses.
        /// </summary>
        /// <remarks>
        /// This Information Element is particularly useful in address-sharing
        /// deployments that adhere to REQ-4 of [RFC6888]. Limiting the number
        /// of ports assigned to each user ensures fairness among users and
        /// mitigates the denial-of-service attack that a user could launch
        /// against other users through the address-sharing device in order to
        /// grab more ports.
        /// </remarks>
        [FieldLength(2)]
        [ClrType(typeof(ushort))]
        SourceTransportPortsLimit = 458,

        /// <summary>
        /// The HTTP request method, as defined in section 4 of [RFC7231], associated
        /// with a flow. String with up to 8 UTF-8 characters.
        /// </summary>
        [FieldLength]
        [ClrType(typeof(string))]
        HttpRequestMethod = 459,    // TODO: UTF-8

        /// <summary>
        /// The HTTP request host, as defined in section 5.4 of [RFC7230] or, in the
        /// case of HTTP/2, the content of the :authority pseudo-header field as
        /// defined in section 8.1.2.3 of [RFC7240].
        /// </summary>
        /// <remarks>
        /// Encoded in UTF-8.
        /// </remarks>
        [FieldLength]
        [ClrType(typeof(string))]
        HttpRequestHost = 460,  // TODO: UTF-8

        /// <summary>
        /// The HTTP request target, as defined in section 2 of [RFC7231] and in
        /// section 5.3 of [RFC7230], associated with a flow.
        /// </summary>
        /// <remarks>
        /// Or the HTTP/2 :path pseudo-header field as defined in section 8.1.2.3 of
        /// [RFC7240]. Encoded in UTF-8.
        /// </remarks>
        [FieldLength]
        [ClrType(typeof(string))]
        HttpRequestTarget = 461,    // TODO: UTF-8

        /// <summary>
        /// The version of an HTTP/1.1 message as indicated by the HTTP-version
        /// field, defined in section 2.6 of [RFC7230], or the version identification
        /// of an HTTP/2 frame as defined in [RFC7240] section 3.1.
        /// </summary>
        /// <remarks>
        /// The length of this field is limited to 10 characters, UTF-8 encoded.
        /// </remarks>
        [FieldLength(MaximumLength = 10)]
        [ClrType(typeof(string))]
        HttpMessageVersion = 462, // TODO: UTF-8

        /// <summary>
        /// This Information Element uniquely identifies an Instance of the NAT
        /// that runs on a NAT middlebox function after the packet passes the
        /// Observation Point.
        /// </summary>
        /// <remarks>
        /// <see cref="NatInstanceID"/> is defined in [RFC7659].
        /// </remarks>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        NatInstanceID = 463,

        /// <summary>
        /// This Information Element represents the internal address realm where
        /// the packet is originated from or destined to.
        /// </summary>
        /// <remarks>
        /// By definition, a NAT mapping can be created from two address realms,
        /// one from internal and one from external. Realms are
        /// implementation-dependent and can represent a Virtual Routing and
        /// Forwarding (VRF) ID, a VLAN ID, or some unique identifier. Realms
        /// are optional and, when left unspecified, would mean that the
        /// external and internal realms are the same.
        /// </remarks>
        [FieldLength]
        InternalAddressRealm = 464,

        /// <summary>
        /// This Information Element represents the external address realm where
        /// the packet is originated from or destined to.
        /// </summary>
        /// <remarks>
        /// The detailed definition is in the <see cref="InternalAddressRealm"/>
        /// as specified above.
        /// </remarks>
        [FieldLength]
        ExternalAddressRealm = 465,

        /// <summary>
        /// This Information Element identifies the type of a NAT Quota Exceeded
        /// event.
        /// </summary>
        /// <remarks>
        /// Values for this Information Element are listed in the NAT Quota
        /// Exceeded Event Type registry, see
        /// [http://www.iana.org/assignments/ipfix/ipfix.xhtml#ipfix-nat-quota-exceeded-event].
        /// New assignments of values will be administered by IANA and are
        /// subject to Expert Review [RFC8126]. Experts need to check
        /// definitions of new values for completeness, accuracy and
        /// redundancy.
        /// </remarks>
        [FieldLength(4)]
        [ClrType(typeof(NatQuotaExceededEventType))]
        NatQuotaExceededEvent = 466,

        /// <summary>
        /// This Information Element identifies a type of a NAT Threshold event.
        /// </summary>
        /// <remarks>
        /// Values for this Information Element are listed in the NAT Threshold
        /// Event Type registry, see
        /// [http://www.iana.org/assignments/ipfix/ipfix.xhtml#ipfix-nat-threshold-event].
        /// New assignments of values will be administered by IANA and are
        /// subject to Expert Review [RFC8126]. Experts need to check
        /// definitions of new values for completeness, accuracy and
        /// redundancy.
        /// </remarks>
        [FieldLength(4)]
        [ClrType(typeof(NatThresholdEventType))]
        NatThresholdEvent = 467,

        /// <summary>
        /// The HTTP User-Agent header field as defined in section 5.5.3 of
        /// [RFC7231].
        /// </summary>
        /// <remarks>
        /// Encoded in UTF-8.
        /// </remarks>
        [FieldLength]
        [ClrType(typeof(string))]
        HttpUserAgent = 468,    // TODO: utf-8

        /// <summary>
        /// The HTTP Content-Type header field as defined in section 3.1.1.5
        /// of [RFC7231].
        /// </summary>
        /// <remarks>
        /// Encoded in UTF-8.
        /// </remarks>
        [FieldLength]
        [ClrType(typeof(string))]
        HttpContentType = 469,  // TOOD: UTF-8

        /// <summary>
        /// The HTTP reason phrase as defined in section 6.1 of of [RFC7231].
        /// </summary>
        [FieldLength]
        [ClrType(typeof(string))]
        HttpReasonPhrase = 470,

        /// <summary>
        /// This element represents the maximum session entries that can be
        /// created by the NAT device.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        MaximumSessionEntries = 471,

        /// <summary>
        /// This element represents the maximum BIB entries that can be created
        /// by the NAT device.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        MaximumBibEntries = 472,

        /// <summary>
        /// This element represents the maximum NAT entries that can be created
        /// per user by the NAT device.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        MaximumEntriesPerUser = 473,

        /// <summary>
        /// This element represents the maximum subscribers or maximum hosts
        /// that are allowed by the NAT device.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        MaximumSubscribers = 474,

        /// <summary>
        /// This element represents the maximum fragments that the NAT device
        /// can store for reassembling the packet.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        MaximumFragmentsPendingReassembly = 475,

        /// <summary>
        /// This element represents the high threshold value of the number of
        /// public IP addresses in the address pool.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        AddressPoolHighThreshold = 476,

        /// <summary>
        /// This element represents the low threshold value of the number of
        /// public IP addresses in the address pool.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        AddressPoolLowThreshold = 477,

        /// <summary>
        /// This element represents the high threshold value of the number of
        /// address and port mappings.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        AddressPortMappingHighThreshold = 478,

        /// <summary>
        /// This element represents the low threshold value of the number of
        /// address and port mappings.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        AddressPortMappingLowThreshold = 479,

        /// <summary>
        /// This element represents the high threshold value of the number of
        /// address and port mappings that a single user is allowed to create
        /// on a NAT device.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        AddressPortMappingPerUserHighThreshold = 480,

        /// <summary>
        /// This element represents the high threshold value of the number of
        /// address and port mappings that a single user is allowed to create on
        /// a NAT device in a paired address pooling behaviour.
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        GlobalAddressMappingHighThreshold = 481,

        /// <summary>
        /// VPN ID in the format specified by [RFC2685].
        /// </summary>
        /// <remarks>
        /// The size of this Information Element is 7 octets.
        /// </remarks>
        [FieldLength(7)]
        VpnIdentifier = 482,

        /// <summary>
        /// BGP community as defined in [RFC1997].
        /// </summary>
        [FieldLength(4)]
        [ClrType(typeof(uint))]
        BgpCommunity = 483,

        /// <summary>
        /// Basic list of zero or more <see cref="BgpCommunity"/> IEs,
        /// containing the BGP communities corresponding with source IP
        /// address of a specific flow.
        /// </summary>
        [FieldLength]
        //[ClrType(typeof(basicList))]
        BgpSourceCommunityList = 484,

        /// <summary>
        /// Basic list of zero or more <see cref="BgpCommunity"/> IEs,
        /// containing the BGP communities corresponding with destination
        /// IP address of a specific flow.
        /// </summary>
        [FieldLength]
        //[ClrType(typeof(basicList))]
        BgpDestinationCommunityList = 485,

        /// <summary>
        /// BGP Extended Community as defined in [RFC4360].
        /// </summary>
        /// <remarks>
        /// The size of this IE MUST be 8 octets.
        /// </remarks>
        [FieldLength(8)]
        BgpExtendedCommunity = 486,

        /// <summary>
        /// Basic list of zero or more <see cref="BgpExtendedCommunity"/> IEs,
        /// containing the BGP Extended Communities corresponding with source
        /// IP address of a specific flow.
        /// </summary>
        [FieldLength]
        //[ClrType(typeof(basicList))]
        BgpSourceExtendedCommunityList = 487,

        /// <summary>
        /// Basic list of zero or more <see cref="BgpExtendedCommunity"/> IEs,
        /// containing the BGP Extended Communities corresponding with
        /// destination IP address of a specific flow.
        /// </summary>
        [FieldLength]
        //[ClrType(typeof(basicList))]
        BgpDestinationExtendedCommunityList = 488,

        /// <summary>
        /// BGP Large Community as defined in [RFC8092].
        /// </summary>
        /// <remarks>
        /// The size of this IE MUST be 12 octets.
        /// </remarks>
        [FieldLength(12)]
        BgpLargeCommunity = 489,

        /// <summary>
        /// basicList of zero or more bgpLargeCommunity IEs, containing the BGP
        /// Large Communities corresponding with source IP address of a
        /// specific flow.
        /// </summary>
        [FieldLength]
        //[ClrType(typeof(basicList))]
        BgpSourceLargeCommunityList = 490,

        /// <summary>
        /// basicList of zero or more bgpLargeCommunity IEs, containing the BGP
        /// Large Communities corresponding with destination IP address of a
        /// specific flow
        /// </summary>
        [FieldLength]
        //[ClrType(typeof(basicList))]
        BgpDestinationLargeCommunityList = 491,

    }
}
