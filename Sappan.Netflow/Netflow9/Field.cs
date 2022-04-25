// <copyright file="Field.cs" company="Universität Stuttgart">
// Copyright © 2020 SAPPAN Consortium. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.Globalization;
using System.Linq;
using System.Reflection;


namespace Sappan.Netflow.Netflow9 {

    /// <summary>
    /// Descriptor of a single NetFlow field.
    /// </summary>
    /// <remarks>
    /// We cannot use <see cref="LayoutKind.Sequential"/> for serialisation,
    /// because all numbers need to be in network byte order, wherefore each
    /// property needs to be swapped separately.
    /// </remarks>
    public class Field : IEquatable<Field> {

        #region Public constructors
        /// <summary>
        /// Initialises a new instance.
        /// </summary>
        /// <param name="type">The type of the field.</param>
        /// <param name="length">The length of the field in bytes.</param>
        public Field(FieldType type, ushort length)
            => (this.Type, this.Length) = (type, length);

        /// <summary>
        /// Initialises a new instance.
        /// </summary>
        /// <remarks>
        /// This constructor supports only fields with non-variable size.
        /// </remarks>
        /// <param name="type">The type of the field.</param>
        /// <exception cref="ArgumentException">If <paramref name="type"/> is
        /// not annotated with a non-variable size.</exception>
        public Field(FieldType type) {
            this.Type = type;

            var flags = BindingFlags.Public | BindingFlags.Static;
            var field = (from m in type.GetType().GetFields(flags)
                         where m.GetValue(null) as FieldType? == type
                         select m).SingleOrDefault();

            try {
                this.Length = FieldLengthAttribute.GetLength(field);
            } catch (ArgumentNullException ex) {
                throw new ArgumentException(ex.Message, ex);
            }
        }
        #endregion

        #region Public properties
        /// <summary>
        /// Gets or sets the length of the field in bytes.
        /// </summary>
        [OnWireOrder(1)]
        public ushort Length { get; set; }

        /// <summary>
        /// Gets or sets the type of the field.
        /// </summary>
        /// <remarks>
        /// This numeric value represents the type of the field. The possible
        /// values of the field type are vendor-specific. Cisco-supplied values
        /// are consistent across all platforms that support NetFlow version 9.
        /// The known field types are defined in <see cref="FieldType"/>.
        /// </remarks>
        [OnWireOrder(0)]
        public FieldType Type { get; set; }
        #endregion

        #region Public methods
        /// <summary>
        /// Test for equality.
        /// </summary>
        /// <param name="rhs">The object to be compared.</param>
        /// <returns><c>true</c> if this object an <paramref name="rhs"/> are
        /// equal.</returns>
        public bool Equals(Field rhs) {
            return object.ReferenceEquals(this, rhs) || ((rhs != null)
                && ((this.Length, this.Type) == (rhs.Length, rhs.Type)));
        }

        /// <inheritdoc />
        public override bool Equals(object rhs) {
            return this.Equals(rhs as Field);
        }

        /// <inheritdoc />
        public override int GetHashCode() {
            return (int) this.Type | this.Length << 4;
        }
        #endregion

        #region Internal constructors
        /// <summary>
        /// Initialises a new instance.
        /// </summary>
        public Field() => (this.Type, this.Length) = (0, 0);
        #endregion
    }
}
