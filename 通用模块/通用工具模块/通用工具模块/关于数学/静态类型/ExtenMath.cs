using System.IO.Pipelines;
using System.Text;
using System.Threading.Tasks;

namespace System.Maths
{
    /// <summary>
    /// 有关数学的静态类
    /// </summary>
    public static class ExtenMath
    {
        #region 有关二进制转换
        #region 二进制转换字符串
        /// <summary>
        /// 
        /// </summary>
        /// <param name="convert"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static async Task<string> ConvertText(this BinaryConvert convert, string text)
        {
            var arry = Encoding.Unicode.GetBytes(text);
            var pipe = new Pipe();
            await pipe.Writer.WriteAsync(arry);
            await pipe.Writer.CompleteAsync();
            var results = await convert(pipe.Reader);
        }
        #endregion
        #endregion
    }
}
