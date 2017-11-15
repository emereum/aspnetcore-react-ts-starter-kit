using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NHibernate.Cfg;
using NHibernate.Event;
using TemplateProductName.Common;

namespace TemplateProductName.Persistence
{
    internal static class ConfigurationExtensions
    {
        /// <summary>
        /// Adds pre-insert and pre-update hooks from the specified assembly.
        /// </summary>
        public static void AddListenersFromAssembly(this Configuration configuration, Assembly assembly)
        {
            new Dictionary<Type, ListenerType>
            {
                {typeof (IPreInsertEventListener), ListenerType.PreInsert},
                {typeof (IPreUpdateEventListener), ListenerType.PreUpdate}
            }
            .Each(listener =>
            {
                assembly
                .GetTypes()
                .Where(x => x.ImplementsInterface(listener.Key))
                .Each(x =>
                {
                    var wrapper = Array.CreateInstance(x, 1);
                    wrapper.SetValue(Activator.CreateInstance(x), 0);
                    configuration.AppendListeners(listener.Value, (object[])wrapper);
                });
            });
        }
    }
}
