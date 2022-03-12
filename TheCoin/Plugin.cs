// -----------------------------------------------------------------------
// <copyright file="Plugin.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace TheCoin
{
    using System;
    using Exiled.API.Features;
    using Scp914Handlers = Exiled.Events.Handlers.Scp914;

    /// <summary>
    /// The main plugin class.
    /// </summary>
    public class Plugin : Plugin<Config>
    {
        private EventHandlers eventHandlers;

        /// <inheritdoc />
        public override string Author => "Build";

        /// <inheritdoc />
        public override Version RequiredExiledVersion { get; } = new Version(5, 0, 0);

        /// <inheritdoc />
        public override void OnEnabled()
        {
            eventHandlers = new EventHandlers(this);
            Scp914Handlers.UpgradingItem += eventHandlers.OnUpgradingItem;
            Scp914Handlers.UpgradingPlayer += eventHandlers.OnUpgradingPlayer;
            Scp914Handlers.UpgradingInventoryItem += eventHandlers.OnUpgradingInventoryItem;
            base.OnEnabled();
        }

        /// <inheritdoc />
        public override void OnDisabled()
        {
            Scp914Handlers.UpgradingItem -= eventHandlers.OnUpgradingItem;
            Scp914Handlers.UpgradingPlayer -= eventHandlers.OnUpgradingPlayer;
            Scp914Handlers.UpgradingInventoryItem -= eventHandlers.OnUpgradingInventoryItem;
            eventHandlers = null;
            base.OnDisabled();
        }
    }
}