using Microsoft.AspNetCore.Components.Container;

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.AspNetCore.Components.Services
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以为Blazor组件提供默认组件模板和项模板
    /// </summary>
    public interface IProvidedDefaultTemplate
    {
        #region 提供默认组件模板
        #region 非泛型方法
        /// <summary>
        /// 为指定类型的组件提供默认模板
        /// </summary>
        /// <param name="type">请求模板的组件类型，必须继承自<see cref="TemplatedComponents"/></param>
        /// <returns></returns>
        RenderFragment<TemplatedComponents> GetTemplate(Type type);
        #endregion
        #region 泛型方法
        /// <summary>
        /// 为指定类型的组件提供默认模板
        /// </summary>
        /// <typeparam name="Components">请求模板的组件类型</typeparam>
        /// <returns></returns>
        RenderFragment<TemplatedComponents> GetTemplate<Components>()
            where Components : TemplatedComponents
            => GetTemplate(typeof(Components));
        #endregion
        #endregion
        #region 提供默认项模板
        #region 非泛型方法
        /// <summary>
        /// 为<see cref="Multiple{Content}"/>提供默认项模板
        /// </summary>
        /// <typeparam name="Content">组件所容纳的子内容类型</typeparam>
        /// <param name="type">请求项模板的组件类型，必须继承自<see cref="Multiple{Content}"/></param>
        /// <returns></returns>
        RenderFragment<(Multiple<Content> Components, Content Item)> GetItemTemplate<Content>(Type type);
        #endregion
        #region 泛型方法
        /// <summary>
        /// 为<see cref="Multiple{Content}"/>提供默认项模板
        /// </summary>
        /// <typeparam name="Content">组件所容纳的子内容类型</typeparam>
        /// <typeparam name="Component">请求项模板的组件类型</typeparam>
        /// <returns></returns>
        RenderFragment<(Multiple<Content> Components, Content Item)> GetItemTemplate<Content, Component>()
            where Component : Multiple<Content>
            => GetItemTemplate<Content>(typeof(Component));
        #endregion
        #endregion
    }
}
