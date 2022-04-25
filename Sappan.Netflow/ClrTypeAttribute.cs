// <copyright file="ClrTypeAttribute.cs" company="Universität Stuttgart">
// Copyright © 2020 SAPPAN Consortium. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using System;


namespace Sappan.Netflow {

    /// <summary>
    /// Annotates a member of <see cref="Field"/> with the CLR type that the
    /// reader should use to represent the data.
    /// </summary>
    /// <remarks>
    /// If there are multiple attributes on one field, the reader should use
    /// the one with the matching on-wire size.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true,
        Inherited = false)]
    internal sealed class ClrTypeAttribute : Attribute {

        /// <summary>
        /// Initialises a new instance.
        /// </summary>
        /// <param name="type"></param>
        public ClrTypeAttribute(Type type) {
            this.Type = type
                ?? throw new ArgumentNullException(nameof(type));
        }

        /// <summary>
        /// Gets the type the reader should use for the field.
        /// </summary>
        public Type Type { get; }
    }
}
