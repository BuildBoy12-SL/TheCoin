// -----------------------------------------------------------------------
// <copyright file="EventHandlers.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace TheCoin
{
    using System.Linq;
    using Exiled.API.Features;
    using Exiled.API.Features.Items;
    using Exiled.Events.EventArgs;
    using MEC;
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

            Timing.CallDelayed(0.1f, () =>
            {
                ev.Item.Destroy();
                GetItem(ev.KnobSetting).Spawn(ev.OutputPosition);
            });
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Scp914.OnUpgradingPlayer(UpgradingPlayerEventArgs)"/>
        public void OnUpgradingPlayer(UpgradingPlayerEventArgs ev)
        {
            if (ev.HeldOnly)
            {
                UpgradeItem(ev.Player.CurrentItem, ev.KnobSetting, ev.Player);
                return;
            }

            foreach (var item in ev.Player.Items.ToList())
                UpgradeItem(item, ev.KnobSetting, ev.Player);
        }

        /// <inheritdoc cref="Exiled.Events.Handlers.Scp914.OnUpgradingInventoryItem(UpgradingInventoryItemEventArgs)"/>
        public void OnUpgradingInventoryItem(UpgradingInventoryItemEventArgs ev)
        {
            UpgradeItem(ev.Player.CurrentItem, ev.KnobSetting, ev.Player);
        }

        private void UpgradeItem(Item item, Scp914KnobSetting setting, Player player)
        {
            if (item.Type != ItemType.Coin)
                return;

            Timing.CallDelayed(0.1f, () =>
            {
                player.RemoveItem(item);
                GetItem(setting).Give(player);
            });
        }

        private Item GetItem(Scp914KnobSetting setting)
        {
            ItemType itemType = plugin.Config.Recipes[setting].ElementAt(Exiled.Loader.Loader.Random.Next(plugin.Config.Recipes[setting].Count));
            Item newItem = new Item(itemType);
            if (newItem is MicroHid microHid)
                microHid.Energy = 1f;

            return newItem;
        }
    }
}