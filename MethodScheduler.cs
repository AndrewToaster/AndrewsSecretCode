using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AndrewsSecretCode
{
    public class MethodScheduler
    {
        private readonly Dictionary<int, ScheduleTask> _tasks;
        private readonly HashSet<int> _registeredIds;
        private readonly Random _rng;

        public MethodScheduler()
        {
            _tasks = new Dictionary<int, ScheduleTask>();
            _registeredIds = new HashSet<int>();
            _rng = new Random();
        }

        public void Tick()
        {
            foreach (var task in _tasks.ToArray())
            {
                if (task.Value.CanExecute())
                {
                    _registeredIds.Remove(task.Key);
                    _tasks.Remove(task.Key);
                    Task.Run(task.Value.Method);
                }
            }
        }

        public int Schedule(Action action, TimeSpan delay)
        {
            int id = GetId();
            _tasks[id] = new ScheduleTask(DateTime.UtcNow + delay, action);

            return id;
        }

        public void Halt()
        {
            _tasks.Clear();
            _registeredIds.Clear();
        }

        public bool Cancel(int id)
        {
            return _tasks.Remove(id);
        }

        private int GetId()
        {
            if (_tasks.Count == int.MaxValue)
            {
                throw new InvalidOperationException("You did it. You crazy son of a bitch, you did it. You overflowed the task list with more than 2147483647 tasks. " +
                                                    "I tip my hat you to, one legend to another");
            }

            int id = _rng.Next(-1, int.MaxValue) + 1;

            while (_registeredIds.Contains(id))
            {
                id = _rng.Next(-1, int.MaxValue) + 1;
            }

            return id;
        }

        private struct ScheduleTask
        {
            public DateTime Date { get; }
            public Action Method { get; }

            public ScheduleTask(DateTime date, Action task)
            {
                Date = date;
                Method = task;
            }

            public bool CanExecute()
            {
                return Date <= DateTime.UtcNow;
            }
        }
    }
}
