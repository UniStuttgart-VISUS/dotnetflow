// <copyright file="DataSet.cs" company="Universität Stuttgart">
// Copyright © 2020 - 2022 Institut für Visualisierung und Interaktive Systeme. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using System;


namespace DotNetFlow.Ipfix {

    /// <summary>
    /// A set of actual flow data.
    /// </summary>
    [OnWirePadding(32)]
    public sealed class DataSet : SetBase<object> {

        #region Public constructors
        /// <summary>
        /// Initialises a new instance.
        /// </summary>
        /// <param name="id">The ID of the flow set (which is equivalent to the
        /// ID of the template describing the flow set).</param>
        public DataSet(ushort id) => this.ID = id;
        #endregion

        #region Public properties
        /// <summary>
        /// Gets or sets the flow set ID of the template that is used.
        /// </summary>
        public override ushort ID {
            get => this._id;
            set {
                if (value < 256) {
                    throw new ArgumentException(
                        Properties.Resources.ErrorWrongDataSetID,
                        nameof(value));
                }
                this._id = value;
            }
        }
        #endregion

        #region Internal constructors
        /// <summary>
        /// Initialises a new instance.
        /// </summary>
        internal DataSet() : this(256) { }
        #endregion

        #region Private fields
        /// <summary>
        /// Backing field for <see cref="ID"/>.
        /// </summary>
        private ushort _id;
        #endregion
    }
}
