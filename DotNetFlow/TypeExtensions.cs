// <copyright file="TypeExtensions.cs" company="Universität Stuttgart">
// Copyright © 2020 - 2022 Institut für Visualisierung und Interaktive Systeme. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;


namespace DotNetFlow {

    /// <summary>
    /// Extension methods for <see cref="Type"/>.
    /// </summary>
    internal static class TypeExtensions {

        /// <summary>
        /// Gets the all public instance fields of <paramref name="that"/> that
        /// have an <see cref="OnWireOrderAttribute"/> and pass the optional
        /// <paramref name="filter"/>.
        /// </summary>
        /// <param name="that">The type to get the fields of.</param>
        /// <param name="filter">An optional filter that must be passed if not
        /// <c>null</c>.</param>
        /// <returns>The fields with the above-mentioned properties.</returns>
        /// <exception cref="ArgumentNullException">If <paramref name="that"/>
        /// is <c>null</c>.</exception>
        public static IEnumerable<FieldInfo> GetOnWireFields(this Type that,
                Predicate<FieldInfo> filter = null) {
            if (that == null) {
                throw new ArgumentNullException(nameof(that));
            }

            foreach (var f in that.GetFields(Flags)) {
                if (f.GetCustomAttribute<OnWireOrderAttribute>() != null) {
                    if ((filter == null) || filter(f)) {
                        yield return f;
                    }
                }
            }
        }

        /// <summary>
        /// Gets all public instance members of <paramref name="that"/> that have
        /// an <see cref="OnWireOrderAttribute"/>.
        /// </summary>
        /// <param name="that">The type to retrieve the on-wire members of.
        /// </param>
        /// <param name="first">The first (inclusive) ordinal that is being
        /// returned.</param>
        /// <param name="last">The last (inclusive) ordinal that is being
        /// returned.</param>
        /// <returns>The members of the on-wire representation of
        /// <paramref name="that"/>, sorted by the order specified by their
        /// <see cref="OnWireOrderAttribute"/>.</returns>
        public static IEnumerable<MemberInfo> GetOnWireMembers(this Type that,
                int first = int.MinValue, int last = int.MaxValue) {
            var fields = that.GetOnWireFields();
            var props = that.GetOnWireProperties();
            return from m in fields.Cast<MemberInfo>().Concat(props)
                   let a = m.GetCustomAttribute<OnWireOrderAttribute>()
                   let o = a.Position
                   where (o >= first) && (o <= last)
                   orderby o ascending
                   select m;
        }

        /// <summary>
        /// Gets the all public instance properties of <paramref name="that"/>
        /// that have an <see cref="OnWireOrderAttribute"/> and pass the optional
        /// <paramref name="filter"/>.
        /// </summary>
        /// <param name="that">The type to get the properties of.</param>
        /// <param name="filter">An optional filter that must be passed if not
        /// <c>null</c>.</param>
        /// <returns>The properties with the above-mentioned properties.
        /// </returns>
        /// <exception cref="ArgumentNullException">If <paramref name="that"/>
        /// is <c>null</c>.</exception>
        public static IEnumerable<PropertyInfo> GetOnWireProperties(
                this Type that, Predicate<PropertyInfo> filter = null) {
            if (that == null) {
                throw new ArgumentNullException(nameof(that));
            }

            foreach (var p in that.GetProperties(Flags)) {
                if (p.GetCustomAttribute<OnWireOrderAttribute>() != null) {
                    if ((filter == null) || filter(p)) {
                        yield return p;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the static size of the on-wire representation of the specified
        /// type or a negative number if the on-wire representation has a
        /// dynamic size.
        /// </summary>
        /// <param name="that">The type to get the on-wire size of.</param>
        /// <returns>The on-wire size of the specified type, or a negative
        /// number if this size cannot be statically determined.</returns>
        public static int GetOnWireSize(this Type that) {
            if (that == null) {
                throw new ArgumentNullException(nameof(that));
            }

            if (that.IsPrimitive) {
                // Private types are directly convertible.
                return Marshal.SizeOf(that);

            } else if (that.IsEnum) {
                // Enums are represented by their underlying type.
                return Marshal.SizeOf(that.GetEnumUnderlyingType());

            } else {
                // The size might be dependent on dynamic allocations.
                return -1;
            }
        }

        /// <summary>
        /// Answer whether <paramref name="that"/> is a floating point type.
        /// </summary>
        /// <param name="that">The type to be tested.</param>
        /// <returns><c>true</c> if <paramref name="that"/> is a floating point
        /// type, <c>false</c> otherwise.</returns>
        public static bool IsFloatingPoint(this Type that) {
            if (that != null) {
                switch (Type.GetTypeCode(that)) {
                    case TypeCode.Decimal:
                    case TypeCode.Double:
                    case TypeCode.Single:
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Answer whether <paramref name="that"/> is a signed numeric type.
        /// </summary>
        /// <param name="that">The type to be tested.</param>
        /// <returns><c>true</c> if <paramref name="that"/> is a signed
        /// numeric type, <c>false</c> otherwise.</returns>
        public static bool IsSignedIntegral(this Type that) {
            if (that != null) {
                switch (Type.GetTypeCode(that)) {
                    case TypeCode.SByte:
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                    case TypeCode.Int64:
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Answer whether <paramref name="that"/> is an unsigned numeric type.
        /// </summary>
        /// <param name="that">The type to be tested.</param>
        /// <returns><c>true</c> if <paramref name="that"/> is a unsigned
        /// numeric type, <c>false</c> otherwise.</returns>
        public static bool IsUnsignedIntegral(this Type that) {
            if (that != null) {
                switch (Type.GetTypeCode(that)) {
                    case TypeCode.Byte:
                    case TypeCode.UInt16:
                    case TypeCode.UInt32:
                    case TypeCode.UInt64:
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// The flags for all relevant members.
        /// </summary>
        private const BindingFlags Flags = BindingFlags.Public
            | BindingFlags.Instance;
    }
}
