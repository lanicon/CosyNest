namespace System.Office.Excel
{
    /// <summary>
    /// 这个类型表示Excel单元格的值
    /// </summary>
    public readonly struct RangeValue
    {
        #region 说明文档
        /*说明文档：
           ExcelRange的Value一般是object，
           但本框架则使用这个类型，原因在于：

           虽然Excel是弱类型的模型，
           但是Value的合法类型实际上就只有四种：
           文本，数字，日期和数组，使用这个类型可以屏蔽不合法的输入，
           并且在互相转换的时候会更加方便*/
        #endregion
        #region 创建RangeValue
        /// <summary>
        /// 传入一个值，并创建<see cref="RangeValue"/>
        /// </summary>
        /// <param name="Value">被封装的值，
        /// 仅支持<see cref="string"/>，<see cref="int"/>，
        /// <see cref="double"/>，<see cref="Array"/>和<see cref="DateTime"/>,
        /// 其他类型会引发异常</param>
        /// <returns></returns>
        public static RangeValue? Create(object? Value)
            => Value switch
            {
                null => (RangeValue?)null,
                string or int or double or DateTime or Array => new RangeValue(Value),
                _ => throw new ExceptionTypeUnlawful(Value,
                        typeof(string), typeof(double), typeof(DateTime), typeof(Array)),
            };
        #endregion
        #region 隐式转换
        #region 从String转换
        public static implicit operator RangeValue(string value)
            => new(value);
        #endregion
        #region 从Int转换
        public static implicit operator RangeValue(int value)
            => new(value);
        #endregion
        #region 从Double转换
        public static implicit operator RangeValue(double value)
            => new(value);
        #endregion
        #region 从DateTime转换
        public static implicit operator RangeValue(DateTime value)
            => new(value);
        #endregion
        #region 从数组转换
        public static implicit operator RangeValue(Array value)
            => new(value);
        #endregion
        #endregion
        #region 值的内容
        /// <summary>
        /// 储存单元格Value的实际值
        /// </summary>
        public object Content { get; }
        #endregion
        #region 解释值
        #region 解释为数组
        /// <summary>
        /// 如果这个单元格值是数组，则返回数组，
        /// 否则返回<see langword="null"/>
        /// </summary>
        public Array? ToArray
            => Content as Array;

        /*问：此处为什么不直接使用Object[]，
          而是返回数组的基类Array？
          答：因为单元格值返回的数组不一定是一维的，
          还有可能是二维的*/
        #endregion
        #region 解释为文本
        /// <summary>
        /// 将这个单元格值解释为<see cref="string"/>
        /// </summary>
        public string ToText
            => Content?.ToString() ?? "";
        #endregion
        #region 解释为数字
        /// <summary>
        /// 如果这个单元格值是数字，或可以转换为数字，
        /// 则返回它， 否则返回<see langword="null"/>
        /// </summary>
        public double? ToDouble
            => Content.To<double?>(false);
        #endregion
        #region 解释为日期
        /// <summary>
        /// 如果单元格值是日期，则返回日期，
        /// 否则返回<see langword="null"/>
        /// </summary>
        public DateTime? ToDateTime
            => Content.To<DateTime?>(false);
        #endregion
        #endregion
        #region 重写ToString
        public override string ToString()
            => ToText;
        #endregion
        #region 构造函数
        #region 传入文本
        /// <summary>
        /// 将一个文本值封装进对象
        /// </summary>
        /// <param name="Value">被封装的文本值</param>
        public RangeValue(string Value)
            => this.Content = Value;
        #endregion
        #region 传入数字
        /// <summary>
        /// 将一个数字封装进对象
        /// </summary>
        /// <param name="Value">被封装的数字</param>
        public RangeValue(double Value)
            => Content = Value;
        #endregion
        #region 传入日期
        /// <summary>
        /// 将一个日期封装进对象
        /// </summary>
        /// <param name="Value">被封装的日期</param>
        public RangeValue(DateTime Value)
            => this.Content = Value;
        #endregion
        #region 传入可空值类型数组
        /// <summary>
        /// 将一个数组封装进对象
        /// </summary>
        /// <param name="Array">被封装的数组</param>
        public RangeValue(RangeValue?[] Array)
            => Content = Array;
        #endregion
        #region 传入数组
        /// <summary>
        /// 将一个数组封装进对象
        /// </summary>
        /// <param name="Array">被封装的数组</param>
        public RangeValue(Array Array)
            => Content = Array;
        #endregion
        #region 传入任意类型
        /// <summary>
        /// 将任意类型封装进对象
        /// </summary>
        /// <param name="Value">要封装进对象的类型</param>
        private RangeValue(object Value)
            => Content = Value;
        #endregion
        #endregion
    }
}
