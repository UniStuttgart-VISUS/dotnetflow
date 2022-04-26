// <copyright file="ExtensionsTest.cs" company="Universität Stuttgart">
// Copyright © 2020 - 2022 Institut für Visualisierung und Interaktive Systeme. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotNetFlow.Netflow9;
using System;
using static DotNetFlow.OnWireExtensions;


namespace DotNetFlow.Test {

    /// <summary>
    /// Tests for the extension methods of the library.
    /// </summary>
    [TestClass]
    public sealed class ExtensionsTest {

        [TestMethod]
        public void TestAdjustToNetFlow() {
            {
                var input = "test";
                var bytes = input.AdjustToNetFlow(4);
                Assert.AreEqual(4, bytes.Length);

                var output = string.Empty;
                FromWire(ref output, bytes);
                Assert.AreEqual(input, output);
            }

            {
                var input = "test";
                var bytes = input.AdjustToNetFlow(3);
                Assert.AreEqual(3, bytes.Length);

                var output = string.Empty;
                FromWire(ref output, bytes);
                Assert.AreEqual("tes", output);
            }

            {
                var input = "test";
                var bytes = input.AdjustToNetFlow(5);
                Assert.AreEqual(5, bytes.Length);

                var output = string.Empty;
                FromWire(ref output, bytes);
                Assert.AreEqual("test\0", output);
            }

            {
                var input = "test";
                var bytes = input.AdjustToNetFlow(new Field(FieldType.ApplicationName, 4));
                Assert.AreEqual(4, bytes.Length);

                var output = string.Empty;
                FromWire(ref output, bytes);
                Assert.AreEqual(input, output);
            }

            Assert.ThrowsException<ArgumentNullException>(() => ((string) null).AdjustToNetFlow(4));
            Assert.ThrowsException<ArgumentNullException>(() => "test".AdjustToNetFlow((Field) null));
            Assert.ThrowsException<ArgumentException>(() => "test".AdjustToNetFlow(-1));
        }

        [TestMethod]
        public void TestOnWireSize() {
            Assert.AreEqual(1, ((byte) 0).GetOnWireSize());
            Assert.AreEqual(2, ((short) 0).GetOnWireSize());
            Assert.AreEqual(2, ((ushort) 0).GetOnWireSize());
            Assert.AreEqual(4, ((int) 0).GetOnWireSize());
            Assert.AreEqual(4, ((uint) 0).GetOnWireSize());
            Assert.AreEqual(4, ((float) 0).GetOnWireSize());
            Assert.AreEqual(8, ((double) 0).GetOnWireSize());

            Assert.AreEqual(2 + 2, (new Field()).GetOnWireSize());
            Assert.AreEqual(2 + 2, (new Scope()).GetOnWireSize());
            Assert.AreEqual(2 * 2 + 4 * 4, (new PacketHeader()).GetOnWireSize());

            Assert.ThrowsException<ArgumentNullException>(() => ((Field) null).GetOnWireSize());
        }
    }
}
