using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace System
{
    /// <summary>
    /// 通用工具类
    /// </summary>
    public static class Tool
    {
        #region 注册特殊转换方法
        /// <summary>
        /// 这个字典索引类型的特殊转换方法，
        /// 它可以被<see cref="ExtenTool.To{Ret}(object?, bool, LazyPro{Ret}?)"/>方法所识别，
        /// 字典的键是转换的原类型和目标类型，值是将原类型的对象转换为目标类型的委托
        /// </summary>
        public static IAddOnlyDictionary<(Type From, Type To), Func<object, object>> SpecialConversion { get; }
        = CreateEnumerable.AddOnlyDictionary
            (false, new ConcurrentDictionary<(Type From, Type To), Func<object, object>>());
        #endregion
        #region 拷贝对象
        /// <summary>
        /// 通过反射拷贝对象，并返回它的副本
        /// </summary>
        /// <typeparam name="Ret">拷贝的返回值类型</typeparam>
        /// <param name="obj">被拷贝的对象</param>
        /// <param name="IsShallow">如果这个值为真，则执行浅拷贝，否则执行深拷贝</param>
        /// <param name="Exception">出现在这个集合中的字段或自动属性名将作为例外，不会被拷贝</param>
        /// <returns></returns>
        public static Ret Copy<Ret>(Ret obj, bool IsShallow = true, params string[] Exception)
        {
            if (obj == null)
                return default!;
            var type = obj.GetTypeData();
            var New = type.ConstructorCreate<Ret>();
            var Field = type.Field.Where(x => !x.IsStatic);               //不拷贝静态属性
            if (Exception.Length > 0)
            {
                var FieldName = Exception.Union(Exception).Select(x => $"<{x}>k__BackingField").ToHashSet();        //获取属性，以及该自动属性对应的字段名称
                Field = Field.Where(x => !FieldName.Contains(x.Name));
            }
            Field.ForEach(x =>
            {
                var Value = x.GetValue(obj);
                x.SetValue(New,
                    IsShallow || Value is ValueType ? Value : Copy(Value, IsShallow));
            });
            return New;
        }

        /*说明文档：
           例外的成员如果是属性，则必须是自动属性才能不被拷贝，
           这是因为自动属性所封装的字段都有固定格式的名称，
           而自己封装的属性的字段名称不确定
           
           obj必须拥有无参数构造函数，
           但如果Ret是obj的父类，可以没有无参数构造函数，
           这是因为程序实际上是在obj的类型中搜索构造函数*/
        #endregion
        #region 将两个对象的引用交换
        /// <summary>
        /// 将两个对象的引用交换
        /// </summary>
        /// <typeparam name="Obj">要交换的对象类型</typeparam>
        /// <param name="A">第一个对象</param>
        /// <param name="B">第二个对象</param>
        public static void Exchange<Obj>(ref Obj A, ref Obj B)
        {
            Obj C = B;
            B = A;
            A = C;
        }
        #endregion
    }
}
