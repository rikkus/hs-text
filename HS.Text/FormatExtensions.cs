using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace HS.Text
{
    public class FormatExtensions
    {
        private const string FormatPattern = "{[A-Za-z][A-Za-z0-9-_]*}";

        public static string MagicFormat(string format, object attributeHost)
        {
            if (format == null)
            {
                throw new ArgumentNullException("format");
            }

            if (attributeHost == null)
            {
                throw new ArgumentNullException("attributeHost");
            }

            return Format(format, ReplacementTexts(attributeHost));
        }

        public static string Format(string format, params string[] args)
        {
            if (format == null)
            {
                throw new ArgumentNullException("format");
            }

            if (args == null)
            {
                throw new ArgumentNullException("args");
            }

            return Format(format, ReplacementTexts(args));
        }

        private static string Format(string format, IDictionary<string, string> replacementTexts)
        {
            var matches = new Regex(FormatPattern).Matches(format);

            if (replacementTexts.Count < UniqueMatchCount(matches))
            {
                throw new FormatException(String.Format("Insufficient arguments ({0}) to satisfy format", replacementTexts.Count));
            }

            return ReplaceCaptures(format, matches.Cast<Match>().Select(match => match.Captures[0]), replacementTexts);
        }

        private static int UniqueMatchCount(MatchCollection matchCollection)
        {
            var matchKeys = new Hashtable();

            foreach (Match match in matchCollection)
            {
                matchKeys[match.Captures[0].Value] = 1;
            }

            return matchKeys.Count;
        }

        private static IDictionary<string, string> ReplacementTexts(object attributeHost)
        {
            return attributeHost.GetType().GetProperties().Aggregate
                (
                new Dictionary<string, string>(),
                (dictionary, info) =>
                    {
                        dictionary[info.Name] = info.GetValue(attributeHost, null).ToString();
                        return dictionary;
                    }
                );
        }

        private static IDictionary<string, string> ReplacementTexts(IEnumerable<string> colonSeparatedKeyValuePairs)
        {
            return colonSeparatedKeyValuePairs.Aggregate
                (
                    new Dictionary<string, string>(),
                    (dictionary, pair) =>
                    {
                        var tokens = pair.Split(new [] { ':' }, 2);
                        dictionary[tokens[0]] = tokens[1];
                        return dictionary;   
                    }
                );
        }

        private static string ReplaceCaptures(string format, IEnumerable<Capture> captures, IDictionary<string, string> replacementTexts)
        {
            var ret = new StringBuilder();

            var cursor = 0;

            foreach (var capture in captures)
            {
                ReplaceForCapture(capture, ret, format, replacementTexts, ref cursor);
            }

            // Add any remaining text.

            if (cursor < format.Length)
            {
                ret.Append(format.Substring(cursor, format.Length - cursor));
            }

            return ret.ToString();
        }

        private static void ReplaceForCapture
        (
            Capture capture,
            StringBuilder ret,
            string format,
            IDictionary<string, string> replacementTexts,
            ref int cursor
        )
        {
            if (capture.Index > cursor || capture.Index == 0)
            {
                // Save the text before the match.

                ret.Append(format.Substring(cursor, capture.Index - cursor));
                cursor = capture.Index + capture.Length;
            }

            var tag = capture.Value.Substring(1, capture.Value.Length - 2);

            if (!replacementTexts.ContainsKey(tag))
                throw new FormatException(String.Format("No match found for tag '{0}'", tag));

            ret.Append(replacementTexts[tag]);
        }
    }
}
