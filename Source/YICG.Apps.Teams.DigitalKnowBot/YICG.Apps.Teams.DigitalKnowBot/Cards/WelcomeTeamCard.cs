// <copyright file="WelcomeTeamCard.cs" company="Games For Seva">
// Copyright (c) Games For Seva. All rights reserved.
// </copyright>

namespace YICG.Apps.Teams.DigitalKnowBot.Cards
{
    using System.Collections.Generic;
    using AdaptiveCards;
    using Microsoft.Bot.Schema;
    using YICG.Apps.Teams.DigitalKnowBot.Common.Models;
    using YICG.Apps.Teams.DigitalKnowBot.Properties;

    /// <summary>
    /// This class is for generating the welcome card to be sent in a team.
    /// </summary>
    public static class WelcomeTeamCard
    {
        /// <summary>
        /// This method will render a welcome card after the bot has been installed to a team.
        /// </summary>
        /// <returns>An attachment to append to a message.</returns>
        public static Attachment GetCard()
        {
            AdaptiveCard teamWelcomeCard = new AdaptiveCard("1.0")
            {
                Body = new List<AdaptiveElement>
                {
                    new AdaptiveTextBlock
                    {
                        Text = Strings.WelcomeTeamCardContent,
                        Wrap = true,
                    },
                },
                Actions = new List<AdaptiveAction>
                {
                    new AdaptiveSubmitAction
                    {
                        Title = Strings.TakeATeamTourButtonText,
                        Data = new TeamsAdaptiveSubmitActionData
                        {
                            MsTeams = new CardAction
                            {
                                Type = ActionTypes.MessageBack,
                                DisplayText = Strings.TakeATeamTourButtonText,
                                Text = Constants.TeamTourChannelCommand,
                            },
                        },
                    },
                },
            };

            return new Attachment
            {
                Content = teamWelcomeCard,
                ContentType = AdaptiveCard.ContentType,
            };
        }
    }
}