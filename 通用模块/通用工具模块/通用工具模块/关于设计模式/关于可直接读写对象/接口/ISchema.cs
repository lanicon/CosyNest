using System.Collections.Generic;

namespace System.Design.Direct
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个<see cref="IDirect"/>的架构
    /// </summary>
    public interface ISchema
    {
        #region 返回数据的架构
        /// <summary>
        /// 这个字典的键是数据名称，值是该列数据的合法类型
        /// </summary>
        IReadOnlyDictionary<string, Type> Schema { get; }
        #endregion
        #region 检查架构兼容性
        #region 说明文档
        /*说明文档：
          假设有架构A，B，若满足以下条件，
          则判断为A兼容B，但是除非A和B完全相同，反之不成立：
          A的数据名称是B的子集或真子集，且A的数据类型与B的对应数据相同
          
          举例说明：
          假设架构A存在以下数据：
          名称：Age，类型：int
          
          假设架构B存在以下数据：
          名称：Age，类型：int
          名称：Name，类型：string
        
          则：A兼容B，但是B不兼容A
          但是，如果不是判断两个架构，
          而是一个架构和一条数据之间的兼容性，
          则数据的类型应该支持逆变*/
        #endregion
        #region 基础方法
        /// <summary>
        /// 用于判断架构兼容的基本方法
        /// </summary>
        /// <typeparam name="Value">字典的值类型</typeparam>
        /// <param name="data">指定要判断架构兼容性的字典，
        /// 它可以是另一个架构，也可以是一条数据</param>
        /// <param name="screening">用于比较本架构和<paramref name="data"/>同名Value类型的委托，
        /// 如果它返回<see langword="true"/>，表示架构不兼容</param>
        /// <param name="throw">如果这个值为<see langword="true"/>，
        /// 则在架构不兼容时，不是返回异常，而是直接抛出异常</param>
        /// <returns>如果架构兼容，返回<see langword="null"/>，
        /// 否则返回一个<see cref="ExceptionSchema"/>，报告第一个发现的兼容性问题</returns>
        private ExceptionSchema? SchemaCompatibleBase<Value>(IReadOnlyDictionary<string, Value> data, Func<Type, Value, bool> screening, bool @throw)
        {
            #region 本地函数
            ExceptionSchema? Get()
            {
                foreach (var (name, type) in Schema)
                {
                    if (data.TryGetValue(name, out var value))
                    {
                        if (screening(type, value))
                            return new ExceptionSchemaType(name, value, type);
                    }
                    else return new ExceptionSchemaNotFound(name);
                }
                return null;
            }
            #endregion
            return Get() switch
            {
                null => null,
                var ex => @throw ? throw ex : ex
            };
        }
        #endregion
        #region 判断架构是否兼容（传入架构）
        /// <summary>
        /// 判断本架构是否与另一个架构兼容
        /// </summary>
        /// <param name="schema">待判断的另一个架构</param>
        /// <param name="throw">如果这个值为<see langword="true"/>，
        /// 则在架构不兼容时，不是返回异常，而是直接抛出异常</param>
        /// <returns>如果架构兼容，返回<see langword="null"/>，
        /// 否则返回一个<see cref="ExceptionSchema"/>，报告第一个发现的兼容性问题</returns>
        ExceptionSchema? SchemaCompatible(ISchema schema, bool @throw = false)
            => SchemaCompatibleBase(schema.Schema, (t, v) => t != v, @throw);
        #endregion
        #region 确定架构是否兼容（传入数据）
        /// <summary>
        /// 判断某一数据是否兼容于本架构
        /// </summary>
        /// <param name="data">待判断的数据</param>
        ///  <param name="throw">如果这个值为<see langword="true"/>，
        /// 则在架构不兼容时，不是返回异常，而是直接抛出异常</param>
        /// <returns>如果架构兼容，返回<see langword="null"/>，
        /// 否则返回一个<see cref="ExceptionSchema"/>，报告第一个发现的兼容性问题</returns>
        ExceptionSchema? SchemaCompatible(IReadOnlyDictionary<string, object?> data, bool @throw = false)
            => SchemaCompatibleBase(data, (t, v) => !t.IsAssignableFrom(v), @throw);
        #endregion
        #endregion
    }
}
