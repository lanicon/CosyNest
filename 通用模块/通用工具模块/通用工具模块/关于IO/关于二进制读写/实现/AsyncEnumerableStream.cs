using System.Collections.Generic;
using System.IO;

namespace System.IOFrancis.Bit
{
    /// <summary>
    /// 这个类型可以借助<see cref="IAsyncEnumerable{T}"/>来获取二进制数据，
    /// 它同时是一个<see cref="Stream"/>和<see cref="IBitPipe"/>
    /// </summary>
    class AsyncEnumerableStream : Stream, IBitRead
    {
        #region 封装的对象
        #region 枚举器
        /// <summary>
        /// 获取通过<see cref="Bytes"/>创建的异步枚举器对象，
        /// 当它被刷新时，代表从头开始读取数据
        /// </summary>
        private IAsyncEnumerator<byte[]>? Enumerator { get; set; }
        #endregion
        #region 迭代器
        /// <summary>
        /// 获取一个枚举所有数据的异步迭代器，
        /// 本对象的功能就是通过它实现的
        /// </summary>
        private IAsyncEnumerable<byte[]> Bytes { get; }
        #endregion
        #endregion 
        #region Stream的实现
        #region 清除缓冲区
        public override void Flush()
        {

        }
        #endregion
        #region 查找流
        public override long Seek(long offset, SeekOrigin origin)
        {
            if ((offset, origin) is not (0, SeekOrigin.Begin))
                throw new NotSupportedException($"仅支持跳转到流的开头，" +
                    $"除{nameof(offset)}为0，{nameof(origin)}为{SeekOrigin.Begin}以外，" +
                    $"传入其他任何参数都将引发异常");
            return Position = 0;
        }
        #endregion
        #region 设置流的长度
        public override void SetLength(long value)
            => throw CreateException.NotSupported();
        #endregion
        #region 写入流
        public override void Write(byte[] buffer, int offset, int count)
            => throw CreateException.NotSupported();
        #endregion
        #region 是否可读取
        public override bool CanRead => true;
        #endregion
        #region 是否可写入
        public override bool CanWrite => false;
        #endregion
        #region 是否可跳转
        public override bool CanSeek => false;
        #endregion
        #region 流的位置
        private long PositionField;

        public override long Position
        {
            get => PositionField;
            set
            {
                if (value != 0)
                    throw new NotSupportedException("仅支持跳转到流的开头，所以该属性只能写入0");
                this.Enumerator = Bytes.GetAsyncEnumerator();
                PositionField = 0;
            }
        }
        #endregion
        #endregion
        #region IBitRead的实现
        #region 返回流的长度
        public override long Length
            => throw CreateException.NotSupported();
        #endregion
        #region 转换为Stream对象
        public Stream ToStream()
            => this;
        #endregion
        #region 指示是否释放
        public bool IsFree => false;
        #endregion
        #region 对数据的描述
        public string? Describe { get; }
        #endregion
        #region 数据的格式
        public string Format { get; }
        #endregion
        #region 读取数据
        public async IAsyncEnumerable<byte[]> Read(long? bufferSize = null)
        {
            throw new NotImplementedException();
        }
        #endregion
        #endregion
        #region 未实现的成员
        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的参数初始化对象
        /// </summary>
        /// <param name="Bytes">一个枚举所有数据的异步迭代器，本对象的功能就是通过它实现的</param>
        /// <param name="Format">二进制数据的格式，如果格式未知，则为<see cref="string.Empty"/></param>
        /// <param name="Describe">对数据的描述，如果没有描述，则为<see langword="null"/></param>
        public AsyncEnumerableStream(IAsyncEnumerable<byte[]> Bytes, string? Format, string? Describe)
        {
            this.Bytes = Bytes;
            this.Format = Format ?? "";
            this.Describe = Describe;
        }
        #endregion
    }
}
