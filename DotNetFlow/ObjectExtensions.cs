// <copyright file="ObjectExtensions.cs" company="Universität Stuttgart">
// Copyright © 2020 - 2022 Institut für Visualisierung und Interaktive Systeme. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;


namespace DotNetFlow {

    /// <summary>
    /// Extension methods for <see cref="Object"/>.
    /// </summary>
    internal static class ObjectExtensions {

        /// <summary>
        /// Gets the size of the on-wire representation fo the given object.
        /// </summary>
        /// <remarks>
        /// The size computed by this method only considers members marked with
        /// a <see cref="OnWireOrderAttribute"/>. If the type itself is marked
        /// with a <see cref="OnWirePaddingAttribute"/>, padding bytes are
        /// included in the result.
        /// </remarks>
        /// <param name="that">The object to determine the size of.</param>
        /// <returns>The size of the on-wire representation in bytes.</returns>
        public static int GetOnWireSize(this object that) {
            if (that == null) {
                throw new ArgumentNullException(nameof(that));
            }

            var type = that.GetType();
            var retval = type.GetOnWireSize();

            if (retval < 0) {
                Debug.Assert(!type.IsPrimitive);
                Debug.Assert(!type.IsEnum);
                retval = 0;

                if (that is IHandleOnWire handler) {
                    // If a type implements its on-wire format itself, use this
                    // implementation.
                    return handler.GetOnWireSize();

                } else if (type.IsArray) {
                    // Arrays are the sum of their elements. We need to check
                    // all array members individually, because they might have
                    // dynamically allocated members.
                    var propLength = type.GetProperty(nameof(Array.Length));
                    Debug.Assert(propLength != null);
                    var cnt = (int) propLength.GetValue(that);
                    var size = type.GetElementType().GetOnWireSize();

                    if (size < 0) {
                        // Array has variable-size elements.
                        var array = (Array) that;
                        for (int i = 0; i < cnt; ++i) {
                            retval += array.GetValue(i).GetOnWireSize();
                        }
                    } else {
                        // Array has fixed-size elements.
                        retval = cnt * size;
                    }

                } else if (that is IEnumerable enumerable) {
                    // In case of enumerables, we just count the size of the
                    // enumerated elements.
                    // Note that this check must be done after the array case.
                    foreach (var e in enumerable) {
                        Debug.Assert(e != null);
                        retval += e.GetOnWireSize();
                    }

                } else if (that is IPAddress address) {
                    retval += address.GetAddressBytes().Length;

                } else {
                    // All other types are the sum of their on-wire members. As
                    // for arrays, we need to check the instance (not their
                    // type) individually as the instances might have
                    // dynamically allocated members themselves.
                    // We check the static type size first, because it is
                    // cheaper that retrieving each and every value and because
                    // it allows us to implement the Length property of some
                    // structures using this method without generating an
                    // infinite recursion.

                    foreach (var m in type.GetOnWireMembers()) {
                        if (m is FieldInfo f) {
                            var size = f.FieldType.GetOnWireSize();
                            if (size < 0) {
                                size = f.GetValue(that).GetOnWireSize();
                            }
                            Debug.Assert(size >= 0);
                            retval += size;

                        } else if (m is PropertyInfo p) {
                            var size = p.PropertyType.GetOnWireSize();
                            if (size < 0) {
                                size = p.GetValue(that).GetOnWireSize();
                            }
                            Debug.Assert(size >= 0);
                            retval += size;

                        } else {
                            throw new InvalidOperationException(
                                Properties.Resources.ErrorOnWireMemberType);
                        }
                    }

                    if (retval == 0) {
                        throw new ArgumentException(
                            Properties.Resources.ErrorNoOnWireMembers,
                            nameof(that));
                    }
                }
            } /* end if (retval < 0) */

            {
                // Note: as per https://www.cisco.com/en/US/technologies/tk648/tk362/technologies_white_paper09186a00800a3db9.html
                // the padding is part of the object's on-wire length.
                var att = type.GetCustomAttribute<OnWirePaddingAttribute>();
                if (att != null) {
                    var unaligned = retval % att.ByteAlignment;
                    if (unaligned > 0) {
                        retval += att.ByteAlignment - unaligned;
                    }
                }
            }

            return retval;
        }

    }
}
