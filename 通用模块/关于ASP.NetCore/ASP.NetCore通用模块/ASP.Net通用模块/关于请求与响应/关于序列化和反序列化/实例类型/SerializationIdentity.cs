using System;
using System.Safety;
using System.Security.Principal;
using System.Text;
using System.TreeObject.Json;

namespace Microsoft.AspNetCore.Json
{
    /// <summary>
    /// 这个类型可以用来序列化和反序列化<see cref="IIdentity"/>
    /// </summary>
    class SerializationIdentity : SerializationJsonBase<IIdentity, IIdentity>
    {
        #region 序列化对象
        protected override ReadOnlySpan<byte> SerializationTemplate(object? obj, Encoding? encoding = null)
        {
            if (obj is null)
                return SerializationAided(null, encoding);
            var identity = obj.To<IIdentity>();
            var pseudoIIdentity = new PseudoIIdentity()
            {
                AuthenticationType = identity.AuthenticationType,
                Name = identity.Name
            };
            return SerializationAided(pseudoIIdentity, encoding);
        }
        #endregion
        #region 反序列化对象
        public override IIdentity? Deserialize(ReadOnlySpan<byte> text, Encoding? encoding = null)
        {
            var pseudoIIdentity = DeserializeAided<PseudoIIdentity>(text, encoding);
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
