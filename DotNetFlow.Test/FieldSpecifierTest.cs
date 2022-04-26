// <copyright file="FieldSpecifierTest.cs" company="Universität Stuttgart">
// Copyright © 2020 - 2022 Institut für Visualisierung und Interaktive Systeme. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotNetFlow.Ipfix;


namespace DotNetFlow.Test {

    /// <summary>
    /// Unit test for <see cref="FieldSpecifier"/>.
    /// </summary>
    [TestClass]
    public class FieldSpecifierTest {

        [TestMethod]
        public void TestEquals() {
            var f1 = new FieldSpecifier(InformationElement.ApplicationDescription, 32);
            var f2 = new FieldSpecifier(InformationElement.ApplicationDescription, 32);
            var f3 = new FieldSpecifier(InformationElement.BgpNextHopIPv4Address);
            var f4 = new FieldSpecifier(InformationElement.ApplicationDescription, 16);
            var f5 = new FieldSpecifier(unchecked((short) 0xFFFF), 12, 42);
            var f6 = new FieldSpecifier(unchecked((short) 0xFFFF), 12, 42);
            var f7 = new FieldSpecifier(unchecked((short) 0xFFFF), 12, 43);

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

            Assert.IsFalse(f1.Equals(f5));
            Assert.IsFalse(f1.Equals(f6));
            Assert.IsFalse(f1.Equals(f7));
            Assert.IsTrue(f5.Equals(f6));
            Assert.IsFalse(f5.Equals(f7));

            Assert.AreEqual(f1.GetHashCode(), f2.GetHashCode());
            Assert.AreNotEqual(f1.GetHashCode(), f3.GetHashCode());
            Assert.AreNotEqual(f1.GetHashCode(), f4.GetHashCode());
            Assert.AreEqual(f5.GetHashCode(), f6.GetHashCode());
            Assert.AreNotEqual(f5.GetHashCode(), f7.GetHashCode());
        }

        [TestMethod]
        public void TestOnWireHandling() {
            {
                var expected = new FieldSpecifier(InformationElement.AbsoluteError);
                var wire = expected.ToWire();

                var actual = new FieldSpecifier();
                actual.FromWire(wire, 0);

                Assert.AreEqual(expected.InformationElement, actual.InformationElement);
                Assert.AreEqual(expected.InformationElementIdentifier, actual.InformationElementIdentifier);
                Assert.AreEqual(expected.Length, actual.Length);
            }

            {
                var expected = new FieldSpecifier(-1, 24, 42);
                var wire = expected.ToWire();

                var actual = new FieldSpecifier();
                actual.FromWire(wire, 0);

                Assert.AreEqual(expected.EnterpriseNumber, actual.EnterpriseNumber);
                Assert.AreEqual(expected.InformationElement, actual.InformationElement);
                Assert.AreEqual(expected.InformationElementIdentifier, actual.InformationElementIdentifier);
                Assert.AreEqual(expected.Length, actual.Length);
            }

        }

        [TestMethod]
        public void TestExtensionMethodDispatch() {
            {
                var expected = new FieldSpecifier(InformationElement.AbsoluteError);
                var wire = OnWireExtensions.ToWire(expected);

                var offset = 0;
                var length = 0;
                var actual = OnWireExtensions.FromWire(typeof(FieldSpecifier), wire, ref offset, ref length) as FieldSpecifier;
                Assert.IsNotNull(actual);
                Assert.AreEqual(2 * sizeof(ushort), offset);
                Assert.AreEqual(0, length);

                Assert.AreEqual(expected.InformationElement, actual.InformationElement);
                Assert.AreEqual(expected.InformationElementIdentifier, actual.InformationElementIdentifier);
                Assert.AreEqual(expected.Length, actual.Length);
            }
        }
    }
}
