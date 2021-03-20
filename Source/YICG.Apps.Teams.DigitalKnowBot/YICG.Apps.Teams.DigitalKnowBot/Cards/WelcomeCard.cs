// <copyright file="WelcomeCard.cs" company="Games For Seva">
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
    /// This class is for the welcome card in the personal scope.
    /// </summary>
    public class WelcomeCard
    {
        /// <summary>
        /// This method generates the welcome card.
        /// </summary>
        /// <param name="cardHeader">This is the card header.</param>
        /// <param name="cardContent">This the card content.</param>
        /// <returns>An attachment to the message.</returns>
        public static Attachment GetCard(string cardHeader, string cardContent)
        {
            AdaptiveCard userWelcomeCard = new AdaptiveCard("1.0")
            {
                Body = new List<AdaptiveElement>
                {
                    new AdaptiveTextBlock
                    {
                        HorizontalAlignment = AdaptiveHorizontalAlignment.Center,
                        Text = cardHeader,
                        Wrap = false,
                    },
                    new AdaptiveTextBlock
                    {
                        HorizontalAlignment = AdaptiveHorizontalAlignment.Left,
                        Text = cardHeader,
                        Wrap = true,
                    },
                },
                Actions = new List<AdaptiveAction>
                {
                    new AdaptiveSubmitAction
                    {
                        Title = Strings.TakeATourButtonText,
                        Data = new TeamsAdaptiveSubmitActionData
                        {
                            MsTeams = new CardAction
                            {
                                Type = ActionTypes.MessageBack,
                                DisplayText = Strings.TakeATourButtonText,
                                Text = Constants.TakeATourPersonalCommand,
                            },
                        },
                    },
                },
            };

            return new Attachment
            {
                ContentType = AdaptiveCard.ContentType,
                Content = userWelcomeCard,
            };
        }
    }
}
