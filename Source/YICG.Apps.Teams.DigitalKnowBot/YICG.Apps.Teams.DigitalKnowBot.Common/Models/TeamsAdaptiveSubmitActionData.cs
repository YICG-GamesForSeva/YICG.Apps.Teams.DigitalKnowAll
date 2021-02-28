// <copyright file="TeamsAdaptiveSubmitActionData.cs" company="Games For Seva">
// Copyright (c) Games For Seva. All rights reserved.
// </copyright>

namespace YICG.Apps.Teams.DigitalKnowBot.Common.Models
{
    using Microsoft.Bot.Schema;
    using Newtonsoft.Json;

    /// <summary>
    /// Defines Teams-specific behavior for an adaptive card submit action.
    /// </summary>
    public class TeamsAdaptiveSubmitActionData
    {
        /// <summary>
        /// Gets or sets the Teams-specific action.
        /// </summary>
        [JsonProperty("msteams")]
        public CardAction MsTeams { get; set; }
    }
}
