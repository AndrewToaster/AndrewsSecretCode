using Smod2;
using Smod2.API;
using Smod2.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AndrewsSecretCode.Utility
{
    public class ObjectTagManager
    {
        public Dictionary<object, Dictionary<string, object>> Tags { get; }
        public Dictionary<Type, Func<object, object>> Transformers { get; }

        public ObjectTagManager()
        {
            Tags = new Dictionary<object, Dictionary<string, object>>();
            Transformers = new Dictionary<Type, Func<object, object>>
            {
                { typeof(Player), ply => ((Player)ply).PlayerId },
                { typeof(Item), item => ((Item)item).UniqueIdentifier },
                { typeof(Weapon), weapon => ((Weapon)weapon).UniqueIdentifier }
            };
        }

        public void AddTag(object obj, string name, object tag)
        {
            obj = Transform(obj);

            if (!Tags.ContainsKey(obj))
                Tags[obj] = new Dictionary<string, object>();

            Tags[obj][name] = tag;
        }

        public void AddTags(object obj, IEnumerable<KeyValuePair<string, object>> tags)
        {
            obj = Transform(obj);

            if (!Tags.ContainsKey(obj))
                Tags[obj] = new Dictionary<string, object>();

            Tags[obj].Clear();

            var tagHolder = Tags[obj];
            foreach (var tag in tags)
            {
                tagHolder.Add(tag.Key, tag.Value);
            }
        }

        public T GetTag<T>(object obj, string name)
        {
            obj = Transform(obj);

            if (!Tags.ContainsKey(obj))
                return default;

            if (Tags[obj].TryGetValue(name, out object tag))
            {
                return (T)tag;
            }
            else
            {
                return default;
            }
        }

        public bool RemoveTag(object obj, string name)
        {
            obj = Transform(obj);

            return Tags.ContainsKey(obj) && Tags[obj].Remove(name);
        }

        public T PopTag<T>(object obj, string name)
        {
            obj = Transform(obj);

            if (!Tags.ContainsKey(obj))
                return default;

            if (Tags[obj].TryGetValue(name, out object tag))
            {
                Tags[obj].Remove(name);
                return (T)tag;
            }
            else
            {
                return default;
            }
        }

        public bool HasTag(object obj, string name)
        {
            obj = Transform(obj);

            return Tags.ContainsKey(obj) && Tags[obj].ContainsKey(name);
        }

        public void ClearTags(object obj)
        {
            obj = Transform(obj);

            if (!Tags.ContainsKey(obj))
                return;

            Tags[obj].Clear();
        }

        public IEnumerable<KeyValuePair<string, object>> GetTags(object obj)
        {
            obj = Transform(obj);

            if (!Tags.ContainsKey(obj))
                return Array.Empty<KeyValuePair<string, object>>();

            return Tags[obj].ToArray();
        }

        private object Transform(object obj)
        {
            Type source = obj.GetType();

            foreach (var transformer in Transformers)
            {
                if (transformer.Key.IsAssignableFrom(source))
                {
                    return transformer.Value.Invoke(obj);
                }
            }

            return obj;
        }
    }
}
