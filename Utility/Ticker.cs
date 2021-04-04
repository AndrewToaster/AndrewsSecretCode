using System;
using System.Collections.Generic;
using System.Text;

namespace AndrewsSecretCode.Utility
{
    public class Ticker
    {
        public int TargetTicks { get; set; }
        public int CurrentTick { get; private set; }

        public Ticker(int target)
        {
            TargetTicks = target;
        }

        public bool CheckTick()
        {
            CurrentTick++;

            if (TargetTicks <= CurrentTick)
            {
                CurrentTick = 0;
                return true;
            }

            return false;
        }
    }
}
