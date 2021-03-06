namespace System
{
    /// <summary>
    /// 代表有理数类型，
    /// 它可以兼容一切现有的数字类型，为数学运算提供方便
    /// </summary>
    public readonly struct Num : IComparable<Num>
    {
        #region 说明文档
        /*说明文档：
          问：为什么需要设计本类型？
          答：.Net的数字系统十分复杂，不同长度的数字，以及整数和浮点数的类型都不一样，
          虽然这种设计在计算机上是合理的，但是毕竟给数学运算带来了不便，
          因此设计者编写了一个统一的数字类型，它可以兼容一切现有的数字类型，
          这会产生一定的性能损失，不过可以避免反复声明作用相同但数字类型不同的API*/
        #endregion
        #region 运算符重载
        #region 隐式类型转换
        #region 转换为Num
        public static implicit operator short(Num num)
            => (short)num.Value;
        public static implicit operator int(Num num)
            => (int)num.Value;
        public static implicit operator long(Num num)
            => (long)num.Value;
        public static implicit operator float(Num num)
            => (float)num.Value;
        public static implicit operator double(Num num)
            => (double)num.Value;
        public static implicit operator decimal(Num num)
            => num.Value;
        #endregion
        #region 从Num转换
        public static implicit operator Num(short num)
            => new(num);
        public static implicit operator Num(int num)
            => new(num);
        public static implicit operator Num(long num)
            => new(num);
        public static implicit operator Num(float num)
            => new((decimal)num);
        public static implicit operator Num(double num)
            => new((decimal)num);
        public static implicit operator Num(decimal num)
            => new(num);
        #endregion
        #endregion
        #region 重载一元运算符
        public static Num operator -(Num num)
            => -num.Value;
        public static Num operator ++(Num num)
            => num.Value + 1;
        public static Num operator --(Num num)
            => num.Value - 1;
        #endregion
        #region 重载二元运算符
        public static Num operator +(Num numA, Num numB)
            => numA.Value + numB.Value;
        public static Num operator -(Num numA, Num numB)
            => numA.Value - numB.Value;
        public static Num operator *(Num numA, Num numB)
            => numA.Value * numB.Value;
        public static Num operator /(Num numA, Num numB)
            => numA.Value / numB.Value;
        public static Num operator %(Num numA, Num numB)
            => numA.Value % numB.Value;
        public static bool operator ==(Num numA, Num numB)
            => numA.Value == numB.Value;
        public static bool operator !=(Num numA, Num numB)
            => numA.Value != numB.Value;
        public static bool operator >(Num numA, Num numB)
            => numA.Value > numB.Value;
        public static bool operator <(Num numA, Num numB)
            => numA.Value < numB.Value;
        public static bool operator >=(Num numA, Num numB)
            => numA.Value >= numB.Value;
        public static bool operator <=(Num numA, Num numB)
            => numA.Value <= numB.Value;
        #endregion
        #endregion
        #region 数字的值
        /// <summary>
        /// 返回数字的值
        /// </summary>
        public decimal Value { get; }
        #endregion
        #region 对数字的转换
        #region 返回数字是否为整数
        /// <summary>
        /// 如果这个属性返回<see langword="true"/>，代表该数字是整数，
        /// 否则代表该数字是小数
        /// </summary>
        public bool IsInteger
            => Value % 1 is 0;
        #endregion
        #region 取近似值
        /// <summary>
        /// 将这个数字舍入到指定的小数位数，并返回近似值
        /// </summary>
        /// <param name="Precision">舍入的精度，默认为0，也就是取整数</param>
        /// <returns></returns>
        public Num Rounding(int Precision = 0)
            => Math.Round(Value, Precision);
        #endregion
        #endregion
        #region 接口实现与重写方法
        #region 重写ToString
        public override string ToString()
            => Value.ToString();
        #endregion
        #region 重写GetHashCode
        public override int GetHashCode()
            => Value.GetHashCode();
        #endregion
        #region 重写Equals
        public override bool Equals(object? obj)
            => obj switch
            {
                Num num => Value == num.Value,
                IConvertible => Value == Convert.ToDecimal(obj),
                _ => false
            };
        #endregion
        #region 实现IComparable
        public int CompareTo(Num other)
            => Value.CompareTo(other.Value);
        #endregion
        #endregion
        #region 构造函数
        /// <summary>
        /// 将指定的数字封装进对象
        /// </summary>
        /// <param name="num">待封装的数字</param>
        public Num(decimal num)
        {
            Value = num;
        }
        #endregion
    }
}
