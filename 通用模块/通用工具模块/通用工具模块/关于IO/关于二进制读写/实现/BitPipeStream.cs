using System.Collections.Generic;
using System.Design;
using System.Threading.Tasks;

namespace System.IO
{
    /// <summary>
    /// 这个类型是<see cref="IBitPipe"/>的实现，
    /// 它通过流来读写二进制数据
    /// </summary>
    class BitPipeStream : AutoRelease, IBitPipe
    {
        #region 封装的对象
        #region Stream对象
        /// <summary>
        /// 获取封装的<see cref="IO.Stream"/>对象，
        /// 本对象的功能就是通过它实现的
        /// </summary>
        private Stream Stream { get; }
        #endregion
        #region 缓冲区大小
        /// <summary>
        /// 指定缓冲区的大小，每次读取数据时，
        /// 至多返回该数量的字节，如果为<see langword="null"/>，
        /// 则一次读取全部数据
        /// </summary>
        private long? BufferSize { get; }
        #endregion
        #endregion
        #region 接口实现
        #region 数据的格式
        public string? Format { get; }
        #endregion
        #region 释放对象
        protected override void DisposeRealize()
            => Stream.Dispose();
        #endregion
        #region 数据的长度
        public long Length => Stream.Length;
        #endregion
        #region 读取数据
        public async IAsyncEnumerable<ReadOnlyMemory<byte>> Read()
        {
            if (Stream.Position is not 0)
                Stream.Position = 0;
            var bufferSize = BufferSize ?? Length;
            while (true)
            {
                var memory = new byte[bufferSize].AsMemory();
                switch (await Stream.ReadAsync(memory))
                {
                    case 0:
                        yield break;
                    case var c when c < bufferSize:
                        yield return memory[0..c]; break;
                    case var c:
                        yield return memory; break;
                }
            }
        }
        #endregion
        #region 写入数据
        public async Task Write(ReadOnlyMemory<byte> data)
            => await Stream.WriteAsync(data);
        #endregion
        #endregion 
        #region 构造函数
        /// <summary>
        /// 使用指定的<see cref="IO.Stream"/>对象初始化对象
        /// </summary>
        /// <param name="Stream">封装的<see cref="IO.Stream"/>对象，本对象的功能就是通过它实现的</param>
        /// <param name="BufferSize">缓冲区的大小，每次读取数据时，
        /// 至多返回该数量的字节，如果为<see langword="null"/>，则一次读取全部数据</param>
        /// <param name="Format">二进制数据的格式，如果格式未知，则为<see langword="null"/></param>
        public BitPipeStream(Stream Stream, long? BufferSize, string? Format)
        {
            if (BufferSize is { })
                ExceptionIntervalOut.Check(1L, null, BufferSize.Value);
            this.Stream = Stream;
            this.BufferSize = BufferSize;
            this.Format = Format;
        }
        #endregion
    }
}
