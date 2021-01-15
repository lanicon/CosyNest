using System;

namespace System.Design
{
    /// <summary>
    /// 这个类型可以帮助实现单例模式
    /// </summary>
    /// <typeparam name="Obj">单例模式所返回的对象，
    /// 要求具有无参数构造函数，访问修饰符不限</typeparam>
    public abstract class Singleton<Obj>
        where Obj : Singleton<Obj>
    {
        #region 返回对象的唯一实例
        /// <summary>
        /// 返回这个对象的唯一实例
        /// </summary>
        public static Obj Only { get; }
        = typeof(Obj).GetTypeData().ConstructorCreate<Obj>();
        #endregion
    }
}
