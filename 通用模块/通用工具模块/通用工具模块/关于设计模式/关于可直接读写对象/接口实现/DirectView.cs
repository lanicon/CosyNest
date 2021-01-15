using System.Collections.Generic;
using System.Linq;

namespace System.Design.Direct
{
    /// <summary>
    /// 这个类型是<see cref="IDirectView{Direct}"/>的实现，
    /// 可以视为一个拥有架构的数据容器
    /// </summary>
    /// <typeparam name="Direct">数据容器所容纳的数据类型</typeparam>
    class DirectView<Direct> : EnumerableCache<Direct>, IDirectView<Direct>
        where Direct : IDirect
    {
        #region 封装的迭代器
        /// <summary>
        /// 获取封装的迭代器，
        /// 本对象的功能就是通过它实现的
        /// </summary>
        private IEnumerable<Direct>? PackEnumerable { get; set; }
        #endregion
        #region 是否检查架构
        /// <summary>
        /// 如果这个值为<see langword="true"/>，则在遍历时会检查架构，
        /// 在确定架构一致时，将其设为<see langword="false"/>可以改善性能
        /// </summary>
        private bool CheckSchema { get; }
        #endregion
        #region 获取架构
        private ISchema? SchemaField;
        public ISchema Schema
            => SchemaField ??= CreateDesign.Schema(this.First());
        #endregion
        #region 获取枚举器
        protected override IEnumerable<Direct> GetEnumeratorRealize()
        {
            var (First, Other, HasElements) = PackEnumerable!.First(false);
            if (HasElements)
            {
                var Schema = SchemaField ??= CreateDesign.Schema(First);
                #region 检查架构的本地函数
                Direct Check(Direct Elements)
                {
                    if (CheckSchema)
                    {
                        var sc = Elements.Schema;
                        if (sc == null)
                            Elements.Schema = Schema;
                        else Schema.SchemaCompatible(sc, true);
                    }
                    return Elements;
                }
                #endregion
                yield return Check(First);
                foreach (Direct item in Other)
                    yield return Check(item);
            }
        }
        #endregion
        #region 清理迭代器
        protected override void Clean()
            => PackEnumerable = null;
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的参数初始化对象
        /// </summary>
        /// <param name="PackEnumerable">用于遍历<see cref="IDirect"/>的迭代器</param>
        /// <param name="Schema">指定数据容器的架构，
        /// 如果为<see langword="null"/>，则在遍历容器时自动获取</param>
        /// <param name="CheckSchema">如果这个值为<see langword="true"/>，则在遍历时会检查架构，
        /// 在确定架构一致时，传入<see langword="false"/>可以改善性能</param>
        /// <param name="CacheAll">如果这个值为<see langword="true"/>，表示在获取第一个元素时缓存全部元素，
        /// 否则代表逐个缓存元素，正确指定这个参数可以改善性能</param>
        public DirectView(IEnumerable<Direct> PackEnumerable, ISchema? Schema, bool CheckSchema, bool CacheAll)
            : base(CacheAll)
        {
            this.PackEnumerable = PackEnumerable;
            SchemaField = Schema;
            this.CheckSchema = CheckSchema;
        }
        #endregion
    }
}
