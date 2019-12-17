using System;
using System.Collections.Generic;
using System.Text;

namespace Projeto1_LP2
{
    /// <summary>
    /// 
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        ///  Static method that gets and IEnumerable of strings, and joins 
        ///  them all in one string
        /// </summary>
        /// <param name="iEnumerable"></param>
        /// <returns></returns>
        public static string IEnumerableToString(
            this IEnumerable<string> iEnumerable)
        {
            return string.Join(string.Empty, iEnumerable);
        }
    }
}
