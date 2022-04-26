// <copyright file="IpfixView.cs" company="Universität Stuttgart">
// Copyright © 2020 - 2022 Institut für Visualisierung und Interaktive Systeme. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;


namespace DotNetFlow.Ipfix {


    /// <summary>
    /// Provides a semantic interpretation of a <see cref="Data"/>
    /// based on the matching <see cref="Template"/>.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming",
        "CA1710:Identifiers should have correct suffix",
        Justification = "This is not a collection, but something that offers enumeration for convenience.")]
    public sealed class IpfixView : NetflowViewBase<DataSet, TemplateRecord,
            FieldSpecifier, InformationElement> {

        #region Public constructors
        /// <summary>
        /// Initialises a new instance.
        /// </summary>
        /// <param name="data">The <see cref="Data"/> to be interpreted.
        /// </param>
        /// <param name="template">The <see cref="Template"/> describing
        /// the data.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="data"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">If
        /// <paramref name="template"/> is <c>null</c>.</exception>
        public IpfixView(DataSet data, TemplateRecord template)
            : base(data, template) { }

        /// <summary>
        /// Initialises a new instance.
        /// </summary>
        /// <param name="data">The <see cref="Data"/> to be interpreted.
        /// </param>
        /// <param name="templates">A list of templates, which contains at least
        /// one that matches the ID of <paramref name="data"/>.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="data"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">If
        /// <paramref name="templates"/> is <c>null</c>.</exception>
        public IpfixView(DataSet data,
                IEnumerable<TemplateRecord> templates)
            : this(data, templates.Where(t => t.ID == data.ID)
                .FirstOrDefault()) { }

        /// <summary>
        /// Initialises a new instance.
        /// </summary>
        /// <param name="data">The <see cref="Data"/> to be interpreted.
        /// </param>
        /// <param name="templates">A template flow set containing at least one
        /// <see cref="TemplateRecord" /> that matches the ID of
        /// <paramref name="data"/>.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="data"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">If
        /// <paramref name="template"/> is <c>null</c>.</exception>
        public IpfixView(DataSet data, TemplateSet template)
            : this(data, template?.Records) { }
        #endregion

        #region Public indexers
        /// <summary>
        /// Gets a dynamic view of the <paramref name="record"/>th record.
        /// </summary>
        /// <param name="record">The zero-based index of the record to
        /// retrieve.</param>
        /// <returns>A dynamiv view of the specified record.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If
        /// <paramref name="record"/> is out of range.</exception>
        public override dynamic this[int record] {
            get {
                var index = this.GetIndex(record, 0);
                var retval = (IDictionary<string, object>) new ExpandoObject();

                for (var i = 0; i < this.Template.FieldCount; ++i) {
                    var field = this.Template.Fields[i];
                    var name = string.Empty;

                    if (field.IsEnterpriseNumber) {
                        name = $"Enterprise{field.EnterpriseNumber}";
                    } else {
                        name = field.InformationElement.ToString();
                    }

                    retval[name] = this.Data.Records[index + i];
                }

                return retval;
            }
        }
        #endregion

        #region Protected methods
        /// <inheritdoc />
        protected override void CheckValue(object value, int field) {
            if (value == null) {
                throw new ArgumentNullException(nameof(value));
            }
            if ((field < 0) || (field >= this.Template.FieldCount)) {
                throw new ArgumentOutOfRangeException(nameof(field));
            }

            if (value.GetOnWireSize() != this.Template.Fields[field].Length) {
                throw new ArgumentException(
                    Properties.Resources.ErrorWrongOnWireLength,
                    nameof(value));
            }
        }

        /// <inheritdoc />
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming",
            "CA1721:Property names should not match get methods",
            Justification = "Superclass needs this interface.")]
        protected override IList<FieldSpecifier> GetFields() {
            return this.Template.Fields;
        }

        /// <inheritdoc />
        protected override int GetIndex(InformationElement field) {
            int retval = 0;
            while (this.Template.Fields[retval].InformationElement != field) {
                ++retval;
            }

            Debug.Assert(retval >= 0);
            Debug.Assert(retval < this.Template.FieldCount);
            return retval;
        }

        /// <inheritdoc />
        public override IList<object> GetRecords() {
            return this.Data.Records;
        }
        #endregion
    }
}
