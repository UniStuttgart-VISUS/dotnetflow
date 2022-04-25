// <copyright file="SetBase.cs" company="Universität Stuttgart">
// Copyright © 2020 SAPPAN Consortium. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace Sappan.Netflow.Ipfix {

    /// <summary>
    /// Abstract superclass for sets with a record type of
    /// <typeparamref name="TRecord"/>.
    /// </summary>
    /// <typeparam name="TRecord">The type of the records stored in the set.
    /// </typeparam>
    public abstract class SetBase<TRecord> : ISet {

        /// <inheritdoc />
        [OnWireOrder(0)]
        public abstract ushort ID { get; set; }

        /// <inheritdoc />
        [OnWireOrder(1)]
        public virtual ushort Length {
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
        /// Gets the individual templates in this flow set.
        /// </summary>
        [OnWireOrder(2)]
        public IList<TRecord> Records {
            get;
            internal set;
        } = new List<TRecord>();
    }
}
