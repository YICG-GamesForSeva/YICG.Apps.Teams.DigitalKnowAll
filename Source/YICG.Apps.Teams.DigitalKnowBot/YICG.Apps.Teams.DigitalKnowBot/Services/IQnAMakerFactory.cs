// <copyright file="IQnAMakerFactory.cs" company="Games For Seva">
// Copyright (c) Games For Seva. All rights reserved.
// </copyright>

namespace YICG.Apps.Teams.DigitalKnowBot.Services
{
    using Microsoft.Bot.Builder.AI.QnA;

    /// <summary>
    /// This interface will be used in querying the QnA Maker knowledge base.
    /// </summary>
    public interface IQnAMakerFactory
    {
        /// <summary>
        /// Gets the <see cref="QnAMaker"/> instance to use when querying the knowledge base.
        /// </summary>
        /// <param name="knowledgeBaseId">The knowledge base ID.</param>
        /// <param name="endpointKey">The endpoint key.</param>
        /// <returns>A QnAMaker instance.</returns>
        QnAMaker GetQnAMaker(string knowledgeBaseId, string endpointKey);
    }
}
