// <copyright file="NetFlowWriterBase.cs" company="Universität Stuttgart">
// Copyright © 2020 SAPPAN Consortium. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>

using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;


namespace Sappan.Netflow {

    /// <summary>
    /// Base class for NetFlow writers.
    /// </summary>
    public abstract class NetflowWriterBase : IDisposable {

        #region Finaliser
        /// <summary>
        /// Finalises the instance.
        /// </summary>
        ~NetflowWriterBase() {
            this.Dispose(false);
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the underlying stream of the writer.
        /// </summary>
        public Stream BaseStream => this._writer?.BaseStream;
        #endregion

        #region Public methods
        /// <inheritdoc />
        public void Dispose() {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Writes all pending buffers to the underyling device.
        /// </summary>
        public void Flush() {
            if (this._writer != null) {
                this._writer.Flush();
            }
        }
        #endregion

        #region Internal constructors
        /// <summary>
        /// Initialises a new instance.
        /// </summary>
        /// <param name="stream">The stream to write to.</param>
        /// <param name="leaveOpen">Indicates whether the writer should
        /// keep the stream open if it is disposed.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="stream"/>
        /// is <c>null</c>.</exception>
        internal NetflowWriterBase(Stream stream, bool leaveOpen)
            : this(new BinaryWriter(stream, Encoding.UTF8, leaveOpen)) { }

        /// <summary>
        /// Initialises a new instance.
        /// </summary>
        /// <param name="writer">The <see cref="BinaryWriter"/> used to write to
        /// the target stream.</param>
        /// <exception cref="ArgumentNullException">If <paramref name="writer"/>
        /// is <c>null</c>.</exception>
        internal NetflowWriterBase(BinaryWriter writer) {
            this._remainingFlowSets = 0;
            this._state = State.PacketHeader;
            this._writer = writer
                ?? throw new ArgumentNullException(nameof(writer));
        }
        #endregion

        #region Nested enum State
        /// <summary>
        /// Possible states of the reader.
        /// </summary>
        protected enum State {
            /// <summary>
            /// The reader expects the packet header next.
            /// </summary>
            PacketHeader,

            /// <summary>
            /// The reader expects a flow set next.
            /// </summary>
            Flows
        }
        #endregion

        #region Protected methods
        /// <summary>
        /// Begins a new packet that contains the specified number of flow sets
        /// or flow records.
        /// </summary>
        /// <param name="flows">The expected number of flows as read from the
        /// packet header.</param>
        protected void BeginFlows(ushort flows) {
            Debug.Assert(this._remainingFlowSets == 0);
            Debug.Assert(this._state == State.PacketHeader);
            this._remainingFlowSets = flows;
            this._state = State.Flows;
        }

        /// <summary>
        /// Decrements the counter <see cref="_remainingFlowSets"/> and checks
        /// whether it reached zero. If so, the state is reset to the begin of
        /// a new packet.
        /// </summary>
        /// <remarks>The <paramref name="written"/> parameter is for IPFIX,
        /// which uses bytes instead of the number of flow sets in the header.
        /// </remarks>
        /// <param name="written">The number of flow sets to be subtracted from
        /// the counter. This parameter defaults to 1.</param>
        protected void CheckEndOfPacket(ushort written = 1) {
            Debug.Assert(this._remainingFlowSets >= written);
            this._remainingFlowSets -= written;
            if (this._remainingFlowSets == 0) {
                this._state = State.PacketHeader;
            }
        }

        /// <summary>
        /// Checks whether the writer is in the given state or throws an
        /// <see cref="InvalidOperationException"/>.
        /// </summary>
        /// <param name="expected">The expected state of the writer.</param>
        /// <exception cref="InvalidOperationException">If the current state of
        /// the writer is not <paramref name="expected"/>.</exception>
        protected void CheckState(State expected) {
            if (this._state != expected) {
                throw new InvalidOperationException(
                    Properties.Resources.ErrorWriterState);
            }
        }

        /// <summary>
        /// Implements the actual disposal of the object.
        /// </summary>
        /// <param name="disposing">Indicates whether the method was called
        /// from <see cref="Dispose"/> or from the finaliser.</param>
        protected virtual void Dispose(bool disposing) {
            try {
                if (disposing) {
                    Debug.Assert(this._remainingFlowSets == 0);
                    this._writer.Flush();
                    this._writer.Dispose();
                }
            } finally {
                this._writer = null;
            }
        }

        /// <summary>
        /// Writes the given array into the stream.
        /// </summary>
        /// <param name="data">The data to be written.</param>
        /// <returns>The number of bytes that have been written, which is the
        /// length of <paramref name="data"/>.</returns>
        /// <exception cref="ObjectDisposedException">If the underlying writer
        /// has already been disposed.</exception>
        protected int Write(byte[] data) {
            Contract.Assert(data != null);
            if (this._writer == null) {
                throw new ObjectDisposedException(nameof(this._writer));
            }

            this._writer.Write(data, 0, data.Length);
            return data.Length;
        }

        /// <summary>
        /// Writes, if necessary, padding bytes into the stream.
        /// </summary>
        /// <param name="target">The object that possibly required padding at its
        /// end.</param>
        /// <param name="written">The number of bytes that have already been
        /// written.</param>
        /// <returns>The size of the padding that has been written.</returns>
        /// <exception cref="ObjectDisposedException">If the underlying writer
        /// has already been disposed.</exception>
        protected int WritePadding(object target, int written) {
            Contract.Assert(target != null);
            if (this._writer == null) {
                throw new ObjectDisposedException(nameof(this._writer));
            }

            var retval = target.GetOnWireSize() - written;

            if (retval > 0) {
#if DEBUG
                var padding = Enumerable.Repeat((byte) 0xBA, retval).ToArray();
#else // DEBUG
                var padding = Enumerable.Repeat((byte) 0, retval).ToArray();
#endif // DEBUG
                this._writer.Write(padding, 0, padding.Length);
            }

            return retval;
        }
#endregion

        #region Protected fields
        /// <summary>
        /// The number of remaining flow sets the writer expects before the begin
        /// of the next packet.
        /// </summary>
        /// <remarks>
        /// This variable is set when writing the packet header.
        /// </remarks>
        private ushort _remainingFlowSets;

        /// <summary>
        /// Tracks the state of the writer in order to ensure valid outputs.
        /// </summary>
        private State _state;

        /// <summary>
        /// The writer that interacts with the output stream.
        /// </summary>
        private BinaryWriter _writer;
        #endregion
    }
}
