using System.Collections.Generic;
using System.ComponentModel;
using System.Design.Direct;

namespace System.DataFrancis
{
    /// <summary>
    /// 这个类型是<see cref="IData"/>的实现，
    /// 可以被当作一个数据进行传递
    /// </summary>
    class Datas : DictionaryRealize<string, object?>, IData
    {
        #region 接口实现
        #region 架构约束
        private ISchema? SchemaField;

        public ISchema? Schema
        {
            get => SchemaField;
            set
            {
                IDirect.CheckSchemaSet(this, value);
                SchemaField = value;
            }
        }
        #endregion
        #region 关于读取和写入数据
        #region 辅助方法：通过键写入值
        /// <summary>
        /// 通过键写入值，如果键不存在，会引发异常
        /// </summary>
        /// <param name="columnName">数据的列名</param>
        /// <param name="newValue">数据的新值</param>
        private void SetValueAided(string columnName, object? newValue)
        {
            if (PackDictionary.ContainsKey(columnName))
            {
                PackDictionary[columnName] = newValue;
                this.Changed(PropertyChanged, columnName);
            }
            else throw new KeyNotFoundException($"列名{columnName}不存在于数据中");
        }
        #endregion
        #region 索引器
        public override object? this[string columnName]
        {
            get => PackDictionary[columnName];
            set
            {
                SetValueAided(columnName, value);
                Binding?.NoticeUpdateToSource(columnName, value);
            }
        }
        #endregion
        #region 修改数据时引发的事件
        public event PropertyChangedEventHandler? PropertyChanged;
        #endregion
        #endregion 
        #region 关于数据更新
        #region 刷新数据
        public void Refresh()
            => this.Changed(PropertyChanged, string.Empty);
        #endregion
        #region 获取或设置数据绑定
        private IDataBinding? BindingField;
        public IDataBinding? Binding
        {
            get => BindingField;
            set
            {
                if (BindingField is { })
                {
                    BindingField.NoticeUpdateToData -= SetValueAided;
                    BindingField.NoticeDeleteToData -= DeleteAided;
                }
                if (value is { })
                {
                    value.NoticeUpdateToData += SetValueAided;
                    value.NoticeDeleteToData += DeleteAided;
                }
                BindingField = value;
            }
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
            if (DeleteEvent != null)
            {
                DeleteEvent(this);
                DeleteEvent = null;
            }
            PropertyChanged = null;
            Binding = null;
        }
        #endregion
        #region 删除数据
        public void Delete()
        {
            Binding?.NoticeDeleteToSource();
            DeleteAided();
        }
        #endregion
        #region 删除数据时引发的事件
        public event Action<IData>? DeleteEvent;
        #endregion
        #endregion 
        #endregion
        #endregion
        #region 重写ToString
        public override string? ToString()
            => this.Join(x => $"{x.Key}：{x.Value}", ";");
        #endregion
        #region 构造函数
        /// <summary>
        /// 用一个键是列名的键值对集合（通常是字典）初始化数据
        /// </summary>
        /// <param name="dictionary">一个键值对集合，它的元素的键</param>
        /// <param name="copyValue">如果这个值为真，则会复制键值对的值，否则不复制</param>
        public Datas(IEnumerable<KeyValuePair<string, object?>> dictionary, bool copyValue = false)
        {
            foreach (var (key, value) in dictionary)
                PackDictionary[key] = copyValue ? value : null;
        }
        /*注释：
          #dictionary参数不使用IDictionary的原因在于：
          使用IEnumerable这个类型，
          可以同时兼容IDictionary和IReadOnlyDictionary*/
        #endregion
    }
}
