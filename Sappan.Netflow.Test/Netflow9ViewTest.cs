// <copyright file="Netflow9ViewTest.cs" company="Universität Stuttgart">
// Copyright © 2020 SAPPAN Consortium. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sappan.Netflow.Netflow9;
using System;
using System.Linq;
using System.Net;


namespace Sappan.Netflow.Test {

    /// <summary>
    /// Tests for <see cref="NetflowView"/>.
    /// </summary>
    [TestClass]
    public sealed class Netflow9ViewTest {

        [TestMethod]
        public void TestEnumerator() {
            var data = CreateTestData();
            var view = new NetflowView(data.Item2, data.Item1.Records[0]);
            var i = 0;

            foreach (dynamic r in view) {
                Assert.AreEqual(view[i, view.Template.Fields[0]], r.IPv4SourceAddress);
                Assert.AreEqual(view[i, view.Template.Fields[1]], r.IPv4DestinationAddress);
                ++i;
            }

            {
                var e = view.GetEnumerator();
                Assert.IsNotNull(e);
                Assert.IsTrue(e.MoveNext());
                Assert.IsTrue(e.MoveNext());
                Assert.IsTrue(e.MoveNext());
                Assert.IsFalse(e.MoveNext());
                Assert.ThrowsException<InvalidOperationException>(() => e.Current);
                e.Dispose();
                Assert.ThrowsException<ObjectDisposedException>(() => e.Current);
            }
        }

        [TestMethod]
        public void TestFieldIndexer() {
            var data = CreateTestData();
            var view = new NetflowView(data.Item2, data.Item1.Records);

            Assert.AreEqual(IPAddress.Parse("127.0.0.1"), view[0, view.Template.Fields[0]]);
            Assert.AreEqual(IPAddress.Parse("255.255.255.255"), view[0, view.Template.Fields[1]]);

            Assert.AreEqual(IPAddress.Parse("127.0.0.2"), view[1, view.Template.Fields[0]]);
            Assert.AreEqual(IPAddress.Parse("255.255.255.254"), view[1, view.Template.Fields[1]]);

            Assert.AreEqual(IPAddress.Parse("127.0.0.3"), view[2, view.Template.Fields[0]]);
            Assert.AreEqual(IPAddress.Parse("255.255.255.253"), view[2, view.Template.Fields[1]]);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => view[3, view.Template.Fields[0]]);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => view[0, view.Template.Fields[3]]);
            Assert.ThrowsException<ArgumentNullException>(() => view[0, (Field) null]);

            view[0, view.Template.Fields[0]] = IPAddress.Parse("128.0.0.1");
            Assert.AreEqual(IPAddress.Parse("128.0.0.1"), view[0, view.Template.Fields[0]]);
            view[0, view.Template.Fields[1]] = IPAddress.Parse("254.255.255.255");
            Assert.AreEqual(IPAddress.Parse("254.255.255.255"), view[0, view.Template.Fields[1]]);

            view[1, view.Template.Fields[0]] = IPAddress.Parse("128.0.0.2");
            Assert.AreEqual(IPAddress.Parse("128.0.0.2"), view[1, view.Template.Fields[0]]);
            view[1, view.Template.Fields[1]] = IPAddress.Parse("254.255.255.254");
            Assert.AreEqual(IPAddress.Parse("254.255.255.254"), view[1, view.Template.Fields[1]]);

