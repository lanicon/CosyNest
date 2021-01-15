using System;

namespace System.NetFrancis
{
    /// <summary>
    /// 这个静态类枚举HTTP协议的媒体类型的名称
    /// </summary>
    public static class MediaTypeName
    {
        #region Json
        /// <summary>
        /// 返回Json的媒体类型
        /// </summary>
        public const string Json = "application/json";
        #endregion
        #region XML
        /// <summary>
        /// 返回XML的媒体类型
        /// </summary>
        public const string XML = "application/xml";
        #endregion
    }
}
