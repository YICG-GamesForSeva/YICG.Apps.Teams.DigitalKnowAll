// <copyright file="Constants.cs" company="Games For Seva">
// Copyright (c) Games For Seva. All rights reserved.
// </copyright>

namespace YICG.Apps.Teams.DigitalKnowBot.Common.Models
{
    /// <summary>
    /// This class defines all the constants that the Ganga Game Bot is required to use.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// This constant represents a conversation in the personal scope (1:1 with the bot).
        /// </summary>
        public const string ConversationTypePersonal = "personal";

        /// <summary>
        /// This constant represents a conversation in a Team (many users + bot).
        /// </summary>
        public const string ConversationTypeChannel = "channel";

        /// <summary>
        /// This constant represents the command to be taking a tour in the personal scope.
        /// </summary>
        public const string TakeATourPersonalCommand = "take a tour";
    }
}