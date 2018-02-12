using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Tupi.Indexing
{
    public sealed class TokenSource
    {
        private readonly TextReader _reader;
        
        public TokenSource(TextReader reader)
        {
            _reader = reader;
        }

        public char[] Buffer { get; } = new char[256];
        public int Size { get; private set; } = 0;

        public bool Next()
        {
            Size = 0;
            int r;
            while ((r = _reader.Read()) != -1) // EOF
            {
                var ch = (char)r;

                if (ch == '\r' || ch == '\n' || char.IsWhiteSpace(ch) || char.IsPunctuation(ch))
                {
                    if (Size > 0)
                    {
                        return true;
                    }
                }
                else
                {
                    Recognize(ch);
                }
            }

            return Size > 0;
        }

        private void Recognize(char curr)
        {
            Buffer[Size++] = curr;
        }

        public IEnumerable<string> ReadAll()
        {
            while (Next())
            {
                yield return ToString();
            }
        }

        public IEnumerable<string> ReadAll(Func<TokenSource, bool> filter)
        {
            while (Next())
            {
                if (filter(this))
                {
                    yield return ToString();
                }
            }
        }



        public override string ToString()
        {
            return new string(Buffer, 0, Size);
        }
    }
}
