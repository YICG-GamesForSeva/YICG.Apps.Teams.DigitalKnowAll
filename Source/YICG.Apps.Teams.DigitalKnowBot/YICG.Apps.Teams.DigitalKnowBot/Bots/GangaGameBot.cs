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
    using Microsoft.Bot.Connector.Authentication;
    using Microsoft.Bot.Schema;
    using Microsoft.Bot.Schema.Teams;
    using Newtonsoft.Json.Linq;
    using YICG.Apps.Teams.DigitalKnowBot.Cards;
    using YICG.Apps.Teams.DigitalKnowBot.Common.Models;
    using YICG.Apps.Teams.DigitalKnowBot.Properties;

    /// <summary>
    /// This class is our main bot class that will execute all of the functionality.
    /// </summary>
    public class GangaGameBot : ActivityHandler
    {
        private readonly string appBaseUri;
        private readonly MicrosoftAppCredentials microsoftAppCredentials;

        /// <summary>
        /// Initializes a new instance of the <see cref="GangaGameBot"/> class.
        /// </summary>
        /// <param name="appBaseUri">Application Base URL.</param>
        /// <param name="microsoftAppCredentials">The Microsoft Application Credentials.</param>
        public GangaGameBot(string appBaseUri, MicrosoftAppCredentials microsoftAppCredentials)
        {
            this.appBaseUri = appBaseUri;
            this.microsoftAppCredentials = microsoftAppCredentials;
        }

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
                    var userTourCards = TourCarousel.GetUserTourCards(this.appBaseUri);
                    await turnContext.SendActivityAsync(MessageFactory.Carousel(userTourCards), cancellationToken);
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
                    var teamTourCards = TourCarousel.GetTeamTourCards(this.appBaseUri);
                    await turnContext.SendActivityAsync(MessageFactory.Carousel(teamTourCards));
                    break;
                default:
                    await turnContext.SendActivityAsync(MessageFactory.Text("Ooook... My 🤖 🧠 cannot understand!!!"), cancellationToken);
                    break;
            }
        }

        private async Task OnMembersAddedToTeamAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var activity = turnContext.Activity;
            if (membersAdded.Any(m => m.Id == activity.Recipient.Id))
            {
                var teamDetails = ((JObject)turnContext.Activity.ChannelData).ToObject<TeamsChannelData>();
                var botDisplayName = turnContext.Activity.Recipient.Name;
                var teamWelcomeCardAttachment = WelcomeTeamCard.GetCard();
                await this.SendCardToTeamAsync(turnContext, teamWelcomeCardAttachment, teamDetails.Team.Id, cancellationToken);
            }
        }

        private async Task<ConversationResourceResponse> SendCardToTeamAsync(ITurnContext<IConversationUpdateActivity> turnContext, Attachment cardToSend, string teamId, CancellationToken cancellationToken)
        {
            var conversationParameters = new ConversationParameters
            {
                Activity = (Activity)MessageFactory.Attachment(cardToSend),
                ChannelData = new TeamsChannelData { Channel = new ChannelInfo(teamId) },
            };

            var tcs = new TaskCompletionSource<ConversationResourceResponse>();
            await ((BotFrameworkAdapter)turnContext.Adapter).CreateConversationAsync(
                null,  // If we set channel = "msteams", there is an error as preinstalled middleware expects ChannelData to be present
                turnContext.Activity.ServiceUrl,
                this.microsoftAppCredentials,
                conversationParameters,
                (newTurnContext, newCancellationToken) =>
                {
                    var activity = newTurnContext.Activity;
                    tcs.SetResult(new ConversationResourceResponse
                    {
                        Id = activity.Conversation.Id,
                        ActivityId = activity.Id,
                        ServiceUrl = activity.ServiceUrl,
                    });
                    return Task.CompletedTask;
                },
                cancellationToken);

            return await tcs.Task;
        }

        private async Task OnMembersAddedToPersonalChatAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var activity = turnContext.Activity;
            if (membersAdded.Any(m => m.Id == activity.Recipient.Id))
            {
                var userWelcomeCardAttachment = WelcomeCard.GetCard(Strings.UserWelcomeCardHeader, Strings.UserWelcomeCardContent);
                await turnContext.SendActivityAsync(MessageFactory.Attachment(userWelcomeCardAttachment), cancellationToken);
            }
        }
    }
}