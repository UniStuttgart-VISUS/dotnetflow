// <copyright file="PacketHeaderTest.cs" company="Universität Stuttgart">
// Copyright © 2020 SAPPAN Consortium. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Sappan.Netflow.Test {

    /// <summary>
    /// Tests for <see cref="PacketHeader"/>.
    /// </summary>
    [TestClass]
    public sealed class PacketHeaderTest {

        [TestMethod]
        public void TestEquals5() {
            var p1 = new Netflow5.PacketHeader();
            var p2 = new Netflow5.PacketHeader() {
                SystemUptime = p1.SystemUptime,
                UnixSeconds = p1.UnixSeconds,
                UnixNanoseconds = p1.UnixNanoseconds
            };
            var p3 = new Netflow5.PacketHeader(1, 0);
            var p4 = new Netflow5.PacketHeader(1, 1);

            Assert.IsTrue(p1.Equals(p2));
            Assert.IsTrue(p2.Equals(p1));

            Assert.IsFalse(p1.Equals(p3));
            Assert.IsFalse(p3.Equals(p1));

            Assert.IsFalse(p1.Equals(p4));
            Assert.IsFalse(p4.Equals(p1));


            Assert.IsFalse(p1.Equals(null));
            Assert.IsTrue(p1.Equals((object) p2));

            Assert.IsTrue(p1.Equals(p1));
            Assert.IsTrue(p2.Equals(p2));
            Assert.IsTrue(p3.Equals(p3));
            Assert.IsTrue(p4.Equals(p4));

            Assert.AreEqual(p1.GetHashCode(), p2.GetHashCode());
            Assert.AreNotEqual(p1.GetHashCode(), p3.GetHashCode());
            Assert.AreNotEqual(p1.GetHashCode(), p4.GetHashCode());
        }

        [TestMethod]
        public void TestEquals9() {
            var p1 = new Netflow9.PacketHeader();
            var p2 = new Netflow9.PacketHeader() {
                SystemUptime = p1.SystemUptime,
                UnixSeconds = p1.UnixSeconds
            };
            var p3 = new Netflow9.PacketHeader(1, 0, 0);
            var p4 = new Netflow9.PacketHeader(1, 1, 0);
            var p5 = new Netflow9.PacketHeader(1, 1, 1);

            Assert.IsTrue(p1.Equals(p2));
            Assert.IsTrue(p2.Equals(p1));

            Assert.IsFalse(p1.Equals(p3));
            Assert.IsFalse(p3.Equals(p1));

            Assert.IsFalse(p1.Equals(p4));
            Assert.IsFalse(p4.Equals(p1));

            Assert.IsFalse(p1.Equals(p5));
            Assert.IsFalse(p5.Equals(p1));

            Assert.IsFalse(p1.Equals(null));
            Assert.IsTrue(p1.Equals((object) p2));

            Assert.IsTrue(p1.Equals(p1));
            Assert.IsTrue(p2.Equals(p2));
            Assert.IsTrue(p3.Equals(p3));
            Assert.IsTrue(p4.Equals(p4));
            Assert.IsTrue(p5.Equals(p5));

            Assert.AreEqual(p1.GetHashCode(), p2.GetHashCode());
            Assert.AreNotEqual(p1.GetHashCode(), p3.GetHashCode());
            Assert.AreNotEqual(p1.GetHashCode(), p4.GetHashCode());
            Assert.AreNotEqual(p1.GetHashCode(), p5.GetHashCode());
        }

        [TestMethod]
        public void TestEquals10() {
            var p1 = new Ipfix.PacketHeader();
            var p2 = new Ipfix.PacketHeader() {
                ExportTime = p1.ExportTime,
            };
            var p3 = new Netflow9.PacketHeader(1, 0, 0);
            var p4 = new Netflow9.PacketHeader(1, 1, 0);
            var p5 = new Netflow9.PacketHeader(1, 1, 1);

            Assert.IsTrue(p1.Equals(p2));
            Assert.IsTrue(p2.Equals(p1));

            Assert.IsFalse(p1.Equals(p3));
            Assert.IsFalse(p3.Equals(p1));

            Assert.IsFalse(p1.Equals(p4));
            Assert.IsFalse(p4.Equals(p1));

            Assert.IsFalse(p1.Equals(p5));
            Assert.IsFalse(p5.Equals(p1));

            Assert.IsFalse(p1.Equals(null));
            Assert.IsTrue(p1.Equals((object) p2));

            Assert.IsTrue(p1.Equals(p1));
            Assert.IsTrue(p2.Equals(p2));
            Assert.IsTrue(p3.Equals(p3));
            Assert.IsTrue(p4.Equals(p4));
            Assert.IsTrue(p5.Equals(p5));

            Assert.AreEqual(p1.GetHashCode(), p2.GetHashCode());
            Assert.AreNotEqual(p1.GetHashCode(), p3.GetHashCode());
            Assert.AreNotEqual(p1.GetHashCode(), p4.GetHashCode());
            Assert.AreNotEqual(p1.GetHashCode(), p5.GetHashCode());
        }

        [TestMethod]
        public void TestInitialiser5() {
            {
                var p = new Netflow5.PacketHeader();
                Assert.AreEqual((ushort) 0, p.Count);
                Assert.AreEqual((byte) 0, p.EngineID);
                Assert.AreEqual((byte) 0, p.EngineType);
                Assert.AreEqual((ushort) 0, p.SamplingInterval);
                Assert.AreEqual((ushort) 0, p.SequenceNumber);
                Assert.AreEqual((ushort) 5, p.Version);
            }

            {
                var p = new Netflow5.PacketHeader(1, 2);
                Assert.AreEqual((ushort) 1, p.Count);
                Assert.AreEqual((ushort) 2, p.SequenceNumber);
                Assert.AreEqual((ushort) 5, p.Version);
            }
        }

        [TestMethod]
        public void TestInitialiser9() {
            {
                var p = new Netflow9.PacketHeader();
                Assert.AreEqual((ushort) 0, p.Count);
                Assert.AreEqual((ushort) 0, p.SequenceNumber);
                Assert.AreEqual((ushort) 0, p.SourceID);
                Assert.AreEqual((ushort) 9, p.Version);
            }

            {
                var p = new Netflow9.PacketHeader(1, 2, 3);
                Assert.AreEqual((ushort) 1, p.Count);
                Assert.AreEqual((ushort) 2, p.SequenceNumber);
                Assert.AreEqual((ushort) 3, p.SourceID);
                Assert.AreEqual((ushort) 9, p.Version);
            }
        }

        [TestMethod]
        public void TestInitialiser10() {
            {
                var p = new Ipfix.PacketHeader();
                Assert.AreEqual((ushort) 0, p.ObservationDomainID);
                Assert.AreEqual((ushort) 0, p.SequenceNumber);
                Assert.AreEqual((ushort) 10, p.Version);
            }

            {
                var p = new Ipfix.PacketHeader(1, 2, 3);
                Assert.AreEqual((ushort) 1, p.Length);
                Assert.AreEqual((ushort) 2, p.SequenceNumber);
                Assert.AreEqual((ushort) 3, p.ObservationDomainID);
                Assert.AreEqual((ushort) 10, p.Version);
            }
        }
    }
}
