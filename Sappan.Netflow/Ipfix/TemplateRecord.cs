// <copyright file="TemplateRecord.cs" company="Universität Stuttgart">
// Copyright © 2020 SAPPAN Consortium. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace Sappan.Netflow.Ipfix {

    /// <summary>
    /// A record in a <see cref="TemplateSet"/>.
    /// </summary>
    public sealed class TemplateRecord {

        #region Public constructors
        /// <summary>
        /// Initialises a new instance.
        /// </summary>
        /// <param name="id">The ID of the template, which must be 256 or larger
        /// by convention.</param>
        public TemplateRecord(ushort id) => this.ID = id;
        #endregion

        #region Public properties
        /// <summary>
        /// Gets the number of fields in this template record.
        /// </summary>
        [OnWireOrder(1)]
        public ushort FieldCount {
            get {
                Debug.Assert(this.Fields != null);
                var retval = this.Fields.Count;
                if (retval > ushort.MaxValue) {
                    throw new InvalidOperationException(
                        Properties.Resources.ErrorLengthTooLarge);
                }
                return (ushort) retval;
            }
        }

        /// <summary>
        /// Gets the fields in the template.
        /// </summary>
        [OnWireOrder(2)]
        public IList<FieldSpecifier> Fields {
            get;
            internal set;
        } = new List<FieldSpecifier>();

        /// <summary>
        /// Gets or sets the template ID.
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
        #endregion

        #region Private fields
        /// <summary>
        /// Backing field for <see cref="ID"/>.
        /// </summary>
        private ushort _id;
        #endregion
    }
}
