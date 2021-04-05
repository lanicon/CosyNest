using System.Design;
using System.IO;
using System.IOFrancis.Bit;
using System.IOFrancis.FileSystem;
using System.Performance;

namespace System.Office
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个Office文件，
    /// 通过它可以加载Word文档和Excel工作簿
    /// </summary>
    public interface IOfficeFile : IDisposablePro
    {
        #region 返回文件路径
        /// <summary>
        /// 返回该Office文件的绝对文件路径，
        /// 如果为<see langword="null"/>，代表尚未保存到文件中
        /// </summary>
        string? Path { get; }
        #endregion
        #region 关于保存
        #region 保存Office文件
        /// <summary>
        /// 在指定的路径保存Office文件
        /// </summary>
        /// <param name="path">指定的保存路径，
        /// 如果为<see langword="null"/>，代表原地保存</param>
        void Save(PathText? path = null);

        /*在实现本API时，请遵循以下规范：
          #如果不需要保存或不能保存，就不执行保存操作

          不需要保存的情况举例：
          该Office文件未修改，而且保存模式为原地保存
        
          不能保存的情况举例：
          该Office文件是一个Excel工作簿，
          而且其中没有任何工作表，保存它会出现异常，
          但如果该工作簿尚未保存到文件中，且没有指定保存路径，
          这种情况下应该引发异常，因为这是由于调用者的疏忽导致*/
        #endregion
        #region 返回代表Office文件的流
        /// <summary>
        /// 返回代表Office文件的流
        /// </summary>
        /// <returns></returns>
        IBitRead Read()
        {
            var format = Path is { } p ? ToolPath.Split(p).Extended : "";
            var file = ToolPerfo.CreateTemporaryFile(format);
            Save(file.Path);
            file.Refresh();
            return file.GetBitPipe(FileMode.Open);
        }
        #endregion
        #region 是否自动保存
        /// <summary>
        /// 如果这个值为真，
        /// 则在执行<see cref="IDisposable.Dispose"/>方法时，还会自动保存文件，
        /// 前提是文件的路径不为<see langword="null"/>
        /// </summary>
        bool AutoSave { get; set; }
        #endregion
        #endregion 
    }
}
