// <copyright file="FieldTest.cs" company="Universität Stuttgart">
// Copyright © 2020 SAPPAN Consortium. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sappan.Netflow.Netflow9;
using System;


namespace Sappan.Netflow.Test {

    /// <summary>
    /// Tests <see cref="Field"/>.
    /// </summary>
    [TestClass]
    public class FieldTest {

        [TestMethod]
        public void TestEquals() {
            var f1 = new Field(FieldType.ApplicationDescription, 32);
            var f2 = new Field(FieldType.ApplicationDescription, 32);
            var f3 = new Field(FieldType.BgpIPv4NextHop);
            var f4 = new Field(FieldType.ApplicationDescription, 16);

            Assert.IsTrue(f1.Equals(f2));
            Assert.IsTrue(f2.Equals(f1));

            Assert.IsFalse(f1.Equals(f3));
            Assert.IsFalse(f3.Equals(f1));

            Assert.IsFalse(f1.Equals(f4));
            Assert.IsFalse(f4.Equals(f1));

            Assert.IsFalse(f1.Equals(null));
            Assert.IsTrue(f1.Equals((object) f2));

            Assert.IsTrue(f1.Equals(f1));
            Assert.IsTrue(f2.Equals(f2));
            Assert.IsTrue(f3.Equals(f3));
            Assert.IsTrue(f4.Equals(f4));

            Assert.AreEqual(f1.GetHashCode(), f2.GetHashCode());
            Assert.AreNotEqual(f1.GetHashCode(), f3.GetHashCode());
            Assert.AreNotEqual(f1.GetHashCode(), f4.GetHashCode());
        }

