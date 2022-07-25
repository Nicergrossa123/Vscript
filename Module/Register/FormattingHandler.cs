/*using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Nexus
{
    public static class FormattingHandler
    {

        public static string RemoveSpecialCharacters(this string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '_' || c == '-')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
        public static string RemoveSpecialGBCharacters(this string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || c == '.')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
}
*/