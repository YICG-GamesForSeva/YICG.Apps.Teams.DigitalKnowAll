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
    using Microsoft.Bot.Builder.AI.QnA;
    using Microsoft.Bot.Connector.Authentication;
    using Microsoft.Bot.Schema;
    using Microsoft.Bot.Schema.Teams;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using YICG.Apps.Teams.DigitalKnowBot.Cards;
    using YICG.Apps.Teams.DigitalKnowBot.Common.Models;
    using YICG.Apps.Teams.DigitalKnowBot.Common.Providers;
    using YICG.Apps.Teams.DigitalKnowBot.Properties;
    using YICG.Apps.Teams.DigitalKnowBot.Services;

    /// <summary>
    /// This class is our main bot class that will execute all of the functionality.
    /// </summary>
    public class GangaGameBot : ActivityHandler
    {
        private readonly string appBaseUri;
        private readonly string qnaMakerEndpointKey;
        private readonly string qnaMakerKbId;
        private readonly string teamId;
        private readonly MicrosoftAppCredentials microsoftAppCredentials;
        private readonly IQnAMakerFactory qnaMakerFactory;
        private readonly ITicketsProvider ticketsProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="GangaGameBot"/> class.
        /// </summary>
        /// <param name="appBaseUri">Application Base URL.</param>
        /// <param name="qnaMakerEndpointKey">QnA Maker Endpoint Key.</param>
        /// <param name="qnaMakerKbId">QnA Maker KB (knowledge base) ID.</param>
        /// <param name="teamId">This is the Experts Team ID.</param>
        /// <param name="qnaMakerFactory">Our QnA Maker API DI.</param>
        /// <param name="microsoftAppCredentials">The Microsoft Application Credentials.</param>
        /// <param name="ticketsProvider">The logic to create tickets and modify them.</param>
        public GangaGameBot(string appBaseUri, string qnaMakerEndpointKey, string qnaMakerKbId, string teamId, IQnAMakerFactory qnaMakerFactory, MicrosoftAppCredentials microsoftAppCredentials, ITicketsProvider ticketsProvider)
        {
            this.appBaseUri = appBaseUri;
            this.qnaMakerEndpointKey = qnaMakerEndpointKey;
            this.qnaMakerKbId = qnaMakerKbId;
            this.qnaMakerFactory = qnaMakerFactory;
            this.microsoftAppCredentials = microsoftAppCredentials;
            this.ticketsProvider = ticketsProvider;
            this.teamId = teamId;
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

            if (!string.IsNullOrEmpty(message.ReplyToId) && (message.Value != null) && ((JObject)message.Value).HasValues)
            {
                await this.OnAdaptiveCardSubmitInPersonalChatAsync(message, turnContext, cancellationToken);
                return;
            }

            string messageText = (message.Text ?? string.Empty).Trim().ToLower();
            switch (messageText)
            {
                case Constants.TakeATourPersonalCommand:
                    var userTourCards = TourCarousel.GetUserTourCards(this.appBaseUri);
                    await turnContext.SendActivityAsync(MessageFactory.Carousel(userTourCards), cancellationToken);
                    break;
                case Constants.AskAnExpertPersonalCommand:
                    await turnContext.SendActivityAsync(MessageFactory.Attachment(AskAnExpertCard.GetCard()), cancellationToken);
                    break;
                case Constants.ShareFeedbackPersonalCommand:
                    await turnContext.SendActivityAsync(MessageFactory.Text("You're telling on me?! Really?! How rude!! 👀"), cancellationToken);
                    break;
                default:
                    // var replyText = $"Echo: {messageText}";
                    // await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
                    var queryResult = await this.GetAnswerFromQnAMakerAsync(messageText, turnContext, cancellationToken);
                    if (queryResult != null)
                    {
                        await turnContext.SendActivityAsync(MessageFactory.Attachment(ResponseCard.GetCard(queryResult.Questions[0], queryResult.Answer, messageText)), cancellationToken);
                    }
                    else
                    {
                        await turnContext.SendActivityAsync(MessageFactory.Text("Zoinks! I can't understand your question... 😭"), cancellationToken);
                    }

                    break;
            }
        }

        private async Task OnAdaptiveCardSubmitInPersonalChatAsync(IMessageActivity message, ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            // This card will notify the team of experts.
            Attachment smeTeamCard = null;

            // This card will notify the end user.
            Attachment userCard = null;

            // This is a new ticket to be inserted into our database.
            TicketEntity newTicket = null;

            switch (message.Text)
            {
                case Constants.AskAnExpertPersonalCommand:
                    break;
                case Constants.ShareFeedbackPersonalCommand:
                    break;
                case AskAnExpertCard.AskAnExpertSubmitText:
                    var askAnExpertPayload = ((JObject)message.Value).ToObject<AskAnExpertCardPayload>();

                    // Validating the fields of data.
                    if (string.IsNullOrEmpty(askAnExpertPayload.Title))
                    {
                        var updateCardActivity = new Activity(ActivityTypes.Message)
                        {
                            Id = turnContext.Activity.ReplyToId,
                            Conversation = turnContext.Activity.Conversation,
                            Attachments = new List<Attachment> { AskAnExpertCard.GetCard(askAnExpertPayload) },
                        };

                        await turnContext.UpdateActivityAsync(updateCardActivity, cancellationToken);
                        return;
                    }

                    var userDetails = await this.GetUserDetailsInPersonalChatAsync(turnContext, cancellationToken);
                    newTicket = await this.CreateTicketAsync(message, askAnExpertPayload, userDetails);
                    smeTeamCard = new SmeTicketCard(newTicket).ToAttachment(message.LocalTimestamp);
                    userCard = new UserNotificationCard(newTicket).ToAttachment(Strings.NotificationCardContent, message.LocalTimestamp);
                    break;
                case ShareFeedbackCard.ShareFeedbackSubmitText:
                    break;
                default:
                    await turnContext.SendActivityAsync(MessageFactory.Text("I am not sure what you are submitting here!"), cancellationToken);
                    break;
            }

            if (smeTeamCard != null)
            {
                // TODO: Populate the channelId through the appsettings.json - next thing to do.
                var channelId = this.teamId;
                var resourceResponse = await this.SendCardToTeamAsync(turnContext, smeTeamCard, channelId, cancellationToken);

                // If a new ticket was created, update the conversation information to the ticket.
                if (newTicket != null)
                {
                    newTicket.SmeCardActivityId = resourceResponse.ActivityId;
                    newTicket.SmeThreadConversationId = resourceResponse.Id;
                    await this.ticketsProvider.SaveOrUpdateTicketAsync(newTicket);
                }
            }

            // Notify the end user.
            if (userCard != null)
            {
                await turnContext.SendActivityAsync(MessageFactory.Attachment(userCard), cancellationToken);
            }
        }

        private async Task<TeamsChannelAccount> GetUserDetailsInPersonalChatAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var members = await ((BotFrameworkAdapter)turnContext.Adapter).GetConversationMembersAsync(turnContext, cancellationToken);
            return JsonConvert.DeserializeObject<TeamsChannelAccount>(JsonConvert.SerializeObject(members[0]));
        }

        // Creates a new ticket based on the input.
        private async Task<TicketEntity> CreateTicketAsync(IMessageActivity message, AskAnExpertCardPayload data, TeamsChannelAccount member)
        {
            TicketEntity ticket = new TicketEntity
            {
                TicketId = Guid.NewGuid().ToString(),
                Status = (int)TicketState.Open,
                DateCreated = DateTime.UtcNow,
                Title = data.Title,
                Description = data.Description,
                RequesterName = member.Name,
                RequesterUserPrincipalName = member.UserPrincipalName,
                RequesterGivenName = member.GivenName,
                RequesterConversationId = message.Conversation.Id,
                LastModifiedByName = message.From.Name,
                LastModifiedByObjectId = message.From.AadObjectId,
                UserQuestion = data.UserQuestion,
                KnowledgeBaseAnswer = data.KnowledgeBaseAnswer,
            };

            await this.ticketsProvider.SaveOrUpdateTicketAsync(ticket);

            return ticket;
        }

        private async Task<QueryResult> GetAnswerFromQnAMakerAsync(string messageText, ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(messageText))
            {
                return null;
            }

            try
            {
                var kbId = this.qnaMakerKbId;
                var endpointKey = this.qnaMakerEndpointKey;
                var qnaMaker = this.qnaMakerFactory.GetQnAMaker(kbId, endpointKey);
                var response = await qnaMaker.GetAnswersAsync(turnContext);

                return response?.FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
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
                    await turnContext.SendActivityAsync(MessageFactory.Carousel(teamTourCards), cancellationToken);
                    break;
                default:
                    var unrecognizedInputCard = UnrecognizedTeamInputCard.GetCard();
                    await turnContext.SendActivityAsync(MessageFactory.Attachment(unrecognizedInputCard), cancellationToken);
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

        private async Task<ConversationResourceResponse> SendCardToTeamAsync(ITurnContext turnContext, Attachment cardToSend, string teamId, CancellationToken cancellationToken)
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