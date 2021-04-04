using System;
using System.Collections.Generic;
using System.Text;
using Smod2.API;
using Smod2.EventHandlers;
using Smod2.Events;

namespace AndrewsSecretCode.Events
{
    public class WeaponFireEvent : Event
    {
        public Player Player { get; }
        public Weapon Weapon { get; }

        public WeaponFireEvent(Player player, Weapon weapon)
        {
            Player = player;
            Weapon = weapon;
        }

        public override void ExecuteHandler(IEventHandler handler)
        {
            ((IEventHandlerWeaponFire)handler).OnWeaponFire(this);
        }
    }
}
