// <copyright file="IpfixViewTest.cs" company="Universität Stuttgart">
// Copyright © 2020 - 2022 Institut für Visualisierung und Interaktive Systeme. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotNetFlow.Ipfix;
using System;
using System.Linq;
using System.Net;


namespace DotNetFlow.Test {

    /// <summary>
    /// Tests for <see cref="IpfixView"/>.
    /// </summary>
    [TestClass]
    public sealed class IpfixViewTest {

        [TestMethod]
        public void TestEnumerator() {
            var data = CreateTestData();
            var view = new IpfixView(data.Item2, data.Item1.Records[0]);
            var i = 0;

            foreach (dynamic r in view) {
                Assert.AreEqual(view[i, view.Template.Fields[0]], r.SourceIPv4Address);
                Assert.AreEqual(view[i, view.Template.Fields[1]], r.DestinationIPv4Address);
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
            var view = new IpfixView(data.Item2, data.Item1.Records);

            Assert.AreEqual(IPAddress.Parse("127.0.0.1"), view[0, view.Template.Fields[0]]);
            Assert.AreEqual(IPAddress.Parse("255.255.255.255"), view[0, view.Template.Fields[1]]);

            Assert.AreEqual(IPAddress.Parse("127.0.0.2"), view[1, view.Template.Fields[0]]);
            Assert.AreEqual(IPAddress.Parse("255.255.255.254"), view[1, view.Template.Fields[1]]);

            Assert.AreEqual(IPAddress.Parse("127.0.0.3"), view[2, view.Template.Fields[0]]);
            Assert.AreEqual(IPAddress.Parse("255.255.255.253"), view[2, view.Template.Fields[1]]);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => view[3, view.Template.Fields[0]]);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => view[0, view.Template.Fields[3]]);
            Assert.ThrowsException<ArgumentNullException>(() => view[0, (FieldSpecifier) null]);

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
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => view[0, new FieldSpecifier(InformationElement.IPNextHopIPv4Address)] = IPAddress.Any);
            Assert.ThrowsException<ArgumentNullException>(() => view[0, (FieldSpecifier) null] = IPAddress.Any);

            Assert.ThrowsException<ArgumentNullException>(() => view[0, view.Template.Fields[0]] = null);
            Assert.ThrowsException<ArgumentException>(() => view[0, view.Template.Fields[0]] = IPAddress.IPv6Any);
        }

        [TestMethod]
        public void TestFieldTypeIndexers() {
            var data = CreateTestData();
            var view = new IpfixView(data.Item2, data.Item1);

            Assert.AreEqual(IPAddress.Parse("127.0.0.1"), view[0, InformationElement.SourceIPv4Address]);
            Assert.AreEqual(IPAddress.Parse("255.255.255.255"), view[0, InformationElement.DestinationIPv4Address]);

            Assert.AreEqual(IPAddress.Parse("127.0.0.2"), view[1, InformationElement.SourceIPv4Address]);
            Assert.AreEqual(IPAddress.Parse("255.255.255.254"), view[1, InformationElement.DestinationIPv4Address]);

            Assert.AreEqual(IPAddress.Parse("127.0.0.3"), view[2, InformationElement.SourceIPv4Address]);
            Assert.AreEqual(IPAddress.Parse("255.255.255.253"), view[2, InformationElement.DestinationIPv4Address]);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => view[3, InformationElement.SourceIPv4Address]);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => view[0, InformationElement.DestinationIPv6Address]);

            view[0, InformationElement.SourceIPv4Address] = IPAddress.Parse("128.0.0.1");
            Assert.AreEqual(IPAddress.Parse("128.0.0.1"), view[0, InformationElement.SourceIPv4Address]);
            view[0, InformationElement.DestinationIPv4Address] = IPAddress.Parse("254.255.255.255");
            Assert.AreEqual(IPAddress.Parse("254.255.255.255"), view[0, InformationElement.DestinationIPv4Address]);

            view[1, InformationElement.SourceIPv4Address] = IPAddress.Parse("128.0.0.2");
            Assert.AreEqual(IPAddress.Parse("128.0.0.2"), view[1, InformationElement.SourceIPv4Address]);
            view[1, InformationElement.DestinationIPv4Address] = IPAddress.Parse("254.255.255.254");
            Assert.AreEqual(IPAddress.Parse("254.255.255.254"), view[1, InformationElement.DestinationIPv4Address]);

            view[2, InformationElement.SourceIPv4Address] = IPAddress.Parse("128.0.0.3");
            Assert.AreEqual(IPAddress.Parse("128.0.0.3"), view[2, InformationElement.SourceIPv4Address]);
            view[2, InformationElement.DestinationIPv4Address] = IPAddress.Parse("254.255.255.253");
            Assert.AreEqual(IPAddress.Parse("254.255.255.253"), view[2, InformationElement.DestinationIPv4Address]);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => view[3, InformationElement.SourceIPv4Address] = IPAddress.Any);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => view[0, InformationElement.DestinationIPv6Address] = IPAddress.Any);

            Assert.ThrowsException<ArgumentNullException>(() => view[0, InformationElement.SourceIPv4Address] = null);
            Assert.ThrowsException<ArgumentException>(() => view[0, InformationElement.SourceIPv4Address] = IPAddress.IPv6Any);
        }

        [TestMethod]
        public void TestInitialiser() {
            {
                var template = new TemplateRecord(256);
                var data = new DataSet(256);
                var view = new IpfixView(data, template);
                Assert.AreSame(data, view.Data);
                Assert.AreSame(template, view.Template);
            }

            Assert.ThrowsException<ArgumentNullException>(() => new IpfixView(null, new TemplateRecord(256)));
            Assert.ThrowsException<ArgumentNullException>(() => new IpfixView(new DataSet(), (TemplateRecord) null));
        }

        [TestMethod]
        public void TestNumericIndexers() {
            var data = CreateTestData();
            var view = new IpfixView(data.Item2, data.Item1.Records[0]);

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
            var view = new IpfixView(data.Item2, data.Item1.Records);

            Assert.AreEqual(3, view.Count);
            Assert.AreEqual(data.Item1.Records[0].Fields[0], view.Fields.First());
            Assert.AreEqual(data.Item1.Records[0].Fields[1], view.Fields.Last());
        }

        [TestMethod]
        public void TestRecordIndexer() {
            var data = CreateTestData();
            var view = new IpfixView(data.Item2, data.Item1);

            {
                var record = view[0];
                Assert.AreEqual(IPAddress.Parse("127.0.0.1"), record.SourceIPv4Address);
                Assert.AreEqual(IPAddress.Parse("255.255.255.255"), record.DestinationIPv4Address);
            }

            {
                var record = view[1];
                Assert.AreEqual(IPAddress.Parse("127.0.0.2"), record.SourceIPv4Address);
                Assert.AreEqual(IPAddress.Parse("255.255.255.254"), record.DestinationIPv4Address);
            }

            {
                var record = view[2];
                Assert.AreEqual(IPAddress.Parse("127.0.0.3"), record.SourceIPv4Address);
                Assert.AreEqual(IPAddress.Parse("255.255.255.253"), record.DestinationIPv4Address);
            }

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => view[3]);
        }


        private static (TemplateSet, DataSet) CreateTestData() {
            var record = new TemplateRecord(256);
            record.Fields.Add(new FieldSpecifier(InformationElement.SourceIPv4Address));
            record.Fields.Add(new FieldSpecifier(InformationElement.DestinationIPv4Address));

            var template = new TemplateSet();
            template.Records.Add(record);

            var data = new DataSet(256);
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
