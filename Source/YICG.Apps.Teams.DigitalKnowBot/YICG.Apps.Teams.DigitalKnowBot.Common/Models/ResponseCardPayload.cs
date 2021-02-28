// <copyright file="ResponseCardPayload.cs" company="Games For Seva">
// Copyright (c) Games For Seva. All rights reserved.
// </copyright>

namespace YICG.Apps.Teams.DigitalKnowBot.Common.Models
{
    /// <summary>
    /// Represents the payload of a response card.
    /// </summary>
    public class ResponseCardPayload : TeamsAdaptiveSubmitActionData
    {
        /// <summary>
        /// Gets or sets the question that was originally asked by the user.
        /// </summary>
        public string UserQuestion { get; set; }

        /// <summary>
        /// Gets or sets the response given by the bot to the user.
        /// </summary>
        public string KnowledgeBaseAnswer { get; set; }
    }
}