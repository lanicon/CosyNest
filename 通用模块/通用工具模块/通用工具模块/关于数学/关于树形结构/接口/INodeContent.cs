using System.Collections.Generic;
using System.Linq;

namespace System.Maths
{
    /// <summary>
    /// 凡是实现这个接口的类型，
    /// 都可以视为一个封装了特定内容的树形结构节点
    /// </summary>
    /// <typeparam name="Obj">节点封装的内容的类型</typeparam>
    public interface INodeContent<out Obj> : INode
    {
        #region 隐藏基类成员
        #region 获取父节点
        /// <summary>
        /// 获取这个节点的父节点，
        /// 如果是根节点，则返回<see langword="null"/>
        /// </summary>
        new INodeContent<Obj>? Father
             => (INodeContent<Obj>?)this.To<INode>().Father;
        #endregion
        #region 返回根节点
        /// <summary>
        /// 返回本节点的根节点，
        /// 如果本节点已经是根节点，则返回自身
        /// </summary>
        new INodeContent<Obj> Ancestors
             => (INodeContent<Obj>)this.To<INode>().Ancestors;
        #endregion
        #region 获取子节点
        /// <summary>
        /// 获取这个节点的直接子节点
        /// </summary>
        new IEnumerable<INodeContent<Obj>> Son
             => this.To<INode>().Son.Cast<INodeContent<Obj>>();
        #endregion
        #region 递归获取所有子节点
        /// <summary>
        /// 获取一个遍历所有直接与间接子节点的枚举器
        /// </summary>
        /// <returns></returns>
        new IEnumerable<INodeContent<Obj>> SonAll
              => this.To<INode>().SonAll.Cast<INodeContent<Obj>>();
        #endregion
        #endregion
        #region 获取节点的内容
        /// <summary>
        /// 获取节点的内容
        /// </summary>
        Obj Content { get; }
        #endregion
    }
}
