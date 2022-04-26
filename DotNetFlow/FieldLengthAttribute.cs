// <copyright file="FieldLengthAttribute.cs" company="Universität Stuttgart">
// Copyright © 2020 - 2022 Institut für Visualisierung und Interaktive Systeme. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.Globalization;
using System.Linq;
using System.Reflection;


namespace DotNetFlow {

    /// <summary>
    /// Annotates the members of <see cref="FieldType"/> with the expected
    /// length of the annotated field.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false,
        Inherited = false)]
    internal class FieldLengthAttribute : Attribute {

        #region Public class methods
        /// <summary>
        /// Gets the annotated length of the given field.
        /// </summary>
        /// <remarks>
        /// <para>Implementation note: As of now, there seems no option to
        /// further generalise this implementation, e.g. by use of a generic,
        /// because there are no constraints that allow for restricting generics
        /// to enums.</para>
        /// </remarks>
        /// <param name="field">The field to get the annotated length for.
        /// </param>
        /// <returns>The annotated (default or specific) length of the field.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <paramref name="field"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If the field is not annotated
        /// or the annotated length is variable.</exception>
        public static ushort GetLength(FieldInfo field) {
            if (field == null) {
                throw new ArgumentNullException(nameof(field));
            }

            var att = field.GetCustomAttribute<FieldLengthAttribute>();
            if (att != null) {
                var retval = att.Default;

                if (retval > 0) {
                    return retval;
                }
            }

            var msg = Properties.Resources.ErrorNonFixedFieldType;
            msg = string.Format(CultureInfo.CurrentCulture, msg, field.Name);
            throw new ArgumentException(msg, nameof(field));
        }
        #endregion

        #region Public constructors
        /// <summary>
        /// Initialises a new instance with variable field length.
        /// </summary>
        public FieldLengthAttribute() : this(0) { }

        /// <summary>
        /// Initialises a new instance with the given field length.
        /// </summary>
        /// <param name="length">The length of the field in bytes.</param>
        public FieldLengthAttribute(ushort length) => this.Length = length;
        #endregion

        #region Public properties
        /// <summary>
        /// Gets or sets the default field length.
        /// </summary>
        /// <remarks>
        /// This property is only meaningful if <see cref="IsVariable"/> is
        /// <c>true</c>. Otherwise, <see cref="Length"/> will be returned by
        /// the getter.
        /// </remarks>
        public ushort Default {
            get => this.IsVariable ? this._default : this.Length;
            set {
                this._default = value;
            }
        }

        /// <summary>
        /// Gets whether the field length is variable.
        /// </summary>
        public bool IsVariable => (this.Length < 1);

        /// <summary>
        /// Gets the field length in bytes.
        /// </summary>
        /// <remarks>
        /// If the value is zero, the length of the field is variable.
        /// </remarks>
        public ushort Length {
            get;
        }

        /// <summary>
        /// Gets or sets the maximum length of variable-length field in bytes.
        /// </summary>
        /// <remarks>
        /// If zero, no maximum length has been specified.
        /// </remarks>
        public ushort MaximumLength {
            get;
            set;
        }
        #endregion

        #region Private fields
        /// <summary>
        /// Backing field for <see cref="Default"/>.
        /// </summary>
        private ushort _default = 0;
        #endregion
    }
}
