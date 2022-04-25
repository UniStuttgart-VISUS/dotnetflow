// <copyright file="AttributeTest.cs" company="Universität Stuttgart">
// Copyright © 2020 SAPPAN Consortium. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;


namespace Sappan.Netflow.Test {

    /// <summary>
    /// Tests for custom attribtues.
    /// </summary>
    [TestClass]
    public sealed class AttributeTest {

        [TestMethod]
        public void TestClrTypeAttribute() {
            {
                var att = new ClrTypeAttribute(typeof(int));
                Assert.AreEqual(typeof(int), att.Type);
            }

            Assert.ThrowsException<ArgumentNullException>(() => new ClrTypeAttribute(null));
        }

        [TestMethod]
        public void TestFieldLengthAttribute() {
            {
                var att = new FieldLengthAttribute(1);
                Assert.AreEqual(1, att.Length);
                Assert.AreEqual(1, att.Default);
            }

            {
                var att = new FieldLengthAttribute(0);
                Assert.AreEqual(0, att.Length);
                Assert.AreEqual(0, att.Default);
            }

            {
                var att = new FieldLengthAttribute();
                Assert.AreEqual(0, att.Length);
                Assert.AreEqual(0, att.Default);
            }

            {
                var att = new FieldLengthAttribute();
                att.Default = 42;
                Assert.AreEqual(0, att.Length);
                Assert.AreEqual(42, att.Default);
            }
        }

        [TestMethod]
        public void TestOnWireOrderAttribute() {
            {
                var att = new OnWireOrderAttribute(0);
                Assert.AreEqual(0, att.Position);
            }
        }

        [TestMethod]
        public void TestOnWirePaddingAttribute() {
            {
                var att = new OnWirePaddingAttribute(4);
                Assert.AreEqual(4u, att.Alignment);
                Assert.ThrowsException<InvalidOperationException>(() => att.ByteAlignment);
            }

            {
                var att = new OnWirePaddingAttribute(32);
                Assert.AreEqual(32u, att.Alignment);
                Assert.AreEqual(4, att.ByteAlignment);
            }
        }
    }
}
