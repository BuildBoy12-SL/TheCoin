// -----------------------------------------------------------------------
// <copyright file="EventHandlers.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace TheCoin
{
    using System.Collections.Generic;
    using System.Linq;
    using Exiled.API.Features;
    using Exiled.API.Features.Items;
    using Exiled.Events.EventArgs;
    using Scp914;

    /// <summary>
    /// Handles events derived from <see cref="Exiled.Events.Handlers"/>.
    /// </summary>
    public class EventHandlers
    {
        private readonly Plugin plugin;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventHandlers"/> class.
        /// </summary>
        /// <param name="plugin">An instance of the <see cref="Plugin"/> class.</param>
        public EventHandlers(Plugin plugin) => this.plugin = plugin;

        /// <inheritdoc cref="Exiled.Events.Handlers.Scp914.OnUpgradingItem(UpgradingItemEventArgs)"/>
        public void OnUpgradingItem(UpgradingItemEventArgs ev)
        {
            if (ev.Item.Type != ItemType.Coin)
                return;

            ev.Item.Destroy();
            GetItem(ev.KnobSetting)?.Spawn(ev.OutputPosition);
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Scp914.OnUpgradingPlayer(UpgradingPlayerEventArgs)"/>
        public void OnUpgradingPlayer(UpgradingPlayerEventArgs ev)
        {
            if (ev.Player.SessionVariables.ContainsKey("IsNPC"))
                return;

            if (ev.HeldOnly && ev.Player.CurrentItem != null)
            {
                UpgradeItem(ev.Player.CurrentItem, ev.KnobSetting, ev.Player);
                return;
            }

            foreach (Item item in ev.Player.Items.ToList())
                UpgradeItem(item, ev.KnobSetting, ev.Player);
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Scp914.OnUpgradingInventoryItem(UpgradingInventoryItemEventArgs)"/>
        public void OnUpgradingInventoryItem(UpgradingInventoryItemEventArgs ev)
        {
            if (ev.Player.SessionVariables.ContainsKey("IsNPC"))
                return;

            if (ev.Player.CurrentItem != null)
                UpgradeItem(ev.Player.CurrentItem, ev.KnobSetting, ev.Player);
        }

        private void UpgradeItem(Item item, Scp914KnobSetting setting, Player player)
        {
            if (item.Type != ItemType.Coin)
                return;

            if (player.RemoveItem(item))
                GetItem(setting)?.Give(player);
        }

        private Item GetItem(Scp914KnobSetting setting)
        {
            if (!plugin.Config.Recipes.TryGetValue(setting, out List<ItemType> items))
                return null;

            if (items.Count == 0)
                return null;

            ItemType itemType = items[Exiled.Loader.Loader.Random.Next(items.Count)];
            if (itemType == ItemType.None)
                return null;

            Item newItem = new Item(itemType);
            if (newItem is MicroHid microHid)
                microHid.Energy = 1f;

            return newItem;
        }
    }
}