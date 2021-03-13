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
    using YICG.Apps.Teams.DigitalKnowBot.Bots.Cards;
    using YICG.Apps.Teams.DigitalKnowBot.Common.Models;

    /// <summary>
    /// This class is our main bot class that will execute all of the functionality.
    /// </summary>
    public class GangaGameBot : ActivityHandler
    {
        /// <summary>
        /// This method always executes whenever a message is being sent to the bot.
        /// </summary>
        /// <param name="turnContext">The current turn in an converstaion.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A unit of execution.</returns>
        public override Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
        {
            // Conduct a null check on the turnContext
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
                    turnContext.SendActivityAsync(MessageFactory.Text("I have no idea what you're saying, please try again!"), cancellationToken);
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
                        await this.OnMessageActivityInChannelChatAsync(message, turnContext, cancellationToken);
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

            // await this.SendTypingIndicatorAsync(turnContext);

            // var replyText = $"Echo: {turnContext.Activity.Text}";
            // await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
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

            var welcomeText = "Hi! I’m the Ganga River Rescue GameBot! If you want to know more about what I do, click on Take a tour below. Ask me a question about Ganga River Rescue, and I will do my best to help!";
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }

                var userWelcomeCardAttachment = WelcomeCard.GetCard(welcomeText);
                await turnContext.SendActivityAsync(MessageFactory.Attachment(userWelcomeCardAttachment), cancellationToken);
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
                case Constants.AskAnExpertFromCard:
                    await turnContext.SendActivityAsync(MessageFactory.Text("Pump the breaks! You need help already?! I'll give it to you!"), cancellationToken);
                    break;
                case Constants.ShareFeedbackPersonalCommand:
                    await turnContext.SendActivityAsync(MessageFactory.Text("You're on telling me! Really? How rude... 🙄"), cancellationToken);
                    break;
                default:
                    var replyText = $"Echo: {messageText}";
                    await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
                    break;
            }
        }

        private async Task OnMessageActivityInChannelChatAsync(IMessageActivity message, ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
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
                    await turnContext.SendActivityAsync(MessageFactory.Text("Cool, you want to see how I help team. I'll show you in a minute!"), cancellationToken);
                    break;
                default:
                    await turnContext.SendActivityAsync(MessageFactory.Text("Ooooook, my 🤖 🧠 can't understand! SORRY not sorry."), cancellationToken);
                    break;
            }
        }
    }
}