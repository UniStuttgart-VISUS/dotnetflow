// <copyright file="IOTest10.cs" company="Universität Stuttgart">
// Copyright © 2020 - 2022 Institut für Visualisierung und Interaktive Systeme. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotNetFlow.Ipfix;
using System;
using System.IO;
using System.Linq;
using System.Net;


namespace DotNetFlow.Test {

    /// <summary>
    /// Tests the IPFIX reader and writer.
    /// </summary>
    [TestClass]
    public sealed class IOTest10 {

        [TestMethod]
        public void TestNetflowReader() {
            var expected = CreateTestData();

            using (var ms = new MemoryStream()) {

                // Write the test data.
                using (var nw = new IpfixWriter(ms, true)) {
                    nw.Write(expected.Item1);   // Packet header
                    nw.Write(expected.Item2);   // Data template
                    nw.Write(expected.Item4);   // Options template
                    nw.Write(expected.Item3);   // Data
                    nw.Write(expected.Item5);   // Options data
                }

                // Reset the stream.
                ms.Seek(0, SeekOrigin.Begin);

                // Test the reader.
                using (var nr = new IpfixReader(ms)) {
                    Assert.AreSame(ms, nr.BaseStream);

                    var packetHeader = nr.ReadPacketHeader();
                    Assert.IsNotNull(packetHeader);
                    Assert.AreEqual(expected.Item1, packetHeader);
                    Assert.ThrowsException<InvalidOperationException>(() => nr.ReadPacketHeader());

                    var templateFlowSet = nr.ReadFlowSet() as TemplateSet;
                    Assert.IsNotNull(templateFlowSet);
                    Assert.AreEqual(expected.Item2.ID, templateFlowSet.ID);
                    Assert.AreEqual(expected.Item2.Records.Count, templateFlowSet.Records.Count);
                    Assert.AreEqual(1, templateFlowSet.Records.Count);
                    Assert.AreEqual(expected.Item2.Records[0].ID, templateFlowSet.Records[0].ID);
                    Assert.AreEqual(expected.Item2.Records[0].FieldCount, templateFlowSet.Records[0].FieldCount);

                    for (ushort i = 0; i < expected.Item2.Records[0].FieldCount; ++i) {
                        Assert.AreEqual(expected.Item2.Records[0].Fields[i], templateFlowSet.Records[0].Fields[i], $"Template field {i}");
                    }

                    var optionsTemplateFlowSet = nr.ReadFlowSet() as OptionsTemplateSet;
                    Assert.IsNotNull(optionsTemplateFlowSet);
                    Assert.AreEqual(expected.Item4.ID, optionsTemplateFlowSet.ID);
                    Assert.AreEqual(1, optionsTemplateFlowSet.Records.Count);

                    Assert.AreEqual(expected.Item4.Records[0].ScopeFields[0], optionsTemplateFlowSet.Records[0].ScopeFields[0]);
                    Assert.AreEqual(expected.Item4.Records[0].Fields[0], optionsTemplateFlowSet.Records[0].Fields[0]);
                    Assert.AreEqual(expected.Item4.Records[0].Fields[1], optionsTemplateFlowSet.Records[0].Fields[1]);

                    var dataFlowSet = nr.ReadFlowSet() as DataSet;
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

                    var optionsDataFlowSet = nr.ReadFlowSet() as DataSet;
                    Assert.IsNotNull(optionsDataFlowSet);
                    Assert.AreEqual(expected.Item5.ID, optionsDataFlowSet.ID);

                    //Assert.AreEqual(expected.Item5.Records.Count, optionsDataFlowSet.Records.Count);
                    //{
                    //    var actual = ushort.MaxValue;
                    //    FromWire(ref actual, (byte[]) optionsDataFlowSet.Scopes[0]);
                    //    Assert.AreEqual(expected.Item5.Scopes[0], actual);
                    //}

                    //Assert.AreEqual(expected.Item5.Options.Count, optionsDataFlowSet.Options.Count);
                    //{
                    //    var actual = ushort.MaxValue;
                    //    FromWire(ref actual, (byte[]) optionsDataFlowSet.Options[0]);
                    //    Assert.AreEqual(expected.Item5.Options[0], actual);
                    //}

                    //Assert.AreEqual(expected.Item5.Options[1], optionsDataFlowSet.Options[1]);
                }
            }
        }

