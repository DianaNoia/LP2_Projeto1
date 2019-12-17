using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto1_LP2
{
    public static class Extensions
    {
        public static string IEnumerableToString(
            this IEnumerable<string> iEnumerable)
        {
            return string.Join(string.Empty, iEnumerable);
        }
    }
}
