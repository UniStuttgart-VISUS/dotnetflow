// <copyright file="TemplateRecord.cs" company="Universität Stuttgart">
// Copyright © 2020 - 2022 Institut für Visualisierung und Interaktive Systeme. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace DotNetFlow.Netflow9 {

    /// <summary>
    /// Represents a single template in a <see cref="TemplateRecord"/>.
    /// </summary>
    public sealed class TemplateRecord {

        #region Public constructors
        /// <summary>
        /// Initialises a new instance.
        /// </summary>
        /// <param name="id">The ID of the template, which must be 256 or larger
        /// by convention.</param>
        public TemplateRecord(ushort id) => (this.ID) = id;
        #endregion

        #region Public properties
        /// <summary>
        /// Gets the number of fields in this template record.
        /// </summary>
        /// <remarks>
        /// As a template FlowSet may contain multiple template records, this
        /// field allows the parser to determine the end of the current template
        /// record and the start of the next.
        /// </remarks>
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
        public IList<Field> Fields {
            get;
            internal set;
        } = new List<Field>();

        /// <summary>
        /// Gets or sets the template ID.
        /// </summary>
        /// <remarks>
        /// As a router generates different template FlowSets to match the type
        /// of NetFlow data it will be exporting, each template is given a
        /// unique ID. This uniqueness is local to the router that generated the
        /// template ID. Templates that define data record formats begin
        /// numbering at 256 since 0-255 are reserved for FlowSet IDs.
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
