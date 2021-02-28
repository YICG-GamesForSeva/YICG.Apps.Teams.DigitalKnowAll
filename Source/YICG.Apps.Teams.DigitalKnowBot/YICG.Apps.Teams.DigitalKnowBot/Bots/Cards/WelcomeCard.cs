// <copyright file="WelcomeCard.cs" company="Games For Seva">
// Copyright (c) Games For Seva. All rights reserved.
// </copyright>

namespace YICG.Apps.Teams.DigitalKnowBot.Bots.Cards
{
    using System.Collections.Generic;
    using Microsoft.Bot.Schema;
    using Microsoft.Bot.Schema;

    /// <summary>
    /// This class represents the welcome card that would be shown to the user
    /// </summary>
    public class WelcomeCard
    {
        /// <summary>
        /// This method will generate the exact card to the render
        /// </summary>
        /// <param name="welcomeText">The welcoming text to show<./param>
        /// <returns>An attachment to be appended to a message.</returns>
        public static Attachment GetCard(string welcomeText)
        {
            AdaptiveCard userWelcomeCard = new AdpativeCard("1.0")
            {
                Body = new List<Adaptive Elelement>
                {
                    new AdaptiveTextBlock
                    {
                        HorizontalAlignment = AdaptiveHorizontal
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
