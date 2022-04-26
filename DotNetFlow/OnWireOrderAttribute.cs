// <copyright file="OnWireOrderAttribute.cs" company="Universität Stuttgart">
// Copyright © 2020 - 2022 Institut für Visualisierung und Interaktive Systeme. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using System;


namespace DotNetFlow {

    /// <summary>
    /// This attribute describes the on-wire order of fields in case the data
    /// cannot be expressed as structure, but require manual serialisation.
    /// </summary>
    /// <remarks>
    /// <para>The implementation of the binary formatter for NetFlow 9 provided
    /// in this library will only read or write fields annotated with this
    /// attribute.</para>
    /// <para>Implementors of data types are responsible for making sure that
    /// the attributes are unique and ordered correctly.</para>
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field,
        AllowMultiple = false, Inherited = true)]
    internal sealed class OnWireOrderAttribute : Attribute {

        /// <summary>
        /// Initialises a new instance.
        /// </summary>
        /// <param name="position">The position of the property or field in the
        /// on-wire representation.</param>
        public OnWireOrderAttribute(int position)
            => this.Position = position;

        /// <summary>
        /// Gets the position of the property or field in the on-wire
        /// representation.
        /// </summary>
        public int Position { get; }
    }
}
