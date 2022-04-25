// <copyright file="OptionsDataRecord.cs" company="Universität Stuttgart">
// Copyright © 2020 SAPPAN Consortium. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace Sappan.Netflow.Netflow9 {

    /// <summary>
    /// Representation of a so-called &quot;options data record&quot;, which is
    /// a data flow set that is described by a
    /// <see cref="OptionsTemplateFlowSet"/> rather than a standard template
    /// flow set.
    /// </summary>
    [OnWirePadding(32)]
    public sealed class OptionsDataFlowSet : IFlowSet {

        #region Public constructors
        /// <summary>
        /// Initialises a new instance.
        /// </summary>
        /// <param name="id">The ID of the flow set (which is equivalent to the
        /// ID of the options data template describing the structure).</param>
        public OptionsDataFlowSet(ushort id) => this.ID = id;
        #endregion

        #region Public properties
        /// <summary>
        /// Gets or sets the flow set ID of the template that is used.
        /// </summary>
        /// <remarks>
        /// A FlowSet ID precedes each group of records within a NetFlow
        /// version 9 data FlowSet. The FlowSet ID maps to a (previously
        /// received) template ID. The collector and display applications
        /// should use the FlowSet ID to map the appropriate type and length
        /// to any field values that follow.
        /// </remarks>
        [OnWireOrder(0)]
        public ushort ID {
            get => this._id;
            set {
                if (value < 256) {
                    throw new ArgumentException(
                        Properties.Resources.ErrorWrongDataFlowSetID,
                        nameof(value));
                }
                this._id = value;
            }
        }

        /// <summary>
        /// Gets the length of the data flow set.
        /// </summary>
        /// <remarks>
        /// <para>Length is expressed in TLV format, meaning that the value
        /// includes the bytes used for the FlowSet ID and the length bytes
        /// themselves, as well as the combined lengths of any included data
        /// records.</para>
        /// </remarks>
        [OnWireOrder(1)]
        public ushort Length {
            get {
                Debug.Assert(this.Options != null);
                Debug.Assert(this.Scopes != null);
                var retval = this.GetOnWireSize();

                if (retval > ushort.MaxValue) {
                    throw new InvalidOperationException(
                        Properties.Resources.ErrorLengthTooLarge);
                }

                return (ushort) retval;
            }
        }

        /// <summary>
        /// Gets the option values.
        /// </summary>
        [OnWireOrder(3)]
        public IList<object> Options {
            get;
            internal set;
        } = new List<object>();

        /// <summary>
        /// Gets the option scopes.
        /// </summary>
        [OnWireOrder(2)]
        public IList<object> Scopes {
            get;
            internal set;
        } = new List<object>();
        #endregion

        #region Internal constructors
        /// <summary>
        /// Initialises a new instance.
        /// </summary>
        internal OptionsDataFlowSet() : this(256) { }
        #endregion

        #region Private fields
        /// <summary>
        /// Backing field for <see cref="ID"/>.
        /// </summary>
        private ushort _id;
        #endregion
    }
}
