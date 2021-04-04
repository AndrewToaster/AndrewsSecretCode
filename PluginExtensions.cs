using Smod2;
using Smod2.EventHandlers;
using System;
using System.Linq;
using System.Reflection;

namespace AndrewsSecretCode
{
    public static class PluginExtensions
    {
        public static void LoadEventHandlers<TPlugin>(this TPlugin plugin, bool forceLoad = false) where TPlugin : Plugin
        {
            Type pluginType = plugin.GetType();
            Type baseHandler = typeof(PluginEventHandler<TPlugin>);

            object[] handleCtorArgs = new object[] { plugin };

            foreach (Type type in pluginType.Assembly.GetTypes().Where(type => (baseHandler.IsAssignableFrom(type) || type.BaseType == baseHandler) && type != baseHandler))
            {
                ConstructorInfo constructor = type.GetConstructor(new Type[] { pluginType });

                if (constructor is null)
                {
                    plugin.Warn($"Event handler class '{type.FullName}' does not contain a constructor that takes a '{pluginType.FullName}' parameter! Skipping...");
                    continue;
                }

                if (!typeof(IEventHandler).IsAssignableFrom(type))
                {
                    plugin.Warn($"Event handler class '{type.FullName}' does not contain a any interfaces deriving from '{typeof(IEventHandler).FullName}' interface! Skipping...");
                    continue;
                }

                // Laziness cost me 5 hours of wasted time, thanks me
                object boxedHandler = Activator.CreateInstance(type, handleCtorArgs);
                PluginEventHandler<TPlugin> eventHandler = (PluginEventHandler<TPlugin>)boxedHandler;
                IEventHandler interfaceHandler = (IEventHandler)boxedHandler;

                if (forceLoad || eventHandler.AutoLoad)
                {
                    // Load BaseEventHandler
                    plugin.EventManager.AddEventHandlers(plugin, interfaceHandler, eventHandler.HandlerPriority);
                    plugin.Info($"Loaded event handler '{type.FullName}'");
                }
            }
        }

        public static void Info(this Plugin plugin, object val)
        {
            plugin.Info(val.ToString());
        }

        public static void Warn(this Plugin plugin, object val)
        {
            plugin.Warn(val.ToString());
        }

        public static void Error(this Plugin plugin, object val)
        {
            plugin.Error(val.ToString());
        }

        public static void Debug(this Plugin plugin, object val)
        {
            plugin.Debug(val.ToString());
        }
    }
}
