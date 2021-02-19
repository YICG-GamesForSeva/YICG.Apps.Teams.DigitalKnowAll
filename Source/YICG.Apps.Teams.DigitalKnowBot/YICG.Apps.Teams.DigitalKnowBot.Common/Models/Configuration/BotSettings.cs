// <copyright file="BotSettings.cs" company="Games For Seva">
// Copyright (c) Games For Seva. All rights reserved.
// </copyright>

namespace YICG.Apps.Teams.DigitalKnowBot.Common.Models.Configuration
{
    /// <summary>
    /// This class represents all of the bot settings.
    /// </summary>
    public class BotSettings
    {
        /// <summary>
        /// Gets or sets the access cache expiry in days.
        /// </summary>
        public int AccessCacheExpiryInDays { get; set; }

        /// <summary>
        /// Gets or sets the application base uri.
        /// </summary>
#pragma warning disable CA1056 // URI-like properties should not be strings
        public string AppBaseUri { get; set; }
#pragma warning restore CA1056 // URI-like properties should not be strings

        /// <summary>
        /// Gets or sets the Microsoft App ID.
        /// </summary>
        public string MicrosoftAppId { get; set; }

        /// <summary>
        /// Gets or sets the TenantID.
        /// </summary>
        public string TenantId { get; set; }
    }
}