using System.Collections.Generic;
using System.Design.Direct;
using System.IOFrancis.FileSystem;
using System.Linq;
using System.Text.Json.Serialization;

namespace System.DataFrancis
{
    /// <summary>
    /// 这个静态类可以用来帮助创建一些关于数据的对象
    /// </summary>
    public static class CreateDataObj
    {
        #region 创建IData
        #region 用指定的列名和值
        /// <summary>
        /// 用指定的列名和值创建<see cref="IData"/>
        /// </summary>
        /// <param name="parameters">这个数组的元素是一个元组，
        /// 分别指示列的名称和值</param>
        public static IData Data(params (string Column, object? Value)[] parameters)
            => Data(parameters.ToDictionary(true));
        #endregion
        #region 用指定的列名
        /// <summary>
        /// 用指定的数据列名创建<see cref="IData"/>
        /// </summary>
        /// <param name="columnName">指定的列名，一经初始化不可增减</param>
        public static IData Data(params string[] columnName)
            => Data(columnName.Select(x => (x, (object?)null)).ToArray());
        #endregion
        #region 用指定的字典
        /// <summary>
        /// 用一个键是列名的键值对集合（通常是字典）创建<see cref="IData"/>
        /// </summary>
        /// <param name="dictionary">一个键值对集合，它的元素的键</param>
        /// <param name="copyValue">如果这个值为真，则会复制键值对的值，否则不复制</param>
        public static IData Data(IEnumerable<KeyValuePair<string, object?>> dictionary, bool copyValue = true)
            => new Datas(dictionary, copyValue);
        #endregion
        #region 将多条数据合并 
        /// <summary>
        /// 将一条数据和另一些数据合并，并返回合并后的新数据
        /// </summary>
        /// <param name="data">待合并的数据</param>
        /// <param name="dataMerge">待合并的另一些数据，
        /// 如果存在列名相同的数据，则以后面的数据为准</param>
        /// <returns></returns>
        public static IData Data(IDirect data, params (string Name, object? Value)[] dataMerge)
            => CreateDataObj.Data(data.Union
                (dataMerge.Select
                (x => x.ToKV())));
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
        /// <param name="dataSource">实际用来获取数据的管道</param>
        /// <returns></returns>
        public static IDataPipeQuery QueryMerge(params IDataPipeQuery[] dataSource)
            => new DataQueryMerge(dataSource);
        #endregion
        #region 从集合中获取数据
        /// <summary>
        /// 创建一个数据查询管道，
        /// 它可以从一个集合中获取数据
        /// </summary>
        /// <param name="datas">被作为数据源的集合</param>
        /// <returns></returns>
        public static IDataPipeQuery QueryFromCollection(IEnumerable<IData> datas)
            => new DataQueryFromCollection(datas);
        #endregion
        #endregion
        #region 创建IDataPipeAdd
        #region 将数据添加到多个数据源
        /// <summary>
        /// 创建一个数据添加管道，
        /// 它可以同时将数据添加到多个管道中
        /// </summary>
        /// <param name="pipes">数据将被添加到这些管道中</param>
        /// <param name="buffer">指定缓冲区的大小，以元素数量为单位，为提升性能，
        /// 数据先填充到缓冲区，然后再发送给数据管道</param>
        /// <returns></returns>
        public static IDataPipeAdd AddDistribute(IEnumerable<IDataPipeAdd> pipes, int buffer)
            => new DataAddDistribute(pipes, buffer);
        #endregion
        #endregion
        #region 创建IDataPipe
        #region 从文本文件中读写数据
        /// <summary>
        /// 创建一个通过文本文件读写数据的管道
        /// </summary>
        /// <param name="path">读取或保存数据的路径</param>
        /// <param name="separated">属性的分隔符，
        /// 它在文本文件的一行中分隔<see cref="IData"/>的属性</param>
        /// <param name="mappingNull">指定将属性中的<see langword="null"/>值映射为哪个字符</param>
        /// <param name="columnType">枚举每一列的数据类型，如果为<see langword="null"/>，默认全部为<see cref="string"/>，
        /// 这个参数不会对写入数据产生影响</param>
        /// <returns></returns>
        public static IDataPipe PipeFromFile(PathText path, string separated = "\t", string mappingNull = "@null@", IEnumerable<Type>? columnType = null)
            => new DataPipeFromFile(path, separated, mappingNull, columnType);
        #endregion
        #endregion
        #endregion
        #region 创建序列化和反序列化对象
        #region 返回支持序列化数据的对象
        /// <summary>
        /// 返回一个支持序列化和反序列化<see cref="IDirect"/>和<see cref="IData"/>的对象
        /// </summary>
        /// <returns></returns>
        public static JsonConverterIDirect JsonDirect { get; } = new();
        #endregion
        #endregion
    }
}
