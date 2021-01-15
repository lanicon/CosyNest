using System.Collections.Generic;
using System.Design.Direct;
using System.Linq;

namespace System.Design
{
    /// <summary>
    /// 这个静态类可以用来帮助创建和设计模式有关的类型
    /// </summary>
    public static class CreateDesign
    {
        #region 创建ISchema
        #region 直接创建
        /// <summary>
        /// 指定架构，并创建一个<see cref="ISchema"/>
        /// </summary>
        /// <param name="Schema">这个字典指定架构的属性名称和类型</param>
        /// <returns></returns>
        public static ISchema Schema(IReadOnlyDictionary<string, Type> Schema)
            => new Schemas(Schema);
        #endregion
        #region 快速创建
        /// <summary>
        /// 通过若干描述属性名称和类型的元组创建架构
        /// </summary>
        /// <param name="Propertys">这个元组的项分别是属性的名称以及类型</param>
        /// <returns></returns>
        public static ISchema Schema(params (string Name, Type Type)[] Propertys)
            => Schema(Propertys.ToDictionary(true));
        #endregion
        #region 通过IDirect创建
        /// <summary>
        /// 返回一个<see cref="IDirect"/>的架构，
        /// 如果<see cref="IDirect.Schema"/>未指定，
        /// 则通过它现有的属性和值生成架构
        /// </summary>
        /// <param name="Direct">待返回架构的<see cref="IDirect"/></param>
        /// <returns></returns>
        public static ISchema Schema(IDirect Direct)
            => Direct.Schema ??
            Schema(Direct.ToDictionary(x => (x.Key, x.Value?.GetType() ?? typeof(object)), true));
        #endregion
        #endregion
    }
}
