using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace System.DataFrancis
{
    /// <summary>
    /// 这个类型可以将数据同时添加到多个管道中
    /// </summary>
    class DataAddDistribute : IDataPipeAdd
    {
        #region 数据的去向
        /// <summary>
        /// 数据将被添加到这些管道中
        /// </summary>
        private IEnumerable<IDataPipeAdd> Pipes { get; }
        #endregion
        #region 缓冲区大小
        /// <summary>
        /// 获取缓冲区的大小，以元素数量为单位，为提升性能，
        /// 数据先填充到缓冲区，然后再发送给数据管道
        /// </summary>
        private int Buffer { get; }
        #endregion
        #region 同步添加数据
        public Task Add(IEnumerable<IData> datas, bool binding = false)
            => Task.Run(() => datas.Split(Buffer, true).
            ForEach(d => Pipes.ForEach(pipe => pipe.Add(d, binding))));
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的参数初始化对象
        /// </summary>
        /// <param name="pipes">数据将被添加到这些管道中</param>
        /// <param name="buffer">指定缓冲区的大小，以元素数量为单位，为提升性能，
        /// 数据先填充到缓冲区，然后再发送给数据管道</param>
        public DataAddDistribute(IEnumerable<IDataPipeAdd> pipes, int buffer)
        {
            ExceptionIntervalOut.Check(1, null, buffer);
            this.Pipes = pipes.ToArray();
            this.Buffer = buffer;
        }
        #endregion
    }
}
