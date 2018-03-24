namespace Tupi.Indexing.Filters
{
    public class PorterStemmerFilter : IFilter
    {
        public bool Process(TokenSource source)
        {
            PerformStep1(source);
            PerformStep2(source);
            PerformStep3(source);
            PerformStep4(source);
            PerformStep5(source);
            PerformStep6(source);
            return source.Size > 0;
        }


        /* gets rid of plurals and -ed or -ing. e.g.
			   caresses  ->  caress
			   ponies    ->  poni
			   ties      ->  ti
			   caress    ->  caress
			   cats      ->  cat

			   feed      ->  feed
			   agreed    ->  agree
			   disabled  ->  disable

			   matting   ->  mat
			   mating    ->  mate
			   meeting   ->  meet
			   milling   ->  mill
			   messing   ->  mess

			   meetings  ->  meet

		*/
        public void PerformStep1(TokenSource source)
        {
            if (source.EndsWith('s'))
            {
                if (source.EndsWith("sses") || source.EndsWith("ies"))
                {
                    source.Size -= 2;
                }
                else if (source.Size >= 2 && source.Buffer[source.Size - 2] != 's')
                {
                    source.Size -= 1;
                }
            }

            if (source.EndsWith("eed"))
            {
                var limit = source.Size - 3; // source.Length
                if (source.NumberOfConsoantSequences(limit) > 0)
                {
                    source.Size -= 1;
                }
            }
            else
            {
                var limit = 0;
                if (source.EndsWith("ed"))
                {
                    limit = source.Size - 2;
                }
                else if (source.EndsWith("ing"))
                {
                    limit = source.Size - 3;
                }

                if (limit != 0 && source.ContainsVowel(limit))
                {
                    source.Size = limit;
                    if (
                        source.EndsWith("at") ||
                        source.EndsWith("bl") ||
                        source.EndsWith("iz")
                    )
                    {
                        source.InsertIntoBuffer('e');
                    }
                    else if (source.EndsWithDoubleConsonant())
                    {
                        var ch = source.LastChar;
                        if (ch != 'l' && ch != 's' && ch != 'z')
                        {
                            source.Size--;
                        }
                    }
                    else if (
                        source.NumberOfConsoantSequences(source.Size - 1) == 1 &&
                        source.HasCvcAt(source.Size - 1)
                    )
                    {
                        source.InsertIntoBuffer('e');
                    }
                }
            }
        }

        /* turns terminal y to i when there is another vowel in the stem. */
        public void PerformStep2(TokenSource source)
        {
            if (source.EndsWith('y')
                && source.ContainsVowel(source.Size - 2)
            )
            {
                source.Buffer[source.Size - 1] = 'i';
            }
        }

        /* maps double suffices to single ones. so -ization ( = -ize plus
		   -ation) maps to -ize etc. note that the string before the suffix must give
		   m() > 0. */
        public void PerformStep3(TokenSource source)
        {
            if (source.Size < 2)
                return;

            switch (source.Buffer[source.Size - 2])
            {
                case 'a':
                    if (source.ChangeSuffix("ational", "ate")) break;
                    source.ChangeSuffix("tional", "tion");
                    break;
                case 'c':
                    if (source.ChangeSuffix("enci", "ence")) break;
                    source.ChangeSuffix("anci", "ance");
                    break;
                case 'e':
                    source.ChangeSuffix("izer", "ize");
                    break;
                case 'l':
                    if (source.ChangeSuffix("bli", "ble")) break;
                    if (source.ChangeSuffix("alli", "al")) break;
                    if (source.ChangeSuffix("entli", "ent")) break;
                    if (source.ChangeSuffix("eli", "e")) break;
                    source.ChangeSuffix("ousli", "ous");
                    break;
                case 'o':
                    if (source.ChangeSuffix("ization", "ize")) break;
                    if (source.ChangeSuffix("ation", "ate")) break;
                    source.ChangeSuffix("ator", "ate");
                    break;
                case 's':
                    if (source.ChangeSuffix("alism", "al")) break;
                    if (source.ChangeSuffix("iveness", "ive")) break;
                    if (source.ChangeSuffix("fulness", "ful")) break;
                    source.ChangeSuffix("ousness", "ous");
                    break;
                case 't':
                    if (source.ChangeSuffix("aliti", "al")) break;
                    if (source.ChangeSuffix("iviti", "ive")) break;
                    source.ChangeSuffix("biliti", "ble");
                    break;
                case 'g':
                    source.ChangeSuffix("logi", "log");
                    break;
            }
        }


        /* deals with -ic-, -full, -ness etc. similar strategy to step3. */
        public void PerformStep4(TokenSource source)
        {
            if (source.Size == 0)
                return;

            switch (source.LastChar)
            {
                case 'e':
                    if (source.ChangeSuffix("icate", "ic")) break;
                    if (source.RemoveSuffix("ative")) break;
                    source.ChangeSuffix("alize", "al");
                    break;
                case 'i':
                    source.ChangeSuffix("iciti", "ic");
                    break;
                case 'l':
                    if (source.ChangeSuffix("ical", "ic")) break;
                    source.RemoveSuffix("ful");
                    break;
                case 's':
                    source.RemoveSuffix("ness");
                    break;
            }
        }

        public void PerformStep5(TokenSource source)
        {
            if (source.Size < 2)
                return;

            switch (source.Buffer[source.Size - 2])
            {
                case 'a':
                    source.RemoveSuffix("al");
                    return;
                case 'c':
                    if (source.RemoveSuffix("ance")) return;
                    source.RemoveSuffix("ence");
                    return;
                case 'e':
                    source.RemoveSuffix("er");
                    return;
                case 'i':
                    source.RemoveSuffix("ic");
                    return;
                case 'l':
                    if (source.RemoveSuffix("able")) return;
                    source.RemoveSuffix("ible");
                    return;
                case 'n':
                    if (source.RemoveSuffix("ant")) return;
                    if (source.RemoveSuffix("ement")) return;
                    if (source.RemoveSuffix("ment")) return;
                    source.RemoveSuffix("ent");
                    return;
                case 'o':
                    if (source.ChangeSuffix("tion", "t")) return;
                    if (source.ChangeSuffix("sion", "s")) return;
                    source.RemoveSuffix("ou");
                    return;
                case 's':
                    source.RemoveSuffix("ism");
                    return;
                case 't':
                    if (source.RemoveSuffix("ate")) return;
                    source.RemoveSuffix("iti");
                    return;
                case 'u':
                    source.RemoveSuffix("ous");
                    return;
                case 'v':
                    source.RemoveSuffix("ive");
                    return;
                case 'z':
                    source.RemoveSuffix("ize");
                    return;
                default:
                    return;
            }
        }

        public void PerformStep6(TokenSource source)
        {
            switch (source.LastChar)
            {
                case 'e':
                    var a = source.NumberOfConsoantSequences(source.Size - 1);
                    if (a > 1 || a == 1 && !source.HasCvcAt(source.Size - 2))
                        source.Size --;
                    break;
                case 'l' when source.ContainsDoubleConsonantAt(source.Size - 1) && source.NumberOfConsoantSequences(source.Size - 2) > 0:
                    source.Size--;
                    break;
            }
        }
    }
}
