// <copyright file="Layout10Test.cs" company="Universität Stuttgart">
// Copyright © 2020 SAPPAN Consortium. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sappan.Netflow.Ipfix;
using System;


namespace Sappan.Netflow.Test {

    /// <summary>
    /// Performs tests of the on-wire layout of IPFIX messages that is
    /// automatically extracted from the types.
    /// </summary>
    [TestClass]
    public sealed class Layout10Test {

        [TestMethod]
        public void TestDataSet() {
            var set = new DataSet();
            Assert.AreEqual(sizeof(ushort) + sizeof(ushort), set.Length);

            set.Records.Add(5);
            Assert.AreEqual(2 * sizeof(ushort) + sizeof(int), set.Length);

            set.Records.Add(5.0f);
            Assert.AreEqual(2 * sizeof(ushort) + sizeof(int) + sizeof(float), set.Length);

            set.Records.Add(new[] { 1, 2, 3, 4 });
            Assert.AreEqual(2 * sizeof(ushort) + sizeof(int) + sizeof(float) + 4 * sizeof(int), set.Length);

            Assert.ThrowsException<ArgumentException>(() => new DataSet(0));
            Assert.ThrowsException<ArgumentException>(() => new DataSet(255));
        }

        [TestMethod]
        public void TestFieldSpecifier() {
            {
                var fs = new FieldSpecifier(InformationElement.AbsoluteError);
                Assert.IsFalse(fs.IsEnterpriseNumber);
                var wire = fs.ToWire();
                Assert.AreEqual(2 * sizeof(ushort), fs.GetOnWireSize());
                Assert.AreEqual((ushort) 0, BitConverter.ToUInt16(wire).ToHostByteOrder() & 0x1);
                Assert.AreEqual((ushort) (InformationElement.AbsoluteError), BitConverter.ToUInt16(wire).ToHostByteOrder());
                Assert.AreEqual((ushort) 8, BitConverter.ToUInt16(wire, 2).ToHostByteOrder());
            }

            {
                var fs = new FieldSpecifier(-1, 24, 42);
                Assert.IsTrue(fs.IsEnterpriseNumber);
                var wire = fs.ToWire();
                Assert.AreEqual(2 * sizeof(ushort) + sizeof(uint), fs.GetOnWireSize());
                Assert.AreEqual((ushort) 0xffff, BitConverter.ToUInt16(wire).ToHostByteOrder());
                Assert.AreEqual((ushort) 24, BitConverter.ToUInt16(wire, 2).ToHostByteOrder());
                Assert.AreEqual((uint) 42, BitConverter.ToUInt32(wire, 4).ToHostByteOrder());
            }
        }

        [TestMethod]
        public void TestOptionsTemplateRecord() {
            var record = new OptionsTemplateRecord(256);
            Assert.AreEqual(3 * sizeof(ushort) + sizeof(ushort), record.GetOnWireSize());
            Assert.IsTrue(record.GetOnWireSize() % 4 == 0);
            Assert.AreEqual(0, record.FieldCount);
            Assert.AreEqual(0, record.ScopeFieldCount);

            record.Fields.Add(new FieldSpecifier(InformationElement.EngineID));
            Assert.AreEqual(1, record.Fields.Count);
            Assert.AreEqual(record.Fields.Count, record.FieldCount);

            record.ScopeFields.Add(new FieldSpecifier(InformationElement.ExportInterface, 4));
            Assert.AreEqual(1, record.ScopeFields.Count);
            Assert.AreEqual(2, record.FieldCount);
        }

        [TestMethod]
        public void TestOptionsTemplateFlowSet() {
            var record = new OptionsTemplateRecord(256);
            record.Fields.Add(new FieldSpecifier(InformationElement.EngineID));
            record.ScopeFields.Add(new FieldSpecifier(InformationElement.ExportInterface, 4));
            Assert.AreEqual(3 * sizeof(ushort) + 2 * 2 * sizeof(ushort) + 2, record.GetOnWireSize());

            var flowset = new OptionsTemplateSet();
            Assert.AreEqual(SetIDs.OptionsTemplateSet, flowset.ID);
            Assert.AreEqual(2 * sizeof(ushort), flowset.GetOnWireSize());

            flowset.Records.Add(record);
            Assert.AreEqual(2 * sizeof(ushort)  // Set header
                + 3 * sizeof(ushort)            // Template header
                + 2 * 2 * sizeof(ushort)        // Content
                + 2,                            // Padding
                flowset.GetOnWireSize());
        }

        [TestMethod]
        public void TestPadding() {
            {
                var set = new DataSet();
                set.Records.Add((uint) 1);
                Assert.AreEqual(2 * sizeof(ushort) + sizeof(uint), set.Length);
                Assert.IsTrue(set.Length % 4 == 0);
            }

            {
                var flowset = new DataSet();
                flowset.Records.Add((ushort) 1);
                Assert.AreEqual(2 * sizeof(ushort) + sizeof(uint), flowset.Length);
                Assert.IsTrue(flowset.Length % 4 == 0);
            }
        }

        [TestMethod]
        public void TestPacketHeader() {
            var header = new PacketHeader();
            Assert.AreEqual(2 * sizeof(ushort) + 3 * sizeof(uint), header.GetOnWireSize());
        }

        [TestMethod]
        public void TestTemplateFlowSet() {
            var flowset = new TemplateSet();
            Assert.AreEqual(SetIDs.TemplateSet, flowset.ID);
            Assert.IsNotNull(flowset.Records);
            Assert.AreEqual(2 * sizeof(ushort), flowset.Length);
            Assert.AreEqual((int) flowset.Length, flowset.GetOnWireSize());

            var record = new TemplateRecord(256);
            flowset.Records.Add(record);
            Assert.AreEqual(4 * sizeof(ushort), flowset.Length);
        }

        [TestMethod]
        public void TestTemplateRecord() {
            var record = new TemplateRecord(256);
            Assert.AreEqual(256, record.ID);
            Assert.AreEqual(0, record.FieldCount);
            Assert.AreEqual(2 * sizeof(ushort), record.GetOnWireSize());

            var field = new FieldSpecifier(InformationElement.BgpNextHopIPv4Address);
            Assert.AreEqual(2 * sizeof(ushort), field.GetOnWireSize());
            record.Fields.Add(field);
            Assert.AreEqual(2 * sizeof(ushort) + 2 * sizeof(ushort), record.GetOnWireSize());

            Assert.ThrowsException<ArgumentException>(() => new TemplateRecord(0));
            Assert.ThrowsException<ArgumentException>(() => new TemplateRecord(255));
        }
    }
}
