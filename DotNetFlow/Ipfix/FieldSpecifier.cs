// <copyright file="FieldSpecifier.cs" company="Universität Stuttgart">
// Copyright © 2020 - 2022 Institut für Visualisierung und Interaktive Systeme. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.Linq;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace DotNetFlow.Ipfix {

    /// <summary>
    /// Descriptor of a single IPFIX field.
    /// </summary>
    /// <remarks>
    /// We cannot use <see cref="LayoutKind.Sequential"/> for serialisation,
    /// because all numbers need to be in network byte order and because the
    /// field specifier thas a dynamic layout in contrast to NetFlow, wherefore
    /// each property needs to be swapped separately.
    /// </remarks>
    public class FieldSpecifier : IEquatable<FieldSpecifier>, IHandleOnWire {

        #region Public constructors
        /// <summary>
        /// Initialises a new instance.
        /// </summary>
        /// <param name="ie">The information element in the field.</param>
        /// <param name="length">The length of the field in bytes.</param>
        public FieldSpecifier(InformationElement ie, ushort length)
            => (this.InformationElement, this.Length) = (ie, length);

        /// <summary>
        /// Initialises a new instance.
        /// </summary>
        /// <remarks>
        /// This constructor supports only fields with non-variable size.
        /// </remarks>
        /// <param name="ie">The information element in the field.</param>
        /// <exception cref="ArgumentException">If <paramref name="ie"/> is
        /// not annotated with a non-variable size.</exception>
        public FieldSpecifier(InformationElement ie) {
            this.InformationElement = ie;

            var flags = BindingFlags.Public | BindingFlags.Static;
            var field = (from m in ie.GetType().GetFields(flags)
                         where m.GetValue(null) as InformationElement? == ie
                         select m).SingleOrDefault();

            try {
                this.Length = FieldLengthAttribute.GetLength(field);
            } catch (ArgumentNullException ex) {
                throw new ArgumentException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Initialises a new instance.
        /// </summary>
        /// <param name="ie">The information element in the field. If this value
        /// is negative, the enterprise bit will be set.</param>
        /// <param name="length">The length of the field in bytes.</param>
        /// <param name="enterpriseNumber">The enterprise number in case the
        /// enterprise bit is set.</param>
        public FieldSpecifier(short ie, ushort length, uint enterpriseNumber) {
            this.EnterpriseNumber = enterpriseNumber;
            this.InformationElementIdentifier = ie;
            this.Length = length;
        }
        #endregion

        #region Public properties
        /// <summary>
        /// Gets the IANA enterprise number [IANA-PEN] of the authority defining
        /// the Information Element identifier in a template record.
        /// </summary>
        [OnWireOrder(2)]
        public uint EnterpriseNumber { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the information element.
        /// </summary>
        /// <remarks>
        /// The information element is stored in
        /// <see cref="InformationElementIdentifier"/>. The value might not be
        /// valid if the enterprise bit is set.
        /// </remarks>
        public InformationElement InformationElement {
            get => (InformationElement) Math.Abs(this.InformationElementIdentifier);
            set => this.InformationElementIdentifier = (short) value;
        }

        /// <summary>
        /// Gets or sets a numeric value that represents the Information
        /// Element including the enterprise bit.
        /// </summary>
        /// <remarks>
        /// Implementation note: if this value is negative, the enterprise bit
        /// is set.
        /// </remarks>
        [OnWireOrder(0)]
        public short InformationElementIdentifier { get; set; }

        /// <summary>
        /// Gets whether the <see cref="InformationElementIdentifier"/>
        /// has the enterprise bit set.
        /// </summary>
        /// <remarks>
        /// The enterprise bit is the first bit of the Field Specifier. If this
        /// bit is zero, the <see cref="InformationElementIdentifier"/>
        /// identifies an Information Element in [IANA-IPFIX], and the
        /// four-octet <see cref="EnterpriseNumber"/> field MUST NOT be
        /// present. If this bit is one, the Information Element identifier
        /// identifies an enterprise-specific Information Element, and the
        /// Enterprise Number field MUST be present.
        /// </remarks>
        public bool IsEnterpriseNumber {
            get => (this.InformationElementIdentifier < 0);
        }

        /// <summary>
        /// Gets or sets the length of the field in bytes.
        /// </summary>
        /// <remarks>
        /// Refer to [IANA-IPFIX]. The Field Length may be smaller than that
        /// listed in [IANA-IPFIX] if the reduced-size encoding is used (see
        /// Section 6.2). The value 65535 is reserved for variable-length
        /// Information Elements (see Section 7).
        /// </remarks>
        [OnWireOrder(1)]
        public ushort Length { get; set; }
        #endregion

        #region Public methods
        /// <summary>
        /// Test for equality.
        /// </summary>
        /// <param name="rhs">The object to be compared.</param>
        /// <returns><c>true</c> if this object an <paramref name="rhs"/> are
        /// equal.</returns>
        public bool Equals(FieldSpecifier rhs) {
            // Handle trivial equality and inequality.
            if (object.ReferenceEquals(this, rhs)) {
                return true;
            }
            if (rhs == null) {
                return false;
            }

            // Check base fields.
            if ((this.InformationElementIdentifier, this.Length)
                    != (rhs.InformationElementIdentifier, rhs.Length)) {
                return false;
            }
            Debug.Assert(this.IsEnterpriseNumber == rhs.IsEnterpriseNumber);

            // Check enterprise number as necessary.
            return (!this.IsEnterpriseNumber
                || (this.EnterpriseNumber == rhs.EnterpriseNumber));
        }

        /// <inheritdoc />
        public override bool Equals(object rhs) {
            return this.Equals(rhs as FieldSpecifier);
        }

        /// <inheritdoc />
        public int FromWire(byte[] data, int offset) {
            var retval = offset;

            {
                short value = 0;
                offset += OnWireExtensions.FromWire(ref value, data, offset);
                this.InformationElementIdentifier = value;
            }

            {
                ushort value = 0;
                offset += OnWireExtensions.FromWire(ref value, data, offset);
                this.Length = value;
            }

            if (this.IsEnterpriseNumber) {
                uint value = 0;
                offset += OnWireExtensions.FromWire(ref value, data, offset);
                this.EnterpriseNumber = value;
            }

            retval = offset - retval;
            return retval;
        }

        /// <inheritdoc />
        public override int GetHashCode() {
            var retval = (int) this.InformationElementIdentifier
                | this.Length << 4;

            if (this.IsEnterpriseNumber) {
                retval ^= (int) this.EnterpriseNumber;
            }

            return retval;
        }

        /// <inheritdoc />
        public int GetOnWireSize() {
            var retval = Marshal.SizeOf(this.InformationElementIdentifier)
                + Marshal.SizeOf(this.Length);

            if (this.IsEnterpriseNumber) {
                retval += Marshal.SizeOf(this.EnterpriseNumber);
            }

            return retval;
        }

        /// <inheritdoc />
        public byte[] ToWire() {
            var offset = 0;
            var retval = new byte[this.GetOnWireSize()];

            {
                var v = this.InformationElementIdentifier.ToWire();
                Array.Copy(v, 0, retval, offset, v.Length);
                offset += v.Length;
            }

            {
                var v = this.Length.ToWire();
                Array.Copy(v, 0, retval, offset, v.Length);
                offset += v.Length;
            }

            if (this.IsEnterpriseNumber) {
                var v = this.EnterpriseNumber.ToWire();
                Array.Copy(v, 0, retval, offset, v.Length);
                offset += v.Length;
            }

            return retval;
        }
        #endregion

        #region Internal constructors
        /// <summary>
        /// Initialises a new instance.
        /// </summary>
        public FieldSpecifier() {
            this.EnterpriseNumber = 0;
            this.InformationElementIdentifier = 0;
            this.Length = 0;
        }
        #endregion
    }
}
