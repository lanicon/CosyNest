using System;

namespace System.Linq
{
    public static partial class ExtenIEnumerable
    {
        /*所有有关数组的扩展方法，全部放在这个部分类中，
          本部分类中的API专为数组优化，性能较高*/

        #region 合并数组
        /// <summary>
        /// 取多个数组的并集
        /// </summary>
        /// <typeparam name="Obj">数组的元素类型</typeparam>
        /// <param name="array">原始数组</param>
        /// <param name="arrayOther">要合并的其他数组</param>
        /// <returns></returns>
        public static Obj[] Union<Obj>(this Obj[] array, params Obj[][] arrayOther)
        {
            var len = array.Length + arrayOther.Sum(x => x.Length);
            var newArray = new Obj[len];
            var pos = 0;
            foreach (var item in arrayOther)
            {
                item.CopyTo(newArray, pos);
                pos += item.Length;
            }
            return newArray;
        }
        #endregion
    }
}
