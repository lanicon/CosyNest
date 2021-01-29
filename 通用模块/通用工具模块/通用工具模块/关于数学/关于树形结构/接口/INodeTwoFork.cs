using System.Linq;

namespace System.Maths
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视作一个二叉树节点
    /// </summary>
    public interface INodeTwoFork : INode
    {
        #region 返回左值
        /// <summary>
        /// 返回左边的子节点
        /// </summary>
        INodeTwoFork? Left
           => (INodeTwoFork?)Son.ElementAt(0);
        #endregion
        #region 返回右值
        /// <summary>
        /// 返回右边的子节点
        /// </summary>
        INodeTwoFork? Right
            => (INodeTwoFork?)Son.ElementAt(1);
        #endregion
    }
}
