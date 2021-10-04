// -----------------------------------------------------------------------
// <copyright file="Config.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace TheCoin
{
    using System.Collections.Generic;
    using Exiled.API.Interfaces;
    using Scp914;

    /// <inheritdoc />
    public class Config : IConfig
    {
        /// <inheritdoc />
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets a collection of recipes that a coin can generate to.
        /// </summary>
        public Dictionary<Scp914KnobSetting, List<ItemType>> Recipes { get; set; } = new Dictionary<Scp914KnobSetting, List<ItemType>>
        {
            [Scp914KnobSetting.Rough] = new List<ItemType>
            {
                ItemType.Coin,
                ItemType.None,
                ItemType.None,
                ItemType.Flashlight,
            },
            [Scp914KnobSetting.Coarse] = new List<ItemType>
            {
                ItemType.Radio,
                ItemType.None,
                ItemType.None,
            },
            [Scp914KnobSetting.OneToOne] = new List<ItemType>
            {
                ItemType.Coin,
                ItemType.Coin,
                ItemType.Coin,
                ItemType.KeycardScientist,
            },
            [Scp914KnobSetting.Fine] = new List<ItemType>
            {
                ItemType.Medkit,
                ItemType.Painkillers,
                ItemType.KeycardResearchCoordinator,
                ItemType.Coin,
                ItemType.None,
            },
            [Scp914KnobSetting.VeryFine] = new List<ItemType>
            {
                ItemType.None,
                ItemType.None,
                ItemType.None,
                ItemType.KeycardContainmentEngineer,
                ItemType.Coin,
                ItemType.SCP018,
            },
        };
    }
}