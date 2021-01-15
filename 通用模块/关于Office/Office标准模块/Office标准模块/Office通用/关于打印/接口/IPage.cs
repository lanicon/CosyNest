using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Underlying;

namespace System.Office
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个页面对象，
    /// 它可以管理Word或Excel的页面设置和打印
    /// </summary>
    public interface IPage : IOfficePrint
    {

    }
}
