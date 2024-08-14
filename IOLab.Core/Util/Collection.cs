using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOLab.Core.Util
{
    internal static class Collection
    {
        /// <summary>
        /// 集合取并集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static List<T> Merge<T>(this List<T> source, List<T> target)
        {
            List<T> mergedList = new List<T>(source);
            mergedList.AddRange(target.Except(source));
            return mergedList;
        }
    }
}
