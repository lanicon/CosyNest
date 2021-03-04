using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.JSInterop
{
    /// <summary>
    /// 这个类型可以通过JS互操作来索引和修改Cookie
    /// </summary>
    class JSCookie : JSRuntimeBase, IAsyncDictionary<string, string>
    {
        #region 封装的对象
        #region 通过JS读写Cookie文本
        #region 读取
        /// <summary>
        /// 通过JS互操作直接读取document.cookie属性
        /// </summary>
        /// <returns></returns>
        private ValueTask<string> GetCookie()
            => JSRuntime.GetProperty<string>("document.cookie");
        #endregion
        #region 写入
        /// <summary>
        /// 通过JS互操作直接写入document.cookie属性
        /// </summary>
        /// <param name="cookie">要写入的Cookie文本</param>
        /// <returns></returns>
        private ValueTask SetCookie(string cookie)
            => JSRuntime.SetProperty("document.cookie", cookie);
        #endregion
        #endregion
        #region 返回最小UTC时间的字符串
        /// <summary>
        /// 返回JS中UTC最小时间的字符串格式，
        /// 在删除Cookie时会用到
        /// </summary>
        private static string MinDate { get; } = "Thu, 01 Jan 1970 00:00:00 GMT";
        #endregion
        #endregion
        #region 关于读取和写入Cookie
        #region 读取Cookie
        public async Task<string> GetValueAsync(string key)
            => (await this.FirstAsync(x => x.Key == key)).Value;
        #endregion
        #region 读取Cookie且不引发异常
        public async Task<(bool Exist, string? Value)> TryGetValueAsync(string key)
        {
            var kv = await this.FirstOrDefaultAsync(x => x.Key == key);
            return kv.Equals(default(KeyValuePair<string, string>)) ? (false, null) : (true, kv.Value);
        }
        #endregion
        #region 写入Cookie
        public Task SetValueAsync(string key, string value)
            => SetCookie($"{key}={value}").AsTask();
        #endregion
        #endregion
        #region 关于集合
        #region 枚举所有键值对
        public async IAsyncEnumerator<KeyValuePair<string, string>> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            foreach (var item in ToolRegex.KeyValuePairExtraction(await GetCookie(), ";"))
            {
                yield return item;
            }
        }
        #endregion
        #region 检查键值对是否存在
        public async Task<bool> AsyncContains(KeyValuePair<string, string> item)
            => (await TryGetValueAsync(item.Key)) is (true, var value) && Equals(value, item.Value);
        #endregion
        #region 返回键值对数量
        public Task<int> AsyncCount
            => this.CountAsync().AsTask();
        #endregion
        #endregion
        #region 关于添加和删除键值对
        #region 删除指定的键
        public async Task<bool> RemoveAsync(string key)
        {
            if (await this.To<IAsyncDictionary<string, string>>().ContainsKeyAsync(key))
            {
                await SetCookie($"{key}=; expires={MinDate}");
                return true;
            }
            return false;
        }
        #endregion
        #region 删除指定的键值对
        public Task<bool> AsyncRemove(KeyValuePair<string, string> item)
            => RemoveAsync(item.Key);
        #endregion
        #region 全部删除
        public async Task AsyncClear()
        {
            var keys = await this.Select(x => x.Key).ToArrayAsync();
            foreach (var key in keys)
            {
                await RemoveAsync(key);
            }
        }
        #endregion
        #region 添加键值对
        public Task AsyncAdd(KeyValuePair<string, string> item)
            => SetValueAsync(item.Key, item.Value);
        #endregion
        #endregion
        #region 构造函数
        /// <summary>
        /// 使用指定的JS运行时初始化对象
        /// </summary>
        /// <param name="JSRuntime">封装的JS运行时对象，本对象的功能就是通过它实现的</param>
        public JSCookie(IJSRuntime JSRuntime)
            : base(JSRuntime)
        {

        }
        #endregion
    }
}
