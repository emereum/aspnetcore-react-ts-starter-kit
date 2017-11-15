using System;
using Newtonsoft.Json.Serialization;
using NHibernate.Proxy;

namespace TemplateProductName.WebApi.Infrastructure {
    /// <summary>
    /// Prevents serialization of NHibernate-related properties when
    /// serializing entities without converting to models.
    /// </summary>
    public class NHibernateContractResolver : DefaultContractResolver {
        protected override JsonContract CreateContract(Type objectType) {
            return typeof(INHibernateProxy).IsAssignableFrom(objectType)
                ? base.CreateContract(objectType.BaseType)
                : base.CreateContract(objectType);
        }
    }
}
