using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.JSInterop
{
    /// <summary>
    /// 这个类型可以通过JS互操作来索引和修改本地存储
    /// </summary>
    class JSLocalStorage : JSRuntimeBase, IAsyncDictionary<string, string>
    {
        #region 关于根据键读写值
        #region 根据键读取值
        public async Task<string> GetValueAsync(string key)
            => (await JSRuntime.InvokeAsync<string?>("localStorage.getItem", key)) ??
            throw new KeyNotFoundException($"未找到键{key}");
        #endregion
        #region 根据键读取值（不会引发异常）
        public async Task<(bool Exist, string? Value)> TryGetValueAsync(string key)
        {
            var value = await JSRuntime.InvokeAsync<string?>("localStorage.getItem", key);
            return (value is { }, value);
        }
        #endregion
        #region 根据键写入值
        public Task SetValueAsync(string key, string value)
            => JSRuntime.InvokeVoidAsync("localStorage.setItem", key, value).AsTask();
        #endregion
        #endregion
        #region 关于集合
        #region 返回键值对数量
        public Task<int> CountAsync
            => JSRuntime.GetProperty<int>("localStorage.length").AsTask();
        #endregion
        #region 枚举所有键值对
        public async IAsyncEnumerator<KeyValuePair<string, string>> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            for (int count = await CountAsync, i = 0; i < count; i++)
            {
                var key = await JSRuntime.InvokeAsync<string>("localStorage.key", i);
                yield return new(key, await GetValueAsync(key));
            }
        }
        #endregion
        #region 关于添加和删除键值对
        #region 删除全部键值对
        public Task ClearAsync()
            => JSRuntime.InvokeVoidAsync("localStorage.clear").AsTask();
        #endregion
        #region 删除指定键
        public async Task<bool> RemoveAsync(string key)
        {
            if (await this.To<IAsyncDictionary<string, string>>().ContainsKeyAsync(key))
            {
                await JSRuntime.InvokeVoidAsync("localStorage.removeItem", key);
                return true;
            }
            return false;
        }
        #endregion
        #region 删除指定键值对
        public Task<bool> RemoveAsync(KeyValuePair<string, string> item)
            => RemoveAsync(item.Key);
        #endregion
        #region 添加键值对
        public Task AddAsync(KeyValuePair<string, string> item)
            => SetValueAsync(item.Key, item.Value);
        #endregion
        #endregion
        #region 检查键值对是否存在
        public async Task<bool> ContainsAsync(KeyValuePair<string, string> item)
            => (await TryGetValueAsync(item.Key)) is (true, var value) && Equals(value, item.Value);
        #endregion
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的JS运行时初始化对象
        /// </summary>
        /// <param name="JSRuntime">封装的JS运行时对象，本对象的功能就是通过它实现的</param>
        public JSLocalStorage(IJSRuntime JSRuntime)
            : base(JSRuntime)
        {

        }
        #endregion
    }
}
