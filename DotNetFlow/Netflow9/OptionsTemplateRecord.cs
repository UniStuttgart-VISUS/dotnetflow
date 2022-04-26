// <copyright file="OptionsTemplateRecord.cs" company="Universität Stuttgart">
// Copyright © 2020 - 2022 Institut für Visualisierung und Interaktive Systeme. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;


namespace DotNetFlow.Netflow9 {

    /// <summary>
    /// An record in an options template.
    /// </summary>
    public sealed class OptionsTemplateRecord {
        // TODO: IPFIX pads option template records to 4-byte boundary. Check NF9 spec again ...

        #region Public constructors
        /// <summary>
        /// Initialises a new instance.
        /// </summary>
        /// <param name="id">The ID of the template.</param>
        public OptionsTemplateRecord(ushort id) => this.ID = id;
        #endregion

        #region Public properties
        /// <summary>
        /// Gets the template ID.
        /// </summary>
        /// <remarks>
        /// As a router generates different template FlowSets to match the type
        /// of NetFlow data it will be exporting, each template is given a
        /// unique ID. This uniqueness is local to the router that generated the
        /// template ID. The Template ID is greater than 255. Template IDs
        /// smaller than 255 are reserved.
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
        public int ActualRecordSize
            => this.Options.Sum(o => o.Length) + this.Scopes.Sum(s => s.Length);

        /// <summary>
        /// Gets the length of any options field definition contained in this
        /// template in bytes.
        /// </summary>
        /// <remarks>
        /// This field gives the length (in bytes) of any Options field
        /// definitions contained in this options template.
        /// </remarks>
        [OnWireOrder(2)]
        public ushort OptionsLength {
            get {
                Debug.Assert(this.Options != null);
                var retval = this.Options.GetOnWireSize();

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
        [OnWireOrder(4)]
        public List<Field> Options {
            get;
            internal set;
        } = new List<Field>();

        /// <summary>
        /// Gets the length of any scope fields contained in this options
        /// template in bytes.
        /// </summary>
        /// <remarks>
        /// This field gives the length in bytes of any scope fields contained
        /// in this options template (the use of scope is described below).
        /// </remarks>
        [OnWireOrder(1)]
        public ushort ScopesLength {
            get {
                Debug.Assert(this.Scopes != null);
                var retval = this.Scopes.GetOnWireSize();

                if (retval > ushort.MaxValue) {
                    throw new InvalidOperationException(
                        Properties.Resources.ErrorLengthTooLarge);
                }

                return (ushort) retval;
            }
        }

        /// <summary>
        /// Gets the scope fields in the template.
        /// </summary>
        [OnWireOrder(3)]
        public List<Scope> Scopes {
            get;
            internal set;
        } = new List<Scope>();
        #endregion

        #region Private fields
        /// <summary>
        /// Backing field for <see cref="ID" />.
        /// </summary>
        private ushort _id;
        #endregion
    }
}
