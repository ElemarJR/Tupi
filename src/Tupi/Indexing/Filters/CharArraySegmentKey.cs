using System;
using System.Runtime.CompilerServices;

namespace Tupi.Indexing.Filters
{
    // based on: 
    // https://github.com/ayende/Corax/blob/master/Corax/Indexing/Filters/ArraySegmentKey.cs
    internal struct CharArraySegmentKey
        : IEquatable<CharArraySegmentKey>
    {
        private readonly string _rawString;
        private readonly char[] _buffer;
        private readonly int _size;

        public CharArraySegmentKey(char[] buffer)
            : this(buffer, buffer.Length)
        {
        }

        public CharArraySegmentKey(char[] buffer, int size)
            : this()
        {
            _buffer = buffer;
            _size = size;
        }

        public CharArraySegmentKey(string buffer)
            : this()
        {
            _rawString = buffer;
            _size = _rawString.Length;
        }

        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(CharArraySegmentKey other)
        {
            if (_size != other._size)
            {
                return false;
            }

            if (_rawString != null)
            {
                if (other._rawString != null)
                {
                    return _rawString == other._rawString;
                }

                for (var i = 0; i < _size; i++)
                {
                    if (_rawString[i] != other._buffer[i])
                        return false;
                }

                return true;
            }

            if (other._rawString != null)
            {
                for (var i = 0; i < _size; i++)
                {
                    if (_buffer[i] != other._rawString[i])
                        return false;
                }

                return true;
            }

            for (var i = 0; i < _size; i++)
            {
                if (_buffer[i] != other._buffer[i])
                    return false;
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            return obj is CharArraySegmentKey key && Equals(key);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                if (_buffer == null && _rawString == null)
                    return -1;

                var hc = _size;
                for (var i = 0; i < _size; i++)
                {
                    var c = _rawString != null ? _rawString[i] : _buffer[i];
                    hc = c.GetHashCode() * 397 ^ hc;
                }
                return hc;
            }
        }

        public override string ToString()
        {
            if (_rawString != null)
                return _rawString;

            if (_buffer is char[] c)
                return new string(c, 0, _size);
            
            return base.ToString();
        }
    }
}