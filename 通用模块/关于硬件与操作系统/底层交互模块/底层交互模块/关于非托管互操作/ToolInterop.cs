using System;

namespace System.Runtime.InteropServices
{
    /// <summary>
    /// 有关互操作的工具类
    /// </summary>
    public class ToolInterop
    {
        #region 关于二进制转换
        #region 从高层到底层
        #region 将非托管类型转换为字节数组
        /// <summary>
        /// 将非托管类型转换为字节数组
        /// </summary>
        /// <typeparam name="Obj">待转换的非托管类型</typeparam>
        /// <param name="structObj">待转换的对象</param>
        /// <returns></returns>
        public static byte[] StructToBytes<Obj>(Obj structObj)
            where Obj : unmanaged
        {
            var size = Marshal.SizeOf<Obj>();
            var bytes = new byte[size];
            var structPtr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(structObj, structPtr, false);
            Marshal.Copy(structPtr, bytes, 0, size);
            Marshal.FreeHGlobal(structPtr);
            return bytes;
        }
        #endregion
        #region 将字节转换为比特位
        /// <summary>
        /// 将一个字节拆开，返回它的每个比特位
        /// </summary>
        /// <param name="Byte">待拆开的字节</param>
        /// <returns></returns>
        public static byte[] BytesToBit(byte Byte)
        {
            var Arry = new byte[8];
            for (byte i = 8; i > 0; i--)
            {
                Arry[i - 1] = (byte)(Byte % 2);
                Byte /= 2;
            }
            return Arry;
        }
        #endregion
        #endregion
        #region 从底层到高层
        #region 将字节数组转换为非托管类型
        /// <summary>
        /// 将字节数组解析为非托管类型
        /// </summary>
        /// <typeparam name="Obj">目标非托管类型</typeparam>
        /// <param name="bytes">要解析的字节数组</param>
        /// <returns></returns>
        public static Obj BytesToStruct<Obj>(byte[] bytes)
            where Obj : unmanaged
        {
            var size = Marshal.SizeOf<Obj>();
            if (size != bytes.Length)
                throw new Exception("数组的长度不等于非托管对象的字节数");
            var structPtr = Marshal.AllocHGlobal(size);
            Marshal.Copy(bytes, 0, structPtr, size);
            var obj = Marshal.PtrToStructure<Obj>(structPtr);
            Marshal.FreeHGlobal(structPtr);
            return obj;
        }
        #endregion
        #region 将比特位转换为字节
        /// <summary>
        /// 将比特位组合起来，使其成为字节
        /// </summary>
        /// <param name="Bytes">待组合的比特位，
        /// 如果它的长度不是8，或者包含了1和0以外的数字，则会引发异常</param>
        /// <returns></returns>
        public static byte BitToBytes(byte[] Bytes)
        {
            if (Bytes.Length != 8)
                throw new Exception("字节数组必须有8个元素");
            byte Num = 0;
            for (byte i = 8, Pow = 1; i > 0; i--)
            {
                var Bit = Bytes[i - 1];
                if (Bit > 1)
                    throw new Exception("比特位只允许0和1两个值");
                Num += (byte)(Bit * Pow);
                Pow *= 2;
            }
            return Num;
        }
        #endregion
        #endregion
        #endregion
    }
}
