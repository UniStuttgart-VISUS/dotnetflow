// <copyright file="OptionsTemplateRecord.cs" company="Universität Stuttgart">
// Copyright © 2020 - 2022 Institut für Visualisierung und Interaktive Systeme. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace DotNetFlow.Ipfix {

    /// <summary>
    /// Represents an entry in a <see cref="OptionsTemplateSet"/>.
    /// </summary>
    [OnWirePadding(32)]
    public sealed class OptionsTemplateRecord {

        #region Public constructors
        /// <summary>
        /// Initialises a new instance.
        /// </summary>
        /// <param name="id">The ID of the template.</param>
        public OptionsTemplateRecord(ushort id) => this.ID = id;
        #endregion

        #region Public properties
        /// <summary>
        /// Gets the total number of fields in the record (options and scopes).
        /// </summary>
        /// <remarks>
        /// This field gives the length (in bytes) of any Options field
        /// definitions contained in this options template.
        /// </remarks>
        [OnWireOrder(1)]
        public ushort FieldCount {
            get {
                Debug.Assert(this.Fields != null);
                var retval = this.Fields.Count + this.ScopeFieldCount;

                if (retval > ushort.MaxValue) {
                    throw new InvalidOperationException(
                        Properties.Resources.ErrorTooManyFields);
                }

                return (ushort) retval;
            }
        }

        /// <summary>
        /// Gets the option fields in the template.
        /// </summary>
        [OnWireOrder(4)]
        public List<FieldSpecifier> Fields {
            get;
            internal set;
        } = new List<FieldSpecifier>();

        /// <summary>
        /// Gets the template ID.
        /// </summary>
        /// <remarks>
        /// Each template record is given a unique template ID in the range 256
        /// to 65535. This uniqueness is local to the transport session and
        /// observation domain that generated the template ID. Since template
        /// IDs are used as set IDs in the sets they describe, values 0 - 255
        /// are reserved for special set types (e.g. template sets themselves),
        /// and templates and options templates cannot share template IDs within
        /// a transport session and observation domain. There are no constraints
        /// regarding the order of the template ID allocation. As exporting
        /// processes are free to allocate template IDs as they see fit,
        /// collecting processes MUST NOT assume incremental template IDs or
        /// anything about the contents of a template based on its template ID
        /// alone.
        /// </remarks>
        [OnWireOrder(0)]
        public ushort ID {
            get => this._id;
            set {
                if (value < 256) {
                    throw new ArgumentException(
                        Properties.Resources.ErrorWrongTemplateID,
                        nameof(value));
                }
                this._id = value;
            }
        }

        /// <summary>
        /// Gets the actual size of a record described by this template.
        /// </summary>
        public int ActualRecordSize => this.Fields.Sum(o => o.Length)
            + this.ScopeFields.Sum(s => s.Length);

        /// <summary>
        /// Gets the number of scope fields in the record.
        /// </summary>
        /// <remarks>
        /// <para>The scope fields are normal fields, except that they are
        /// interpreted as scope at the collector. A scope field count of N
        /// specifies that the first N field specifiers in the template record
        /// are scope fields. The scope field count MUST NOT be zero.</para>
        /// <para>Note that this implementation enforces the required order of
        /// fields. The user must only make sure to add the fields to the
        /// correct collection, i.e. <see cref="ScopeFields"/> for scope fields
        /// and <see cref="Fields"/> for all option fields.</para>
        /// </remarks>
        [OnWireOrder(2)]
        public ushort ScopeFieldCount {
            get {
                Debug.Assert(this.ScopeFields != null);
                var retval = this.ScopeFields.Count;

                if (retval > ushort.MaxValue) {
                    throw new InvalidOperationException(
                        Properties.Resources.ErrorTooManyFields);
                }

                return (ushort) retval;
            }
        }

        /// <summary>
        /// Gets the scope fields in the template.
        /// </summary>
        [OnWireOrder(3)]
        public List<FieldSpecifier> ScopeFields {
            get;
            internal set;
        } = new List<FieldSpecifier>();
        #endregion

        #region Private fields
        /// <summary>
        /// Backing field for <see cref="ID" />.
        /// </summary>
        private ushort _id;
        #endregion
    }
}
