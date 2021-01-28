using System.Maths;

namespace System.IOFrancis.FileSystem
{
    /// <summary>
    /// 这个类型是<see cref="IUTStorage"/>的实现，
    /// 可以视为一个计算机存储单位
    /// </summary>
    class UTStorage : UT, IUTStorage
    {
        #region 返回单位的类型
        protected override Type UTType
            => typeof(IUTStorage);
        #endregion
        #region 构造方法
        /// <summary>
        /// 用指定的名称和转换常数初始化计算机存储单位
        /// </summary>
        /// <param name="Name">计算机存储单位的名称</param>
        /// <param name="Conver">一个用来和公制单位进行转换的常数，
        /// 假设本单位为a，常数为b，公制单位为c，c=a*b，a=c/b</param>
        public UTStorage(string Name, Num Conver)
            : base(Name, Conver)
        {

        }
        #endregion
    }
}
