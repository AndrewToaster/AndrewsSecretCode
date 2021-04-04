using System;
using System.Collections.Generic;
using System.Text;
using Smod2.API;

namespace AndrewsSecretCode.Helper
{
    public static class PlayerHelper
    {
        public static (int index, Item item) GetItemAndIndex(this Player player, ItemType type)
        {
            var inventory = player.GetInventory();

            int index = inventory.FindIndex(x => x.ItemType == type);

            if (index == -1)
                return (-1, default);

            return (index, inventory[index]);
        }

        public static Item GetFirstItem(this Player player, ItemType type)
        {
            return player.GetInventory().Find(x => x.ItemType == type);
        }

        public static bool IsDead(this Player player)
        {
            return player.TeamRole.Role == RoleType.SPECTATOR;
        }

        public static bool IsRole(this Player player, RoleType role)
        {
            return player.TeamRole.Role == role;
        }
    }
}
