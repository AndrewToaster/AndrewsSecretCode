using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AndrewsSecretCode.Utility;
using Smod2.API;

namespace AndrewsSecretCode
{
    public struct BroadcastMessage
    {
        public string Message { get; }
        public TimeSpan Duration { get; }
        public bool Monospaced { get; }

        public DateTime CreatedAt { get; }
        public DateTime ExpireAt { get; }

        public BroadcastMessage(string msg, TimeSpan duration, bool monospaced = false)
        {
            Message = msg;
            Duration = duration;
            Monospaced = monospaced;
            CreatedAt = DateTime.UtcNow;
            ExpireAt = CreatedAt + Duration;
        }

        public void SendMessage(Player player, SecondRounding rounding = SecondRounding.Round)
        {
            uint time = 0;

            switch (rounding)
            {
                case SecondRounding.Round:
                    time = (uint)Math.Round(Duration.TotalSeconds);
                    break;

                case SecondRounding.Floor:
                    time = (uint)Math.Floor(Duration.TotalSeconds);
                    break;

                case SecondRounding.Ceil:
                    time = (uint)Math.Ceiling(Duration.TotalSeconds);
                    break;
            }

            player.PersonalBroadcast(time, Message, Monospaced);
        }
    }

    public class BroadcastManager
    {
        private readonly PlayerDictionary<SortedDictionary<int, BroadcastMessage>> _stack;
        private readonly Ticker _ticker;
        private readonly HashSet<int> _dirtySlots;

        public SecondRounding Rounding { get; set; }

        public BroadcastManager(SecondRounding rounding = SecondRounding.Round)
        {
            Rounding = rounding;
            _stack = new PlayerDictionary<SortedDictionary<int, BroadcastMessage>>();
            _ticker = new Ticker(4);
            _dirtySlots = new HashSet<int>();
        }

        public void Tick(Server server)
        {
            if (_ticker.CheckTick() && _dirtySlots.Count > 0)
            {
                UpdateBroadcastStack(server);
            }
        }

        public void UpdateBroadcastStack(Server server)
        {
            var players = server.GetPlayers();
            _stack.PruneExclusive(players);

            DateTime utcNow = DateTime.UtcNow;

            foreach (int dirtySlot in _dirtySlots.ToArray())
            {
                var broadStack = _stack[dirtySlot];
                var copyStack = broadStack.ToArray();
                var player = players.Find(x => x.PlayerId == dirtySlot);
                broadStack.Clear();

                bool isFirst = true;

                foreach (var broadcast in copyStack)
                {
                    int priority = broadcast.Key;
                    var broadMsg = broadcast.Value;

                    if (broadMsg.ExpireAt <= utcNow)
                        continue;

                    if (isFirst)
                    {
                        isFirst = false;
                        _dirtySlots.Remove(dirtySlot);
                        player.PersonalClearBroadcasts();
                        broadMsg.SendMessage(player, Rounding);
                    }

                    broadStack[priority] = broadMsg;
                }
            }
        }

        public void SendMessage(Player player, string message, TimeSpan duration, int priority = 10, bool monospaced = false)
        {
            if (!_stack.ContainsKey(player))
            {
                _stack[player] = new SortedDictionary<int, BroadcastMessage>();
            }

            if (_stack[player].Count == 0 || priority <= _stack[player].First().Key)
            {
                if (!_dirtySlots.Contains(player.PlayerId))
                    _dirtySlots.Add(player.PlayerId);

                _stack[player][priority] = new BroadcastMessage(message, duration, monospaced);
            }
        }

        public void SendServerMessage(IEnumerable<Player> players, string message, TimeSpan duration, int priority = 10, bool monospaced = false)
        {
            foreach (var player in players)
            {
                SendMessage(player, message, duration, priority, monospaced);
            }
        }
    }

    public enum SecondRounding
    {
        Round,
        Floor,
        Ceil
    }
}
