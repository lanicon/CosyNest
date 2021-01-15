using System;

namespace System.NetFrancis.Http
{
    #region 提供HttpRequestRecording的委托
    #region 有参数
    /// <summary>
    /// 通过指定参数，提供一个<see cref="HttpRequestRecording"/>
    /// </summary>
    /// <typeparam name="Parameters">参数的类型</typeparam>
    /// <param name="Parameters">指定的参数</param>
    /// <returns></returns>
    public delegate HttpRequestRecording ProvideHttpRequest<in Parameters>(Parameters Parameters);
    #endregion
    #region 无参数
    /// <summary>
    /// 调用这个委托以提供一个<see cref="HttpRequestRecording"/>
    /// </summary>
    /// <returns></returns>
    public delegate HttpRequestRecording ProvideHttpRequest();
    #endregion
    #endregion
}
