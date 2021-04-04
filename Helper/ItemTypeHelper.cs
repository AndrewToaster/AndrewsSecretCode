using System;
using System.Collections.Generic;
using System.Text;
using Smod2.API;

namespace AndrewsSecretCode.Helper
{
    public static class ItemTypeHelper
    {
        public static bool IsWeapon(this ItemType type)
        {
            switch (type)
            {
                case ItemType.GUN_E11_SR:
                case ItemType.GUN_LOGICER:
                case ItemType.GUN_COM15:
                case ItemType.GUN_MP7:
                case ItemType.GUN_PROJECT90:
                case ItemType.GUN_USP:
                    return true;

                default:
                    return false;
            }
        }

        public static bool IsGrenade(this ItemType type)
        {
            switch (type)
            {
                case ItemType.GRENADE_FRAG:
                case ItemType.GRENADE_FLASH:
                case ItemType.SCP018:
                    return true;

                default:
                    return false;
            }
        }
    }
}
