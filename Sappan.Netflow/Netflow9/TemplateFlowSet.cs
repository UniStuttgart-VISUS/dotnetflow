// <copyright file="TemplateFlowSet.cs" company="Universität Stuttgart">
// Copyright © 2020 SAPPAN Consortium. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace Sappan.Netflow.Netflow9 {

    /// <summary>
    /// Represents a template flow set describing the structure of the data.
    /// </summary>
    public sealed class TemplateFlowSet : IFlowSet {

        #region Public constructors
        /// <summary>
        /// Initialises a new instance.
        /// </summary>
        /// <param name="templateID">The ID of the template, which must be 256
        /// or larger by convention.</param>
        public TemplateFlowSet() => this.ID = 0;
        #endregion

        #region Public properties
        /// <summary>
        /// Gets or sets the FlowSet ID.
        /// </summary>
        /// <remarks>
        /// The FlowSet ID is used to distinguish template records from data
        /// records. A template record always has a FlowSet ID in the range of
        /// 0-255. Currently, the template record that describes flow fields has
        /// a FlowSet ID of 0 and the template record that describes option
        /// fields has a FlowSet ID of 1. A data record always has a non-zero
        /// FlowSet ID greater than 255.
        /// </remarks>
        [OnWireOrder(0)]
        public ushort ID {
            get => this._id;
            set {
                if (value >= 256) {
                    throw new ArgumentException(
                        Properties.Resources.ErrorWrongTemplateFlowSetID,
                        nameof(value));
                }
                this._id = value;
            }
        }

        /// <summary>
        /// Gets the total length of the FlowSet.
        /// </summary>
        /// <remarks>
        /// <para>As an individual template FlowSet may contain multiple
        /// template IDs, the length value should be used to determine the
        /// position of the next FlowSet record, which could be either a
        /// template or a data FlowSet.</para>
        /// <para>Length is expressed in Type/Length/Value (TLV) format,
        /// meaning that the value includes the bytes used for the FlowSet ID
        /// and the length bytes themselves, as well as the combined lengths of
        /// all template records included in this FlowSet.</para>
        /// <para>This property is computed by the <see cref="TemplateFlowSet"/>
        /// itself from its annotated fields and the <see cref="Records"/>
        /// property. When deserialising data, the length is only needed to
        /// compute offsets, but does not need to be stored explicitly.</para>
        /// </remarks>
        [OnWireOrder(1)]
        public ushort Length {
            get {
                Debug.Assert(this.Records != null);
                var retval = this.GetOnWireSize();

                if (retval > ushort.MaxValue) {
                    throw new InvalidOperationException(
                        Properties.Resources.ErrorLengthTooLarge);
                }

                return (ushort) retval;
            }
        }

        /// <summary>
        /// Gets the individual template records in this flow set.
        /// </summary>
        [OnWireOrder(2)]
        public IList<TemplateRecord> Records {
            get;
            internal set;
        } = new List<TemplateRecord>();
        #endregion

        #region Private fields
        /// <summary>
        /// Backing field for <see cref="ID"/>.
        /// </summary>
        private ushort _id;
        #endregion
    }
}
