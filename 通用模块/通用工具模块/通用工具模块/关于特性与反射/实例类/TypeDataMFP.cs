using System.Collections.Generic;
using System.Linq;

namespace System.Reflection
{
    public partial class TypeData
    {
        //这个类型专门用来储存关于属性，字段和方法的反射

        #region 关于字段
        #region 枚举所有字段
        private IEnumerable<FieldInfo>? FieldField;
        /// <summary>
        /// 获取一个枚举所有字段的枚举器
        /// </summary>
        public IEnumerable<FieldInfo> Field
            => FieldField ??= InitialEnum<FieldInfo>();
        #endregion
        #region 按名称索引字段
        private ILookup<string, FieldInfo>? FieldDictField;
        /// <summary>
        /// 获取一个按名称索引字段的字典
        /// </summary>
        public ILookup<string, FieldInfo> FieldDict
            => FieldDictField ??= Field.ToLookup(x => x.Name);
        #endregion
        #region 按类型索引字段
        private ILookup<Type, FieldInfo>? FieldDictTypeField;
        /// <summary>
        /// 获取一个按类型索引字段的字典
        /// </summary>
        public ILookup<Type, FieldInfo> FieldDictType
            => FieldDictTypeField ??= Field.ToLookup(x => x.FieldType);
        #endregion
        #region 筛选字段
        /// <summary>
        /// 根据名称和声明类型筛选字段
        /// </summary>
        /// <param name="Name">字段的名称</param>
        /// <param name="DeclaringType">声明该字段的类型，
        /// 如果为<see langword="null"/>，则忽略</param>
        /// <returns>符合条件的字段，如果没有找到，或找到了多个字段，则引发异常</returns>
        public FieldInfo FieldFind(string Name, Type? DeclaringType = null)
            => Find(FieldDict, Name, DeclaringType);
        #endregion
        #endregion
        #region 关于属性
        #region 枚举所有属性
        private IEnumerable<PropertyInfo>? PropertyFiled;
        /// <summary>
        /// 获取一个枚举所有属性（不包括索引器）的枚举器
        /// </summary>
        public IEnumerable<PropertyInfo> Property
            => PropertyFiled ??= InitialEnum<PropertyInfo>().Where(x => !x.IsIndexing()).ToArray();
        #endregion
        #region 按名称索引
        private ILookup<string, PropertyInfo>? PropertyDictField;
        /// <summary>
        /// 获取一个按名称索引属性（不包括索引器）的字典
        /// </summary>
        public ILookup<string, PropertyInfo> PropertyDict
            => PropertyDictField ??= Property.ToLookup(x => x.Name);
        #endregion
        #region 按类型索引
        private ILookup<Type, PropertyInfo>? PropertyDictTypeField;
        /// <summary>
        /// 获取一个按类型索引属性（不包括索引器）的字典
        /// </summary>
        public ILookup<Type, PropertyInfo> PropertyDictType
            => PropertyDictTypeField ??= Property.ToLookup(x => x.PropertyType);
        #endregion
        #region 筛选属性
        /// <summary>
        /// 根据名称和声明类型筛选属性
        /// </summary>
        /// <param name="Name">属性的名称</param>
        /// <param name="DeclaringType">声明该属性的类型，
        /// 如果为<see langword="null"/>，则忽略</param>
        /// <returns>符合条件的属性，如果没有找到，或找到了多个属性，则引发异常</returns>
        public PropertyInfo PropertyFind(string Name, Type? DeclaringType = null)
            => Find(PropertyDict, Name, DeclaringType);
        #endregion
        #endregion
        #region 关于索引器
        #region 枚举所有索引器
        private IEnumerable<PropertyInfo>? IndexingField;
        /// <summary>
        /// 获取一个枚举所有索引器的枚举器
        /// </summary>
        public IEnumerable<PropertyInfo> Indexing
            => IndexingField ??= InitialEnum<PropertyInfo>().Where(x => x.IsIndexing()).ToArray();
        #endregion
        #region 按签名索引索引器
        private ILookup<MethodSignature, PropertyInfo>? IndexingDictField;
        /// <summary>
        /// 获取一个按签名索引索引器的字典，注意：
        /// 无论索引器是不是可读的，都是以它的读访问器的签名作为键
        /// </summary>
        public ILookup<MethodSignature, PropertyInfo> IndexingDict
            => IndexingDictField ??= Indexing.ToLookup(x =>
              new MethodSignature(x.PropertyType, x.GetIndexParameters().GetParType()));
        #endregion
        #region 筛选索引器
        /// <summary>
        /// 根据签名和声明类型筛选索引器
        /// </summary>
        /// <param name="Signature">索引器的签名</param>
        /// <param name="DeclaringType">声明该索引器的类型，
        /// 如果为<see langword="null"/>，则忽略</param>
        /// <returns>符合条件的索引器，如果没有找到，或找到了多个索引器，则引发异常</returns>
        public PropertyInfo IndexingFind(MethodSignature Signature, Type? DeclaringType = null)
            => Find(IndexingDict, Signature, DeclaringType);
        #endregion
        #endregion
        #region 关于方法
        #region 枚举所有方法
        private IEnumerable<MethodInfo>? MethodField;
        /// <summary>
        /// 获取一个枚举所有方法的枚举器
        /// </summary>
        public IEnumerable<MethodInfo> Method
            => MethodField ??= InitialEnum<MethodInfo>();
        #endregion
        #region 按照名称索引方法
        private ILookup<string, MethodInfo>? MethodDictField;
        /// <summary>
        /// 获取一个按照名称索引方法的字典
        /// </summary>
        public ILookup<string, MethodInfo> MethodDict
            => MethodDictField ??= Method.ToLookup(x => x.Name);
        #endregion
        #region 按签名索引方法
        private ILookup<MethodSignature, MethodInfo>? MethodDictSignatureField;
        /// <summary>
        /// 获取一个按签名索引方法的字典
        /// </summary>
        public ILookup<MethodSignature, MethodInfo> MethodDictSignature
            => MethodDictSignatureField ??= Method.ToLookup(x => x.GetSignature());
        #endregion
        #region 筛选方法
        /// <summary>
        /// 根据名称，签名和声明类型筛选方法
        /// </summary>
        /// <param name="Name">方法的名称</param>
        /// <param name="Signature">声明该方法的类型，
        /// 如果为<see langword="null"/>，则忽略</param>
        /// <param name="DeclaringType"></param>
        /// <returns>符合条件的方法，如果没有找到，或找到了多个方法，则引发异常</returns>
        public MethodInfo MethodFind(string Name, MethodSignature? Signature = null, Type? DeclaringType = null)
            => Find(MethodDict, Name, DeclaringType,
                x => Signature == null || x.IsSame(Signature));
        #endregion
        #endregion
    }
}
