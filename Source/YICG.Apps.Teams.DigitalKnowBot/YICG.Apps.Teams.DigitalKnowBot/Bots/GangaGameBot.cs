// <copyright file="GangaGameBot.cs" company="Games For Seva">
// Copyright (c) Games For Seva. All rights reserved.
// </copyright>

namespace YICG.Apps.Teams.DigitalKnowBot.Bots
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Schema;

    /// <summary>
    /// This class is our main bot class that will execute all of the functionality.
    /// </summary>
    public class GangaGameBot : ActivityHandler
    {
        /// <summary>
        /// This method always executes whenever a message is sent to the bot, or a message reaction is posted.
        /// </summary>
        /// <param name="turnContext">The current turn in the conversation.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A unit of execution.</returns>
        public override Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
        {
            // Conduct a null check on the turnContext - do we have some data in the conversation turn?
            if (turnContext is null)
            {
                throw new ArgumentNullException(nameof(turnContext));
            }

            switch (turnContext.Activity.Type)
            {
                case ActivityTypes.Message:
                    return this.OnMessageActivityAsync(new DelegatingTurnContext<IMessageActivity>(turnContext), cancellationToken);
                case ActivityTypes.ConversationUpdate:
                    return this.OnConversationUpdateActivityAsync(new DelegatingTurnContext<IConversationUpdateActivity>(turnContext), cancellationToken);
                default:
                    turnContext.SendActivityAsync(MessageFactory.Text("I have no clue what you are saying, please try again!"), cancellationToken);
                    break;
            }

            return base.OnTurnAsync(turnContext, cancellationToken);
        }

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

            await this.SendTypingIndicatorAsync(turnContext);

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

            var welcomeText = "Hello and welcome!";
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }
            }
        }

        private async Task SendTypingIndicatorAsync(ITurnContext turnContext)
        {
            try
            {
                var typingActivity = turnContext.Activity.CreateReply();
                typingActivity.Type = ActivityTypes.Typing;
                await turnContext.SendActivityAsync(typingActivity);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}