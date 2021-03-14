// <copyright file="WelcomeCard.cs" company="Games For Seva">
// Copyright (c) Games For Seva. All rights reserved.
// </copyright>

namespace YICG.Apps.Teams.DigitalKnowBot.Cards
{
    using System.Collections.Generic;
    using AdaptiveCards;
    using Microsoft.Bot.Schema;

    /// <summary>
    /// This class represents the welcome card that would be shown to the user.
    /// </summary>
    public class WelcomeCard
    {
        /// <summary>
        /// This method will generate the exact card to render.
        /// </summary>
        /// <param name="welcomeText">The welcoming text to show.</param>
        /// <returns>An attachment to be appended to a message.</returns>
        public static Attachment GetCard(string welcomeText)
        {
            AdaptiveCard userWelcomeCard = new AdaptiveCard("1.0")
            {
                Body = new List<AdaptiveElement>
                {
                    new AdaptiveTextBlock
                    {
                        HorizontalAlignment = AdaptiveHorizontalAlignment.Left,
                        Text = welcomeText,
                        Wrap = true,
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
