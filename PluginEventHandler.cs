using Smod2;
using Smod2.Events;
using System;

namespace AndrewsSecretCode
{
    public abstract class PluginEventHandler<T> where T : Plugin
    {
        public virtual Priority HandlerPriority { get; } = Priority.Normal;
        public virtual bool AutoLoad { get; } = true;

        public T Plugin { get; }

        public PluginEventHandler(T plugin)
        {
            Plugin = plugin;
        }

        public virtual void OnReload()
        {
        }

        public void Info(string message)
        {
            Plugin.Info(message);
        }
        public void Info(object val)
        {
            Plugin.Info(val.ToString());
        }

        public void Warn(string message)
        {
            Plugin.Warn(message);
        }
        public void Warn(object val)
        {
            Plugin.Warn(val.ToString());
        }

        public void Error(string message)
        {
            Plugin.Error(message);
        }
        public void Error(object val)
        {
            Plugin.Error(val.ToString());
        }

        public void Debug(string message)
        {
            Plugin.Debug(message);
        }
        public void Debug(object val)
        {
            Plugin.Debug(val.ToString());
        }
    }
}
