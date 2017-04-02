namespace StreamingServiceCompare
{
    // Source: http://stackoverflow.com/questions/4685705/good-csv-writer-for-c#4685745
    public static class CSV
    {
        private const string QUOTE = "\"";
        private const string ESCAPED_QUOTE = "\"\"";
        private static readonly char[] CHARACTERS_THAT_MUST_BE_QUOTED = new char[] { ';', '"', '\n' };

        public static string Escape(string s)
        {
            if (s == null)
            {
                return null;
            }

            if (s.Contains(QUOTE))
            {
                s = s.Replace(QUOTE, ESCAPED_QUOTE);
            }

            if (s.IndexOfAny(CHARACTERS_THAT_MUST_BE_QUOTED) > -1)
            {
                s = QUOTE + s + QUOTE;
            }

            return s;
        }

        public static string Unescape(string s)
        {
            if (s == null)
            {
                return null;
            }

            if (s.StartsWith(QUOTE) && s.EndsWith(QUOTE))
            {
                s = s.Substring(1, s.Length - 2);

                if (s.Contains(ESCAPED_QUOTE))
                {
                    s = s.Replace(ESCAPED_QUOTE, QUOTE);
                }
            }

            return s;
        }
    }
}
