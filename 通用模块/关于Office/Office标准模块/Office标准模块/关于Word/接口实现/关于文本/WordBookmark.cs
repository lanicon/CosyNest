﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Maths;

namespace System.Office.Word.Realize
{
    /// <summary>
    /// 这个类型是<see cref="IWordBookmark"/>的实现，
    /// 可以视为一个Word书签
    /// </summary>
    public class WordBookmark : IWordBookmark
    {
        #region 书签的位置
        public int Pos { get; private set; }
        #endregion
        #region 书签所在的文档
        public IWordDocument Document { get; }
        #endregion
        #region 构造函数
        #region 辅助方法
        #region 用来注册事件的函数
        /// <summary>
        /// 这个函数被用来注册到文档的LengthChange事件，
        /// 使文本被修改时，书签位置能够同步更新
        /// </summary>
        /// <param name="R">被修改的范围</param>
        /// <param name="Change">修改前和修改后的片段的字数差异</param>
        private void LengthChange(Range R, int Change)
        {
            var Begin = R.Start.Value;
            var End = R.End.Value - 1;
            switch (IInterval.CheckInInterval(Pos, Begin, End))
            {
                case IntervalPosition.Overflow:
                    Pos += Change;
                    break;
                case IntervalPosition.Located when Change < 0:
                    Pos = Math.Min(Pos, End + Change);
                    break;
            }
        }

        /*说明文档：
           在文本被修改时，书签位置的变动遵循以下原则：
           1.如果被修改的片段在当前位置之前，
           将当前位置加上变更的字数
           
           2.如果被修改的片段在当前位置之后，则位置不变，
           因为这个修改不影响到位置
           
           3.如果当前位置正好处于被修改的片段内部，则区分两种情况：
           3.1.如果字数增加了，则位置不变
           3.2.如果字数减少了，则获取当前位置和被缩减的片段末尾，取其较小者*/
        #endregion
        #endregion
        #region 用指定的文档和初始位置
        /// <summary>
        /// 用指定的文档和初始位置构造对象
        /// </summary>
        /// <param name="Document">书签所在的文档</param>
        /// <param name="Pos">书签的初始位置</param>
        public WordBookmark(IWordDocument Document, int Pos)
        {
            this.Document = Document;
            this.Pos = Pos;
            Document.LengthChange += LengthChange;
        }
        #endregion
        #endregion
    }
}
