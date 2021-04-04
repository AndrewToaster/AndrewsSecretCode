using System;
using System.Collections.Generic;
using System.Text;
using Smod2.API;
using Smod2.EventHandlers;
using Smod2.Events;

namespace AndrewsSecretCode.Events
{
    public class WeaponReloadEvent : Event
    {
        public Player Player { get; }
        public Weapon Weapon { get; }

        public int PreviousBulletCount { get; }
        public int NewBulletCount { get; }
        public int BulletsReloaded { get; }

        public WeaponReloadEvent(Player player, Weapon weapon, int previousAmount, int newAmount)
        {
            Player = player;
            Weapon = weapon;
            PreviousBulletCount = previousAmount;
            NewBulletCount = newAmount;
            BulletsReloaded = newAmount - previousAmount;
        }

        public override void ExecuteHandler(IEventHandler handler)
        {
            ((IEventHandlerWeaponReload)handler).OnWeaponReload(this);
        }
    }
}
