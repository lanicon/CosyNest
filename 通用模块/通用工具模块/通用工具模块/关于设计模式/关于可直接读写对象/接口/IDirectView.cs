using System.Collections.Generic;

namespace System.Design.Direct
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个拥有架构的数据容器，
    /// 被放置其中的数据对象具有一致的架构
    /// </summary>
    /// <typeparam name="Direct">容器所容纳的数据类型</typeparam>
    public interface IDirectView<out Direct> : IEnumerable<Direct>
        where Direct : IDirect
    {
        #region 说明文档
        /*问：为什么要设计本接口？
          答：在没有本接口的时候，通常直接使用IDirect来读写数据，
          但是如果有多个架构不同的IDirect放在同一个容器中，
          则很容易引发属性不存在或类型不正确的异常，
          因此设计者定义了一个专用的容器，
          凡是存放在同一容器的IDirect，都可以保证架构相同
          
          问：实现本接口需要遵循哪些规范？
          答：#在向本容器添加IDirect的时候，需要检查新对象的架构，如果不一致，则应引发异常，
          如果IDirect的Schema为null，还应该将约束写入
        
          #在枚举完毕本对象后，应该将枚举的元素缓存起来，
          这是因为获取数据（可能需要与服务器进行通讯）和检查架构的开销较大，
          如果每次枚举都会重复执行，会造成严重的性能问题*/
        #endregion
        #region 返回架构
        /// <summary>
        /// 返回这个集合的架构，
        /// 它决定了这个集合所能容纳的数据的名称和类型
        /// </summary>
        ISchema Schema { get; }
        #endregion
    }
}
