using System;
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
        public int Size { get; set; }
        public long Position { get; private set; }

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
                        Position++;
                        return true;
                    }
                }
                else
                {
                    InsertIntoBuffer(ch);
                }
            }

            Position++;
            return Size > 0;
        }

        public void InsertIntoBuffer(char curr)
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

        public bool EndsWith(char c) =>
            Size > 0 && Buffer[Size - 1] == c;


        public bool EndsWith(string chars)
        {
            if (Size < chars.Length)
                return false;

            var origin = Size - chars.Length;

            for (var i = 0; i < chars.Length; i++)
                if (Buffer[origin + i] != chars[i])
                    return false;

            return true;
        }

        public override string ToString()
        {
            return new string(Buffer, 0, Size);
        }


        /* if c is
		   a consonant sequence and v a vowel sequence, and <..> indicates arbitrary
		   presence,

			  <c><v>       gives 0
			  <c>vc<v>     gives 1
			  <c>vcvc<v>   gives 2
			  <c>vcvcvc<v> gives 3
			  ....
		*/
        public int NumberOfConsoantSequences(int limit)
        {
            var result = 0;
            var i = 0;

            while (true)
            {
                if (i > limit) return result;
                if (!IsConsonant(i)) break;
                i++;
            }
            i++;

            while (true)
            {
                while (true)
                {
                    if (i > limit) return result;
                    if (IsConsonant(i)) break;
                    i++;
                }
                i++;
                result++;
                while (true)
                {
                    if (i > limit) return result;
                    if (!IsConsonant(i)) break;
                    i++;
                }
                i++;
            }
        }

        public bool ContainsVowel(int limit)
        {
            for (var i = 0; i <= limit; i++)
                if (!IsConsonant(i))
                    return true;
            return false;
        }

        public bool IsConsonant(int index)
        {
            switch (Buffer[index])
            {
                case 'a': case 'e': case 'i': case 'o': case 'u': return false;
                case 'y': return (index == 0) || !IsConsonant(index - 1);
                default: return true;
            }
        }

        public bool ContainsDoubleConsonantAt(int index)
        {
            if (index < 1) return false;
            return Buffer[index] == Buffer[index - 1] && IsConsonant(index);
        }

        public bool EndsWithDoubleConsonant() => 
            ContainsDoubleConsonantAt(Size - 1);


        /* HasCvcAt(i) is true <=> i-2,i-1,i has the form consonant - vowel - consonant
		   and also if the second c is not w,x or y. this is used when trying to
		   restore an e at the end of a short word. e.g.

			  cav(e), lov(e), hop(e), crim(e), but
			  snow, box, tray.
		*/
        public bool HasCvcAt(int index)
        {
            if (index < 2 || !IsConsonant(index) || IsConsonant(index - 1) || !IsConsonant(index - 2))
                return false;

            int ch = LastChar;
            return ch != 'w' && ch != 'x' && ch != 'y';
        }

        public bool ChangeSuffix(string suffix, string replacement)
        {
            if (!EndsWith(suffix))
            {
                return false;
            }

            var k = Size - suffix.Length;
            if (NumberOfConsoantSequences(k) < 1)
            {
                return false;
            }

            for (var i = 0; i < replacement.Length; i++)
            {
                Buffer[k + i] = replacement[i];
            }
            Size = k + replacement.Length;
            return true;
        }

        public bool RemoveSuffix(string suffix)
        {
            if (!EndsWith(suffix))
            {
                return false;
            }

            var k = Size - suffix.Length;
            if (NumberOfConsoantSequences(k) < 1)
            {
                return false;
            }

            Size = k;
            return true;
        }
        public char LastChar => Buffer[Size - 1];


    }
}

