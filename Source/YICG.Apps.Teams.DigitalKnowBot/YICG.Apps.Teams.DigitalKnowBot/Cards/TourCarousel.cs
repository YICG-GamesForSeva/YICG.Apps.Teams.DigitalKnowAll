// <copyright file="TourCarousel.cs" company="Games For Seva">
// Copyright (c) Games For Seva. All rights reserved.
// </copyright>

namespace YICG.Apps.Teams.DigitalKnowBot.Cards
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Bot.Schema;
    using YICG.Apps.Teams.DigitalKnowBot.Properties;

    /// <summary>
    /// This class is responsible to generate the tour carousel in the personal and team scopes.
    /// </summary>
    public class TourCarousel
    {
        /// <summary>
        /// This methood generates the user tour carousel in personal scope.
        /// </summary>
        /// <param name="appBaseUri">Application Base URI.</param>
        /// <returns>Tour Card.</returns>
        public static IEnumerable<Attachment> GetUserTourCards(string appBaseUri)
        {
            if (appBaseUri is null)
            {
                throw new ArgumentNullException(nameof(appBaseUri));
            }

            return new List<Attachment>
            {
                GetCard(Strings.FunctionCardHeader, Strings.FunctionCardContent, appBaseUri + "/content/Askaquestion.png"),
                GetCard(Strings.AskAGuideHeader, Strings.AskAGuideContent, appBaseUri + "/content/Expertinquiry.png"),
                GetCard(Strings.ShareFeedbackHeader, Strings.ShareFeedbackContent, appBaseUri + "/content/Sharefeedback.png"),
            };
        }

        private static Attachment GetCard(string cardHeader, string cardContent, string imageLocation)
        {
            HeroCard tourCarouselCard = new HeroCard
            {
                Title = cardHeader,
                Text = cardContent,
                Images = new List<CardImage>
                {
                    new CardImage(imageLocation),
                },
            };
            return tourCarouselCard.ToAttachment();
        }
    }
}
