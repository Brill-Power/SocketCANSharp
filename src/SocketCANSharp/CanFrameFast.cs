#region License
/*
BSD 3-Clause License

Copyright (c) 2025, Brill Power
All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:

1. Redistributions of source code must retain the above copyright notice, this
   list of conditions and the following disclaimer.

2. Redistributions in binary form must reproduce the above copyright notice,
   this list of conditions and the following disclaimer in the documentation
   and/or other materials provided with the distribution.

3. Neither the name of the copyright holder nor the names of its
   contributors may be used to endorse or promote products derived from
   this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/
#endregion

#if NET8_0_OR_GREATER
using System;
using System.Runtime.InteropServices;

namespace SocketCANSharp
{
    /// <summary>
    /// Represents a CAN frame (classical or FD) that exists only on the stack.
    /// </summary>
    public readonly ref struct CanFrameFast
    {
        private readonly Span<byte> _buffer;
        private readonly Span<uint> _bufferAsUint;

        /// <summary>
        /// Construct a new CanFrameFast instance with the supplied buffer.
        /// </summary>
        public CanFrameFast(ref Span<byte> buffer)
        {
            if (buffer.Length != 16 && buffer.Length != 72)
            {
                throw new ArgumentException("Supplied buffer is too small.");
            }
            _buffer = buffer;
            _bufferAsUint = MemoryMarshal.Cast<byte, uint>(buffer);
        }

        /// <summary>
        /// Gets a reference to the 11 or 29-bit CAN ID.
        /// </summary>
        public ref uint CanId
        {
            get { return ref _bufferAsUint[0]; }
        }

        /// <summary>
        /// Frame length in bytes.
        /// </summary>
        public ref byte Length
        {
            get { return ref _buffer[4]; }
        }

        /// <summary>
        /// CAN FD specific flags for ESI, BRS, etc.
        /// </summary>
        public CanFdFlags Flags
        {
            get { return (CanFdFlags)_buffer[5]; }
            set { _buffer[5] = (byte)value; }
        }

        /// <summary>
        /// CAN frame payload.
        /// </summary>
        public Span<byte> Data => _buffer.Slice(8);

        /// <summary>
        /// Gets the total size of this CAN frame (either 16 or 72 bytes).
        /// </summary>
        public int Size => _buffer.Length;

        /// <summary>
        /// Indexes into the payload of this CAN frame.
        /// </summary>
        public ref byte this[int index]
        {
            get { return ref _buffer[8 + index]; }
        }

        internal Span<byte> Buffer => _buffer;
    }
}
#endif // NET8_0_OR_GREATER