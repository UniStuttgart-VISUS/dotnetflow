// <copyright file="OnWirePaddingAttribute.cs" company="Universität Stuttgart">
// Copyright © 2020 SAPPAN Consortium. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using System;


namespace Sappan.Netflow {

    /// <summary>
    /// Annotates a class with any on-wire padding requirements.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false,
        Inherited = false)]
    internal sealed class OnWirePaddingAttribute : Attribute {

        /// <summary>
        /// Initialises a new instance.
        /// </summary>
        /// <param name="alignment">The bits of the alignment border that the
        /// serialiser must enforce by adding padding.</param>
        public OnWirePaddingAttribute(uint alignment)
            => this.Alignment = alignment;

        /// <summary>
        /// Gets the required alignment of the end of the structure in
        /// bits.
        /// </summary>
        public uint Alignment { get; }

        /// <summary>
        /// Gets the required alignment in bytes.
        /// </summary>
        /// <exception cref="InvalidOperationException">If
        /// <see cref="Alignment"/> is not a multiple of one byte.</exception>
        public int ByteAlignment {
            get {
                if ((this.Alignment % 8) != 0) {
                    throw new InvalidOperationException(
                        Properties.Resources.ErrorAlignmentNotByteMultiple);
                }
                return (int) this.Alignment / 8;
            }
        }
    }
}
