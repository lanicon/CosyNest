using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.IO
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个文件类型
    /// </summary>
    public interface IFileType
    {
        #region 已注册的文件类型
        #region 公开属性
        /// <summary>
        /// 返回已注册的文件类型，键是扩展名，值是文件类型，
        /// 在注册之后，文件类型可以被<see cref="IFile"/>识别
        /// </summary>
        public static IReadOnlyDictionary<string, IEnumerable<IFileType>> RegisteredFileType
            => RegisteredFileTypePR.ToDictionary(x => x.Key, x => (IEnumerable<IFileType>)x.Value);
        #endregion
        #region 私有属性
        /// <summary>
        /// 返回已注册的文件类型，键是扩展名，值是文件类型，
        /// 在注册之后，文件类型可以被<see cref="IFile"/>识别
        /// </summary>
        protected static Dictionary<string, List<IFileType>> RegisteredFileTypePR { get; }
        = new Dictionary<string, List<IFileType>>();

        /*注释：文件类型使用集合的原因在于：
           同一个扩展名可能对应多个文件类型，例如：
           exe既是程序集文件，也是可执行文件*/
        #endregion
        #endregion
        #region 对文件类型的说明
        /// <summary>
        /// 对文件类型的说明
        /// </summary>
        string Description { get; }
        #endregion
        #region 关于扩展名集合
        #region 受支持的扩展名
        /// <summary>
        /// 表示属于这个文件类型的扩展名，不带点号
        /// </summary>
        IEnumerable<string> ExtensionName { get; }
        #endregion
        #region 注册扩展名
        /// <summary>
        /// 将一个新扩展名注册到文件类型
        /// </summary>
        /// <param name="NewExtensionName">要注册的新扩展名</param>
        void Registered(string NewExtensionName);


        /*注意事项：
          1.本方法的目的是：
          让文件类型初始化以后能够扩展，
          请勿用于其他用途
          2.为了减少副作用，
          本类型不提供注销扩展名的方法*/
        #endregion
        #endregion
        #region 关于文件类型兼容性
        #region 传入扩展名
        /// <summary>
        /// 检查扩展名与文件类型是否兼容
        /// </summary>
        /// <param name="ExtensionName">指定的扩展名</param>
        /// <returns>如果兼容，返回<see langword="true"/>，否则返回<see langword="false"/></returns>
        bool IsCompatibleName(string ExtensionName)
            => this.ExtensionName.Contains(ExtensionName);
        #endregion
        #region 传入路径
        /// <summary>
        /// 检查路径的扩展名与文件类型是否兼容
        /// </summary>
        /// <param name="Path">指定的路径</param>
        /// <returns>如果兼容，返回<see langword="true"/>，否则返回<see langword="false"/></returns>
        bool IsCompatiblePath(PathText Path)
           => IsCompatibleName(ToolPath.Split(Path).Extended);
        #endregion
        #region 返回两个文件类型是否兼容
        /// <summary>
        /// 检查另一个文件类型是否与本文件类型兼容
        /// </summary>
        /// <param name="fileTypeB">要判断的文件类型B</param>
        /// <returns>如果兼容，返回<see langword="true"/>，否则返回<see langword="false"/>，
        /// 判断标准为：<paramref name="fileTypeB"/>扩展名是本对象扩展名的子集（不可是父集）</returns>
        bool IsCompatibleFileType(IFileType fileTypeB)
           => fileTypeB.ExtensionName.IsSupersetOf(ExtensionName, true).IsSubset();
        #endregion
        #endregion
    }
}
