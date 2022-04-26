// <copyright file="OptionsTemplateFlowSet.cs" company="Universität Stuttgart">
// Copyright © 2020 - 2022 Institut für Visualisierung und Interaktive Systeme. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;


namespace DotNetFlow.Netflow9 {

    /// <summary>
    /// An options template (and its corresponding options data record).
    /// </summary>
    /// <remarks>
    ///  Rather than supplying information about IP flows, options are used to
    ///  supply &quot;meta-data&quot; about the NetFlow process itself.
    /// </remarks>
    [OnWirePadding(32)]
    public sealed class OptionsTemplateFlowSet : IFlowSet {

        #region Public constructors
        /// <summary>
        /// Initialises a new instance.
        /// </summary>
        public OptionsTemplateFlowSet() => this.ID = 1;
        #endregion

        #region Public properties
        /// <summary>
        /// Gets or sets the flow set ID.
        /// </summary>
        /// <remarks>
        /// The FlowSet ID is used to distinguish template records from data
        /// records. A template record always has a FlowSet ID of 1. A data
        /// record always has a nonzero FlowSet ID which is greater than 255.
        /// </remarks>
        [OnWireOrder(0)]
        public ushort ID {
            get => this._id;
            set {
                if (value >= 256) {
                    throw new ArgumentException(
                        Properties.Resources.ErrorWrongOptionsTemplateID,
                        nameof(value));
                }
                this._id = value;
            }
        }

        /// <summary>
        /// Gets the total length of the flow set.
        /// </summary>
        /// <remarks>
        /// <para>This field gives the total length of this FlowSet. Because an
        /// individual template FlowSet may contain multiple template IDs, the
        /// length value should be used to determine the position of the next
        /// FlowSet record, which could be either a template or a data FlowSet.
        /// </para>
        /// <para>Length is expressed in TLV format, meaning that the value
        /// includes the bytes used for the FlowSet ID and the length bytes
        /// themselves, as well as the combined lengths of all template records
        /// included in this FlowSet.</para>
        /// </remarks>
        [OnWireOrder(1)]
        public ushort Length {
            get {
                Debug.Assert(this.Records != null);
                var retval = this.GetOnWireSize();
                Debug.Assert(retval % 4 == 0);

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
        public IList<OptionsTemplateRecord> Records {
            get;
            internal set;
        } = new List<OptionsTemplateRecord>();
        #endregion

        #region Private fields
        /// <summary>
        /// Backing field for <see cref="ID"/>.
        /// </summary>
        private ushort _id;
        #endregion
    }
}
