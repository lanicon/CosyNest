using System.Threading.Tasks;

namespace System.IOFrancis.Bit
{
    #region 说明文档
    /*问：“翻译”和“验证”这两个概念非常广泛和模糊，
      请问它们指的是什么？
      答：“翻译”指的是将二进制数据转换为另一个二进制数据，
      “验证”指的是对二进制数据返回一个布尔值，
      但是除此之外，并不考虑它们的目的，
      这是作者有意这样设计的，所以这两个委托定义虽然很简单，但是极其灵活
    
      问：这两个委托可以用来做什么？
      答：它们为很多目的截然不同的二进制操作提供了统一抽象，
      举例说明，加密明文，解密密文，将mp3格式转换为ogg格式，
      这些都是翻译，可以用BitTranslation表示，
      哈希校验，验证数字签名，这些都是验证，可以用BitVerify表示，
      这种抽象非常泛化，可以避免大量的接口定义*/
    #endregion
    #region 翻译二进制数据
    /// <summary>
    /// 将二进制数据翻译成另一种格式
    /// </summary>
    /// <param name="read">用来读取翻译前二进制数据的管道</param>
    /// <returns>用来读取翻译后二进制数据的管道</returns>
    public delegate IBitRead BitTranslation(IBitRead read);
    #endregion
    #region 验证二进制数据（不需要比较）
    /// <summary>
    /// 对二进制数据进行验证，不需要与另一个对象进行比较
    /// </summary>
    /// <param name="read">用来读取待验证二进制数据的管道</param>
    /// <returns>如果返回<see langword="true"/>，代表验证通过，否则为不通过</returns>
    public delegate Task<bool> BitVerify(IBitRead read);
    #endregion
    #region 验证二进制数据（需要比较）
    /// <summary>
    /// 对二进制数据进行验证
    /// </summary>
    /// <typeparam name="Comparison">和二进制数据进行比较的对象的类型</typeparam>
    /// <param name="read">用来读取待验证二进制数据的管道</param>
    /// <param name="comparison">一个用来与二进制数据进行比较的对象</param>
    /// <returns>如果返回<see langword="true"/>，代表验证通过，否则为不通过</returns>
    public delegate Task<bool> BitVerify<in Comparison>(IBitRead read, Comparison comparison);
    #endregion
}
