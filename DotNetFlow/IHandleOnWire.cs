// <copyright file="IOnWireFormatter.cs" company="Universität Stuttgart">
// Copyright © 2020 - 2022 Institut für Visualisierung und Interaktive Systeme. All rights reserved.
// </copyright>
// <author>Christoph Müller</author>


namespace DotNetFlow {

    /// <summary>
    /// Classes can declare that they handle their on-wire format themselves by
    /// implementing this interface.
    /// </summary>
    public interface IHandleOnWire {

        /// <summary>
        /// Restore the object's state from the given on-wire representation.
        /// </summary>
        /// <param name="data">The data of the on-wire representation.</param>
        /// <param name="offset">The offset into <paramref name="data"/> where
        /// the on-wire representation starts.</param>
        /// <returns>The number of bytes that have been consumed for the on-wire
        /// representation of this object.</returns>
        int FromWire(byte[] data, int offset);

        /// <summary>
        /// Answer the amount of memory, in bytes, that the object requires for
        /// its on-wire representation.
        /// </summary>
        /// <returns>The on-wire size in bytes.</returns>
        int GetOnWireSize();

        /// <summary>
        /// Answer the on-wire representation of the object.
        /// </summary>
        /// <returns>The on-wire representation of the object.</returns>
        byte[] ToWire();

    }
}
