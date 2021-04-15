namespace Microsoft.Extensions.DependencyInjection
{
    #region 委托定义
    /// <summary>
    /// 通过服务的名称获取服务
    /// </summary>
    /// <typeparam name="Service">要获取的服务类型</typeparam>
    /// <param name="serviceName">服务的名称</param>
    /// <returns></returns>
    public delegate Service NamingService<out Service>(string serviceName);

    /*问：该委托具有什么意义？
      答：在依赖注入中，有些服务具有相同的类型和不同的应用场合，
      举例说明：可能有两个服务，它们都是ICryptology，但是包含的密钥不同，
      而ASPNet的依赖注入在请求服务时，只根据类型和服务生存期进行区分，
      这不足以满足需求，因此作者声明了本接口，
      它可以根据名称请求类型相同，但是适用场景不同的服务*/
    #endregion
}
