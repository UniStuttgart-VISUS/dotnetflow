// <copyright file="Layout5Test.cs" company="Universität Stuttgart">
// Copyright © 2020 - 2022 Institut für Visualisierung und Interaktive Systeme. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotNetFlow.Netflow5;


namespace DotNetFlow.Test {

    /// <summary>
    /// On-Wire layout of NetFlow v5 messages.
    /// </summary>
    [TestClass]
    public sealed class Layout5Test {

        [TestMethod]
        public void TestFlowRecord() {
            var record = new FlowRecord();
            Assert.AreEqual(48, record.GetOnWireSize());
        }

        [TestMethod]
        public void TestPacketHeader() {
            var header = new PacketHeader();
            Assert.AreEqual(24, header.GetOnWireSize());
        }
    }
}
