using System.IO.Pipelines;
using System.Threading.Tasks;

namespace System.Safety.Algorithm
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个摘要算法
    /// </summary>
    public interface IHash
    {
        #region 摘要数据
        /// <summary>
        /// 读取指定管道中的数据，并对其进行摘要
        /// </summary>
        /// <param name="reader">待读取数据的管道</param>
        /// <returns>通过摘要算法生成的哈希值结果</returns>
        Task<ReadOnlyMemory<byte>> Summary(PipeReader reader);
        #endregion
        #region 验证数据
        /// <summary>
        /// 验证数据的完整性
        /// </summary>
        /// <param name="hash">用来和数据进行比较的哈希值</param>
        /// <param name="reader">函数会从这个管道中读取待验证的数据</param>
        /// <returns>如果返回<see langword="true"/>，代表数据完整，否则代表数据不完整</returns>
        async Task<bool> Verify(ReadOnlyMemory<byte> hash, PipeReader reader)
             => hash.Equals(await Summary(reader));
        #endregion
    }
}
