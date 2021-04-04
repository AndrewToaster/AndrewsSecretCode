using System;
using System.Collections.Generic;
using System.Text;
using Smod2.EventHandlers;

namespace AndrewsSecretCode.Events
{
    public interface IEventHandlerWeaponReload : IEventHandler
    {
        void OnWeaponReload(WeaponReloadEvent ev);
    }
}
