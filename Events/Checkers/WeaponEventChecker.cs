using System.Collections.Generic;
using Smod2;
using Smod2.API;
using AndrewsSecretCode.Utility;

namespace AndrewsSecretCode.Events.Checkers
{
    public static class WeaponEventChecker
    {
        private static readonly PlayerDictionary<ItemBulletPair> _itemData;

        static WeaponEventChecker()
        {
            _itemData = new PlayerDictionary<ItemBulletPair>();
        }

        public static void Check(Plugin plugin)
        {
            foreach (var player in plugin.Server.GetPlayers())
            {
                var item = player.GetCurrentItem();

                if (item.IsWeapon)
                {
                    var weapon = item.ToWeapon();
                    var newAmmo = (int)weapon.AmmoInClip;

                    if (_itemData.TryGetValue(player, out ItemBulletPair previous) && item.ItemType == previous.Type)
                    {
                        if (newAmmo < previous.BulletCount)
                        {
                            plugin.EventManager.HandleEvent<IEventHandlerWeaponFire>(new WeaponFireEvent(player, weapon));
                        }
                        if (newAmmo > previous.BulletCount)
                        {
                            plugin.EventManager.HandleEvent<IEventHandlerWeaponReload>(new WeaponReloadEvent(player, weapon, previous.BulletCount, newAmmo));
                        }
                    }

                    _itemData[player] = new ItemBulletPair(item.ItemType, newAmmo);
                }
                else
                {
                    _itemData[player] = new ItemBulletPair(item.ItemType, 0);
                }
            }
        }

        private struct ItemBulletPair
        {
            public ItemType Type { get; }
            public int BulletCount { get; }

            public ItemBulletPair(ItemType type, int bullets)
            {
                Type = type;
                BulletCount = bullets;
            }
        }
    }
}
