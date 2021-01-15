﻿using System;
using System.Collections.Generic;
using System.Text;

namespace System.Time.Plan
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个计划任务触发器
    /// </summary>
    public interface IPlanTrigger
    {
        #region 说明文档
        /*问：为什么本接口没有任何成员？
          答：因为触发器存在一种特殊情况，
          那就是在硬件启动时执行计划任务，
          由于没有固定的运行时间，这种情况难以抽象，
          但出于归一化的要求，所有功能相似的类型必须实现共同的接口，
          所以就定义了一个没有任何成员的类型，作为所有所有触发器的基接口，
          但是这是不得已为之，事实上，抽象这种模式最合适的方式应该是F#中的可区分联合，
          如果C#以后支持这个功能，请将其重构*/
        #endregion
    }
}
