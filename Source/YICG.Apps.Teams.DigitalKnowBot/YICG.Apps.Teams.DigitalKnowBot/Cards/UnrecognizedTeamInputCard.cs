// <copyright file="UnrecognizedTeamInputCard.cs" company="Games For Seva">
// Copyright (c) Games For Seva. All rights reserved.
// </copyright>

namespace YICG.Apps.Teams.DigitalKnowBot.Cards
{
    using System.Collections.Generic;
    using Microsoft.Bot.Schema;
    using YICG.Apps.Teams.DigitalKnowBot.Common.Models;
    using YICG.Apps.Teams.DigitalKnowBot.Properties;

    /// <summary>
    /// This class is responsible for creating the unrecognized input whenever there is a message
    /// sent to the bot that's installed in a team.
    /// </summary>
    public class UnrecognizedTeamInputCard
    {
        /// <summary>
        /// This method will generate an adaptive card to be sent in a team conversation.
        /// </summary>
        /// <returns>An attachment to append to a message.</returns>
        public static Attachment GetCard()
        {
            var card = new HeroCard
            {
                Text = Strings.UnrecognizedTeamInputMessage,
                Buttons = new List<CardAction>
                {
                    new CardAction(ActionTypes.MessageBack)
                    {
                        Title = Strings.TakeATeamTourButtonText,
                        DisplayText = Strings.TakeATeamTourButtonText,
                        Text = Constants.TeamTourChannelCommand,
                    },
                },
            };

            return card.ToAttachment();
        }
    }
}
