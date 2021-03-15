using System.Maths;

namespace System.Mapping.Settlement
{
    /// <summary>
    /// 凡是实现这个接口的类型，都可以视为沉降观测中的一站，
    /// 它的高程不是已知的，而是需要通过记录计算出来
    /// </summary>
    public interface ISettlementObservation : ISettlement
    {
        #region 说明文档
        /*问：为什么本接口不区分前站和后站？
          答：因为前站和后站是一对一的关系，
          它只适合手工记录，不适合使用编程中的树形结构进行描述，
          本接口中的记录指的是观测这一站的沉降点或上一站的转点所得到的结果，
          它不分方向，也不分前视后视*/
        #endregion
        #region 记录
        /// <summary>
        /// 获取这一站的记录，
        /// 也就是观测这一站的沉降点或上一站的转点所得到的结果
        /// </summary>
        IUTLength Recording { get; }
        #endregion
    }
}
