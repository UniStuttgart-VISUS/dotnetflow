// <copyright file="OptionsTemplateSet.cs" company="Universität Stuttgart">
// Copyright © 2020 SAPPAN Consortium. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>


namespace Sappan.Netflow.Ipfix {

    /// <summary>
    /// A set holding <see cref="OptionsTemplateRecord"/>s.
    /// </summary>
    [OnWirePadding(32)]
    public sealed class OptionsTemplateSet : SetBase<OptionsTemplateRecord> {

        #region Public constructors
        /// <summary>
        /// Initialises a new instance.
        /// </summary>
        public OptionsTemplateSet() { }
        #endregion

        #region Public properties
        /// <summary>
        /// Gets or sets the flow set ID.
        /// </summary>
        /// <remarks>
        /// The set ID of template sets is always 3. Therefore, the setter as no
        /// effect and is only provided for forward compatibility of
        /// <see cref="ISet"/>.
        /// </remarks>
        public override ushort ID {
            get => SetIDs.OptionsTemplateSet;
            set => _ = value;
        }
        #endregion
    }
}
