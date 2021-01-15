using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// 有关异常的工具类
    /// </summary>
    public static class ToolException
    {
        #region 忽略指定的异常
        #region 无返回值
        /// <summary>
        /// 执行代码块，并忽略掉指定类型的异常
        /// </summary>
        /// <typeparam name="Exception">要忽略的异常类型</typeparam>
        /// <param name="try">在try代码块中执行的委托</param>
        /// <param name="catch">在catch代码块中执行的委托，
        /// 如果为<see langword="null"/>，则不会执行</param>
        /// <param name="finally">在finally代码块中执行的委托，
        /// 如果为<see langword="null"/>，则不会执行</param>
        /// <returns>如果成功执行，未出现异常，则返回<see langword="true"/>，否则返回<see langword="false"/></returns>
        public static bool Ignore<Exception>(Action @try, Action? @catch = null, Action? @finally = null)
            where Exception : System.Exception
        {
            try
            {
                @try();
                return true;
            }
            catch (Exception)
            {
                @catch?.Invoke();
                return false;
            }
            finally
            {
                @finally?.Invoke();
            }
        }
        #endregion
        #region 有返回值
        /// <summary>
        /// 执行代码块，并忽略掉指定类型的异常，并返回返回值
        /// </summary>
        /// <typeparam name="Exception">要忽略的异常类型</typeparam>
        /// <typeparam name="Ret">返回值类型</typeparam>
        /// <param name="try">在try代码块中执行的委托</param>
        /// <param name="catch">当执行<paramref name="try"/>出现异常时，通过这个延迟对象获取返回值</param>
        /// <param name="finally">在finally代码块中执行的委托，
        /// 如果为<see langword="null"/>，则不会执行</param>
        /// <returns>一个元组，它的第一个项是是否执行成功，第二个项通过委托获取的返回值</returns>
        public static (bool IsSuccess, Ret? Return) Ignore<Exception, Ret>(Func<Ret?> @try, LazyPro<Ret>? @catch = null, Action? @finally = null)
            where Exception : System.Exception
        {
            try
            {
                return (true, @try());
            }
            catch (Exception)
            {
                return (false, @catch);
            }
            finally
            {
                @finally?.Invoke();
            }
        }
        #endregion
        #endregion
        #region 忽略业务异常
        #region 说明文档
        /*问：什么是业务异常？
          答：业务异常指由于非技术原因所引发的异常，
          它不是执行程序中所发生的意外，而是本身就是架构设计的一部分，
          例如，假设有一个验证身份的API，因为用户输入密码错误而引发的异常就是业务异常

          问：业务异常和普通异常有什么区别？
          答：业务异常不应该使整个程序崩溃，上层API应该捕获并忽略它们，
          然后执行一些其他操作，例如给予用户一个提示

          问：如果业务中的所有异常都被忽略掉，会导致难以排查Bug吗？
          答：是这样，所以作者认为不宜忽略掉所有异常，而是只忽略掉业务异常
        
          问：如何区分业务异常和普通异常？
          答：按照规范，Exception.Data字典中存在键IsBusinessExceptions，
          且值为true的异常是业务异常，否则为普通异常
        
          问：根据Net规范，不推荐使用异常来处理业务问题，
          因为异常需要获取调用堆栈，成本较高，请问为什么还要这么设计？
          答：首先必须承认，这种设计对性能存在负面影响，
          但如果不这样设计的话，就需要声明一个类型来区分成功的业务和失败的业务，
          这会导致所有可能发生业务异常的API都需要重构返回值类型，
          你可以参考同步方法和异步方法的返回值的区别，就是这种情况，
          它会极大地增加框架的复杂程度*/
        #endregion
        #region 同步方法
        #region 有返回值
        /// <summary>
        /// 执行一个业务，并忽略其中的业务异常
        /// </summary>
        /// <typeparam name="Ret">业务的返回值类型</typeparam>
        /// <param name="business">待执行的业务</param>
        /// <returns>一个元组，它的项分别是执行业务时所引发的业务异常（如果没有发生业务异常，则为<see langword="null"/>），
        /// 以及业务的返回值（如果发生了业务异常，则为默认值）</returns>
        public static (Exception? Exception, Ret? Return) IgnoreBusiness<Ret>(Func<Ret?> business)
        {
            try
            {
                return (null, business());
            }
            catch (Exception ex) when (ex.IsBusinessExceptions())
            {
                return (ex, default);
            }
        }
        #endregion
        #region 无返回值
        /// <summary>
        /// 执行一个业务，并忽略其中的业务异常
        /// </summary>
        /// <param name="business">待执行的业务</param>
        /// <returns>执行业务时发生的业务异常，
        /// 如果没有发生异常，则为<see langword="null"/></returns>
        public static Exception? IgnoreBusiness(Action business)
            => IgnoreBusiness(() =>
            {
                business();
                return default(object);
            }).Exception;
        #endregion
        #endregion
        #region 异步方法
        #region 有返回值
        /// <summary>
        /// 执行一个异步业务，并忽略其中的业务异常
        /// </summary>
        /// <typeparam name="Ret">异步业务的返回值类型</typeparam>
        /// <param name="business">待执行的异步业务</param>
        /// <returns>一个<see cref="Task{TResult}"/>，等待它可以获得一个元组，
        /// 它的项分别是执行业务时所引发的业务异常（如果没有发生业务异常，则为<see langword="null"/>），
        /// 以及业务的返回值（如果发生了业务异常，则为默认值）</returns>
        public static async Task<(Exception? Exception, Ret? Return)> IgnoreBusinessAsync<Ret>(Func<Task<Ret>> business)
        {
            try
            {
                return (null, await business());
            }
            catch (Exception ex) when (ex.IsBusinessExceptions())
            {
                return (ex, default);
            }
        }
        #endregion
        #region 无返回值
        /// <summary>
        /// 执行一个异步业务，并忽略其中的业务异常
        /// </summary>
        /// <param name="business">待执行的异步业务</param>
        /// <returns>执行业务时发生的业务异常，
        /// 如果没有发生异常，则为<see langword="null"/></returns>
        public static async Task<Exception?> IgnoreBusinessAsync(Func<Task> business)
        {
            try
            {
                await business();
                return null;
            }
            catch (Exception ex) when (ex.IsBusinessExceptions())
            {
                return ex;
            }
        }
        #endregion
        #endregion
        #endregion
    }
}
