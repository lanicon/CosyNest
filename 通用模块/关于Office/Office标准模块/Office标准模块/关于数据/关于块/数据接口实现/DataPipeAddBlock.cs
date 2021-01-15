using System;
using System.Collections.Generic;
using System.Design.Direct;
using System.Linq;
using System.Office.Excel;

namespace System.DataFrancis.Excel.Block
{
    /// <summary>
    /// 这个类型是<see cref="IDataPipeAdd"/>的实现，
    /// 可以通过块向Excel工作簿提交数据
    /// </summary>
    class DataPipeAddBlock : IDataPipeAdd
    {
        #region 获取是否支持绑定
        public bool CanBinding => true;
        #endregion
        #region 添加数据所需要的信息
        #region 块地图
        /// <summary>
        /// 获取块地图，它描述块的特征
        /// </summary>
        private IBlockMap Map { get; }
        #endregion
        #region 用来返回下一个块的枚举器
        /// <summary>
        /// 这个枚举器用来返回下一个块
        /// </summary>
        private IEnumerator<IExcelCells> Next { get; }
        #endregion
        #endregion
        #region 同步添加数据
        #region 辅助方法
        #region 写入标题
        /// <summary>
        /// 辅助方法，向块中写入标题
        /// </summary>
        private void SetTitle()
        {
            var Title = Map.Property.Values.Select(x => x.SetTitle).Where(x => x != null).ToArray();
            if (Title.Any())
            {
                var IsH = Map.IsHorizontal;
                var TitleCell = Next.Current.Offset(Right: IsH ? -1 : 0, Down: IsH ? 0 : -1);
                Title.ForEach(x => x!(TitleCell));
            }
        }
        #endregion
        #region 遍历并添加数据
        /// <summary>
        /// 辅助方法，遍历并添加数据，然后返回数据的数量
        /// </summary>
        /// <param name="Data">待添加的数据</param>
        /// <param name="Binding">如果这个值为<see langword="true"/>，
        /// 则在添加数据的时候，还会将数据绑定</param>
        /// <returns></returns>
        private int DataAddAided(IEnumerable<IData> Data, bool Binding)
            => Data.Aggregate(0, (seed, data) =>
            {
                var Current = Next.Current;
                foreach (var (name, (_, set, _)) in Map.Property)
                {
                    set(Current, data[name]);
                }
                if (Binding)
                    data.Binding = new BlockBinding(Current, Map.Property);
                Next.MoveNext();
                return ++seed;
            });
        #endregion
        #endregion
        #region 正式方法
        public void Add(IDirectView<IData> Data, bool Binding)
        {
            Next.MoveNext();
            var First = Next.Current;
            SetTitle();
            var (_, BC, _, EC) = First.Address;
            var Count = DataAddAided(Data, Binding);
            var End = Map.IsHorizontal ? BC + Count * Map.Size.PixelCount.Horizontal : EC;
            ToolException.Ignore<NotImplementedException>
                (() => First.Sheet.GetRC(BC, End, false).AutoFit());            //自动适配列的大小，提升可读性
        }

        /*问：为什么在适配列的大小的时候，要忽略掉NotImplementedException异常？
          答：因为AutoFit是非关键API，某些Excel模块可能没有实现它，
          如果因为这个，让它们不能使用块来提取数据是不恰当的*/
        #endregion
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的参数初始化对象
        /// </summary>
        /// <param name="Map">块地图，它描述块的特征</param>
        /// <param name="Next">这个枚举器用来返回下一个块</param>
        public DataPipeAddBlock(IBlockMap Map, IEnumerator<IExcelCells> Next)
        {
            this.Map = Map;
            this.Next = Next;
        }
        #endregion
    }
}
