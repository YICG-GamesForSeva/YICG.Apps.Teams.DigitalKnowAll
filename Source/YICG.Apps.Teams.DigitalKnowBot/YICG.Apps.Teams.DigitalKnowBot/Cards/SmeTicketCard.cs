// <copyright file="SmeTicketCard.cs" company="Games For Seva">
// Copyright (c) Games For Seva. All rights reserved.
// </copyright>

namespace YICG.Apps.Teams.DigitalKnowBot.Cards
{
    using System;
    using System.Collections.Generic;
    using AdaptiveCards;
    using Microsoft.Bot.Schema;
    using YICG.Apps.Teams.DigitalKnowBot.Common.Models;
    using YICG.Apps.Teams.DigitalKnowBot.Properties;

    /// <summary>
    /// This class will construct the card that an expert will see in the team when a user requests more help.
    /// Also this is where the expert can change the status of a ticket.
    /// </summary>
    public class SmeTicketCard
    {
        private readonly TicketEntity ticket;

        /// <summary>
        /// Initializes a new instance of the <see cref="SmeTicketCard"/> class.
        /// </summary>
        /// <param name="ticket">The ticket with the latest details.</param>
        public SmeTicketCard(TicketEntity ticket)
        {
            this.ticket = ticket;
        }

        /// <summary>
        /// Gets the necessary information which is required for constructing this card.
        /// </summary>
        protected TicketEntity Ticket => this.ticket;

        /// <summary>
        /// This method will return an attachment based on the state and information of the ticket.
        /// </summary>
        /// <param name="localTimeStamp">Local timestamp of the user activity.</param>
        /// <returns>An attachment that will be sent in a message.</returns>
        public Attachment ToAttachment(DateTimeOffset? localTimeStamp)
        {
            var card = new AdaptiveCard("1.0")
            {
                Body = new List<AdaptiveElement>
                {
                    new AdaptiveTextBlock
                    {
                        Text = this.Ticket.Title,
                        Size = AdaptiveTextSize.Large,
                        Weight = AdaptiveTextWeight.Bolder,
                        Wrap = true,
                    },
                    new AdaptiveTextBlock
                    {
                        Text = string.Format(Strings.QuestionForExpertSubHeaderText, this.Ticket.RequesterName),
                        Wrap = true,
                    },
                    new AdaptiveFactSet
                    {
                        Facts = this.BuildFactSet(localTimeStamp),
                    },
                },
                Actions = this.BuildActions(),
            };

            return new Attachment
            {
                ContentType = AdaptiveCard.ContentType,
                Content = card,
            };
        }

        private List<AdaptiveFact> BuildFactSet(DateTimeOffset? localTimeStamp)
        {
            List<AdaptiveFact> factList = new List<AdaptiveFact>();

            if (!string.IsNullOrEmpty(this.Ticket.Description))
            {
                factList.Add(new AdaptiveFact
                {
                    Title = Strings.DescriptionFactTitle,
                    Value = this.Ticket.Description,
                });
            }

            if (!string.IsNullOrEmpty(this.Ticket.UserQuestion))
            {
                factList.Add(new AdaptiveFact
                {
                    Title = Strings.QuestionAskedFactTitle,
                    Value = this.Ticket.UserQuestion,
                });
            }

            factList.Add(new AdaptiveFact
            {
                Title = Strings.StatusFactTitle,
                Value = CardHelper.GetTicketDisplayStatusForSme(this.Ticket),
            });

            if (this.Ticket.Status == (int)TicketState.Closed)
            {
                factList.Add(new AdaptiveFact
                {
                    Title = Strings.ClosedFactTitle,
                    Value = CardHelper.GetFormattedDateInUserTimeZone(this.Ticket.DateClosed.Value, localTimeStamp),
                });
            }

            return factList;
        }
    }
}
