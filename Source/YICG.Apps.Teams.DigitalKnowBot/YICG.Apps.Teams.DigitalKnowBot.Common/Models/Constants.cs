// <copyright file="Constants.cs" company="Games For Seva">
// Copyright (c) Games For Seva. All rights reserved.
// </copyright>

namespace YICG.Apps.Teams.DigitalKnowBot.Common.Models
{
    /// <summary>
    /// This class defines all the constants that the Ganga Game bot is working.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// This constant represents a conversation in personal scope.
        /// </summary>
        public const string ConversationTypePersonal = "personal";

        /// <summary>
        /// This constant represents a conversation in team scope.
        /// </summary>
        public const string ConversationTypeChannel = "channel";

        /// <summary>
        /// This constant represents a command to be taking a tour in personal scope.
        /// </summary>
        public const string TakeATourPersonalCommand = "take a tour";

        /// <summary>
        /// This constant represents a command to ask an expert in personal scope.
        /// </summary>
        public const string AskAnExpertPersonalCommand = "ask an expert";

        /// <summary>
        /// This constant represents a command to share feedback in personal scope.
        /// </summary>
        public const string ShareFeedbackPersonalCommand = "share feedback";

        /// <summary>
        /// This constant represents a command to ask an expert from a response card.
        /// </summary>
        public const string AskAnExpertFromCard = "ask an expert";

        /// <summary>
        /// This constant represents a command to share feedback from a response card.
        /// </summary>
        public const string ShareFeedbackFromCard = "share feedback";

        /// <summary>
        /// This constant represents a command to take a tour int team scope.
        /// </summary>
        public const string TeamTourChannelCommand = "team tour";
    }
}