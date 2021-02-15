using System.Collections.Generic;
using System.Linq;
using System.Performance;
using System.Reflection;

namespace System
{
    public static partial class ExtenReflection
    {
        //这个部分类专门储存有关方法，委托，构造函数和事件的扩展方法

        #region 关于委托
        #region 将一个方法对象转换为委托
        /// <summary>
        /// 将一个<see cref="MethodInfo"/>转换为委托
        /// </summary>
        /// <typeparam name="Del">转换的目标类型</typeparam>
        /// <param name="met">要创建委托的方法</param>
        /// <param name="target">用来执行方法的类型实例</param>
        /// <returns></returns>
        public static Del CreateDelegate<Del>(this MethodInfo met, object? target = null)
            where Del : Delegate
            => (Del)Delegate.CreateDelegate(typeof(Del), target, met);

        /*说明文档：
           即便met是实例方法，target参数仍然可以传入null，
           但是在这种情况下，目标委托类型的第一个参数必须为方法实例类型，举例说明：

           假设类型Class有一个实例方法叫Fun，它有一个int参数，无返回值，
           那么假如target为null，目标委托类型应为Action<Class,int>，
           或其他与之签名兼容的委托类型
           
           经测试，这个方法支持委托的协变与逆变，
           但是按照C#的规范，值类型不在支持范围之内，
           测试的时候请多加注意*/
        #endregion
        #region 返回一个类型是否为委托
        /// <summary>
        /// 判断一个类型是否为委托
        /// </summary>
        /// <param name="type">待判断的类型</param>
        /// <returns></returns>
        public static bool IsDelegate(this Type type)
            => typeof(Delegate).IsAssignableFrom(type);
        #endregion
        #endregion
        #region 关于方法
        #region 关于签名
        #region 返回参数的类型
        /// <summary>
        /// 返回参数列表中参数的类型
        /// </summary>
        /// <param name="pars">参数列表</param>
        /// <returns></returns>
        public static Type[] GetParType(this IEnumerable<ParameterInfo> pars)
            => pars.Select(x => x.ParameterType).ToArray();
        #endregion
        #region 返回一个方法或构造函数的签名
        #region 缓存字典
        /// <summary>
        /// 这个字典缓存方法的签名，
        /// 以确保相同的方法签名只会初始化一次
        /// </summary>
        private static ICache<MethodBase, MethodBaseSignature> CacheSignature { get; }
        = CreateCache.CacheThreshold(
            x =>
            {
                var par = x.GetParameters().GetParType();
                return x switch
                {
                    MethodInfo a => (MethodBaseSignature)new MethodSignature(a.ReturnType, par),
                    ConstructorInfo a => new ConstructSignature(par),
                    _ => throw new Exception($"{x}不是方法也不是构造函数"),
                };
            },
            500, CacheSignature);
        #endregion
        #region 返回MethodBase的签名
        /// <summary>
        /// 获取一个方法或构造函数的签名
        /// </summary>
        /// <param name="met">要获取签名的方法或构造函数</param>
        /// <returns></returns>
        public static MethodBaseSignature GetSignature(this MethodBase met)
            => CacheSignature[met];
        #endregion
        #region 返回方法签名
        /// <summary>
        /// 获取一个方法的签名
        /// </summary>
        /// <param name="met">要获取签名的方法</param>
        /// <returns></returns>
        public static MethodSignature GetSignature(this MethodInfo met)
            => (MethodSignature)CacheSignature[met];
        #endregion
        #region 返回构造函数签名
        /// <summary>
        /// 获取一个构造函数的签名
        /// </summary>
        /// <param name="con">要获取签名的构造函数</param>
        /// <returns></returns>
        public static ConstructSignature GetSignature(this ConstructorInfo con)
            => (ConstructSignature)CacheSignature[con];
        #endregion
        #endregion
        #region 返回委托的签名
        #region 已知委托的实例
        /// <summary>
        /// 返回一个委托的签名
        /// </summary>
        /// <param name="del">待返回签名的委托</param>
        /// <returns></returns>
        public static MethodSignature GetSignature(this Delegate del)
            => del.Method.GetSignature();
        #endregion
        #region 已知委托的类型
        /// <summary>
        /// 如果一个类型是委托，则返回它的签名，否则引发异常
        /// </summary>
        /// <param name="type">待返回签名的委托类型</param>
        /// <returns></returns>
        public static MethodSignature GetDelegateSignature(this Type type)
            => type.IsDelegate() ?
            type.GetTypeData().MethodFind("Invoke").GetSignature() :
            throw new ExceptionTypeUnlawful(type, typeof(Delegate));
        #endregion
        #endregion
        #region 判断方法签名是否兼容
        #region 传入方法签名
        /// <summary>
        /// 判断一个方法或构造函数是否与一个签名兼容
        /// </summary>
        /// <param name="met">要判断的方法</param>
        /// <param name="signature">要检查兼容的签名</param>
        /// <param name="ExactlySame">如果这个值为<see langword="true"/>，则要求两个签名完全相同，
        /// 如果为<see langword="false"/>，则可以支持协变与逆变</param>
        /// <returns></returns>
        public static bool IsSame(this MethodBase met, MethodBaseSignature signature, bool ExactlySame = true)
            => met.GetSignature().IsSame(signature, ExactlySame);
        #endregion
        #region 传入另一个方法
        /// <summary>
        /// 判断两个方法或构造函数的签名是否兼容
        /// </summary>
        /// <param name="MetA">要判断的第一个方法</param>
        /// <param name="MetB">要判断的第二个方法</param>
        /// <param name="ExactlySame">如果这个值为<see langword="true"/>，则要求两个签名完全相同，
        /// 如果为<see langword="false"/>，则可以支持协变与逆变</param>
        /// <returns></returns>
        public static bool IsSame(this MethodBase MetA, MethodBase MetB, bool ExactlySame = true)
            => MetA.IsSame(MetB.GetSignature(), ExactlySame);
        #endregion
        #endregion
        #endregion
        #region 调用方法
        /// <summary>
        /// 调用一个<see cref="MethodInfo"/>
        /// </summary>
        /// <typeparam name="Ret">返回值类型</typeparam>
        /// <param name="method">要调用的方法</param>
        /// <param name="target">调用方法的目标，如果是静态方法，则为<see langword="null"/></param>
        /// <param name="parameters">方法的参数列表</param>
        /// <returns>方法的返回值</returns>
        public static Ret? Invoke<Ret>(this MethodInfo method, object? target, params object?[] parameters)
            => (Ret)method.Invoke(target, parameters);
        #endregion
        #region 调用构造函数
        /// <summary>
        /// 调用一个构造函数
        /// </summary>
        /// <typeparam name="Ret">返回值类型</typeparam>
        /// <param name="constructor">要调用的构造函数，如果它是静态构造函数，则引发异常</param>
        /// <param name="parameters">构造函数的参数列表</param>
        /// <returns>调用构造函数所构造出的对象</returns>
        public static Ret Invoke<Ret>(this ConstructorInfo constructor, params object?[] parameters)
            => constructor.IsStatic ? throw new ArgumentException("无法通过静态构造函数构造对象") : (Ret)constructor.Invoke(parameters);
        #endregion
        #endregion
        #region 关于事件
        #region 根据布尔值，注册或注销事件
        /// <summary>
        /// 根据一个布尔值，从事件中注册或注销委托
        /// </summary>
        /// <param name="event">待注册或注销委托的事件</param>
        /// <param name="target">事件所依附的对象实例，如果为静态事件，则为<see langword="null"/></param>
        /// <param name="IsAdd">如果这个值为<see langword="true"/>，则注册事件，否则注销事件</param>
        /// <param name="delegate">待注册或注销的委托</param>
        public static void AddOrRemove(this EventInfo @event, object? target, bool IsAdd, Delegate @delegate)
        {
            if (IsAdd)
                @event.AddEventHandler(target, @delegate);
            else @event.RemoveEventHandler(target, @delegate);
        }
        #endregion
        #endregion
    }
}
