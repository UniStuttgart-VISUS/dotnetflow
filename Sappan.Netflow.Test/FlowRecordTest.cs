// <copyright file="FlowRecordTest.cs" company="Universität Stuttgart">
// Copyright © 2020 SAPPAN Consortium. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sappan.Netflow.Netflow5;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;


namespace Sappan.Netflow.Test {

    /// <summary>
    /// Tests for <see cref="Netflow5.FlowRecord"/>.
    /// </summary>
    [TestClass]
    public sealed class FlowRecordTest {

        [TestMethod]
        public void TestErrorHandling() {
            var record = new FlowRecord();

            Assert.ThrowsException<ArgumentNullException>(() => record.DestinationAddress = null);
            Assert.ThrowsException<ArgumentNullException>(() => record.NextHop = null);
            Assert.ThrowsException<ArgumentNullException>(() => record.SourceAddress = null);

            Assert.ThrowsException<ArgumentException>(() => record.DestinationAddress = IPAddress.IPv6Any);
            Assert.ThrowsException<ArgumentException>(() => record.NextHop = IPAddress.IPv6Any);
            Assert.ThrowsException<ArgumentException>(() => record.SourceAddress = IPAddress.IPv6Any);
        }

        [TestMethod]
        public void TestInitialiser() {
            var record = new FlowRecord();

            Assert.AreEqual(IPAddress.Any, record.DestinationAddress);
            Assert.AreEqual((ushort) 0, record.DestinationAutonomousSystem);
            Assert.AreEqual((byte) 0, record.DestinationMask);
            Assert.AreEqual((ushort) 0, record.DestinationPort);
            Assert.AreEqual((uint) 0, record.End);
            Assert.AreEqual((ushort) 0, record.InputInterface);
            Assert.AreEqual(IPAddress.Any, record.NextHop);
            Assert.AreEqual((uint) 0, record.Octets);
            Assert.AreEqual((ushort) 0, record.OutputInterface);
            Assert.AreEqual((uint) 0, record.Packets);
            Assert.AreEqual((byte) 0, record.Padding1);
            Assert.AreEqual((byte) 0, record.Protocol);
            Assert.AreEqual(IPAddress.Any, record.SourceAddress);
            Assert.AreEqual((ushort) 0, record.SourceAutonomousSystem);
            Assert.AreEqual((byte) 0, record.SourceMask);
            Assert.AreEqual((ushort) 0, record.SourcePort);
            Assert.AreEqual((uint) 0, record.Start);
            Assert.AreEqual((byte) 0, record.TcpFlags);
            Assert.AreEqual((byte) 0, record.TypeOfService);
        }
    }
}
