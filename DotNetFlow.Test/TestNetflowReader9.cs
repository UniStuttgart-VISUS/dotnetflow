// <copyright file="TestNetflowReader9.cs" company="Universität Stuttgart">
// Copyright © 2020 - 2022 Institut für Visualisierung und Interaktive Systeme. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotNetFlow.Netflow9;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace DotNetFlow.Test {

    /// <summary>
    /// Unit tests for <see cref="NetflowReader"/> except for I/O operations.
    /// </summary>
    [TestClass]
    public sealed class TestNetflowReader9 {

        [TestMethod]
        public void TestInitialise() {
            {
                var t = new TemplateRecord(256);
                var r = new NetflowReader(new MemoryStream(), 1, new[] { t });
                Assert.IsNull(r.CurrentDataTemplates);

                Assert.IsNotNull(r.GetDataTemplates(1));
                Assert.IsTrue(r.GetDataTemplates(1).Any());
                Assert.IsTrue(r.GetDataTemplates(1).ContainsKey(t.ID));
                Assert.AreEqual(t, r.GetDataTemplates(1)[t.ID]);

                Assert.IsNotNull(r.GetDataTemplates(2));
                Assert.IsFalse(r.GetDataTemplates(2).Any());

                r.Dispose();
            }

            {
                var t = new TemplateRecord(256);
                var r = new NetflowReader(new BinaryReader(new MemoryStream()), 1, new[] { t });
                Assert.IsNull(r.CurrentDataTemplates);

                Assert.IsNotNull(r.GetDataTemplates(1));
                Assert.IsTrue(r.GetDataTemplates(1).Any());
                Assert.IsTrue(r.GetDataTemplates(1).ContainsKey(t.ID));
                Assert.AreEqual(t, r.GetDataTemplates(1)[t.ID]);

                Assert.IsNotNull(r.GetDataTemplates(2));
                Assert.IsFalse(r.GetDataTemplates(2).Any());

                r.Dispose();
            }

            {
                var t1 = new TemplateRecord(256);
                var t2 = new TemplateRecord(256);
                var r = new NetflowReader(new MemoryStream(),
                    new Dictionary<uint, IEnumerable<TemplateRecord>> {
                        { 1, new[] { t1 } },
                        { 2, new[] { t2 } }
                    });
                Assert.IsNull(r.CurrentDataTemplates);

                Assert.IsNotNull(r.GetDataTemplates(1));
                Assert.IsTrue(r.GetDataTemplates(1).Any());
                Assert.IsTrue(r.GetDataTemplates(1).ContainsKey(t1.ID));

                Assert.IsNotNull(r.GetDataTemplates(2));
                Assert.IsTrue(r.GetDataTemplates(2).Any());
                Assert.IsTrue(r.GetDataTemplates(2).ContainsKey(t2.ID));

                Assert.IsNotNull(r.GetDataTemplates(3));
                Assert.IsFalse(r.GetDataTemplates(3).Any());

                r.Dispose();
            }

            {
                var t1 = new TemplateRecord(256);
                var t2 = new TemplateRecord(256);
                var r = new NetflowReader(new BinaryReader(new MemoryStream()),
                    new Dictionary<uint, IEnumerable<TemplateRecord>> {
                        { 1, new[] { t1 } },
                        { 2, new[] { t2 } }
                    });
                Assert.IsNull(r.CurrentDataTemplates);

                Assert.IsNotNull(r.GetDataTemplates(1));
                Assert.IsTrue(r.GetDataTemplates(1).Any());
                Assert.IsTrue(r.GetDataTemplates(1).ContainsKey(t1.ID));

                Assert.IsNotNull(r.GetDataTemplates(2));
                Assert.IsTrue(r.GetDataTemplates(2).Any());
                Assert.IsTrue(r.GetDataTemplates(2).ContainsKey(t2.ID));

                Assert.IsNotNull(r.GetDataTemplates(3));
                Assert.IsFalse(r.GetDataTemplates(3).Any());

                r.Dispose();
            }

            {
                var r = new NetflowReader(new MemoryStream());
                Assert.IsNull(r.CurrentDataTemplates);

                Assert.IsNotNull(r.GetDataTemplates(1));
                Assert.IsFalse(r.GetDataTemplates(1).Any());

                r.Dispose();
            }
        }

        [TestMethod]
        public void TestInitialiseErrors() {
            Assert.ThrowsException<ArgumentNullException>(() => new NetflowReader((Stream) null));
            Assert.ThrowsException<ArgumentNullException>(() => new NetflowReader((BinaryReader) null));
        }

        [TestMethod]
        public void TestDispose() {
            var r = new NetflowReader(new MemoryStream());
            r.Dispose();
            Assert.ThrowsException<ObjectDisposedException>(() => r.ReadPacketHeader());
            Assert.IsNull(r.BaseStream);
        }
    }
}
