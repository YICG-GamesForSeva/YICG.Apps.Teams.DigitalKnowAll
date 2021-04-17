// <copyright file="ResponseCard.cs" company="Games For Seva">
// Copyright (c) Games For Seva. All rights reserved.
// </copyright>

namespace YICG.Apps.Teams.DigitalKnowBot.Cards
{
    using System.Collections.Generic;
    using AdaptiveCards;
    using Microsoft.Bot.Schema;
    using YICG.Apps.Teams.DigitalKnowBot.Properties;

    /// <summary>
    /// This class is resposible for rendering a QnA Maker .
    /// </summary>
    public static class ResponseCard
    {
        /// <summary>
        /// This method will construct a card, and render the QnA Maker answer in a user friendly way.
        /// </summary>
        /// <param name="question">The Question from the QnA Maker.</param>
        /// <param name="answer">The Answer from the QnA maker.</param>
        /// <param name="userQuestion">The question tha the user has asked the bot.</param>
        /// <returns>An attachment to append to a message.</returns>
        public static Attachment GetCard(string question, string answer, string userQuestion)
        {
            AdaptiveCard responseCard = new AdaptiveCard("1.0")
            {
                Body = new List<AdaptiveElement>
                {
                    new AdaptiveTextBlock
                    {
                        Weight = AdaptiveTextWeight.Bolder,
                        Text = Strings.ResponseCardHeaderText,
                        Wrap = true,
                    },
                    new AdaptiveTextBlock
                    {
                        Text = answer,
                        Wrap = true,
                    },
                },
            };

            return new Attachment
            {
                Content = responseCard,
                ContentType = AdaptiveCard.ContentType,
            };
        }
    }
}
