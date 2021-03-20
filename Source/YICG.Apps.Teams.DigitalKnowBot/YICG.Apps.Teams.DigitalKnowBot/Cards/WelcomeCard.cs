namespace YICG.Apps.Teams.DigitalKnowBot.Cards
{
    using System.Collections.Generic;
    using AdaptiveCards;
    using Microsoft.Bot.Schema;
    using YICG.Apps.Teams.DigitalKnowBot.Common.Models;
    using YICG.Apps.Teams.DigitalKnowBot.Properties;

    public class WelcomeCard
    {
        /// <summary>
        /// This method will construct the user welcome card when bot is added in personal scope.
        /// </summary>
        /// <param name="cardHeader">Gets welcome text.</param>
        /// <param name="cardContent">The card content.</param>
        /// <returns>User welcome card.</returns>
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
                        Text = cardContent,
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
