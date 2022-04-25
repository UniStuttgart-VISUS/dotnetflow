// <copyright file="ConversionTest.cs" company="Universität Stuttgart">
// Copyright © 2020 SAPPAN Consortium. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sappan.Netflow.Netflow9;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Sappan.Netflow.Test {

    /// <summary>
    /// Performs tests of conversion from and to on-wire formats.
    /// </summary>
    [TestClass]
    public sealed class ConversionTest {

        [TestMethod]
        public void TestComplexToWire() {
            {
                var expected = new[] { (byte) 'H', (byte) 'o', (byte) 'r', (byte) 's', (byte) 't' };
                var actual = "Horst".ToWire();
                Assert.IsTrue(Enumerable.SequenceEqual(expected, actual));
            }

            {
                var expected = new[] {
                    (byte) 0x12, (byte) 0x34, (byte) 0x56, (byte) 0x78,
                    (byte) 0x87, (byte) 0x65, (byte) 0x43, (byte) 0x21
                };
                var actual = (new uint[] { 0x12345678, 0x87654321 }).ToWire();
                Assert.IsTrue(Enumerable.SequenceEqual(expected, actual));
            }

            {
                var expected = new[] {
                    (byte) 0x00, (byte) 0x08,
                    (byte) 0x00, (byte) 0x04
                };
                var actual = (new Field(FieldType.IPv4SourceAddress)).ToWire();
                Assert.IsTrue(Enumerable.SequenceEqual(expected, actual));
            }
        }

        [TestMethod]
        public void TestFromWrireErrorHandling() {
            {
                int offset = 0, length = 0;
                Assert.ThrowsException<ArgumentNullException>(() => ((Type) null).FromWire(Array.Empty<byte>(), ref offset, ref length));
            }

            {
                int offset = 0, length = 0;
                Assert.ThrowsException<ArgumentNullException>(() => typeof(int).FromWire(null, ref offset, ref length));
            }

            {
                int offset = 0, length = 0;
                Assert.ThrowsException<ArgumentException>(() => typeof(int).FromWire(Array.Empty<byte>(), ref offset, ref length));
            }

            {
                int offset = 1, length = 0;
                Assert.ThrowsException<ArgumentException>(() => typeof(int).FromWire(new byte[4], ref offset, ref length));
            }

            {
                int offset = -1, length = 0;
                Assert.ThrowsException<ArgumentException>(() => typeof(int).FromWire(new byte[4], ref offset, ref length));
            }

            {
                int result = 0;
                Assert.ThrowsException<ArgumentNullException>(() => OnWireExtensions.FromWire(ref result, null));
            }

            {
                int result = 0;
                Assert.ThrowsException<ArgumentException>(() => OnWireExtensions.FromWire(ref result, new byte[4], -1));
            }

            {
                int result = 0;
                Assert.ThrowsException<ArgumentException>(() => OnWireExtensions.FromWire(ref result, new byte[4], 1));
            }

            {
                var result = string.Empty;
                Assert.ThrowsException<ArgumentException>(() => OnWireExtensions.FromWire(ref result, new byte[4], 1, 4));
            }

            {
                int offset = 0, length = 0;
                Assert.IsNull(typeof(object).FromWire(new byte[4], ref offset, ref length));
            }
        }

        [TestMethod]
        public void TestTrivialFromWire() {
            {
                var expected = (byte) 1;
                var actual = (byte) 0;
                var offset = 0;
                var length = int.MaxValue;
                var consumed = OnWireExtensions.FromWire(ref actual, expected.ToWire(), ref offset, ref length);
                Assert.AreEqual(expected, actual);
                Assert.AreEqual(1, offset);
                Assert.AreEqual(0, length);
                Assert.AreEqual(1, consumed);
            }

            {
                var expected = (byte) 1;
                var actual = (byte) 0;
                var input = new[] { (byte) 0 }.Concat(expected.ToWire()).ToArray();
                var consumed = OnWireExtensions.FromWire(ref actual, input, 1);
                Assert.AreEqual(expected, actual);
                Assert.AreEqual(1, consumed);
            }

            {
                var expected = short.MaxValue;
                var actual = (short) 0;
                var offset = 0;
                var length = int.MaxValue;
                var consumed = OnWireExtensions.FromWire(ref actual, expected.ToWire(), ref offset, ref length);
                Assert.AreEqual(expected, actual);
                Assert.AreEqual(2, offset);
                Assert.AreEqual(0, length);
                Assert.AreEqual(2, consumed);
            }

            {
                var expected = short.MaxValue;
                var actual = (short) 0;
                var input = new[] { (byte) 0 }.Concat(expected.ToWire()).ToArray();
                var consumed = OnWireExtensions.FromWire(ref actual, input, 1);
                Assert.AreEqual(expected, actual);
                Assert.AreEqual(2, consumed);
            }

            {
                var expected = ushort.MaxValue;
                var actual = (ushort) 0;
                var offset = 0;
                var length = int.MaxValue;
                var consumed = OnWireExtensions.FromWire(ref actual, expected.ToWire(), ref offset, ref length);
                Assert.AreEqual(expected, actual);
                Assert.AreEqual(2, offset);
                Assert.AreEqual(0, length);
                Assert.AreEqual(2, consumed);
            }

            {
                var expected = int.MaxValue;
                var actual = (int) 0;
                var offset = 0;
                var length = int.MaxValue;
                var consumed = OnWireExtensions.FromWire(ref actual, expected.ToWire(), ref offset, ref length);
                Assert.AreEqual(expected, actual);
                Assert.AreEqual(4, offset);
                Assert.AreEqual(0, length);
                Assert.AreEqual(4, consumed);
            }

            {
                var expected = uint.MaxValue;
                var actual = (uint) 0;
                var offset = 0;
                var length = int.MaxValue;
                var consumed = OnWireExtensions.FromWire(ref actual, expected.ToWire(), ref offset, ref length);
                Assert.AreEqual(expected, actual);
                Assert.AreEqual(4, offset);
                Assert.AreEqual(0, length);
                Assert.AreEqual(4, consumed);
            }

            {
                var expected = long.MaxValue;
                var actual = (long) 0;
                var offset = 0;
                var length = int.MaxValue;
                var consumed = OnWireExtensions.FromWire(ref actual, expected.ToWire(), ref offset, ref length);
                Assert.AreEqual(expected, actual);
                Assert.AreEqual(8, offset);
                Assert.AreEqual(0, length);
                Assert.AreEqual(8, consumed);
            }

            {
                var expected = ulong.MaxValue;
                var actual = (ulong) 0;
                var offset = 0;
                var length = int.MaxValue;
                var consumed = OnWireExtensions.FromWire(ref actual, expected.ToWire(), ref offset, ref length);
                Assert.AreEqual(expected, actual);
                Assert.AreEqual(8, offset);
                Assert.AreEqual(0, length);
                Assert.AreEqual(8, consumed);
            }

            {
                var expected = 1.0f;
                var actual = 0.0f;
                var offset = 0;
                var length = int.MaxValue;
                var consumed = OnWireExtensions.FromWire(ref actual, expected.ToWire(), ref offset, ref length);
                Assert.AreEqual(expected, actual);
                Assert.AreEqual(4, offset);
                Assert.AreEqual(0, length);
                Assert.AreEqual(4, consumed);
            }

            {
                var expected = 1.0;
                var actual = 0.0;
                var offset = 0;
                var length = int.MaxValue;
                var consumed = OnWireExtensions.FromWire(ref actual, expected.ToWire(), ref offset, ref length);
                Assert.AreEqual(expected, actual);
                Assert.AreEqual(8, offset);
                Assert.AreEqual(0, length);
                Assert.AreEqual(8, consumed);
            }

            {
                var data = new[] { (byte) 'H', (byte) 'o', (byte) 'r', (byte) 's', (byte) 't' };
                var actual = (string) null;
                var consumed = OnWireExtensions.FromWire(ref actual, data);
                Assert.AreEqual("Horst", actual);
                Assert.AreEqual(data.Length, consumed);
            }

            {
                var data = new[] { (byte) 'H', (byte) 'o', (byte) 'r', (byte) 's', (byte) 't' };
                var actual = (string) null;
                var offset = 0;
                var length = 0;
                var consumed = OnWireExtensions.FromWire(ref actual, data, ref offset, ref length);
                Assert.AreEqual("Horst", actual);
                Assert.AreEqual(data.Length, offset);
                Assert.AreEqual(0, length);
                Assert.AreEqual(data.Length, consumed);
            }

            {
                var expected = long.MaxValue;
                var offset = 0;
                var length = 0;
                var actual = OnWireExtensions.FromWire(expected.GetType(), expected.ToWire(), ref offset, ref length);
                Assert.AreEqual(expected, actual);
                Assert.AreEqual(8, offset);
                Assert.AreEqual(0, length);
            }

            {
                var expected = FieldType.ApplicationDescription;
                var offset = 0;
                var length = 0;
                var actual = (FieldType) OnWireExtensions.FromWire(expected.GetType(), expected.ToWire(), ref offset, ref length);
                Assert.AreEqual(expected, actual);
                Assert.AreEqual(sizeof(FieldType), offset);
                Assert.AreEqual(0, length);
            }

            {
                // Note: Field is a complex type and we do not support automatic decoding of complex types.
                var orginial = new Field(FieldType.BgpIPv4NextHop);
                var offset = 0;
                var length = 0;
                var input = orginial.ToWire();
                var actual = OnWireExtensions.FromWire(orginial.GetType(), input, ref offset, ref length);
                Assert.IsNull(actual);
                Assert.AreEqual(0, offset);
                Assert.AreEqual(4, length);
            }
        }

        [TestMethod]
        public void TestToWrireErrorHandling() {
            Assert.ThrowsException<ArgumentNullException>(() => ((string) null).ToWire());
            Assert.ThrowsException<ArgumentNullException>(() => ((IEnumerable) null).ToWire());
            Assert.ThrowsException<ArgumentNullException>(() => ((object) null).ToWire());
            Assert.ThrowsException<ArgumentException>(() => (new object()).ToWire());
        }

        [TestMethod]
        public void TestTrivialToWire() {
            {
                var expected = new[] { (byte) 1 };
                var actual = ((byte) 1).ToWire();
                Assert.IsTrue(Enumerable.SequenceEqual(expected, actual));
            }

            {
                var expected = new[] { (byte) 0, (byte) 16 };
                var actual = ((ushort) 16).ToWire();
                Assert.IsTrue(Enumerable.SequenceEqual(expected, actual));
            }

            {
                var expected = new[] { (byte) 1, (byte) 0 };
                var actual = ((ushort) 256).ToWire();
                Assert.IsTrue(Enumerable.SequenceEqual(expected, actual));
            }

            {
                var expected = new[] { (byte) 0xff, (byte) 0xff };
                var actual = ((ushort) 0xffff).ToWire();
                Assert.IsTrue(Enumerable.SequenceEqual(expected, actual));
            }

            {
                var expected = new[] { (byte) 0x7f, (byte) 0xff };
                var actual = ((short) 32767).ToWire();
                Assert.IsTrue(Enumerable.SequenceEqual(expected, actual));
            }

            {
                var expected = new[] { (byte) 0xff, (byte) 0xff };
                var actual = ((short) -1).ToWire();
                Assert.IsTrue(Enumerable.SequenceEqual(expected, actual));
            }

            {
                var expected = new[] { (byte) 0x12, (byte) 0x34, (byte) 0x56, (byte) 0x78 };
                var actual = ((int) 0x12345678).ToWire();
                Assert.IsTrue(Enumerable.SequenceEqual(expected, actual));
            }

            {
                var expected = new[] { (byte) 0x80, (byte) 0x00, (byte) 0x00, (byte) 0x00 };
                var actual = ((uint) int.MaxValue + 1).ToWire();
                Assert.IsTrue(Enumerable.SequenceEqual(expected, actual));
            }

            {
                var expected = new[] {
                    (byte) 0x12, (byte) 0x34, (byte) 0x56, (byte) 0x78,
                    (byte) 0x9A, (byte) 0xBC, (byte) 0xDE, (byte) 0xF0
                };
                var actual = (0x123456789abcdef0L).ToWire();
                Assert.IsTrue(Enumerable.SequenceEqual(expected, actual));
            }

            {
                var expected = new[] {
                    (byte) 0x80, (byte) 0x00, (byte) 0x00, (byte) 0x00,
                    (byte) 0x00, (byte) 0x00, (byte) 0x00, (byte) 0x00
                };
                var actual = ((ulong) long.MaxValue + 1).ToWire();
                Assert.IsTrue(Enumerable.SequenceEqual(expected, actual));
            }

            {
                var expected = BitConverter.GetBytes(1.0f);
                var actual = 1.0f.ToWire();
                Assert.IsTrue(Enumerable.SequenceEqual(expected, actual));
            }

            {
                var expected = BitConverter.GetBytes(1.0);
                var actual = 1.0.ToWire();
                Assert.IsTrue(Enumerable.SequenceEqual(expected, actual));
            }
        }

    }
}
