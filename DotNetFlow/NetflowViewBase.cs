// <copyright file="NetflowViewBase.cs" company="Universität Stuttgart">
// Copyright © 2020 - 2022 Institut für Visualisierung und Interaktive Systeme. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;


namespace DotNetFlow {

    /// <summary>
    /// Base class for <see cref="Netflow9.NetflowView"/> and
    /// <see cref="Ipfix.IpfixView"/>.
    /// </summary>
    /// <typeparam name="TData">The type of the data that is being interpreted.
    /// </typeparam>
    /// <typeparam name="TTemplate">The type of the template record that
    /// describes the data.</typeparam>
    /// <typeparam name="TField">The type of a field in
    /// <typeparamref name="TTemplate"/>.</typeparam>
    /// <typeparam name="TFieldType">The enumeration specifying the well-known
    /// types of fields.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming",
            "CA1710:Identifiers should have correct suffix",
            Justification = "This is not a collection, but something that offers enumeration for convenience.")]
    public abstract class NetflowViewBase<TData, TTemplate, TField, TFieldType>
            : IEnumerable<object> where TData : class where TTemplate : class {

        #region Public properties
        /// <summary>
        /// Gets the number of individual records in <see cref="Data"/>.
        /// </summary>
        public virtual int Count {
            get {
                var fields = this.GetFields();
                var records = this.GetRecords();
                Debug.Assert(fields != null);
                Debug.Assert(records != null);
                Debug.Assert(records.Count % fields.Count == 0);
                return records.Count / fields.Count;
            }
        }

        /// <summary>
        /// Gets the underlying data flow set.
        /// </summary>
        public TData Data { get; }

        /// <summary>
        /// Gets the fields that are present in the data.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Naming",
            "CA1721:Property names should not match get methods",
            Justification = "Only the property is publicly visible.")]
        public IEnumerable<TField> Fields => this.GetFields();

        /// <summary>
        /// Gets the template flow set to interpret <see cref="Data"/>.
        /// </summary>
        public TTemplate Template { get; }
        #endregion

        #region Public indexers
        /// <summary>
        /// Gets or sets the <paramref name="field"/>th field of the
        /// <see cref="record"/>te data record.
        /// </summary>
        /// <param name="record">The zero-based index of the record to
        /// retrieve.</param>
        /// <param name="field">The zero-based index of the field to
        /// retrieve.</param>
        /// <returns>The specified field of the specified record.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If
        /// <paramref name="record"/> is out of range.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If
        /// <paramref name="field"/> is out of range.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="value"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="value"/>
        /// does not have the expected size.</exception>
        public object this[int record, int field] {
            get => this.GetRecords()[this.GetIndex(record, field)];
            set {
                this.CheckValue(value, field);
                this.GetRecords()[this.GetIndex(record, field)] = value;
            }
        }

        /// <summary>
        /// Gets or sets the (first) field of type <paramref name="field"/> of
        /// the <see cref="record"/>th data record.
        /// </summary>
        /// <remarks>
        /// If the template contains more than one field of the specified type,
        /// the first one will be returned.
        /// </remarks>
        /// <param name="record">The zero-based index of the record to
        /// retrieve.</param>
        /// <param name="field">The type of the field to be retrieved.</param>
        /// <returns>The specified field of the specified record.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If
        /// <paramref name="record"/> is out of range.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If
        /// <paramref name="field"/> is not part of the template.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="value"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="value"/>
        /// does not have the expected size.</exception>
        public virtual object this[int record, TFieldType field] {
            get {
                var f = this.GetIndex(field);
                var r = this.GetRecords();
                return r[this.GetIndex(record, f)];
            }
            set {
                var f = this.GetIndex(field);
                this.CheckValue(value, f);
                var r = this.GetRecords();
                r[this.GetIndex(record, f)] = value;
            }
        }

        /// <summary>
        /// Gets or sets the specified <paramref name="field"/> of the
        /// <see cref="record"/>th data record.
        /// </summary>
        /// <remarks>
        /// The exact field needs to be matched. Use the instances from the
        /// <see cref="Template"/> to make sure that all properties of the field
        /// are the right one.
        /// </remarks>
        /// <param name="record">The zero-based index of the record to
        /// retrieve.</param>
        /// <param name="field">The exact field (from <see cref="Template"/>) to
        /// be retrieved.</param>
        /// <returns>The specified field of the specified record.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If
        /// <paramref name="record"/> is out of range.</exception>
        /// <exception cref="ArgumentNullException">If
        /// <paramref name="field"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If
        /// <paramref name="field"/> is not part of the template.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="value"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="value"/>
        /// does not have the expected size.</exception>
        public virtual object this[int record, TField field] {
            get {
                var f = this.GetIndex(field);
                var r = this.GetRecords();
                return r[this.GetIndex(record, f)];
            }
            set {
                var f = this.GetIndex(field);
                this.CheckValue(value, f);
                var r = this.GetRecords();
                r[this.GetIndex(record, f)] = value;
            }
        }

        /// <summary>
        /// Gets a dynamic view of the <paramref name="record"/>th record.
        /// </summary>
        /// <param name="record">The zero-based index of the record to
        /// retrieve.</param>
        /// <returns>A dynamiv view of the specified record.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If
        /// <paramref name="record"/> is out of range.</exception>
        public abstract dynamic this[int record] { get; }
        #endregion

        #region Public methods
        /// <inheritdoc />
        public IEnumerator<object> GetEnumerator() {
            return new Enumerator(this);
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() {
            return new Enumerator(this);
        }
        #endregion

        #region Protected constructors
        /// <summary>
        /// Initialises a new instance.
        /// </summary>
        /// <param name="data">The <see cref="Data"/> to be interpreted.
        /// </param>
        /// <param name="template">The <see cref="Template"/> describing
        /// the data.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="data"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentNullException">If
        /// <paramref name="template"/> is <c>null</c>.</exception>
        protected NetflowViewBase(TData data, TTemplate template) {
            this.Data = data
                ?? throw new ArgumentNullException(nameof(data));
            this.Template = template
                ?? throw new ArgumentNullException(nameof(template));
        }
        #endregion

        #region Protected methods
        /// <summary>
        /// Checks whether the given value has the expected length for the
        /// specified field.
        /// </summary>
        /// <param name="value">The value to be tested.</param>
        /// <param name="field">The index of the field to be tested.</param>
        /// <exception cref="ArgumentOutOfRangeException">If
        /// <paramref name="field"/> is not part of the template.</exception>
        /// <exception cref="ArgumentNullException">If <paramref name="value"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">If <paramref name="value"/>
        /// does not have the expected size.</exception>
        protected abstract void CheckValue(object value, int field);

        /// <summary>
        /// Gets the fields in <see cref="Template"/>.
        /// </summary>
        /// <returns>The fields in <see cref="Template"/>.</returns>
        protected abstract IList<TField> GetFields();

        /// <summary>
        /// Computes the location of the specified field of the specified
        /// record.
        /// </summary>
        /// <param name="record">The zero-based index of the record to
        /// retrieve.</param>
        /// <param name="field">The zero-based index of the field to
        /// retrieve.</param>
        /// <returns>Index of the specified field</returns>
        /// <exception cref="ArgumentOutOfRangeException">If
        /// <paramref name="record"/> is out of range.</exception>
        /// /// <exception cref="ArgumentOutOfRangeException">If
        /// <paramref name="field"/> is out of range.</exception>
        protected virtual int GetIndex(int record, int field) {
            var cntFields = this.GetFields().Count;

            if ((record < 0) || (record >= this.Count)) {
                throw new ArgumentOutOfRangeException(nameof(record));
            }
            if ((field < 0) || (field >= cntFields)) {
                throw new ArgumentOutOfRangeException(nameof(field));
            }

            return record * cntFields + field;
        }

        /// <summary>
        /// Computes the index of the specified field.
        /// </summary>
        /// <param name="field">The type of the field to be retrieved.</param>
        /// <returns>Index of the specified field.</returns>
        /// <exception cref="ArgumentOutOfRangeException">If
        /// <paramref name="field"/> is not part of the template.</exception>
        protected abstract int GetIndex(TFieldType field);

        /// <summary>
        /// Computes the index of the specifie field.
        /// </summary>
        /// <param name="field">The exact field (from <see cref="Template"/>) to
        /// be retrieved.</param>
        /// <returns>Index of the specified field.</returns>
        /// <exception cref="ArgumentNullException">If
        /// <paramref name="field"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">If
        /// <paramref name="field"/> is not part of the template.</exception>
        protected virtual int GetIndex(TField field) {
            if (field == null) {
                throw new ArgumentNullException(nameof(field));
            }

            var fields = this.GetFields();
            var retval = fields.IndexOf(field);

            if ((retval < 0) || (retval >= fields.Count)) {
                throw new ArgumentOutOfRangeException(nameof(field));
            }

            return retval;
        }

        /// <summary>
        /// Gets the list of records from <see cref="Data"/>.
        /// </summary>
        /// <returns>The records sstored in <see cref="Data"/>.</returns>
        public abstract IList<object> GetRecords();
        #endregion

        #region Nested class Enumerator
        /// <summary>
        /// The enumerator for the dynamically generated records.
        /// </summary>
        private sealed class Enumerator : IEnumerator<object> {

            /// <summary>
            /// Initialises a new instance.
            /// </summary>
            /// <param name="view">The view to be enumerated.</param>
            public Enumerator(NetflowViewBase<TData, TTemplate, TField,
                    TFieldType> view) {
                Debug.Assert(view != null);
                this._view = view;
                this.Reset();
            }

            /// <inheritdoc />
            public object Current {
                get {
                    if (this._view == null) {
                        throw new ObjectDisposedException(nameof(this._view));
                    }

                    try {
                        return this._view[this._position];
                    } catch (Exception ex) {
                        throw new InvalidOperationException(
                            Properties.Resources.ErrorEnumeratorRange, ex);
                    }
                }
            }

            /// <inheritdoc />
            public void Dispose() {
                this._view = null;
            }

            /// <inheritdoc />
            public bool MoveNext() {
                var retval = (this._view != null);
                if (retval) {
                    retval = (++this._position < this._view.Count);
                }
                return retval;
            }

            /// <inheritdoc />
            public void Reset() {
                this._position = -1;
            }

            private int _position;
            private NetflowViewBase<TData, TTemplate, TField, TFieldType> _view;
        }
        #endregion
    }
}
