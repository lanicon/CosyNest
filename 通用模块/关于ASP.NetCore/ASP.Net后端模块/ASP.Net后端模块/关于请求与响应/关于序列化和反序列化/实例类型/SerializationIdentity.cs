using System;
using System.SafetyFrancis;
using System.Security.Principal;
using System.Text.Json;
using System.TreeObject.Json;

namespace Microsoft.AspNetCore.Json
{
    /// <summary>
    /// 这个类型可以用来序列化和反序列化<see cref="IIdentity"/>
    /// </summary>
    class SerializationIdentity : SerializationBase<IIdentity>
    {
        #region 序列化对象
        public override void Write(Utf8JsonWriter writer, IIdentity value, JsonSerializerOptions options)
        {
            var pseudoIIdentity = value is null ? null : new PseudoIIdentity()
            {
                AuthenticationType = value.AuthenticationType,
                Name = value.Name
            };
            JsonSerializer.Serialize(writer, pseudoIIdentity);
        }
        #endregion
        #region 反序列化对象
        public override IIdentity? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var pseudoIIdentity = JsonSerializer.Deserialize<PseudoIIdentity>(ref reader);
            return pseudoIIdentity is null ? null : CreateSafety.Identity(pseudoIIdentity.AuthenticationType, pseudoIIdentity.Name);
        }
        #endregion
        #region 私有辅助类
        private class PseudoIIdentity
        {
            public string? AuthenticationType { get; set; }
            public string? Name { get; set; }
        }
        #endregion
    }
}
