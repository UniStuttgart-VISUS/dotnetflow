// <copyright file="TemplateSet.cs" company="Universität Stuttgart">
// Copyright © 2020 SAPPAN Consortium. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>


namespace Sappan.Netflow.Ipfix {

    /// <summary>
    /// A set containing <see cref="TemplateRecord"/>s.
    /// </summary>
    [OnWirePadding(32)]
    public sealed class TemplateSet : SetBase<TemplateRecord> {

        #region Public constructors
        /// <summary>
        /// Initialises a new instance.
        /// </summary>
        public TemplateSet() { }
        #endregion

        #region Public properties
        /// <summary>
        /// Gets or sets the set ID.
        /// </summary>
        /// <remarks>
        /// The set ID of template sets is always 2. Therefore, the setter as no
        /// effect and is only provided for forward compatibility of
        /// <see cref="ISet"/>.
        /// </remarks>
        public override ushort ID {
            get => SetIDs.TemplateSet;
            set => _ = value;
        }
        #endregion
    }

}
