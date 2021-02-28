// <copyright file="GangaGameBot.cs" company="Games For Seva">
// Copyright (c) Games For Seva. All rights reserved.
// </copyright>

namespace YICG.Apps.Teams.DigitalKnowBot.Bots
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Schema;
    using YICG.Apps.Teams.DigitalKnowBot.Cards;

    /// <summary>
    /// This class is our main bot class that will execute all of the functionality.
    /// </summary>
    public class GangaGameBot : ActivityHandler
    {
        private const string ConversationTypePersonal = "personal";

        private const string ConversationTypeChannel = "channel";

        /// <summary>
        /// This method executes whenever there is a new message coming into the bot.
        /// </summary>
        /// <param name="turnContext">The current turn/execution flow.</param>
        /// <param name="cancellationToken">The cancellation token which propogates to signal execution completion.</param>
        /// <returns>A unit of execution that represents an asynchronous operation.</returns>
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            if (turnContext is null)
            {
                throw new ArgumentNullException(nameof(turnContext));
            }

            var replyText = $"Echo: {turnContext.Activity.Text}";
            await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
        }

        /// <summary>
        /// This method will execute whenever there is a new member added to the conversation.
        /// </summary>
        /// <param name="membersAdded">The list of Channel accounts that are being added to the conversation.</param>
        /// <param name="turnContext">The current turn/execution flow.</param>
        /// <param name="cancellationToken">The cancellation token which propogates to signal execution completion.</param>
        /// <returns>A unit of execution that represents an asynchronous operation.</returns>
        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            if (membersAdded is null)
            {
                throw new ArgumentNullException(nameof(membersAdded));
            }

            if (turnContext is null)
            {
                throw new ArgumentNullException(nameof(turnContext));
            }

            // var welcomeText = "Hello and welcome!";
            // foreach (var member in membersAdded)
            // {
            //     if (member.Id != turnContext.Activity.Recipient.Id)
            //     {
            //         await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
            //     }
            // }
            var activity = turnContext.Activity;
            if (membersAdded.Any(m => m.Id == activity.Recipient.Id))
            {
                var welcomeText = "Hello and Welcome!";
                var userWelcomeCardAttachment = WelcomeCard.GetCard(welcomeText);
            }
        }
    }
}