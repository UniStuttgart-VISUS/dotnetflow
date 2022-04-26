// <copyright file="IOTest5.cs" company="Universität Stuttgart">
// Copyright © 2020 - 2022 Institut für Visualisierung und Interaktive Systeme. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotNetFlow.Netflow5;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;


namespace DotNetFlow.Test {

    /// <summary>
    /// Performs I/O tests using <see cref="NetflowReader"/> and
    /// <see cref="NetflowWriter"/>.
    /// </summary>
    [TestClass]
    public sealed class IOTest5 {

        [TestMethod]
        public void TestNetflowReader() {
            var expected = CreateTestData();

            using (var ms = new MemoryStream()) {

                // Write the test data.
                using (var nw = new NetflowWriter(ms, true)) {
                    nw.Write(expected.Item1);
                    nw.Write(expected.Item2);
                }

                // Reset the stream.
                ms.Seek(0, SeekOrigin.Begin);

                // Test the reader.
                using (var nr = new NetflowReader(ms)) {
                    Assert.AreSame(ms, nr.BaseStream);

                    var packetHeader = nr.ReadPacketHeader();
                    Assert.IsNotNull(packetHeader);
                    Assert.AreEqual(expected.Item1, packetHeader);
                    Assert.ThrowsException<InvalidOperationException>(() => nr.ReadPacketHeader());

                    var record = nr.ReadFlowRecord();
                    Assert.IsNotNull(record);

                    Assert.AreEqual(expected.Item2.DestinationAddress, record.DestinationAddress);
                    Assert.AreEqual(expected.Item2.DestinationAutonomousSystem, record.DestinationAutonomousSystem);
                    Assert.AreEqual(expected.Item2.DestinationMask, record.DestinationMask);
                    Assert.AreEqual(expected.Item2.DestinationPort, record.DestinationPort);
                    Assert.AreEqual(expected.Item2.End, record.End);
                    Assert.AreEqual(expected.Item2.InputInterface, record.InputInterface);
                    Assert.AreEqual(expected.Item2.NextHop, record.NextHop);
                    Assert.AreEqual(expected.Item2.Octets, record.Octets);
                    Assert.AreEqual(expected.Item2.OutputInterface, record.OutputInterface);
                    Assert.AreEqual(expected.Item2.Protocol, record.Protocol);
                    Assert.AreEqual(expected.Item2.SourceAddress, record.SourceAddress);
                    Assert.AreEqual(expected.Item2.SourceAutonomousSystem, record.SourceAutonomousSystem);
                    Assert.AreEqual(expected.Item2.SourceMask, record.SourceMask);
                    Assert.AreEqual(expected.Item2.SourcePort, record.SourcePort);
                    Assert.AreEqual(expected.Item2.Start, record.Start);
                    Assert.AreEqual(expected.Item2.TcpFlags, record.TcpFlags);
                    Assert.AreEqual(expected.Item2.TypeOfService, record.TypeOfService);
                }
            }
        }

        [TestMethod]
        public void TestNetflowWriter() {
            var testData = CreateTestData();

            using (var ms = new MemoryStream()) {

                // Write the flows.
                using (var nw = new NetflowWriter(ms, true)) {
                    Assert.AreSame(ms, nw.BaseStream);

                    Assert.ThrowsException<InvalidOperationException>(() => nw.Write(testData.Item2));
                    Assert.ThrowsException<ArgumentNullException>(() => nw.Write((PacketHeader) null));

                    nw.Write(testData.Item1);
                    Assert.ThrowsException<InvalidOperationException>(() => nw.Write(testData.Item1));
                    Assert.ThrowsException<ArgumentNullException>(() => nw.Write((FlowRecord) null));

                    nw.Write(testData.Item2);
                    Assert.ThrowsException<InvalidOperationException>(() => nw.Write(testData.Item2));
                }

                // Reset the stream.
                ms.Seek(0, SeekOrigin.Begin);

                // Read the stream and check its contents.
                using (var br = new BinaryReader(ms)) {
                    // Packet header
                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(5u, actual, "Netflow version");
                    }
                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(1u, actual, "# of records");
                    }

                    br.ReadUInt32();    // Skip uptime.
                    br.ReadUInt32();    // Skip UNIX seconds.
                    br.ReadUInt32();    // Skip UNIX nanos.

                    {
                        var actual = br.ReadUInt32().ToHostByteOrder();
                        Assert.AreEqual(0u, actual, "package sequence");
                    }

                    {
                        var actual = br.ReadByte().ToHostByteOrder();
                        Assert.AreEqual(42u, actual, "engine type");
                    }

                    {
                        var actual = br.ReadByte().ToHostByteOrder();
                        Assert.AreEqual(43u, actual, "engine ID");
                    }

                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(12345u, actual, "sampling interval");
                    }

