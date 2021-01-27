using System.DrawingFrancis.Graphics;
using System.DrawingFrancis.Text;
using System.IO;
using System.IOFrancis;
using System.Linq;
using System.Maths;
using System.Threading.Tasks;

namespace System.DrawingFrancis
{
    /// <summary>
    /// 这个静态类可以帮助创建一些关于绘图的对象
    /// </summary>
    public static class CreateDrawingObj
    {
        #region 创建文本格式
        /// <summary>
        /// 用指定的参数创建文本样式
        /// </summary>
        /// <param name="FontName">指定的字体</param>
        /// <param name="Size">字体的大小</param>
        /// <param name="Color">文本的颜色</param>
        public static ITextStyleVar TextStyleVar(string FontName, IUnit<IUTFontSize> Size, IColor Color)
            => new TextStyleVar(FontName, Size, Color);
        #endregion
        #region 创建IColor
        /// <summary>
        /// 使用指定的红色，绿色，蓝色和透明度创建<see cref="IColor"/>
        /// </summary>
        /// <param name="R">指定的红色值</param>
        /// <param name="G">指定的绿色值</param>
        /// <param name="B">指定的蓝色值</param>
        /// <param name="A">指定的透明度</param>
        public static IColor Color(byte R, byte G, byte B, byte A = 255)
            => new Color(R, G, B, A);
        #endregion
        #region 创建像素点
        /// <summary>
        /// 使用指定的颜色，位置和图层创建像素点
        /// </summary>
        /// <param name="Color">像素点的颜色</param>
        /// <param name="Position">像素点的位置</param>
        /// <param name="Layer">像素点的图层</param>
        public static IGraphicsPixel GraphicsPixel(IColor Color, IPoint Position, int Layer = 0)
            => new GraphicsPixel(Color, Position, Layer);
        #endregion
        #region 创建内存图像
        #region 指定二进制内容和格式
        /// <summary>
        /// 使用指定的二进制内容和格式创建一个存在于内存中的图像
        /// </summary>
        /// <param name="ImageBinary">图片的二进制形式</param>
        /// <param name="Format">图片的格式</param>
        /// <returns></returns>
        public static IImage ImageMemory(ReadOnlyMemory<byte> ImageBinary, string Format)
            => new ImageMemory(ImageBinary, Format);
        #endregion
        #region 指定流和格式
        /// <summary>
        /// 从指定的流中读取字节数组，并创建具有指定格式的内存图像
        /// </summary>
        /// <param name="stream">用来读取图像内容的流</param>
        /// <param name="Format">图片的格式</param>
        /// <returns></returns>
        public static async Task<IImage> ImageMemory(Stream stream, string Format)
        {
            var arry = await stream.ReadAll();
            return ImageMemory(arry, Format);
        }
        #endregion
        #region 指定文件和格式
        /// <summary>
        /// 将文件读取到内存，并返回读取到的图片
        /// </summary>
        /// <param name="Path">图片所在的文件</param>
        /// <returns></returns>
        public static async Task<IImage> ImageMemory(PathText Path)
        {
            using var stream = CreateIO.File(Path).GetBitPipe();
            return ImageMemory(await stream.Read().FirstAsync(), ToolPath.Split(Path).Extended);
        }
        #endregion
        #endregion
    }
}
