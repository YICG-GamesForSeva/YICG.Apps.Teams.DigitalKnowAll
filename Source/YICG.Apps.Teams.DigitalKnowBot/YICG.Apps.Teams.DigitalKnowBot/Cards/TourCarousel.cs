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
        /// This method generates the user tour carousel in the personale scope.
        /// </summary>
        /// <param name="appBaseUri">Application based Uri.</param>
        /// <returns>A list of cards to render as the user tour.</returns>
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

        /// <summary>
        /// This method will generate the team tour carousel.
        /// </summary>
        /// <param name="appBaseUri">The local address of the bot.</param>
        /// <returns>A type of <see cref="IEnumerable{Attachment}"/> which will allow for the carousel tour to be shown.</returns>
        public static IEnumerable<Attachment> GetTeamTourCards(string appBaseUri)
        {
            if (appBaseUri is null)
            {
                throw new ArgumentNullException(nameof(appBaseUri));
            }

            return new List<Attachment>
            {
                GetCard(Strings.NotificationCardHeader, Strings.NotificationCardContent, appBaseUri + "/content/Notifications.png"),
                GetCard(Strings.TicketSystemCardHeader, Strings.TicketSystemCardContent, appBaseUri + "/content/Ticketsystem.png"),
                GetCard(Strings.EndUserCardHeader, Strings.EndUserCardContent, appBaseUri + "/content/Enduserchat.png"),
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
