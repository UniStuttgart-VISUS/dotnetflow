// <copyright file="IOTest9.cs" company="Universität Stuttgart">
// Copyright © 2020 - 2022 Institut für Visualisierung und Interaktive Systeme. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotNetFlow.Netflow9;
using System;
using System.IO;
using System.Linq;
using System.Net;
using static DotNetFlow.OnWireExtensions;


namespace DotNetFlow.Test {

    /// <summary>
    /// Tests for <see cref="NetFlowReader"/> and <see cref="NetFlowWriter"/>.
    /// </summary>
    [TestClass]
    public sealed class IOTest9 {

        [TestMethod]
        public void TestNetflowReader() {
            var expected = CreateTestData();

            using (var ms = new MemoryStream()) {

                // Write the test data.
                using (var nw = new NetflowWriter(ms, true)) {
                    nw.Write(expected.Item1);   // Packet header
                    nw.Write(expected.Item2);   // Data template
                    nw.Write(expected.Item4);   // Options template
                    nw.Write(expected.Item3);   // Data
                    nw.Write(expected.Item5);   // Options data
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

                    var templateFlowSet = nr.ReadFlowSet() as TemplateFlowSet;
                    Assert.IsNotNull(templateFlowSet);
                    Assert.AreEqual(expected.Item2.ID, templateFlowSet.ID);
                    Assert.AreEqual(expected.Item2.Records.Count, templateFlowSet.Records.Count);
                    Assert.AreEqual(1, templateFlowSet.Records.Count);
                    Assert.AreEqual(expected.Item2.Records[0].ID, templateFlowSet.Records[0].ID);
                    Assert.AreEqual(expected.Item2.Records[0].FieldCount, templateFlowSet.Records[0].FieldCount);

                    for (ushort i = 0; i < expected.Item2.Records[0].FieldCount; ++i) {
                        Assert.AreEqual(expected.Item2.Records[0].Fields[i], templateFlowSet.Records[0].Fields[i], $"Template field {i}");
                    }

                    var optionsTemplateFlowSet = nr.ReadFlowSet() as OptionsTemplateFlowSet;
                    Assert.IsNotNull(optionsTemplateFlowSet);
                    Assert.AreEqual(expected.Item4.ID, optionsTemplateFlowSet.ID);
                    Assert.AreEqual(1, optionsTemplateFlowSet.Records.Count);

                    Assert.AreEqual(expected.Item4.Records[0].Scopes[0], optionsTemplateFlowSet.Records[0].Scopes[0]);
                    Assert.AreEqual(expected.Item4.Records[0].Options[0], optionsTemplateFlowSet.Records[0].Options[0]);
                    Assert.AreEqual(expected.Item4.Records[0].Options[1], optionsTemplateFlowSet.Records[0].Options[1]);

                    var dataFlowSet = nr.ReadFlowSet() as DataFlowSet;
                    Assert.IsNotNull(dataFlowSet);
                    Assert.AreEqual(expected.Item3.ID, dataFlowSet.ID);

                    Assert.AreEqual(expected.Item3.Records[0], dataFlowSet.Records[0]);
                    Assert.AreEqual(expected.Item3.Records[1], dataFlowSet.Records[1]);
                    Assert.AreEqual(expected.Item3.Records[2], dataFlowSet.Records[2]);
                    Assert.AreEqual(expected.Item3.Records[3], dataFlowSet.Records[3]);
                    Assert.AreEqual(expected.Item3.Records[4], dataFlowSet.Records[4]);

                    Assert.AreEqual(expected.Item3.Records[5], dataFlowSet.Records[5]);
                    Assert.AreEqual(expected.Item3.Records[6], dataFlowSet.Records[6]);
                    Assert.AreEqual(expected.Item3.Records[7], dataFlowSet.Records[7]);
                    Assert.AreEqual(expected.Item3.Records[8], dataFlowSet.Records[8]);
                    Assert.AreEqual(expected.Item3.Records[9], dataFlowSet.Records[9]);

                    var optionsDataFlowSet = nr.ReadFlowSet() as OptionsDataFlowSet;
                    Assert.IsNotNull(optionsDataFlowSet);
                    Assert.AreEqual(expected.Item5.ID, optionsDataFlowSet.ID);

                    Assert.AreEqual(expected.Item5.Scopes.Count, optionsDataFlowSet.Scopes.Count);
                    {
                        var actual = ushort.MaxValue;
                        FromWire(ref actual, (byte[]) optionsDataFlowSet.Scopes[0]);
                        Assert.AreEqual(expected.Item5.Scopes[0], actual);
                    }

                    Assert.AreEqual(expected.Item5.Options.Count, optionsDataFlowSet.Options.Count);
                    {
                        var actual = ushort.MaxValue;
                        FromWire(ref actual, (byte[]) optionsDataFlowSet.Options[0]);
                        Assert.AreEqual(expected.Item5.Options[0], actual);
                    }

                    Assert.AreEqual(expected.Item5.Options[1], optionsDataFlowSet.Options[1]);
                }
            }
        }

