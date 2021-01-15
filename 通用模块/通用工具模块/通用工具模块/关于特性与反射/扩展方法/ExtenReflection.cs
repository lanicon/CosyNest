using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System
{
    /// <summary>
    /// 关于反射和程序集的扩展方法，通常无需专门调用
    /// </summary>
    public static partial class ExtenReflection
    {
        //这个部分类专门储存有关字段，属性和成员的扩展方法

        #region 关于字段
        #region 返回一个字段的值
        /// <summary>
        /// 返回一个字段的值，返回值已经经过类型转换
        /// </summary>
        /// <typeparam name="Ret">返回值类型</typeparam>
        /// <param name="Fiel">要返回值的字段</param>
        /// <param name="Obj">字段所依附的对象，如果是静态字段，直接忽略</param>
        /// <returns></returns>
        public static Ret? GetValue<Ret>(this FieldInfo Fiel, object? Obj = null)
            => (Ret?)Fiel.GetValue(Obj);
        #endregion
        #endregion
        #region 关于属性
        #region 关于可见性与访问权限
        #region 返回属性的访问权限
        /// <summary>
        /// 如果一个属性只能读取，返回<see langword="true"/>，
        /// 只能写入，返回<see langword="false"/>，既能读取又能写入，返回<see langword="null"/>
        /// </summary>
        /// <param name="property">待获取访问权限的属性</param>
        /// <returns></returns>
        public static bool? GetPermissions(this PropertyInfo property)
        {
            var CanRead = property.CanRead;
            var CanWrite = property.CanWrite;
            return CanRead && CanWrite ? null : CanRead;
        }
        #endregion
        #region 返回属性的访问修饰符
        /// <summary>
        /// 获取一个属性读写访问器的访问修饰符，
        /// 如果不支持读或写，则该访问器返回<see langword="null"/>
        /// </summary>
        /// <param name="property">待返回访问修饰符的属性</param>
        /// <returns></returns>
        public static (AccessPermissions? Get, AccessPermissions? Set) GetAccess(this PropertyInfo property)
            => (property.GetMethod?.GetAccess(), property.SetMethod?.GetAccess());
        #endregion
        #region 返回属性是否公开
        /// <summary>
        /// 返回一个属性的所有访问器是否全部为Public
        /// </summary>
        /// <param name="property">待检查的属性</param>
        /// <returns></returns>
        public static bool IsPublic(this PropertyInfo property)
        {
            var (Get, Set) = property.GetAccess();
            static bool Check(AccessPermissions? A)
                => A == null || A.Value == AccessPermissions.Public;
            return Check(Get) && Check(Set);
        }
        #endregion
        #endregion
        #region 关于属性的性质
        #region 返回一个属性是否为静态
        /// <summary>
        /// 返回一个属性是否为静态
        /// </summary>
        /// <param name="Pro">要检查的属性</param>
        /// <returns></returns>
        public static bool IsStatic(this PropertyInfo Pro)
            => (Pro.GetMethod?.IsStatic ?? false) ||
                (Pro.SetMethod?.IsStatic ?? false);
        #endregion
        #region 返回一个属性是否为索引器
        /// <summary>
        /// 返回一个属性是否为索引器
        /// </summary>
        /// <param name="Pro">要判断的属性</param>
        /// <returns></returns>
        public static bool IsIndexing(this PropertyInfo Pro)
            => Pro.GetIndexParameters().Any();
        #endregion
        #endregion
        #region 关于属性的值
        #region 返回一个属性的值
        /// <summary>
        /// 通过Get访问器，返回一个属性的值，如果属性不可读，直接报错
        /// </summary>
        /// <typeparam name="Ret">返回值类型，会自动进行转换</typeparam>
        /// <param name="Pro">要返回值的属性</param>
        /// <param name="Sou">属性所依附的对象，如果是静态属性，直接忽略</param>
        /// <param name="Parameters">属性的参数，如果这个属性不是索引器，则应忽略</param>
        /// <returns></returns>
        public static Ret? GetValue<Ret>(this PropertyInfo Pro, object? Sou = null, params object[] Parameters)
            => (Ret?)Pro.GetValue(Sou, Parameters);
        #endregion
        #region 打包属性的Get和Set访问器
        /// <summary>
        /// 打包一个属性的Get访问器和Set访问器，并作为委托返回，
        /// 如果属性不可读或不可写，则对应的访问器返回<see langword="null"/>
        /// </summary>
        /// <typeparam name="Obj">属性所依附的对象类型</typeparam>
        /// <typeparam name="Pro">属性本身的类型</typeparam>
        /// <param name="Property">要打包的属性</param>
        /// <returns></returns>
        public static (Func<Obj, Pro>? Get, Action<Obj, Pro>? Set) GetGS<Obj, Pro>(this PropertyInfo Property)
        {
            var Get =  Property.GetMethod?.CreateDelegate<Func<Obj, Pro>>();
            var Set = Property.SetMethod?.CreateDelegate<Action<Obj, Pro>>();
            return (Get, Set);
        }
        #endregion
        #region 复制属性的值
        /// <summary>
        /// 将一个对象属性的值复制到另一个对象
        /// </summary>
        /// <param name="pro">待复制的属性</param>
        /// <param name="Source">复制的源对象</param>
        /// <param name="Target">复制的目标</param>
        public static void Copy(this PropertyInfo pro, object? Source, object? Target)
            => pro.SetValue(Target, pro.GetValue(Source));
        #endregion
        #endregion 
        #endregion
        #region 关于成员
        #region 获取一个成员的真正类型
        /// <summary>
        /// 获取一个成员的真正类型
        /// </summary>
        /// <param name="member">要获取真正类型的成员</param>
        /// <returns></returns>
        public static Type GetTypeTrue(this MemberInfo member)
            => member.MemberType switch
            {
                MemberTypes.Constructor => typeof(ConstructorInfo),
                MemberTypes.Event => typeof(EventInfo),
                MemberTypes.Field => typeof(FieldInfo),
                MemberTypes.Method => typeof(MethodInfo),
                MemberTypes.Property => typeof(PropertyInfo),
                MemberTypes.TypeInfo => typeof(Type),
                _ => typeof(MemberTypes)     //此为占位，后续会加以修改
            };

        /*需要这个方法的原因在于：
          MemberInfo的GetType方法貌似返回一个动态类型，
          通过它无法方便的判断这个MemberInfo到底是什么类型*/
        #endregion
        #region 获取成员的访问修饰符
        /// <summary>
        /// 获取一个成员的访问修饰符
        /// </summary>
        /// <param name="member">要获取访问修饰符的成员，不支持属性</param>
        /// <returns></returns>
        public static AccessPermissions GetAccess(this MemberInfo member)
        {
            switch (member)
            {
                case Type t:
                    return t.IsPublic ? AccessPermissions.Public : AccessPermissions.Internal;
                case EventInfo e:
                    return e.AddMethod!.GetAccess();
                case PropertyInfo:
                    throw new NotSupportedException("不支持直接获取属性的访问修饰符，因为属性的读写访问器修饰符可能不同");
                default:
                    var dy = (dynamic)member;
                    if (dy.IsPublic)
                        return AccessPermissions.Public;
                    if (dy.IsPrivate)
                        return AccessPermissions.Private;
                    if (dy.IsFamily)
                        return AccessPermissions.Protected;
                    return dy.IsFamilyAndAssembly ?
                        AccessPermissions.InternalProtected : AccessPermissions.PrivateProtected;
            }
        }
        #endregion
        #endregion
        #region 关于类型
        #region 获取类型的所有基类
        /// <summary>
        /// 按照继承树中从下往上的顺序，枚举一个类型的所有基类
        /// </summary>
        /// <param name="type">要枚举基类的类型</param>
        /// <param name="Stop">枚举将在到达这个类型时停止，
        /// 如果为<see langword="null"/>，则默认为到达<see cref="object"/>时停止</param>
        /// <returns></returns>
        public static IEnumerable<Type> BaseTypeAll(this Type type, Type? Stop = null)
        {
            while (true)
            {
                var Base = type.BaseType;
                if (Base == null || Base == Stop)
                    yield break;
                yield return Base;
            }
        }
        #endregion
        #region 对类型的判断
        #region 判断一个类型是否为静态类
        /// <summary>
        /// 判断一个类型是否为静态类
        /// </summary>
        /// <param name="type">待判断的类型</param>
        /// <returns></returns>
        public static bool IsStatic(this Type type)
            => type.IsSealed && type.IsAbstract;
        #endregion
        #region 判断一个对象是否能够赋值给一个类型
        /// <summary>
        /// 确定指定的实例是否能赋值给当前类型的变量
        /// </summary>
        /// <param name="type">被赋值的类型</param>
        /// <param name="obj">赋值给指定类型变量的对象</param>
        /// <returns></returns>
        public static bool IsAssignableFrom(this Type type, object? obj)
            => obj == null ? type.CanNull() : type.IsAssignableFrom(obj.GetType());
        #endregion
        #region 判断一个类型是否可空
        /// <summary>
        /// 判断一个类型是否可空
        /// </summary>
        /// <param name="type">待判断的类型</param>
        /// <returns></returns>
        public static bool CanNull(this Type type)
            => (type.IsClass && !type.IsStatic()) || type.IsInterface ||        //注意：如果某类型为引用类型，但它是静态类，则仍然认为它不可空
                type.IsGenericRealize(typeof(Nullable<>));
        #endregion
        #region 判断类型是否为数字或字符
        /// <summary>
        /// 返回一个类型是否为数字或字符类型，
        /// 这些类型在任何平台，任何语言中都能够得到兼容
        /// </summary>
        /// <param name="type">待检查的类型</param>
        /// <returns></returns>
        public static bool IsPrimitive(this Type type)
            => Type.GetTypeCode(type) switch
            {
                TypeCode.Object or TypeCode.DBNull or TypeCode.Empty or TypeCode.DateTime => false,
                _ => true
            };
        #endregion
        #region 判断泛型类型实现和定义之间的关系
        /// <summary>
        /// 判断一个泛型类型，是否为另一个泛型类型定义的实现，
        /// 类似List&lt;int&gt;和<see cref="List{T}"/>的关系
        /// </summary>
        /// <param name="type">泛型类型实现</param>
        /// <param name="Definition">要检查的泛型类型定义</param>
        /// <returns></returns>
        public static bool IsGenericRealize(this Type type, Type Definition)
            => type.IsGenericType &&
            type.IsConstructedGenericType &&
            Definition.IsGenericTypeDefinition &&
            type.GetGenericTypeDefinition() == Definition;
        #endregion
        #endregion
        #region 获取一个对象的类型数据
        /// <summary>
        /// 对一个对象或类型进行反射，返回它的<see cref="TypeData"/>
        /// </summary>
        /// <param name="obj">如果这个对象是<see cref="Type"/>，则获取它本身，
        /// 如果不是，则调用它的<see cref="object.GetType"/>方法，获取它的<see cref="Type"/></param>
        /// <returns></returns>
        public static TypeData GetTypeData(this object obj)
            => TypeData.GetTypeData(obj is Type a ? a : obj.GetType());
        #endregion
        #region 增强版获取对象类型
        /// <summary>
        /// 如果一个对象是<see cref="Type"/>，则返回它本身，
        /// 如果不是，则调用<see cref="object.GetType"/>获取它的类型
        /// </summary>
        /// <param name="Obj">要获取类型的对象</param>
        /// <returns></returns>
        internal static Type GetTypeObj(this object Obj)
            => Obj is Type a ? a : Obj.GetType();
        #endregion
        #endregion
    }
    #region 用于标志访问修饰符的枚举
    /// <summary>
    /// 这个枚举可以用来表示类型成员的访问权限
    /// </summary>
    public enum AccessPermissions
    {
        /// <summary>
        /// 表示在任何地方都可访问
        /// </summary>
        Public,
        /// <summary>
        /// 表示仅能在类型的内部访问
        /// </summary>
        Private,
        /// <summary>
        /// 表示仅能在派生类中访问
        /// </summary>
        Protected,
        /// <summary>
        /// 表示仅能在程序集内部访问
        /// </summary>
        Internal,
        /// <summary>
        /// 表示仅能在程序集内部或派生类中访问
        /// </summary>
        InternalProtected,
        /// <summary>
        /// 表示仅能在程序集内部的派生类中访问
        /// </summary>
        PrivateProtected
    }
    #endregion
}
