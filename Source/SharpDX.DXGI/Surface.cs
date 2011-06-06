// Copyright (c) 2010-2011 SharpDX - Alexandre Mutel
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;

namespace SharpDX.DXGI
{
    public partial class Surface
    {
        /// <summary>
        /// Acquires access to the surface data.
        /// </summary>
        /// <param name="flags">Flags specifying CPU access permissions.</param>
        /// <returns>A <see cref="T:SharpDX.DataRectangle" /> for accessing the mapped data, or <c>null</c> on failure.</returns>.
        public DataRectangle Map(MapFlags flags)
        {
            MappedRect mappedRect;
            Map(out mappedRect, (int) flags);
            // TODO Check to use MappedRect.Pitch instead of calculating the size of a Format
            var desc = Description;
            int size = (int)(FormatHelper.SizeOfInBytes(desc.Format)*desc.Width*desc.Height);
            var canRead = (flags & MapFlags.Read) != 0;
            var canWrite = (flags & MapFlags.Write) != 0;
            return new DataRectangle(mappedRect.Pitch, new DataStream(mappedRect.PBits, size, canRead, canWrite));     
        }

        /// <summary>
        /// Gets a swap chain back buffer.
        /// </summary>
        /// <param name="swapChain">The swap chain to get the buffer from.</param>
        /// <param name="index">The index of the desired buffer.</param>
        /// <returns>The buffer interface, or <c>null</c> on failure.</returns>
        public static Surface FromSwapChain(SwapChain swapChain, int index)
        {
            IntPtr surfacePointer;
            swapChain.GetBuffer(index, typeof (Surface).GUID, out surfacePointer);
            return new Surface(surfacePointer);
        }
    }
}