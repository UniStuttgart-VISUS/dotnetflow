// <copyright file="DataFlowSet.cs" company="Universität Stuttgart">
// Copyright © 2020 - 2022 Institut für Visualisierung und Interaktive Systeme. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace DotNetFlow.Netflow9 {

    /// <summary>
    /// The data flow set contains the actual data in the format described by
    /// a previously defined template ID.
    /// </summary>
    [OnWirePadding(32)]
    public sealed class DataFlowSet : IFlowSet {

        #region Public constructors
        /// <summary>
        /// Initialises a new instance.
        /// </summary>
        /// <param name="id">The ID of the flow set (which is equivalent to the
        /// ID of the template describing the flow set).</param>
        public DataFlowSet(ushort id) => this.ID = id;
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
        /// Gets or sets the list of field values.
        /// </summary>
        /// <remarks>
        /// <para>The type and length of the fields have been previously defined
        /// in the template record referenced by the FlowSet ID/template
        /// ID.</para>
        /// </remarks>
        [OnWireOrder(2)]
        public IList<object> Records {
            get;
            internal set;
        } = new List<object>();
        #endregion

        #region Internal constructors
        /// <summary>
        /// Initialises a new instance.
        /// </summary>
        internal DataFlowSet() : this(256) { }
        #endregion

        #region Private fields
        /// <summary>
        /// Backing field for <see cref="ID"/>.
        /// </summary>
        private ushort _id;
        #endregion
    }
}
