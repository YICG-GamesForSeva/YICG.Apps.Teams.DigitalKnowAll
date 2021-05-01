// <copyright file="AskAnExpertCard.cs" company="Games For Seva">
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
    /// This class will generate the "Ask an Expert".
    /// </summary>
    public static class AskAnExpertCard
    {
        /// <summary>
        /// Text associated with the ask an expert command.
        /// </summary>
        public const string AskAnExpertSubmitText = "QuestionForExpert";

        /// <summary>
        /// This method will render the ask an expert card, when invoked from the bot power command menu.
        /// </summary>
        /// <returns>The ask an expert card.</returns>
        public static Attachment GetCard()
        {
            return GetCard(false, new AskAnExpertCardPayload());
        }

        /// <summary>
        /// This method will render the ask an expert card when invoked from a KB response.
        /// </summary>
        /// <param name="payload">The payload from the response card that contains the KB details.</param>
        /// <returns>The ask an expert card with the KB details.</returns>
        public static Attachment GetCard(ResponseCardPayload payload)
        {
            var data = new AskAnExpertCardPayload
            {
                Description = payload.UserQuestion,
                UserQuestion = payload.UserQuestion,
                KnowledgeBaseAnswer = payload.KnowledgeBaseAnswer,
            };

            return GetCard(false, data);
        }

        /// <summary>
        /// This method will render the ask an expert card if there are validation errors and marks the errors if any.
        /// </summary>
        /// <param name="payload">The Ask an Expert card details, including errors, if there are any.</param>
        /// <returns>The ask an expert card with validation error details.</returns>
        public static Attachment GetCard(AskAnExpertCardPayload payload)
        {
            return GetCard(true, payload);
        }

        private static Attachment GetCard(bool showValidationErrors, AskAnExpertCardPayload data)
        {
            AdaptiveCard askAnExpertCard = new AdaptiveCard("1.0")
            {
                Body = new List<AdaptiveElement>
                {
                    new AdaptiveTextBlock
                    {
                        Weight = AdaptiveTextWeight.Bolder,
                        Text = Strings.AskAnExpertText1,
                        Size = AdaptiveTextSize.Large,
                        Wrap = true,
                    },
                    new AdaptiveTextBlock
                    {
                        Text = Strings.AskAnExpertSubheaderText,
                        Wrap = true,
                    },
                    new AdaptiveColumnSet
                    {
                        Columns = new List<AdaptiveColumn>
                        {
                            new AdaptiveColumn
                            {
                                Width = AdaptiveColumnWidth.Auto,
                                Items = new List<AdaptiveElement>
                                {
                                    new AdaptiveTextBlock
                                    {
                                        Text = Strings.TitleRequiredText,
                                        Wrap = true,
                                    },
                                },
                            },
                            new AdaptiveColumn
                            {
                                Items = new List<AdaptiveElement>
                                {
                                    new AdaptiveTextBlock
                                    {
                                        Text = (showValidationErrors && string.IsNullOrWhiteSpace(data.Title)) ? Strings.MandatoryTitleFieldText : string.Empty,
                                        Color = AdaptiveTextColor.Attention,
                                        HorizontalAlignment = AdaptiveHorizontalAlignment.Right,
                                        Wrap = true,
                                    },
                                },
                            },
                        },
                    },
                    new AdaptiveTextInput
                    {
                        Id = nameof(AskAnExpertCardPayload.Title),
                        Placeholder = Strings.ShowCardTitleText,
                        IsMultiline = false,
                        Spacing = AdaptiveSpacing.Small,
                        Value = data.Title,
                    },
                    new AdaptiveTextBlock
                    {
                        Text = Strings.DescriptionText,
                        Wrap = true,
                    },
                    new AdaptiveTextInput
                    {
                        Id = nameof(AskAnExpertCardPayload.Description),
                        Placeholder = Strings.AskAnExpertPlaceholderText,
                        IsMultiline = true,
                        Spacing = AdaptiveSpacing.Small,
                        Value = data.Description,
                    },
                },
                Actions = new List<AdaptiveAction>
                {
                    new AdaptiveSubmitAction
                    {
                        Title = Strings.AskAnExpertButtonText,
                        Data = new AskAnExpertCardPayload
                        {
                            MsTeams = new CardAction
                            {
                                Type = ActionTypes.MessageBack,
                                DisplayText = Strings.AskAnExpertDisplayText,
                                Text = AskAnExpertSubmitText,
                            },
                            UserQuestion = data.UserQuestion,
                            KnowledgeBaseAnswer = data.KnowledgeBaseAnswer,
                        },
                    },
                },
            };

            return new Attachment
            {
                Content = askAnExpertCard,
                ContentType = AdaptiveCard.ContentType,
            };
        }
    }
}