        [TestMethod]
        public void TestNetflowWriter() {
            var testData = CreateTestData();
            Assert.AreEqual(4, testData.Item1.Count);
            Assert.AreEqual(1, testData.Item2.Records.Count);
            Assert.AreEqual(5, testData.Item2.Records[0].FieldCount);
            Assert.AreEqual(28, testData.Item2.Length);
            Assert.AreEqual(64, testData.Item3.Length);

            using (var ms = new MemoryStream()) {

                // Write the flows.
                using (var nw = new NetflowWriter(ms, true)) {
                    Assert.AreSame(ms, nw.BaseStream);

                    Assert.ThrowsException<InvalidOperationException>(() => nw.Write(testData.Item2));
                    Assert.ThrowsException<InvalidOperationException>(() => nw.Write(testData.Item3));
                    Assert.ThrowsException<InvalidOperationException>(() => nw.Write(testData.Item4));
                    Assert.ThrowsException<ArgumentNullException>(() => nw.Write((PacketHeader) null));

                    nw.Write(testData.Item1);
                    Assert.ThrowsException<InvalidOperationException>(() => nw.Write(testData.Item1));

                    Assert.ThrowsException<ArgumentNullException>(() => nw.Write((TemplateFlowSet) null));
                    Assert.ThrowsException<ArgumentNullException>(() => nw.Write((DataFlowSet) null));
                    Assert.ThrowsException<ArgumentNullException>(() => nw.Write((OptionsTemplateFlowSet) null));

                    nw.Write(testData.Item2);
                    Assert.ThrowsException<InvalidOperationException>(() => nw.Write(testData.Item1));

                    nw.Write(testData.Item4);
                    Assert.ThrowsException<InvalidOperationException>(() => nw.Write(testData.Item1));

                    nw.Write(testData.Item3);
                    nw.Write(testData.Item5);

                    // Packet is full at this point.
                    Assert.ThrowsException<InvalidOperationException>(() => nw.Write(testData.Item2));
                    Assert.ThrowsException<InvalidOperationException>(() => nw.Write(testData.Item3));
                    Assert.ThrowsException<InvalidOperationException>(() => nw.Write(testData.Item5));
                }

                // Reset the stream.
                ms.Seek(0, SeekOrigin.Begin);

                // Read the stream and check its contents.
                using (var br = new BinaryReader(ms)) {
                    #region Packet header
                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(9u, actual, "Netflow version");
                    }
                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(4u, actual, "# of flow sets");
                    }

                    br.ReadUInt32();    // Skip uptime.
                    br.ReadUInt32();    // Skip UNIX seconds.

                    {
                        var actual = br.ReadUInt32().ToHostByteOrder();
                        Assert.AreEqual(0u, actual, "package sequence");
                    }

                    {
                        var actual = br.ReadUInt32().ToHostByteOrder();
                        Assert.AreEqual(0u, actual, "source ID");
                    }
                    #endregion

                    #region Template flow set
                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(0u, actual, "template flow set ID");
                    }

                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(28u, actual, "template flow set length");
                    }

                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(256u, actual, "template ID");
                    }

                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(5u, actual, "field count");
                    }

                    {
                        var actual = (FieldType) br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(FieldType.IPv4SourceAddress, actual);
                    }

                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(4, actual);
                    }

                    {
                        var actual = (FieldType) br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(FieldType.IPv4DestinationAddress, actual);
                    }

                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(4, actual);
                    }

                    {
                        var actual = (FieldType) br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(FieldType.IPv4NextHop, actual);
                    }

                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(4, actual);
                    }

                    {
                        var actual = (FieldType) br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(FieldType.IncomingPackets, actual);
                    }

                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(4, actual);
                    }

                    {
                        var actual = (FieldType) br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(FieldType.IncomingBytes, actual);
                    }

                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(4, actual);
                    }
                    #endregion

                    #region Options template
                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(1u, actual, "options template flow set ID");
                    }

                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(24u, actual, "options template flow set length");
                        // Cisco's sample is wrong: according to specification,
                        // the length needs to include the two bytes of padding.
                    }

                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(257u, actual, "options template ID");
                    }

                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(4u, actual, "scope length");
                    }

                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(8u, actual, "options length");
                    }

                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(0x2u, actual, "interface");
                    }

                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(2, actual);
                    }

                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual((ushort) FieldType.SamplingInterval, actual, "sampling interval");
                    }

                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(2u, actual);
                    }

                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual((ushort) FieldType.SamplingAlgorithm, actual, "sampling algorithm");
                    }

                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(1u, actual);
                    }

                    {
                        var padding = br.ReadUInt16();
                    }
                    #endregion

                    #region Data flow set
                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(256, actual, "Data flow set ID");
                    }

                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(64, actual, "Data flow set length");
                    }

                    #region Data record
                    {
                        var actual = new IPAddress(br.ReadUInt32());
                        Assert.AreEqual(IPAddress.Parse("192.168.1.12"), actual);
                    }

                    {
                        var actual = new IPAddress(br.ReadUInt32());
                        Assert.AreEqual(IPAddress.Parse("10.5.12.254"), actual);
                    }

                    {
                        var actual = new IPAddress(br.ReadUInt32());
                        Assert.AreEqual(IPAddress.Parse("192.168.1.1"), actual);
                    }

                    {
                        var actual = br.ReadUInt32().ToHostByteOrder();
                        Assert.AreEqual(5009u, actual);
                    }

                    {
                        var actual = br.ReadUInt32().ToHostByteOrder();
                        Assert.AreEqual(5344385u, actual);
                    }
                    #endregion

                    #region Data record
                    {
                        var actual = new IPAddress(br.ReadUInt32());
                        Assert.AreEqual(IPAddress.Parse("192.168.1.27"), actual);
                    }

                    {
                        var actual = new IPAddress(br.ReadUInt32());
                        Assert.AreEqual(IPAddress.Parse("10.5.12.23"), actual);
                    }

                    {
                        var actual = new IPAddress(br.ReadUInt32());
                        Assert.AreEqual(IPAddress.Parse("192.168.1.1"), actual);
                    }

                    {
                        var actual = br.ReadUInt32().ToHostByteOrder();
                        Assert.AreEqual(748u, actual);
                    }

                    {
                        var actual = br.ReadUInt32().ToHostByteOrder();
                        Assert.AreEqual(388934u, actual);
                    }
                    #endregion

                    #region Data record
                    {
                        var actual = new IPAddress(br.ReadUInt32());
                        Assert.AreEqual(IPAddress.Parse("192.168.1.56"), actual);
                    }

                    {
                        var actual = new IPAddress(br.ReadUInt32());
                        Assert.AreEqual(IPAddress.Parse("10.5.12.65"), actual);
                    }

                    {
                        var actual = new IPAddress(br.ReadUInt32());
                        Assert.AreEqual(IPAddress.Parse("192.168.1.1"), actual);
                    }

                    {
                        var actual = br.ReadUInt32().ToHostByteOrder();
                        Assert.AreEqual(5u, actual);
                    }

                    {
                        var actual = br.ReadUInt32().ToHostByteOrder();
                        Assert.AreEqual(6534u, actual);
                    }
                    #endregion
                    #endregion

                    #region Options data
                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(257, actual, "Options data flow set ID");
                    }

                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(12, actual, "Options length");
                        // Note: Ciscos sample seems to be wrong here, because
                        // it contradicts the statement that data should be
                        // padded to 4-byte boundaries.
                    }

                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual((ushort) 2, actual, "scope");
                    }

                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual((ushort) 100, actual, "sampling interval");
                    }

                    {
                        var actual = br.ReadByte().ToHostByteOrder();
                        Assert.AreEqual((byte) 1, actual, "sampling algorithm");
                    }

                    {
                        var padding = br.ReadBytes(3);
                    }
                    #endregion
                }
            }
        }

        [TestMethod]
        public void TestPadding() {
            var header = new PacketHeader(3, 0, 0);
            Assert.IsTrue(header.GetOnWireSize() % 4 == 0, "Header is 4-byte aligned.");

            var record = new TemplateRecord(256);
            Assert.AreEqual(2 * sizeof(ushort), record.GetOnWireSize());
            record.Fields.Add(new Field(FieldType.Protocol));
            Assert.AreEqual(2 * sizeof(ushort) + 2 * sizeof(ushort), record.GetOnWireSize());

            var flowset = new TemplateFlowSet();
            Assert.AreEqual(2 * sizeof(ushort), flowset.GetOnWireSize());
            Assert.AreEqual(flowset.GetOnWireSize(), flowset.Length);

            flowset.Records.Add(record);
            Assert.AreEqual(6 * sizeof(ushort), flowset.Length);
            Assert.IsTrue(flowset.Length % 4 == 0, "Template is 4-byte aligned.");

            var data = new DataFlowSet();
            data.Records.Add((byte) 42);
            Assert.IsTrue(flowset.Length % 4 == 0, "Data are 4-byte aligned.");

            byte[] bytes;

            using (var ms = new MemoryStream())
            using (var nw = new NetflowWriter(ms, false)) {
                nw.Write(header);
                nw.Write(flowset);
                nw.Write(data);
                nw.Write(data);
                nw.Flush();

                bytes = ms.ToArray();
                Assert.IsTrue(bytes.Length % 4 == 0, "Output is 4-byte aligned.");
            }

            using (var ms = new MemoryStream(bytes))
            using (var nr = new NetflowReader(ms)) {
                var h = nr.ReadPacketHeader();
                Assert.AreEqual(3, h.Count);
                var t = nr.ReadFlowSet() as TemplateFlowSet;
                Assert.AreEqual(1, t.Records.Count);
                Assert.AreEqual(256, t.Records[0].ID);

                {
                    var d = nr.ReadFlowSet() as DataFlowSet;
                    Assert.IsNotNull(d);
                    Assert.AreEqual(data.Records[0], d.Records[0]);
                }

                {
                    var d = nr.ReadFlowSet() as DataFlowSet;
                    Assert.IsNotNull(d);
                    Assert.AreEqual(data.Records[0], d.Records[0]);
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
                    nw.Write(testData.Item3);
                    nw.Write(testData.Item4);
                    nw.Write(testData.Item5);
                }

                // Seek source to begin.
                src.Seek(0, SeekOrigin.Begin);

                // Create the copy.
                using (var dst = new MemoryStream()) {
                    src.CopyNetflow9Packet(dst);

                    // Compare the two streams.
                    Assert.IsTrue(src.ToArray().SequenceEqual(dst.ToArray()));
                }
            }
        }

        private static (PacketHeader, TemplateFlowSet, DataFlowSet,
                OptionsTemplateFlowSet, OptionsDataFlowSet) CreateTestData() {
            // The test packet is produced like the example on
            // https://www.cisco.com/en/US/technologies/tk648/tk362/technologies_white_paper09186a00800a3db9.html
            var packetHeader = new PacketHeader(4, 0, 0);

            var templateRecord = new TemplateRecord(256);
            templateRecord.Fields.Add(new Field(FieldType.IPv4SourceAddress));
            templateRecord.Fields.Add(new Field(FieldType.IPv4DestinationAddress));
            templateRecord.Fields.Add(new Field(FieldType.IPv4NextHop));
            templateRecord.Fields.Add(new Field(FieldType.IncomingPackets));
            templateRecord.Fields.Add(new Field(FieldType.IncomingBytes));

            var templateFlowSet = new TemplateFlowSet();
            templateFlowSet.Records.Add(templateRecord);

            var optionsRecord = new OptionsTemplateRecord(257);
            optionsRecord.Scopes.Add(new Scope(OptionScope.Interface, 2));
            optionsRecord.Options.Add(new Field(FieldType.SamplingInterval, 2));
            optionsRecord.Options.Add(new Field(FieldType.SamplingAlgorithm, 1));

            var optionsFlowSet = new OptionsTemplateFlowSet();
            optionsFlowSet.Records.Add(optionsRecord);

            var dataFlowSet = new DataFlowSet(256);
            dataFlowSet.Records.Add(IPAddress.Parse("192.168.1.12"));
            dataFlowSet.Records.Add(IPAddress.Parse("10.5.12.254"));
            dataFlowSet.Records.Add(IPAddress.Parse("192.168.1.1"));
            dataFlowSet.Records.Add(5009);
            dataFlowSet.Records.Add(5344385);

            dataFlowSet.Records.Add(IPAddress.Parse("192.168.1.27"));
            dataFlowSet.Records.Add(IPAddress.Parse("10.5.12.23"));
            dataFlowSet.Records.Add(IPAddress.Parse("192.168.1.1"));
            dataFlowSet.Records.Add(748);
            dataFlowSet.Records.Add(388934);

            dataFlowSet.Records.Add(IPAddress.Parse("192.168.1.56"));
            dataFlowSet.Records.Add(IPAddress.Parse("10.5.12.65"));
            dataFlowSet.Records.Add(IPAddress.Parse("192.168.1.1"));
            dataFlowSet.Records.Add(5);
            dataFlowSet.Records.Add(6534);

            var optionsData = new OptionsDataFlowSet(257);
            optionsData.Scopes.Add((ushort) 2);
            optionsData.Options.Add((ushort) 100);
            optionsData.Options.Add((byte) 1);

            return (packetHeader, templateFlowSet, dataFlowSet, optionsFlowSet, optionsData);
        }
    }
}