        /// <summary>
        /// Tests the annotations for the field sizes described in
        /// https://www.cisco.com/en/US/technologies/tk648/tk362/technologies_white_paper09186a00800a3db9.html
        /// </summary>
        [TestMethod]
        public void TestLengthFormFieldType() {

            {
                var field = new Field(FieldType.IncomingBytes);
                Assert.AreEqual(4, field.Length);
            }

            {
                var field = new Field(FieldType.IncomingPackets);
                Assert.AreEqual(4, field.Length);
            }

            Assert.ThrowsException<ArgumentException>(() => {
                var field = new Field(FieldType.Flows);
            });

            {
                var field = new Field(FieldType.Protocol);
                Assert.AreEqual(1, field.Length);
            }

            {
                var field = new Field(FieldType.SourceServiceType);
                Assert.AreEqual(1, field.Length);
            }

            {
                var field = new Field(FieldType.TcpFlags);
                Assert.AreEqual(1, field.Length);
            }

            {
                var field = new Field(FieldType.Layer4SourcePort);
                Assert.AreEqual(2, field.Length);
            }

            {
                var field = new Field(FieldType.IPv4SourceAddress);
                Assert.AreEqual(4, field.Length);
            }

            {
                var field = new Field(FieldType.IPv4SourceMask);
                Assert.AreEqual(1, field.Length);
            }

            Assert.ThrowsException<ArgumentException>(() => {
                var field = new Field(FieldType.InputSnmp);
            });

            {
                var field = new Field(FieldType.Layer4DestinationPort);
                Assert.AreEqual(2, field.Length);
            }

            {
                var field = new Field(FieldType.IPv4DestinationAddress);
                Assert.AreEqual(4, field.Length);
            }

            {
                var field = new Field(FieldType.IPv4DestinationMask);
                Assert.AreEqual(1, field.Length);
            }

            Assert.ThrowsException<ArgumentException>(() => {
                var field = new Field(FieldType.OutputSnmp);
            });

            {
                var field = new Field(FieldType.IPv4NextHop);
                Assert.AreEqual(4, field.Length);
            }

            {
                var field = new Field(FieldType.SourceAutonomousSystem);
                Assert.AreEqual(2, field.Length);
            }

            {
                var field = new Field(FieldType.DestinationAutonomousSystem);
                Assert.AreEqual(2, field.Length);
            }

            {
                var field = new Field(FieldType.BgpIPv4NextHop);
                Assert.AreEqual(4, field.Length);
            }

            {
                var field = new Field(FieldType.MulticastDestinationPackets);
                Assert.AreEqual(4, field.Length);
            }

            {
                var field = new Field(FieldType.MulticastDestintationBytes);
                Assert.AreEqual(4, field.Length);
            }

            {
                var field = new Field(FieldType.LastSwitched);
                Assert.AreEqual(4, field.Length);
            }

            {
                var field = new Field(FieldType.FirstSwitched);
                Assert.AreEqual(4, field.Length);
            }

            {
                var field = new Field(FieldType.OutgoingBytes);
                Assert.AreEqual(4, field.Length);
            }

            {
                var field = new Field(FieldType.OutgoingPackets);
                Assert.AreEqual(4, field.Length);
            }

            {
                var field = new Field(FieldType.MinimumPacketLength);
                Assert.AreEqual(2, field.Length);
            }

            {
                var field = new Field(FieldType.MaximumPacketLength);
                Assert.AreEqual(2, field.Length);
            }

            {
                var field = new Field(FieldType.IPv6SourceAddress);
                Assert.AreEqual(16, field.Length);
            }

            {
                var field = new Field(FieldType.IPv6DestinationAddress);
                Assert.AreEqual(16, field.Length);
            }

            {
                var field = new Field(FieldType.IPv6SourceMask);
                Assert.AreEqual(1, field.Length);
            }

            {
                var field = new Field(FieldType.IPv6DestinationMask);
                Assert.AreEqual(1, field.Length);
            }

            {
                var field = new Field(FieldType.IPv6FlowLabel);
                Assert.AreEqual(3, field.Length);
            }

            {
                var field = new Field(FieldType.IcmpType);
                Assert.AreEqual(2, field.Length);
            }

            {
                var field = new Field(FieldType.MulticastIgmpType);
                Assert.AreEqual(1, field.Length);
            }

            {
                var field = new Field(FieldType.SamplingInterval);
                Assert.AreEqual(4, field.Length);
            }

            {
                var field = new Field(FieldType.SamplingAlgorithm);
                Assert.AreEqual(1, field.Length);
            }

            {
                var field = new Field(FieldType.FlowActiveTimeout);
                Assert.AreEqual(2, field.Length);
            }

            {
                var field = new Field(FieldType.FlowInactiveTimeout);
                Assert.AreEqual(2, field.Length);
            }

            {
                var field = new Field(FieldType.EngineType);
                Assert.AreEqual(1, field.Length);
            }

            {
                var field = new Field(FieldType.EngineID);
                Assert.AreEqual(1, field.Length);
            }

            {
                var field = new Field(FieldType.TotalBytesExported);
                Assert.AreEqual(4, field.Length);
            }

            {
                var field = new Field(FieldType.TotalPacketsExported);
                Assert.AreEqual(4, field.Length);
            }

            {
                var field = new Field(FieldType.TotalFlowsExported);
                Assert.AreEqual(4, field.Length);
            }

            {
                var field = new Field(FieldType.IPv4SourcePrefix);
                Assert.AreEqual(4, field.Length);
            }

            {
                var field = new Field(FieldType.IPv4DestinationPrefix);
                Assert.AreEqual(4, field.Length);
            }

            {
                var field = new Field(FieldType.MplsTopLabelType);
                Assert.AreEqual(1, field.Length);
            }

            {
                var field = new Field(FieldType.MplsTopLabelIPAddress);
                Assert.AreEqual(4, field.Length);
            }

            {
                var field = new Field(FieldType.FlowSamplerID);
                Assert.AreEqual(1, field.Length);
            }

            {
                var field = new Field(FieldType.FlowSamplerMode);
                Assert.AreEqual(1, field.Length);
            }

            {
                var field = new Field(FieldType.FlowSamplerRandomInterval);
                Assert.AreEqual(4, field.Length);
            }

            {
                var field = new Field(FieldType.MinimumTimeToLive);
                Assert.AreEqual(1, field.Length);
            }

            {
                var field = new Field(FieldType.MaximumTimeToLive);
                Assert.AreEqual(1, field.Length);
            }

            {
                var field = new Field(FieldType.IPv4Identification);
                Assert.AreEqual(2, field.Length);
            }

            {
                var field = new Field(FieldType.DestinationServiceType);
                Assert.AreEqual(1, field.Length);
            }

            {
                var field = new Field(FieldType.IncomingSourceMac);
                Assert.AreEqual(6, field.Length);
            }

            {
                var field = new Field(FieldType.OutgoingSourceMac);
                Assert.AreEqual(6, field.Length);
            }

            {
                var field = new Field(FieldType.SourceVlan);
                Assert.AreEqual(2, field.Length);
            }

            {
                var field = new Field(FieldType.DestinationVlan);
                Assert.AreEqual(2, field.Length);
            }

            {
                var field = new Field(FieldType.Direction);
                Assert.AreEqual(1, field.Length);
            }

            {
                var field = new Field(FieldType.IPv6NextHop);
                Assert.AreEqual(16, field.Length);
            }

            {
                var field = new Field(FieldType.BgpIPv6NextHop);
                Assert.AreEqual(16, field.Length);
            }

            {
                var field = new Field(FieldType.IPv6OptionHeaders);
                Assert.AreEqual(4, field.Length);
            }

            {
                var field = new Field(FieldType.MplsLabel1);
                Assert.AreEqual(3, field.Length);
            }

            {
                var field = new Field(FieldType.MplsLabel2);
                Assert.AreEqual(3, field.Length);
            }

            {
                var field = new Field(FieldType.MplsLabel3);
                Assert.AreEqual(3, field.Length);
            }

            {
                var field = new Field(FieldType.MplsLabel4);
                Assert.AreEqual(3, field.Length);
            }

            {
                var field = new Field(FieldType.MplsLabel5);
                Assert.AreEqual(3, field.Length);
            }

            {
                var field = new Field(FieldType.MplsLabel6);
                Assert.AreEqual(3, field.Length);
            }

            {
                var field = new Field(FieldType.MplsLabel7);
                Assert.AreEqual(3, field.Length);
            }

            {
                var field = new Field(FieldType.MplsLabel8);
                Assert.AreEqual(3, field.Length);
            }

            {
                var field = new Field(FieldType.MplsLabel9);
                Assert.AreEqual(3, field.Length);
            }

            {
                var field = new Field(FieldType.MplsLabel10);
                Assert.AreEqual(3, field.Length);
            }

            {
                var field = new Field(FieldType.OutgoingDestinationMac);
                Assert.AreEqual(6, field.Length);
            }

            {
                var field = new Field(FieldType.OutgoingSourceMac);
                Assert.AreEqual(6, field.Length);
            }

            Assert.ThrowsException<ArgumentException>(() => {
                var field = new Field(FieldType.InterfaceName);
            });

            Assert.ThrowsException<ArgumentException>(() => {
                var field = new Field(FieldType.InterfaceDescription);
            });

            Assert.ThrowsException<ArgumentException>(() => {
                var field = new Field(FieldType.SamplerName);
            });

            {
                var field = new Field(FieldType.IncomingPermanentBytes);
                Assert.AreEqual(4, field.Length);
            }

            {
                var field = new Field(FieldType.IncomingPermanentPackets);
                Assert.AreEqual(4, field.Length);
            }

            {
                var field = new Field(FieldType.FragmentOffset);
                Assert.AreEqual(2, field.Length);
            }

            {
                var field = new Field(FieldType.ForwardingStatus);
                Assert.AreEqual(1, field.Length);
            }

            {
                var field = new Field(FieldType.MplsPalRouteDistinguisher);
                Assert.AreEqual(8, field.Length);
            }

            {
                var field = new Field(FieldType.MplsPrefixLength);
                Assert.AreEqual(1, field.Length);
            }

            {
                var field = new Field(FieldType.SourceTrafficIndex);
                Assert.AreEqual(4, field.Length);
            }

            {
                var field = new Field(FieldType.DestinationTrafficIndex);
                Assert.AreEqual(4, field.Length);
            }

            Assert.ThrowsException<ArgumentException>(() => {
                var field = new Field(FieldType.ApplicationDescription);
            });

            Assert.ThrowsException<ArgumentException>(() => {
                var field = new Field(FieldType.ApplicationTag);
            });

            Assert.ThrowsException<ArgumentException>(() => {
                var field = new Field(FieldType.ApplicationName);
            });

            {
                var field = new Field(FieldType.PostIPDifferentiatedServicesCodePoint);
                Assert.AreEqual(1, field.Length);
            }

            {
                var field = new Field(FieldType.ReplicationFactor);
                Assert.AreEqual(4, field.Length);
            }

            Assert.ThrowsException<ArgumentException>(() => {
                var field = new Field(FieldType.Layer2PacketSectionOffset);
            });

            Assert.ThrowsException<ArgumentException>(() => {
                var field = new Field(FieldType.Layer2PacketSectionSize);
            });

            Assert.ThrowsException<ArgumentException>(() => {
                var field = new Field(FieldType.Layer2PacketSectionData);
            });
        }
    }
}
