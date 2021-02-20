// <copyright file="KnowledgeBaseSettings.cs" company="Games For Seva">
// Copyright (c) Games For Seva. All rights reserved.
// </copyright>

namespace YICG.Apps.Teams.DigitalKnowBot.Common.Models.Configuration
{
    /// <summary>
    /// This class represents the knowledge base settings.
    /// </summary>
    public class KnowledgeBaseSettings
    {
        /// <summary>
        /// Gets or sets the storage connection string.
        /// </summary>
        public string StorageConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the search service name.
        /// </summary>
        public string SearchServiceName { get; set; }

        /// <summary>
        /// Gets or sets the search service query api key.
        /// </summary>
        public string SearchServiceQueryApiKey { get; set; }

        /// <summary>
        /// Gets or sets the search service admin api key.
        /// </summary>
        public string SearchServiceAdminApiKey { get; set; }

        /// <summary>
        /// Gets or sets the search indexing interval in minutes.
        /// </summary>
        public string SearchIndexingIntervalInMinutes { get; set; }
    }
}