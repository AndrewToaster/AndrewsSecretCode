using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Smod2.API;

namespace AndrewsSecretCode.Utility
{
    public class PlayerDictionary<TValue> : IDictionary<int, TValue>
    {
        #region Ctor

        private readonly Dictionary<int, TValue> _dict;

        public PlayerDictionary()
        {
            _dict = new Dictionary<int, TValue>();
        }
        public PlayerDictionary(int capacity)
        {
            _dict = new Dictionary<int, TValue>(capacity);
        }

        public PlayerDictionary(IDictionary<int, TValue> collection)
        {
            _dict = new Dictionary<int, TValue>(collection);
        }

        #endregion Ctor

        #region Custom Functionality

        public TValue this[Player player] { get => _dict[player.PlayerId]; set => _dict[player.PlayerId] = value; }

        public void Add(Player player, TValue value)
        {
            _dict.Add(player.PlayerId, value);
        }

        public bool ContainsKey(Player player)
        {
            return _dict.ContainsKey(player.PlayerId);
        }

        public bool Remove(Player player)
        {
            return _dict.Remove(player.PlayerId);
        }

        public bool TryGetValue(Player player, out TValue value)
        {
            return _dict.TryGetValue(player.PlayerId, out value);
        }

        public int PruneInclusive(IEnumerable<Player> players)
        {
            int count = 0;
            foreach (var player in players)
            {
                if (_dict.Remove(player.PlayerId))
                    count++;
            }

            return count;
        }

        public int PruneInclusive(IEnumerable<int> players)
        {
            int count = 0;
            foreach (var player in players)
            {
                if (_dict.Remove(player))
                    count++;
            }

            return count;
        }

        public int PruneExclusive(IEnumerable<Player> players)
        {
            int count = 0;
            foreach (var player in players.Where(x => !_dict.ContainsKey(x.PlayerId)))
            {
                _dict.Remove(player.PlayerId);
                count++;
            }

            return count;
        }

        public int PruneExclusive(IEnumerable<int> players)
        {
            int count = 0;
            foreach (var player in players.Where(x => !_dict.ContainsKey(x)))
            {
                _dict.Remove(player);
                count++;
            }

            return count;
        }

        #endregion Custom Functionality

        #region Dict Interface

        public TValue this[int key] { get => _dict[key]; set => _dict[key] = value; }

        public ICollection<int> Keys => ((IDictionary<int, TValue>)_dict).Keys;

        public ICollection<TValue> Values => ((IDictionary<int, TValue>)_dict).Values;

        public int Count => _dict.Count;

        public bool IsReadOnly => ((ICollection<KeyValuePair<int, TValue>>)_dict).IsReadOnly;

        public void Add(int key, TValue value)
        {
            _dict.Add(key, value);
        }

        public void Add(KeyValuePair<int, TValue> item)
        {
            ((ICollection<KeyValuePair<int, TValue>>)_dict).Add(item);
        }

        public void Clear()
        {
            _dict.Clear();
        }

        public bool Contains(KeyValuePair<int, TValue> item)
        {
            return ((ICollection<KeyValuePair<int, TValue>>)_dict).Contains(item);
        }

        public bool ContainsKey(int key)
        {
            return _dict.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<int, TValue>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<int, TValue>>)_dict).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<int, TValue>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<int, TValue>>)_dict).GetEnumerator();
        }

        public bool Remove(int key)
        {
            return _dict.Remove(key);
        }

        public bool Remove(KeyValuePair<int, TValue> item)
        {
            return ((ICollection<KeyValuePair<int, TValue>>)_dict).Remove(item);
        }

        public bool TryGetValue(int key, out TValue value)
        {
            return _dict.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_dict).GetEnumerator();
        }

        #endregion Dict Interface
    }
}
