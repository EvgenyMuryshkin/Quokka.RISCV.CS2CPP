using System;
using System.Collections.Generic;
using System.Text;

namespace System.Linq
{
    internal static class NetStandardExtensions
    {
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
        {
            return new HashSet<T>(source);
        }

        public static String[] Split(this string source, string separator)
        {
            return source.Split(separator.ToArray());
        }
    }
}
