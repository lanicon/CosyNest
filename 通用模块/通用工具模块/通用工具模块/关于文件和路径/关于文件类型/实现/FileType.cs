using System.Collections.Generic;
using System.Linq;

namespace System.IO
{
    /// <summary>
    /// 表示文件的类型（例如音乐文件），不考虑文件是否存在
    /// </summary>
    class FileType : IFileType
    {
        #region 对文件类型的说明
        public string Description { get; }
        #endregion
        #region 关于扩展名集合
        #region 受支持的扩展名
        #region 公开属性
        public IEnumerable<string> ExtensionName
            => ExtensionPR;
        #endregion
        #region 私有属性
        /// <summary>
        /// 私有的扩展名集合，可注册新扩展名
        /// </summary>
        private HashSet<string> ExtensionPR { get; }
        #endregion
        #endregion
        #region 注册扩展名
        public void Registered(string NewExtensionName)
        {
            if (ExtensionPR.Add(NewExtensionName))             //如果扩展名没有重复
                IFileType.RegisteredFileTypePR.TrySetValue(NewExtensionName, x => new List<IFileType>()).Value.Add(this);        //则将其注册
        }
        #endregion
        #endregion
        #region 构造函数
        /// <summary>
        /// 用指定的扩展名集合封装文件类型，
        /// 注意：初始化本类型会将文件类型注册
        /// </summary>
        /// <param name="Description">对文件类型的说明</param>
        /// <param name="Extension">指定的扩展名集合，不带点号</param>
        public FileType(string Description, params string[] Extension)
        {
            var Registered = IFileType.RegisteredFileTypePR;
            ExtensionPR = new HashSet<string>(Extension);
            this.Description = Description;
            Extension.ForEach(x =>                                      //注册所有扩展名
            {
                if (Registered.TryGetValue(x, out var value))
                    value.Add(this);
                else
                    Registered.Add(x, new List<IFileType>() { this });
            });
        }
        #endregion
    }
}
