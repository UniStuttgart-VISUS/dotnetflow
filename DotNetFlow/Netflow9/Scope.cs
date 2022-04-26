// <copyright file="Scope.cs" company="Universität Stuttgart">
// Copyright © 2020 - 2022 Institut für Visualisierung und Interaktive Systeme. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.Runtime.InteropServices;


namespace DotNetFlow.Netflow9 {

    /// <summary>
    /// On-wire compatible description of an option scope in an
    /// <see cref="OptionsTemplateFlowSet"/>.
    /// </summary>
    /// <remarks>
    /// We cannot use <see cref="LayoutKind.Sequential"/> for serialisation,
    /// because all numbers need to be in network byte order, wherefore each
    /// property needs to be swapped separately.
    /// </remarks>
    public class Scope : IEquatable<Scope> {

        #region Public constructors
        /// <summary>
        /// Initialises a new instance.
        /// </summary>
        /// <param name="scope">The scope of the option.</param>
        /// <param name="length">The length of the field in bytes.</param>
        public Scope(OptionScope scope, ushort length)
            => (this.OptionScope, this.Length) = (scope, length);
        #endregion

        #region Public properties
        /// <summary>
        /// Gets or sets the length (in bytes) of the scope field, as it would
        /// appear in an options record.
        /// </summary>
        [OnWireOrder(1)]
        public ushort Length { get; set; }

        /// <summary>
        /// Gets or sets the option scope.
        /// </summary>
        /// <remarks>
        /// This field gives the relevant portion of the NetFlow process to
        /// which the options record refers. The currently defined values are
        /// given in <see cref="OptionScope"/>. For instance, sampled NetFlow
        /// can be implemented on a per-interface basis, so if the options
        /// record was reporting on how sampling is configured, the scope for
        /// the report would be 0x0002 (<see cref="OptionScope.Interface"/>).
        /// </remarks>
        [OnWireOrder(0)]
        public OptionScope OptionScope { get; set; }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public bool Equals(Scope other) {
            return (other != null) && (this.Length == other.Length)
                && (this.OptionScope == other.OptionScope);
        }

        /// <inheritdoc />
        public override bool Equals(object obj) {
            return this.Equals(obj as Scope);
        }

        /// <inheritdoc />
        public override int GetHashCode() {
            return this.Length | (((int) this.OptionScope) << 2);
        }
        #endregion

        #region Internal constructors
        /// <summary>
        /// Initialises a new instance.
        /// </summary>
        internal Scope() { }
        #endregion

    }
}
