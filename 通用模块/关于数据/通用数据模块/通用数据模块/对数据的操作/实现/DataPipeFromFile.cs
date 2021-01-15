using System;
using System.Collections.Generic;
using System.Design.Direct;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace System.DataFrancis
{
    /// <summary>
    /// 这个类型可以从文本文件中读写数据
    /// </summary>
    class DataPipeFromFile : IDataPipe
    {
        #region 辅助成员
        #region 保存文件的路径
        /// <summary>
        /// 获取读取或保存数据的路径
        /// </summary>
        private string Path { get; }
        #endregion
        #region 属性的分隔符
        /// <summary>
        /// 获取属性的分隔符，
        /// 它在文本文件的一行中分隔<see cref="IData"/>的属性
        /// </summary>
        private string Separated { get; }
        #endregion
        #region 指定处理null值的方式
        /// <summary>
        /// 指定将属性中的<see langword="null"/>值映射为哪个字符
        /// </summary>
        private string MappingNull { get; }
        #endregion
        #region 枚举每一列的数据类型
        /// <summary>
        /// 枚举每一列的数据类型，如果为<see langword="null"/>，默认全部为<see cref="string"/>，
        /// 这个对象不会对写入数据产生影响
        /// </summary>
        private Type[] ColumnType { get; }
        #endregion
        #endregion
        #region 添加数据
        public void Add(IDirectView<IData> Data, bool Binding)
            => CreateIO.File(Path, false).Atomic(x =>
             {
                 using var steam = x.To<IFile>().GetStream().Stream;
                 using var writer = new StreamWriter(steam);
                 Data.ForEachSplit((data, del) =>
                 {
                     writer.WriteLine(data.Keys.Join(Separated));      //写入标头
                     del();
                 }, data => writer.WriteLine(data.Join(x => (x.Value ?? MappingNull).ToString()!, Separated)));
             });
        #endregion
        #region 查询数据
        public IDirectView<IData> Query(Expression<Func<PlaceholderData, bool>>? Expression, bool Binding)
        {
            using var read = File.OpenText(Path);
            #region 用于提取数据的本地函数
            IEnumerable<IData> Get()
            {
                #region 用于拆分读取到的文本的本地函数
                string[]? Split()
                    => read.ReadLine()?.Split(Separated);
                #endregion
                var first = Split();
                if (first is null)
                    yield break;
                var template = CreateDataObj.Data(first);
                var ColumnType = this.ColumnType.ZipFill(template.Keys, (x, y) => (y, x), () => typeof(string)).ToDictionary(true);
                while (!read.EndOfStream)
                {
                    var NewData = template.Copy(false);
                    foreach (var (key, value) in template.Keys.Zip(Split()!, true))
                        NewData[key] = value == MappingNull ? null : Convert.ChangeType(value, ColumnType[key]);
                    yield return NewData;
                }
            }
            #endregion
            return Get().ToDirectView();
        }
        #endregion
        #region 删除数据
        public void Delete(Expression<Func<PlaceholderData, bool>> Expression)
            => throw new NotImplementedException("不支持此API");
        #endregion
        #region 构造方法
        /// <summary>
        /// 使用指定的参数构造对象
        /// </summary>
        /// <param name="Path">读取或保存数据的路径</param>
        /// <param name="Separated">属性的分隔符，
        /// 它在文本文件的一行中分隔<see cref="IData"/>的属性</param>
        /// <param name="MappingNull">指定将属性中的<see langword="null"/>值映射为哪个字符</param>
        /// <param name="ColumnType">枚举每一列的数据类型，如果为<see langword="null"/>，默认全部为<see cref="string"/>，
        /// 这个参数不会对写入数据产生影响</param>
        public DataPipeFromFile(PathText Path, string Separated = "\t", string MappingNull = "@null@", IEnumerable<Type>? ColumnType = null)
        {
            this.Path = Path;
            this.Separated = Separated;
            this.MappingNull = MappingNull;
            this.ColumnType = ColumnType?.ToArray() ?? Array.Empty<Type>();
        }
        #endregion
    }
}