            view[2, view.Template.Fields[0]] = IPAddress.Parse("128.0.0.3");
            Assert.AreEqual(IPAddress.Parse("128.0.0.3"), view[2, view.Template.Fields[0]]);
            view[2, view.Template.Fields[1]] = IPAddress.Parse("254.255.255.253");
            Assert.AreEqual(IPAddress.Parse("254.255.255.253"), view[2, view.Template.Fields[1]]);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => view[3, view.Template.Fields[0]] = IPAddress.Any);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => view[0, new Field(FieldType.BgpIPv4NextHop)] = IPAddress.Any);
            Assert.ThrowsException<ArgumentNullException>(() => view[0, (Field) null] = IPAddress.Any);

            Assert.ThrowsException<ArgumentNullException>(() => view[0, view.Template.Fields[0]] = null);
            Assert.ThrowsException<ArgumentException>(() => view[0, view.Template.Fields[0]] = IPAddress.IPv6Any);
        }

        [TestMethod]
        public void TestFieldTypeIndexers() {
            var data = CreateTestData();
            var view = new NetflowView(data.Item2, data.Item1);

            Assert.AreEqual(IPAddress.Parse("127.0.0.1"), view[0, FieldType.IPv4SourceAddress]);
            Assert.AreEqual(IPAddress.Parse("255.255.255.255"), view[0, FieldType.IPv4DestinationAddress]);

            Assert.AreEqual(IPAddress.Parse("127.0.0.2"), view[1, FieldType.IPv4SourceAddress]);
            Assert.AreEqual(IPAddress.Parse("255.255.255.254"), view[1, FieldType.IPv4DestinationAddress]);

            Assert.AreEqual(IPAddress.Parse("127.0.0.3"), view[2, FieldType.IPv4SourceAddress]);
            Assert.AreEqual(IPAddress.Parse("255.255.255.253"), view[2, FieldType.IPv4DestinationAddress]);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => view[3, FieldType.IPv4SourceAddress]);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => view[0, FieldType.IPv6DestinationAddress]);

            view[0, FieldType.IPv4SourceAddress] = IPAddress.Parse("128.0.0.1");
            Assert.AreEqual(IPAddress.Parse("128.0.0.1"), view[0, FieldType.IPv4SourceAddress]);
            view[0, FieldType.IPv4DestinationAddress] = IPAddress.Parse("254.255.255.255");
            Assert.AreEqual(IPAddress.Parse("254.255.255.255"), view[0, FieldType.IPv4DestinationAddress]);

            view[1, FieldType.IPv4SourceAddress] = IPAddress.Parse("128.0.0.2");
            Assert.AreEqual(IPAddress.Parse("128.0.0.2"), view[1, FieldType.IPv4SourceAddress]);
            view[1, FieldType.IPv4DestinationAddress] = IPAddress.Parse("254.255.255.254");
            Assert.AreEqual(IPAddress.Parse("254.255.255.254"), view[1, FieldType.IPv4DestinationAddress]);

            view[2, FieldType.IPv4SourceAddress] = IPAddress.Parse("128.0.0.3");
            Assert.AreEqual(IPAddress.Parse("128.0.0.3"), view[2, FieldType.IPv4SourceAddress]);
            view[2, FieldType.IPv4DestinationAddress] = IPAddress.Parse("254.255.255.253");
            Assert.AreEqual(IPAddress.Parse("254.255.255.253"), view[2, FieldType.IPv4DestinationAddress]);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => view[3, FieldType.IPv4SourceAddress] = IPAddress.Any);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => view[0, FieldType.IPv6DestinationAddress] = IPAddress.Any);

            Assert.ThrowsException<ArgumentNullException>(() => view[0, FieldType.IPv4SourceAddress] = null);
            Assert.ThrowsException<ArgumentException>(() => view[0, FieldType.IPv4SourceAddress] = IPAddress.IPv6Any);
        }

        [TestMethod]
        public void TestInitialiser() {
            {
                var template = new TemplateRecord(256);
                var data = new DataFlowSet(256);
                var view = new NetflowView(data, template);
                Assert.AreSame(data, view.Data);
                Assert.AreSame(template, view.Template);
            }

            Assert.ThrowsException<ArgumentNullException>(() => new NetflowView(null, new TemplateRecord(256)));
            Assert.ThrowsException<ArgumentNullException>(() => new NetflowView(new DataFlowSet(), (TemplateRecord) null));
        }

        [TestMethod]
        public void TestNumericIndexers() {
            var data = CreateTestData();
            var view = new NetflowView(data.Item2, data.Item1.Records[0]);

            Assert.AreEqual(IPAddress.Parse("127.0.0.1"), view[0, 0]);
            Assert.AreEqual(IPAddress.Parse("255.255.255.255"), view[0, 1]);

            Assert.AreEqual(IPAddress.Parse("127.0.0.2"), view[1, 0]);
            Assert.AreEqual(IPAddress.Parse("255.255.255.254"), view[1, 1]);

            Assert.AreEqual(IPAddress.Parse("127.0.0.3"), view[2, 0]);
            Assert.AreEqual(IPAddress.Parse("255.255.255.253"), view[2, 1]);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => view[3, 0]);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => view[0, 3]);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => view[-1, 0]);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => view[0, -1]);

            view[0, 0] = IPAddress.Parse("128.0.0.1");
            Assert.AreEqual(IPAddress.Parse("128.0.0.1"), view[0, 0]);
            view[0, 1] = IPAddress.Parse("254.255.255.255");
            Assert.AreEqual(IPAddress.Parse("254.255.255.255"), view[0, 1]);

            view[1, 0] = IPAddress.Parse("128.0.0.2");
            Assert.AreEqual(IPAddress.Parse("128.0.0.2"), view[1, 0]);
            view[1, 1] = IPAddress.Parse("254.255.255.254");
            Assert.AreEqual(IPAddress.Parse("254.255.255.254"), view[1, 1]);

            view[2, 0] = IPAddress.Parse("128.0.0.3");
            Assert.AreEqual(IPAddress.Parse("128.0.0.3"), view[2, 0]);
            view[2, 1] = IPAddress.Parse("254.255.255.253");
            Assert.AreEqual(IPAddress.Parse("254.255.255.253"), view[2, 1]);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => view[3, 0] = IPAddress.Any);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => view[0, 3] = IPAddress.Any);

            Assert.ThrowsException<ArgumentNullException>(() => view[0, 0] = null);
            Assert.ThrowsException<ArgumentException>(() => view[0, 0] = IPAddress.IPv6Any);
        }

        [TestMethod]
        public void TestProperties() {
            var data = CreateTestData();
            var view = new NetflowView(data.Item2, data.Item1.Records);

            Assert.AreEqual(3, view.Count);
            Assert.AreEqual(data.Item1.Records[0].Fields[0], view.Fields.First());
            Assert.AreEqual(data.Item1.Records[0].Fields[1], view.Fields.Last());
        }

        [TestMethod]
        public void TestRecordIndexer() {
            var data = CreateTestData();
            var view = new NetflowView(data.Item2, data.Item1);

            {
                var record = view[0];
                Assert.AreEqual(IPAddress.Parse("127.0.0.1"), record.IPv4SourceAddress);
                Assert.AreEqual(IPAddress.Parse("255.255.255.255"), record.IPv4DestinationAddress);
            }

            {
                var record = view[1];
                Assert.AreEqual(IPAddress.Parse("127.0.0.2"), record.IPv4SourceAddress);
                Assert.AreEqual(IPAddress.Parse("255.255.255.254"), record.IPv4DestinationAddress);
            }

            {
                var record = view[2];
                Assert.AreEqual(IPAddress.Parse("127.0.0.3"), record.IPv4SourceAddress);
                Assert.AreEqual(IPAddress.Parse("255.255.255.253"), record.IPv4DestinationAddress);
            }

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => view[3]);
        }


        private static (TemplateFlowSet, DataFlowSet) CreateTestData() {
            var record = new TemplateRecord(256);
            record.Fields.Add(new Field(FieldType.IPv4SourceAddress));
            record.Fields.Add(new Field(FieldType.IPv4DestinationAddress));

            var template = new TemplateFlowSet();
            template.Records.Add(record);

            var data = new DataFlowSet(256);
            data.Records.Add(IPAddress.Parse("127.0.0.1"));
            data.Records.Add(IPAddress.Parse("255.255.255.255"));

            data.Records.Add(IPAddress.Parse("127.0.0.2"));
            data.Records.Add(IPAddress.Parse("255.255.255.254"));

            data.Records.Add(IPAddress.Parse("127.0.0.3"));
            data.Records.Add(IPAddress.Parse("255.255.255.253"));

            return (template, data);
        }
    }
}
