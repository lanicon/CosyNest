using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Design;
using System.Design.Direct;
using System.Linq;
using System.Performance;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace System.DataFrancis
{
    /// <summary>
    /// 本类型是<see cref="IData"/>的实现，
    /// 它可以将列名和实例属性的名称对应起来，并通过反射读写它们的值
    /// </summary>
    public abstract class Entity : IData
    {
        #region 说明文档
        /*问：由Datas实现的IData已经可以满足需求，为什么需要本类型？
          答：本类型为以下两种人准备：
          1.习惯使用Entity Framework风格的实体类
          2.需要对属性进行静态检查
          
          问：如何使用本类型？
          答：请声明一个实体类（记作A），并继承自本类型，
          所有在A中声明的，公开，实例，且可读写的属性会被缓存，
          它们将可以通过IDirect的索引器进行读写
          
          注意事项：
          #在A中声明属性时，不要使用自动属性，
          而是声明一个字段，按照约定，它的命名模式应该是属性名加上Field，
          在属性的Get访问器中，返回这个字段，
          Set访问器中，写入这个字段，并调用Changed方法，
          如果不遵循这个约定，在数据绑定的时候会出现问题*/
        #endregion
        #region 本对象的接口形式
        /// <summary>
        /// 返回本对象的接口形式，
        /// 它可以用来访问某些显式实现的成员
        /// </summary>
        private IData Interface => this;
        #endregion
        #region 关于架构
        #region 缓存字典
        /// <summary>
        /// 这个字典按类型缓存实体类的架构
        /// </summary>
        private static ICache<Type, ISchema> CacheSchema { get; }
        = CreateCache.CacheThreshold
            (type => CreateDesign.Schema
            (CachePropertie![type].
                Select(x => (x.Key, x.Value.PropertyType)).ToArray()),
            100, CacheSchema);
        #endregion
        #region 正式属性
        ISchema? IDirect.Schema
        {
            get => CacheSchema[GetType()];
            set => throw new NotImplementedException("实体类本身是强类型的，无法添加架构约束");
        }
        #endregion
        #endregion
        #region 关于读写属性
        #region 辅助成员
        #region 缓存属性
        /// <summary>
        /// 这个字典缓存类型中所有公开，非静态，可读写，且由派生类声明的属性，
        /// 它们将被视为可通过<see cref="IDirect"/>的索引器进行读写
        /// </summary>
        private static ICache<Type, IDictionary<string, PropertyInfo>> CachePropertie { get; }
        = CreateCache.CacheThreshold
            (type => type.GetProperties().
            Where(x => !x.IsStatic() && x.DeclaringType != typeof(Entity) && x.GetPermissions() is null).
            ToDictionary(x => (x.Name, x), true),
            100, CachePropertie);
        #endregion
        #region 获取缓存字典
        /// <summary>
        /// 获取一个字典，
        /// 它缓存本类型的所有可被<see cref="IDirect"/>索引器读写的属性
        /// </summary>
        private IDictionary<string, PropertyInfo> Propertys
            => CachePropertie[GetType()];
        #endregion
        #endregion
        #region 属性发生更改时调用的方法
        /// <summary>
        /// 如果需要让数据支持绑定，
        /// 那么在属性发生更改时，应该在Set访问器中调用本方法
        /// </summary>
        /// <param name="PropertyName">发生更改的属性名称，可自动填入</param>
        /// <param name="NewValue">属性的新值</param>
        protected void Changed([CallerMemberName] string? PropertyName = null, object? NewValue = null)
        {
            Interface.Binding?.NoticeUpdateToSource(PropertyName!, NewValue);
            ExtenUI.Changed(this, PropertyChangedField, PropertyName);
        }
        #endregion
        #region 读写属性，会引发异常
        object? IRestrictedDictionary<string, object?>.this[string PropertyName]
        {
            get => Propertys[PropertyName].GetValue(this);
            set => Propertys[PropertyName].SetValue(this, value);
        }
        #endregion
        #region 读写属性，不会引发异常
        bool IReadOnlyDictionary<string, object?>.TryGetValue(string key, out object? value)
        {
            if (Propertys.TryGetValue(key, out var pro))
            {
                value = pro.GetValue(this);
                return true;
            }
            value = default;
            return false;
        }
        #endregion
        #region IReadOnlyDictionary版本
        object? IReadOnlyDictionary<string, object?>.this[string key]
            => Interface[key];
        #endregion
        #endregion
        #region 关于字典
        #region 检查指定的属性名称是否存在
        bool IReadOnlyDictionary<string, object?>.ContainsKey(string key)
            => Propertys.ContainsKey(key);
        #endregion
        #region 返回属性名称集合
        IEnumerable<string> IReadOnlyDictionary<string, object?>.Keys
            => Propertys.Keys;
        #endregion
        #region 返回属性值集合
        IEnumerable<object?> IReadOnlyDictionary<string, object?>.Values
            => Propertys.Values.Select(x => x.GetValue(this));
        #endregion
        #region 返回属性的数量
        int IReadOnlyCollection<KeyValuePair<string, object?>>.Count
            => Propertys.Count;
        #endregion
        #region 返回枚举器
        IEnumerator<KeyValuePair<string, object?>> IEnumerable<KeyValuePair<string, object?>>.GetEnumerator()
            => Propertys.Select(x => new KeyValuePair<string, object?>(x.Key, x.Value.GetValue(this))).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => Interface.GetEnumerator();
        #endregion
        #endregion
        #region 关于绑定
        #region 获取或设置绑定对象
        private IDataBinding? BindingField;
        IDataBinding? IData.Binding
        {
            get => BindingField;
            set
            {
                #region 用来写入数据的本地函数
                void SetValueAided(string Name, object? NewValue)
                    => this.GetTypeData().FieldFind(Name + "Field").SetValue(this, NewValue);
                #endregion
                if (BindingField != null)
                {
                    BindingField.NoticeUpdateToData -= SetValueAided;
                    BindingField.NoticeDeleteToData -= DeleteAided;
                }
                if (value != null)
                {
                    value.NoticeUpdateToData += SetValueAided;
                    value.NoticeDeleteToData += DeleteAided;
                }
                BindingField = value;
            }
        }
        #endregion
        #region 属性被修改时触发的事件
        private PropertyChangedEventHandler? PropertyChangedField;

        event PropertyChangedEventHandler? INotifyPropertyChanged.PropertyChanged
        {
            add => PropertyChangedField += value;
            remove => PropertyChangedField -= value;
        }
        #endregion
        #region 关于删除数据
        #region 辅助方法
        /// <summary>
        /// 删除数据的辅助方法，它不会调用<see cref="IDataBinding.NoticeDeleteToSource"/>，
        /// 可以防止循环调用引发的无限递归
        /// </summary>
        private void DeleteAided()
        {
            if (DeleteEventFiled != null)
            {
                DeleteEventFiled(this);
                DeleteEventFiled = null;
            }
            PropertyChangedField = null;
            Interface.Binding = null;
        }
        #endregion
        #region 删除数据
        void IData.Delete()
        {
            Interface.Binding?.NoticeDeleteToSource();
            DeleteAided();
        }
        #endregion
        #region 删除数据时引发的事件
        private Action<IData>? DeleteEventFiled;

        event Action<IData>? IData.DeleteEvent
        {
            add => DeleteEventFiled += value;
            remove => DeleteEventFiled -= value;
        }
        #endregion
        #endregion
        #region 刷新数据
        void IData.Refresh()
            => this.Changed(PropertyChangedField, string.Empty);
        #endregion
        #endregion
    }
}
