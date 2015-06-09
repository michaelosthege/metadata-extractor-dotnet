// =================================================================================================
// ADOBE SYSTEMS INCORPORATED
// Copyright 2006 Adobe Systems Incorporated
// All Rights Reserved
//
// NOTICE:  Adobe permits you to use, modify, and distribute this file in accordance with the terms
// of the Adobe license agreement accompanying it.
// =================================================================================================

using Sharpen;

namespace Com.Adobe.Xmp.Impl
{
    /// <summary>An <c>OutputStream</c> that counts the written bytes.</summary>
    /// <since>08.11.2006</since>
    public sealed class CountOutputStream : OutputStream
    {
        /// <summary>the decorated output stream</summary>
        private readonly OutputStream _out;

        /// <summary>the byte counter</summary>
        private int _bytesWritten;

        /// <summary>Constructor with providing the output stream to decorate.</summary>
        /// <param name="out">an <c>OutputStream</c></param>
        internal CountOutputStream(OutputStream @out)
        {
            _out = @out;
        }

        /// <summary>Counts the written bytes.</summary>
        /// <seealso cref="OutputStream.Write(byte[], int, int)"/>
        /// <exception cref="System.IO.IOException"/>
        public override void Write(byte[] buf, int off, int len)
        {
            _out.Write(buf, off, len);
            _bytesWritten += len;
        }

        /// <summary>Counts the written bytes.</summary>
        /// <seealso cref="OutputStream.Write(byte[])"/>
        /// <exception cref="System.IO.IOException"/>
        public override void Write(byte[] buf)
        {
            _out.Write(buf);
            _bytesWritten += buf.Length;
        }

        /// <summary>Counts the written bytes.</summary>
        /// <seealso cref="OutputStream.Write(int)"/>
        /// <exception cref="System.IO.IOException"/>
        public override void Write(int b)
        {
            _out.Write(b);
            _bytesWritten++;
        }

        /// <returns>the bytesWritten</returns>
        public int GetBytesWritten()
        {
            return _bytesWritten;
        }
    }
}