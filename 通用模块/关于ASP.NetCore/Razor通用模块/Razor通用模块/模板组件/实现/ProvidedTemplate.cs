using System;
using System.Collections.Generic;
using System.Text;
using System.Design;
using System.Linq;
using Microsoft.AspNetCore.Components.Container;

namespace Microsoft.AspNetCore.Components.Services
{
    /// <summary>
    /// 这个类型是<see cref="IProvidedDefaultTemplate"/>的实现，
    /// 可以为组件提供默认模板
    /// </summary>
    public sealed class ProvidedTemplate : Singleton<ProvidedTemplate>, IProvidedDefaultTemplate
    {
        #region 提供组件模板
        #region 缓存字典
        /// <summary>
        /// 这个字典按类型缓存组件的默认模板，
        /// 且可以重新修改已经存在于字典中的模板
        /// </summary>
        public static IAddOnlyDictionary<Type, RenderFragment<TemplatedComponents>> CacheTemplate { get; }
        = CreateEnumerable.AddOnlyDictionaryConcurrent<Type, RenderFragment<TemplatedComponents>>(true);
        #endregion
        #region 正式方法
        public RenderFragment<TemplatedComponents> GetTemplate(Type type)
        {
            if (!typeof(TemplatedComponents).IsAssignableFrom(type))
                throw new ArgumentException($"请求组件模板的组件类型必须继承自{nameof(TemplatedComponents)}，而{type}不满足此要求");
            if (CacheTemplate.TryGetValue(type, out var templated))
                return templated;
            return CacheTemplate[type] = type.BaseTypeAll(typeof(TemplatedComponents)).
                Select(x => CacheTemplate.TryGetValue(x).Value).
                FirstOrDefault(x => x != null) ?? throw new NullReferenceException($"{type}没有默认组件模板");
        }
        #endregion
        #endregion
        #region 提供项模板
        public RenderFragment<(Multiple<Content> Components, Content Item)> GetItemTemplate<Content>(Type type)
        {
            if (!typeof(Multiple<Content>).IsAssignableFrom(type))
                throw new ArgumentException($"请求项模板的组件类型必须继承自{nameof(Multiple<Content>)}，而{type}不满足此要求");
            return Multiple<Content>.DefaultItemTemplate;
        }
        #endregion
        #region 构造函数
        private ProvidedTemplate()
        {

        }
        #endregion
    }
}
