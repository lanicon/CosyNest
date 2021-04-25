using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Design.Async
{
    /// <summary>
    /// 这个类型为异步事件提供支持
    /// </summary>
    /// <typeparam name="Event">异步事件的委托类型</typeparam>
    public class AsyncEvent<Event>
         where Event : Delegate
    {

    }
}
