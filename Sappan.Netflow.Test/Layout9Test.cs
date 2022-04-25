// <copyright file="Layout9Test.cs" company="Universität Stuttgart">
// Copyright © 2020 SAPPAN Consortium. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sappan.Netflow.Netflow9;
using System;


namespace Sappan.Netflow.Test {

    /// <summary>
    /// Performs tests of the on-wire layout that is automatically extracted
    /// from the types.
    /// </summary>
    [TestClass]
    public sealed class Layout9Test {

        [TestMethod]
        public void TestDataFlowSet() {
            var flowset = new DataFlowSet();
            Assert.AreEqual(sizeof(ushort) + sizeof(ushort), flowset.Length);

            flowset.Records.Add(5);
            Assert.AreEqual(2 * sizeof(ushort) + sizeof(int), flowset.Length);

            flowset.Records.Add(5.0f);
            Assert.AreEqual(2 * sizeof(ushort) + sizeof(int) + sizeof(float), flowset.Length);

            flowset.Records.Add(new[] { 1, 2, 3, 4 });
            Assert.AreEqual(2 * sizeof(ushort) + sizeof(int) + sizeof(float) + 4 * sizeof(int), flowset.Length);

            Assert.ThrowsException<ArgumentException>(() => new DataFlowSet(0));
            Assert.ThrowsException<ArgumentException>(() => new DataFlowSet(255));
        }

        [TestMethod]
        public void TestOptionsTemplateRecord() {
            var record = new OptionsTemplateRecord(256);
            Assert.AreEqual(3 * sizeof(ushort), record.GetOnWireSize());
            Assert.AreEqual(0, record.OptionsLength);
            Assert.AreEqual(0, record.ScopesLength);

            record.Options.Add(new Field(FieldType.EngineID));
            Assert.AreEqual(1, record.Options.Count);
            Assert.AreEqual(2 * sizeof(ushort), record.OptionsLength);

            record.Scopes.Add(new Scope(OptionScope.Interface, 4));
            Assert.AreEqual(1, record.Scopes.Count);
            Assert.AreEqual(2 * sizeof(ushort), record.ScopesLength);

            Assert.AreEqual(3 * sizeof(ushort) + record.OptionsLength + record.ScopesLength, record.GetOnWireSize());
        }

        [TestMethod]
        public void TestOptionsTemplateFlowSet() {
            var record = new OptionsTemplateRecord(256);
            record.Options.Add(new Field(FieldType.EngineID));
            record.Scopes.Add(new Scope(OptionScope.Interface, 4));

            var flowset = new OptionsTemplateFlowSet();
            Assert.AreEqual(2 * sizeof(ushort), flowset.GetOnWireSize());

            flowset.Records.Add(record);
            Assert.AreEqual(5 * sizeof(ushort) + record.OptionsLength + record.ScopesLength + 2, flowset.GetOnWireSize());
        }

        [TestMethod]
        public void TestPadding() {
            {
                var flowset = new DataFlowSet();
                flowset.Records.Add((uint) 1);
                Assert.AreEqual(2 * sizeof(ushort) + sizeof(uint), flowset.Length);
                Assert.IsTrue(flowset.Length % 4 == 0);
            }

            {
                var flowset = new DataFlowSet();
                flowset.Records.Add((ushort) 1);
                Assert.AreEqual(2 * sizeof(ushort) + sizeof(uint), flowset.Length);
                Assert.IsTrue(flowset.Length % 4 == 0);
            }
        }

        [TestMethod]
        public void TestPacketHeader() {
            var header = new PacketHeader();
            Assert.AreEqual(2 * sizeof(ushort) + 4 * sizeof(uint), header.GetOnWireSize());
        }

        [TestMethod]
        public void TestTemplateFlowSet() {
            var flowset = new TemplateFlowSet();
            Assert.AreEqual(0, flowset.ID);
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

            record.Fields.Add(new Field(FieldType.BgpIPv4NextHop));
            Assert.AreEqual(2 * sizeof(ushort) + sizeof(uint), record.GetOnWireSize());

            Assert.ThrowsException<ArgumentException>(() => new TemplateRecord(0));
            Assert.ThrowsException<ArgumentException>(() => new TemplateRecord(255));
        }
    }
}
