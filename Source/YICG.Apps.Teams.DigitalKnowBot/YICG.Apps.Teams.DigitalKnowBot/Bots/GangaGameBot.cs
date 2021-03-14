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
    using YICG.Apps.Teams.DigitalKnowBot.Common.Models;

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
        /// This method executes whenever the bot is installed in a personal scope, or the bot has been added to a team.
        /// </summary>
        /// <param name="turnContext">The current turn of the conversation.</param>
        /// <param name="cancellationToken">A signal of sorts to notify when the conversation turn is done.</param>
        /// <returns>A unit of execution known as a <see cref="Task"/>.</returns>
        protected override async Task OnConversationUpdateActivityAsync(ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            if (turnContext is null)
            {
                throw new ArgumentNullException(nameof(turnContext));
            }

            try
            {
                var activity = turnContext.Activity;

                if (activity.MembersAdded?.Count > 0)
                {
                    switch (activity.Conversation.ConversationType)
                    {
                        case Constants.ConversationTypePersonal:
                            await this.OnMembersAddedToPersonalChatAsync(activity.MembersAdded, turnContext, cancellationToken);
                            break;
                        case Constants.ConversationTypeChannel:
                            await this.OnMembersAddedToTeamAsync(activity.MembersAdded, turnContext, cancellationToken);
                            break;
                        default:
                            await turnContext.SendActivityAsync(MessageFactory.Text("I do not know what's going on...help!"), cancellationToken);
                            break;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
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

            try
            {
                var message = turnContext.Activity;
                await this.SendTypingIndicatorAsync(turnContext);

                switch (message.Conversation.ConversationType)
                {
                    case Constants.ConversationTypePersonal:
                        await this.OnMessageActivityInPersonalChatAsync(message, turnContext, cancellationToken);
                        break;
                    case Constants.ConversationTypeChannel:
                        await this.OnMessageActivityInChannelAsync(message, turnContext, cancellationToken);
                        break;
                    default:
                        await turnContext.SendActivityAsync(MessageFactory.Text($"I cannot understand the {message.Conversation.ConversationType} conversation!"), cancellationToken);
                        break;
                }
            }
            catch (Exception)
            {
                throw;
            }
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

        private async Task OnMessageActivityInPersonalChatAsync(IMessageActivity message, ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            if (message is null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (turnContext is null)
            {
                throw new ArgumentNullException(nameof(turnContext));
            }

            string messageText = (message.Text ?? string.Empty).Trim().ToLower();

            switch (messageText)
            {
                case Constants.TakeATourPersonalCommand:
                    await turnContext.SendActivityAsync(MessageFactory.Text("Hang on! I'm working on the tour, will give it soon!"), cancellationToken);
                    break;
                case Constants.AskAnExpertPersonalCommand:
                    await turnContext.SendActivityAsync(MessageFactory.Text("Pump the breaks! You need help already?! I'll get it to you!"), cancellationToken);
                    break;
                case Constants.ShareFeedbackPersonalCommand:
                    await turnContext.SendActivityAsync(MessageFactory.Text("You're telling on me?! Really?! How rude!! 👀"), cancellationToken);
                    break;
                default:
                    var replyText = $"Echo: {messageText}";
                    await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
                    break;
            }
        }

        private async Task OnMessageActivityInChannelAsync(IMessageActivity message, ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            if (message is null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (turnContext is null)
            {
                throw new ArgumentNullException(nameof(turnContext));
            }

            string text = (message.Text ?? string.Empty).Trim().ToLower();

            switch (text)
            {
                case Constants.TeamTourChannelCommand:
                    await turnContext.SendActivityAsync(MessageFactory.Text("Cool, you want to know how I help team! I'll show you in a minute!"), cancellationToken);
                    break;
                default:
                    await turnContext.SendActivityAsync(MessageFactory.Text("Ooook... My 🤖 🧠 cannot understand!!!"), cancellationToken);
                    break;
            }
        }
    }
}