                    // Flow record.
                    {
                        var actual = new IPAddress(br.ReadUInt32());
                        Assert.AreEqual(IPAddress.Parse("10.5.12.13"), actual, "srcaddr ");
                    }

                    {
                        var actual = new IPAddress(br.ReadUInt32());
                        Assert.AreEqual(IPAddress.Parse("192.168.1.12"), actual, "dstaddr");
                    }

                    {
                        var actual = new IPAddress(br.ReadUInt32());
                        Assert.AreEqual(IPAddress.Parse("10.5.12.254"), actual, "nexthop");
                    }

                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(741u, actual, "input");
                    }

                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(21478u, actual, "output");
                    }

                    {
                        var actual = br.ReadUInt32().ToHostByteOrder();
                        Assert.AreEqual(5009u, actual, "dPkts");
                    }

                    {
                        var actual = br.ReadUInt32().ToHostByteOrder();
                        Assert.AreEqual(5344385u, actual, "dOctets");
                    }

                    {
                        var actual = br.ReadUInt32().ToHostByteOrder();
                        Assert.AreEqual(369u, actual, "First");
                    }

                    {
                        var actual = br.ReadUInt32().ToHostByteOrder();
                        Assert.AreEqual(963u, actual, "Last");
                    }

                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(80u, actual, "srcport");
                    }

                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(81u, actual, "dstport");
                    }

                    {
                        var actual = br.ReadByte().ToHostByteOrder();
                        Assert.AreEqual((byte) 0, actual, "pad1");
                    }

                    {
                        var actual = br.ReadByte().ToHostByteOrder();
                        Assert.AreEqual((byte) 12, actual, "tcp_flags");
                    }

                    {
                        var actual = br.ReadByte().ToHostByteOrder();
                        Assert.AreEqual((byte) ProtocolType.Tcp, actual, "prot");
                    }

                    {
                        var actual = br.ReadByte().ToHostByteOrder();
                        Assert.AreEqual((byte) 33, actual, "tos");
                    }

                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual((ushort) 12, actual, "src_as");
                    }

                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual((ushort) 13, actual, "dst_as");
                    }

                    {
                        var actual = br.ReadByte().ToHostByteOrder();
                        Assert.AreEqual((byte) 8, actual, "src_mask");
                    }

                    {
                        var actual = br.ReadByte().ToHostByteOrder();
                        Assert.AreEqual((byte) 16, actual, "dst_mask");
                    }

                    br.ReadBytes(2);
                }
            }
        }

        [TestMethod]
        public void TestStreamCopy() {
            var testData = CreateTestData();

            using (var src = new MemoryStream()) {

                // Write test data.
                using (var nw = new NetflowWriter(src, true)) {
                    nw.Write(testData.Item1);
                    nw.Write(testData.Item2);
                }

                // Seek source to begin.
                src.Seek(0, SeekOrigin.Begin);

                // Create the copy.
                using (var dst = new MemoryStream()) {
                    src.CopyNetflow5Packet(dst);

                    // Compare the two streams.
                    Assert.IsTrue(src.ToArray().SequenceEqual(dst.ToArray()));
                }
            }
        }

        private static (PacketHeader, FlowRecord) CreateTestData() {
            var packetHeader = new PacketHeader(1, 0) {
                EngineType = 42,
                EngineID = 43,
                SamplingInterval = 12345
            };

            var record = new FlowRecord();
            record.SourceAddress = IPAddress.Parse("10.5.12.13");
            record.DestinationAddress = IPAddress.Parse("192.168.1.12");
            record.NextHop = IPAddress.Parse("10.5.12.254");
            record.InputInterface = 741;
            record.OutputInterface = 21478;
            record.Packets = 5009;
            record.Octets = 5344385;
            record.Start = 369;
            record.End = 963;
            record.SourcePort = 80;
            record.DestinationPort = 81;
            record.TcpFlags = 12;
            record.Protocol = (byte) ProtocolType.Tcp;
            record.TypeOfService = 33;
            record.SourceAutonomousSystem = 12;
            record.DestinationAutonomousSystem = 13;
            record.SourceMask = 8;
            record.DestinationMask = 16;

            return (packetHeader, record);
        }
    }
}
