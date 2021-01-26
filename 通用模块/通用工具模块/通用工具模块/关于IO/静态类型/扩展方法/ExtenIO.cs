using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    /// <summary>
    /// 关于IO的扩展方法
    /// </summary>
    public static class ExtenIO
    {
        #region 根据文件类型筛选文件集合
        /// <summary>
        /// 在一个文件集合中，筛选出所有与指定文件类型兼容的文件
        /// </summary>
        /// <param name="List">要筛选的集合</param>
        /// <param name="fileType">作为条件的路径对象</param>
        /// <param name="IsForward">如果这个值为<see langword="true"/>，选择是这个类型的文件，
        /// 否则选择不是这个类型的文件，即取反</param>
        /// <returns></returns>
        public static IEnumerable<IFile> WhereFileType(this IEnumerable<IFile> List, IFileType fileType, bool IsForward = true)
            => List.Where(x => x.IsCompatible(fileType) == IsForward);
        #endregion
        #region 关于流
        #region 重置流
        /// <summary>
        /// 将流重置到开始位置
        /// </summary>
        /// <param name="stream">待重置的流</param>
        /// <param name="ForceReset">如果这个值为<see langword="true"/>，
        /// 则即便流不支持查找，也会将流重置，
        /// 它可以保证流一定会被重置，但有可能引发<see cref="NotSupportedException"/>异常</param>
        public static void Reset(this Stream stream, bool ForceReset = false)
        {
            if ((ForceReset || stream.CanSeek) && stream.Position != 0)
                stream.Position = 0;
        }
        #endregion
        #region 关于读取
        #region 读取字符
        /// <summary>
        /// 将流解码为字符，并获取一个字符中所有行的枚举器
        /// </summary>
        /// <param name="stream">要读取字符的流</param>
        /// <param name="encoding">字符的编码格式，如果为<see langword="null"/>，则默认为UTF8</param>
        /// <returns></returns>
        public static IEnumerable<string> ReadText(this Stream stream, Encoding? encoding = null)
        {
            stream.Reset();
            using var Read = new StreamReader(stream, encoding ?? Encoding.UTF8);
            while (true)
            {
                var Text = Read.ReadLine();
                if (Text == null)
                    yield break;
                else yield return Text;
            }
        }
        #endregion
        #region 读取流的全部字符
        /// <summary>
        /// 读取流中的全部字符
        /// </summary>
        /// <param name="stream">要读取字符的流</param>
        /// <param name="encoding">字符的编码格式，如果为<see langword="null"/>，则默认为UTF8</param>
        /// <returns></returns>
        public static string ReadTextAll(this Stream stream, Encoding? encoding = null)
            => ReadText(stream, encoding).Join();
        #endregion
        #region 读取流的全部内容
        /// <summary>
        /// 一次性读取流的全部内容，
        /// 并作为字节数组返回
        /// </summary>
        /// <param name="stream">待读取内容的流</param>
        /// <returns></returns>
        public static byte[] ToArray(this Stream stream)
        {
            stream.Reset();
            if (stream is MemoryStream m)
                return m.ToArray();
            var len = stream.Length;
            var arry = new byte[len];
            var pos = 0L;
            while (pos < len)
            {
                pos += stream.Read(arry, (int)pos, (int)len);
            }
            return arry;
        }
        #endregion
        #endregion
        #region 关于写入
        #region 写入字符
        /// <summary>
        /// 向流中写入字符
        /// </summary>
        /// <param name="stream">要写入字符的流</param>
        /// <param name="Texts">要写入的字符</param>
        /// <param name="WriterLine">如果这个值为<see langword="true"/>，则会在字符后面自动加上换行符</param>
        /// <param name="encoding">字符的编码格式，如果为<see langword="null"/>，则默认为UTF8</param>
        public static void WriteText(this Stream stream, IEnumerable<string> Texts, bool WriterLine = false, Encoding? encoding = null)
        {
            using var Writer = new StreamWriter(stream, encoding ?? Encoding.UTF8);
            var Del = WriterLine ? new Action<string>(x => Writer.WriteLine(x)) : x => Writer.Write(x);
            Texts.ForEach(Del);
        }
        #endregion
        #region 一次性向流写入字节数组
        /// <summary>
        /// 一次性向流中写入字节数组的全部内容
        /// </summary>
        /// <param name="stream">待写入字节数组的流</param>
        /// <param name="arry">待写入的字节数组</param>
        public static Task Write(this Stream stream, byte[] arry)
            => stream.WriteAsync(arry, 0, arry.Length);
        #endregion
        #region 将流保存到文件中
        /// <summary>
        /// 将流保存到指定的路径中
        /// </summary>
        /// <param name="stream">待保存的流</param>
        /// <param name="path">保存路径</param>
        public static Task Save(this Stream stream, PathText path)
        {
            if (!stream.CanRead)
                throw new NotSupportedException("流不支持读取，无法保存到路径中");
            stream.Reset();
            using var file = new FileStream(path, FileMode.Create);
            return stream.CopyToAsync(file);
        }
        #endregion
        #endregion
        #endregion
    }
}