        [TestMethod]
        public void TestNetflowWriter() {
            var testData = CreateTestData();
            Assert.AreEqual(16, testData.Item1.GetOnWireSize());    // header
            Assert.AreEqual(152, testData.Item1.Length);
            Assert.AreEqual(1, testData.Item2.Records.Count);       // template
            Assert.AreEqual(5, testData.Item2.Records[0].FieldCount);
            Assert.AreEqual(28, testData.Item2.Length);
            Assert.AreEqual(64, testData.Item3.Length);             // data
            Assert.AreEqual(24, testData.Item4.Length);             // options template
            Assert.AreEqual(1, testData.Item4.Records.Count);
            Assert.AreEqual(3, testData.Item4.Records[0].FieldCount);
            Assert.AreEqual(1, testData.Item4.Records[0].ScopeFieldCount);
            Assert.AreEqual(20, testData.Item5.Length);             // options
            Assert.AreEqual(testData.Item1.Length,
                testData.Item1.GetOnWireSize()
                + testData.Item2.Length + testData.Item3.Length
                + testData.Item4.Length + testData.Item5.Length);


            using (var ms = new MemoryStream()) {

                // Write the flows.
                using (var nw = new IpfixWriter(ms, true)) {
                    Assert.AreSame(ms, nw.BaseStream);

                    Assert.ThrowsException<InvalidOperationException>(() => nw.Write(testData.Item2));
                    Assert.ThrowsException<InvalidOperationException>(() => nw.Write(testData.Item3));
                    Assert.ThrowsException<InvalidOperationException>(() => nw.Write(testData.Item4));
                    Assert.ThrowsException<ArgumentNullException>(() => nw.Write((PacketHeader) null));

                    nw.Write(testData.Item1);
                    Assert.ThrowsException<InvalidOperationException>(() => nw.Write(testData.Item1));

                    Assert.ThrowsException<ArgumentNullException>(() => nw.Write((TemplateSet) null));
                    Assert.ThrowsException<ArgumentNullException>(() => nw.Write((DataSet) null));
                    Assert.ThrowsException<ArgumentNullException>(() => nw.Write((OptionsTemplateSet) null));

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
                        Assert.AreEqual(10u, actual, "Netflow version");
                    }
                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(152, actual, "packet size in bytes");
                    }

                    br.ReadUInt32();    // Skip export time.

                    {
                        var actual = br.ReadUInt32().ToHostByteOrder();
                        Assert.AreEqual(0u, actual, "package sequence");
                    }

                    {
                        var actual = br.ReadUInt32().ToHostByteOrder();
                        Assert.AreEqual(42u, actual, "observation domain");
                    }
                    #endregion

                    #region Template flow set
                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(SetIDs.TemplateSet, actual, "template flow set ID");
                    }

                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(testData.Item2.Length, actual, "template flow set length");
                    }

                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(testData.Item2.Records[0].ID, actual, "template ID");
                    }

                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(testData.Item2.Records[0].FieldCount, actual, "field count");
                    }

                    {
                        var actual = (InformationElement) br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(InformationElement.SourceIPv4Address, actual);
                    }

                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(4, actual);
                    }

                    {
                        var actual = (InformationElement) br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(InformationElement.DestinationIPv4Address, actual);
                    }

                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(4, actual);
                    }

                    {
                        var actual = (InformationElement) br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(InformationElement.IPNextHopIPv4Address, actual);
                    }

                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(4, actual);
                    }

                    {
                        var actual = (InformationElement) br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(InformationElement.PacketDeltaCount, actual);
                    }

                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(4, actual);
                    }

                    {
                        var actual = (InformationElement) br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(InformationElement.OctetDeltaCount, actual);
                    }

                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(4, actual);
                    }
                    #endregion

                    #region Options template
                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(SetIDs.OptionsTemplateSet, actual, "options template flow set ID");
                    }

                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(testData.Item4.Length, actual, "options template flow set length");
                    }

                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(testData.Item4.Records[0].ID, actual, "options template ID");
                    }

                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(testData.Item4.Records[0].FieldCount, actual, "total options fields");
                    }

                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(testData.Item4.Records[0].ScopeFieldCount, actual, "scope fields");
                    }

                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual((ushort) InformationElement.LineCardID, actual, "line card ID");
                    }

                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(4u, actual);
                    }

                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual((ushort) InformationElement.ExportedMessageTotalCount, actual, "exported messages");
                    }


                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(2u, actual);
                    }

                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual((ushort) InformationElement.ExportedFlowRecordTotalCount, actual, "exported flows");
                    }

                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(2u, actual);
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
                        Assert.AreEqual(IPAddress.Parse("192.0.2.12"), actual);
                    }

                    {
                        var actual = new IPAddress(br.ReadUInt32());
                        Assert.AreEqual(IPAddress.Parse("192.0.2.254"), actual);
                    }

                    {
                        var actual = new IPAddress(br.ReadUInt32());
                        Assert.AreEqual(IPAddress.Parse("192.0.2.1"), actual);
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
                        Assert.AreEqual(IPAddress.Parse("192.0.2.27"), actual);
                    }

                    {
                        var actual = new IPAddress(br.ReadUInt32());
                        Assert.AreEqual(IPAddress.Parse("192.0.2.23"), actual);
                    }

                    {
                        var actual = new IPAddress(br.ReadUInt32());
                        Assert.AreEqual(IPAddress.Parse("192.0.2.2"), actual);
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
                        Assert.AreEqual(IPAddress.Parse("192.0.2.56"), actual);
                    }

                    {
                        var actual = new IPAddress(br.ReadUInt32());
                        Assert.AreEqual(IPAddress.Parse("192.0.2.65"), actual);
                    }

                    {
                        var actual = new IPAddress(br.ReadUInt32());
                        Assert.AreEqual(IPAddress.Parse("192.0.2.3"), actual);
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
                        Assert.AreEqual(testData.Item5.ID, actual, "Options data flow set ID");
                    }

                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual(testData.Item5.Length, actual, "Options length");
                    }

                    {
                        var actual = br.ReadUInt32().ToHostByteOrder();
                        Assert.AreEqual(1u, actual, "scope");
                    }

                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual((ushort) 345, actual, "export message count");
                    }

                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual((ushort) 10201, actual, "export record count");
                    }


                    {
                        var actual = br.ReadUInt32().ToHostByteOrder();
                        Assert.AreEqual(2u, actual, "scope");
                    }

                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual((ushort) 690, actual, "export message count");
                    }

                    {
                        var actual = br.ReadUInt16().ToHostByteOrder();
                        Assert.AreEqual((ushort) 20402, actual, "export record count");
                    }
                    #endregion
                }
            }
        }

        [TestMethod]
        public void TestStreamCopy() {
            var testData = CreateTestData();

            using (var src = new MemoryStream()) {

                // Write test data.
                using (var nw = new IpfixWriter(src, true)) {
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
                    src.CopyIpfixPacket(dst);

                    // Compare the two streams.
                    Assert.IsTrue(src.ToArray().SequenceEqual(dst.ToArray()));
                }
            }
        }


        private static (PacketHeader, TemplateSet, DataSet,
                OptionsTemplateSet, DataSet) CreateTestData() {
            // Sample data from https://tools.ietf.org/html/rfc7011#section-3.4.2
            var packetHeader = new PacketHeader(152, 0, 42);

            var templateRecord = new TemplateRecord(256);
            templateRecord.Fields.Add(new FieldSpecifier(InformationElement.SourceIPv4Address));
            templateRecord.Fields.Add(new FieldSpecifier(InformationElement.DestinationIPv4Address));
            templateRecord.Fields.Add(new FieldSpecifier(InformationElement.IPNextHopIPv4Address));
            templateRecord.Fields.Add(new FieldSpecifier(InformationElement.PacketDeltaCount, 4));
            templateRecord.Fields.Add(new FieldSpecifier(InformationElement.OctetDeltaCount, 4));

            var templateFlowSet = new TemplateSet();
            templateFlowSet.Records.Add(templateRecord);

            var optionsRecord = new OptionsTemplateRecord(258);
            optionsRecord.ScopeFields.Add(new FieldSpecifier(InformationElement.LineCardID));
            optionsRecord.Fields.Add(new FieldSpecifier(InformationElement.ExportedMessageTotalCount, 2));
            optionsRecord.Fields.Add(new FieldSpecifier(InformationElement.ExportedFlowRecordTotalCount, 2));

            var optionsFlowSet = new OptionsTemplateSet();
            optionsFlowSet.Records.Add(optionsRecord);

            var dataFlowSet = new DataSet(templateRecord.ID);
            dataFlowSet.Records.Add(IPAddress.Parse("192.0.2.12"));
            dataFlowSet.Records.Add(IPAddress.Parse("192.0.2.254"));
            dataFlowSet.Records.Add(IPAddress.Parse("192.0.2.1"));
            dataFlowSet.Records.Add(5009u);
            dataFlowSet.Records.Add(5344385u);

            dataFlowSet.Records.Add(IPAddress.Parse("192.0.2.27"));
            dataFlowSet.Records.Add(IPAddress.Parse("192.0.2.23"));
            dataFlowSet.Records.Add(IPAddress.Parse("192.0.2.2"));
            dataFlowSet.Records.Add(748u);
            dataFlowSet.Records.Add(388934u);

            dataFlowSet.Records.Add(IPAddress.Parse("192.0.2.56"));
            dataFlowSet.Records.Add(IPAddress.Parse("192.0.2.65"));
            dataFlowSet.Records.Add(IPAddress.Parse("192.0.2.3"));
            dataFlowSet.Records.Add(5u);
            dataFlowSet.Records.Add(6534u);

            var optionsData = new DataSet(optionsRecord.ID);
            optionsData.Records.Add((uint) 1);
            optionsData.Records.Add((ushort) 345);
            optionsData.Records.Add((ushort) 10201);
            optionsData.Records.Add((uint) 2);
            optionsData.Records.Add((ushort) 690);
            optionsData.Records.Add((ushort) 20402);

            return (packetHeader, templateFlowSet, dataFlowSet, optionsFlowSet, optionsData);
        }
    }
}
