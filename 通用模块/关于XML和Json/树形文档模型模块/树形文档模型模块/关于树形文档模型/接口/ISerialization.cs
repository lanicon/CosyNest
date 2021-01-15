using System.Text;

namespace System.TreeObject
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以序列化和反序列化树形文档对象
    /// </summary>
    /// <typeparam name="Output">反序列化的合法输出类型</typeparam>
    public interface ISerialization<out Output>
    {
        #region 说明文档
        /*问：本接口只有一个泛型参数，即输出类型，
          请问为什么不设计为两个泛型参数，一个输入类型和一个输出类型？
          答：因为作者认识到，序列化的对象类型应该支持协变，
          反序列化的对象类型应该支持逆变，因此最好的办法是：
          将序列化类型设计为object，将反序列化类型设计为一个支持逆变的泛型参数，
          如此一来能够在灵活性和静态类型检查中获得良好平衡
        
          问：CanSerialization指示本对象可序列化的类型，
          DeserializeType指示反序列化的输出类型，为什么要将它们分开？
          答：如上一条所述，为了同时支持协变和逆变
        
          问：通过本接口的泛型参数，可以静态的检查输出的类型，
          既然如此，为什么需要DeserializeType这个属性？
          答：因为作者意识到，由于输出类型支持逆变，因此在很多情况下，
          API通常使用ISerialization<object>而不是ISerialization<Output>，
          这给检查反序列化的类型带来了困难，因此需要通过这个属性来告知最终输出类型，
          来判断某一类型是否真的可以执行反序列化
          当然，你也可以选择直接使用ISerialization<Output>，
          它不需要执行这个检查，代价是更强的耦合
        
          问：本接口的功能与BCL原生的类型JsonConverter非常相似，
          是否应该把它们统一起来？
          答：不应该，理由如下：
          1.设计目的不同，本接口可以支持任何序列化协议，
          包括Json，Xml，二进制等，只要实现者提供了对应实现，
          而JsonConverter只能用来序列化Json
          2.本接口的API提高了抽象层次，不假设调用者只使用UTF8编码，
          降低了性能但是增强了灵活性
          3.很多Net框架（例如ASP.NetCore）依赖于JsonConverter，
          因此不宜将这个类型完全取代
        
          问：在何种情况下应该选择本接口或JsonConverter？
          答：如果API需要与底层或其他框架交互，而且交互的对象依赖于JsonConverter，
          此时应该使用JsonConverter以避免不必要的转换，其他情况下应该选择ISerialization，
          本框架通过扩展方法提供了在这两个类型之间互相转换的功能*/
        #endregion
        #region 返回协议名称
        /// <summary>
        /// 返回描述树形文档对象的协议名称，
        /// 例如XML，Json等
        /// </summary>
        string Agreement { get; }
        #endregion
        #region 关于序列化
        #region 是否可序列化
        /// <summary>
        /// 如果指定的类型可以被序列化，则返回<see langword="true"/>，
        /// 否则返回<see langword="false"/>
        /// </summary>
        /// <param name="type">待检查的类型</param>
        /// <returns></returns>
        bool CanSerialization(Type type);
        #endregion
        #region 序列化为指定编码
        /// <summary>
        /// 将受支持的对象序列化为指定编码
        /// </summary>
        /// <param name="obj">待序列化的对象</param>
        /// <param name="encoding">序列化的目标编码，
        /// 如果为<see langword="null"/>，默认为UTF8</param>
        /// <exception cref="ExceptionTypeUnlawful"><paramref name="obj"/>的类型不受支持</exception>
        /// <returns></returns>
        ReadOnlySpan<byte> Serialization(object? obj, Encoding? encoding = null);
        #endregion
        #region 序列化为UTF16
        /// <summary>
        /// 将受支持的对象序列化为UTF16
        /// </summary>
        /// <param name="obj">待序列化的对象</param>
        /// <exception cref="ExceptionTypeUnlawful"><paramref name="obj"/>的类型不受支持</exception>
        /// <returns></returns>
        string SerializationUTF16(object? obj)
        {
            var encoding = Encoding.Unicode;
            return encoding.GetString(Serialization(obj, encoding));
        }
        #endregion
        #endregion
        #region 关于反序列化
        #region 获取具体输出类型
        /// <summary>
        /// 获取本对象反序列化输出的具体类型
        /// </summary>
        Type DeserializeType
            => typeof(Output);
        #endregion
        #region 从指定的编码反序列化
        /// <summary>
        /// 从指定的编码反序列化对象
        /// </summary>
        /// <param name="text">用来描述对象的文本</param>
        /// <param name="encoding">文本的编码，
        /// 如果为<see langword="null"/>，默认为UTF8</param>
        /// <exception cref="ExceptionTypeUnlawful"><typeparamref name="Output"/>的类型不受支持</exception>
        /// <returns></returns>
        Output? Deserialize(ReadOnlySpan<byte> text, Encoding? encoding = null);
        #endregion
        #region 从UTF16反序列化
        /// <summary>
        /// 从UTF16反序列化对象
        /// </summary>
        /// <param name="text">用来描述对象的文本</param>
        /// <exception cref="ExceptionTypeUnlawful"><typeparamref name="Output"/>的类型不受支持</exception>
        /// <returns></returns>
        Output? DeserializeUTF16(string text)
        {
            var encoding = Encoding.Unicode;
            return Deserialize(encoding.GetBytes(text), encoding);
        }
        #endregion
        #endregion
    }
}
