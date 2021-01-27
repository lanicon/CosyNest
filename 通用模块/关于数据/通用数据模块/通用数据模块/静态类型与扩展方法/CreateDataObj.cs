using System.Collections.Generic;
using System.Design.Direct;
using System.IOFrancis;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace System.DataFrancis
{
    /// <summary>
    /// 这个静态类可以用来帮助创建一些关于数据的对象
    /// </summary>
    public static class CreateDataObj
    {
        #region 创建基本数据类型
        #region 创建IData
        #region 用指定的列名和值
        /// <summary>
        /// 用指定的列名和值创建<see cref="IData"/>
        /// </summary>
        /// <param name="Parameters">这个数组的元素是一个元组，
        /// 分别指示列的名称和值</param>
        public static IData Data(params (string Column, object? Value)[] Parameters)
            => Data(Parameters.ToDictionary(true));
        #endregion
        #region 用指定的列名
        /// <summary>
        /// 用指定的数据列名创建<see cref="IData"/>
        /// </summary>
        /// <param name="ColumnName">指定的列名，一经初始化不可增减</param>
        public static IData Data(params string[] ColumnName)
            => Data(ColumnName.Select(x => (x, (object?)null)).ToArray());
        #endregion
        #region 用指定的字典
        /// <summary>
        /// 用一个键是列名的键值对集合（通常是字典）创建<see cref="IData"/>
        /// </summary>
        /// <param name="Dict">一个键值对集合，它的元素的键</param>
        /// <param name="CopyValue">如果这个值为真，则会复制键值对的值，否则不复制</param>
        public static IData Data(IEnumerable<KeyValuePair<string, object?>> Dict, bool CopyValue = true)
            => new Datas(Dict, CopyValue);
        #endregion
        #region 将多条数据合并 
        /// <summary>
        /// 将一条数据和另一些数据合并，并返回合并后的新数据
        /// </summary>
        /// <param name="Data">待合并的数据</param>
        /// <param name="DataMerge">待合并的另一些数据，
        /// 如果存在列名相同的数据，则以后面的数据为准</param>
        /// <returns></returns>
        public static IData Data(IDirect Data, params (string Name, object? Value)[] DataMerge)
            => CreateDataObj.Data(Data.Union
                (DataMerge.Select
                (x => x.ToKV())));
        #endregion
        #endregion
        #endregion
        #region 创建数据管道
        #region 创建IDataPipeQuery
        #region 合并其他管道的输出
        /// <summary>
        /// 创建一个数据查询管道，
        /// 它可以合并其他管道的输出，
        /// 并将其作为一个新的管道
        /// </summary>
        /// <param name="DataSource">实际用来获取数据的管道</param>
        /// <returns></returns>
        public static IDataPipeQuery QueryMerge(params IDataPipeQuery[] DataSource)
            => new DataQueryMerge(DataSource);
        #endregion
        #region 从集合中获取数据
        /// <summary>
        /// 创建一个数据查询管道，
        /// 它可以从一个集合中获取数据
        /// </summary>
        /// <param name="Datas">被作为数据源的集合</param>
        /// <returns></returns>
        public static IDataPipeQuery QueryFromCollection(IEnumerable<IData> Datas)
            => new DataQueryFromCollection(Datas);
        #endregion
        #endregion
        #region 创建IDataPipeAdd
        #region 将数据添加到多个数据源
        /// <summary>
        /// 创建一个数据添加管道，
        /// 它可以同时将数据添加到多个管道中
        /// </summary>
        /// <param name="Pipes">数据将被添加到这些管道中</param>
        /// <param name="Buffer">指定缓冲区的大小，以元素数量为单位，为提升性能，
        /// 数据先填充到缓冲区，然后再发送给数据管道</param>
        /// <returns></returns>
        public static IDataPipeAdd AddDistribute(IEnumerable<IDataPipeAdd> Pipes, int Buffer)
            => new DataAddDistribute(Pipes, Buffer);
        #endregion
        #endregion
        #region 创建IDataPipe
        #region 从文本文件中读写数据
        /// <summary>
        /// 创建一个通过文本文件读写数据的管道
        /// </summary>
        /// <param name="Path">读取或保存数据的路径</param>
        /// <param name="Separated">属性的分隔符，
        /// 它在文本文件的一行中分隔<see cref="IData"/>的属性</param>
        /// <param name="MappingNull">指定将属性中的<see langword="null"/>值映射为哪个字符</param>
        /// <param name="ColumnType">枚举每一列的数据类型，如果为<see langword="null"/>，默认全部为<see cref="string"/>，
        /// 这个参数不会对写入数据产生影响</param>
        /// <returns></returns>
        public static IDataPipe PipeFromFile(PathText Path, string Separated = "\t", string MappingNull = "@null@", IEnumerable<Type>? ColumnType = null)
            => new DataPipeFromFile(Path, Separated, MappingNull, ColumnType);
        #endregion
        #endregion
        #endregion
        #region 创建序列化和反序列化对象
        #region 返回支持序列化数据的JsonSerializerOptions
        /// <summary>
        /// 返回一个支持序列化和反序列化<see cref="IDirect"/>和<see cref="IData"/>的
        /// <see cref="JsonSerializerOptions"/>对象
        /// </summary>
        /// <returns></returns>
        public static JsonSerializerOptions JsonOptionsDirect()
        {
            var op = new JsonSerializerOptions();
            op.Converters.Add(new JsonConverterIDirect());
            return op;
        }
        #endregion
        #endregion
    }
}
