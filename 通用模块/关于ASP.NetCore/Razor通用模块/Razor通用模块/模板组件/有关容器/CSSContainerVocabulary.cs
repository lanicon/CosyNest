using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.AspNetCore.Components.Container
{
    /// <summary>
    /// 这个静态类储存了有关容器的组件，
    /// 所引用的CSS类选择器类名
    /// </summary>
    public static class CSSContainerVocabulary
    {
        #region 有关TilesContainer
        #region 父容器类名
        /// <summary>
        /// 获取<see cref="ContainerTemplate.Tile{Content}"/>模板的磁贴父容器的CSS选择器类名，
        /// 该容器容纳容器内的所有磁贴
        /// </summary>
        public const string TilesFather = nameof(TilesFather);
        #endregion
        #region 子容器类名
        /// <summary>
        /// 获取<see cref="ContainerTemplate.Tile{Content}"/>模板的磁贴子容器的CSS选择器类名，
        /// 该容器容纳单个磁贴
        /// </summary>
        public const string TilesSon = nameof(TilesSon);
        #endregion
        #region 磁贴按钮的类名
        /// <summary>
        /// 获取<see cref="ContainerTemplate.Tile{Content}"/>模板的磁贴按钮的CSS选择器类名，
        /// 该按钮具备磁贴风格样式
        /// </summary>
        public const string TilesButton = nameof(TilesButton);
        #endregion
        #endregion
    }
}
