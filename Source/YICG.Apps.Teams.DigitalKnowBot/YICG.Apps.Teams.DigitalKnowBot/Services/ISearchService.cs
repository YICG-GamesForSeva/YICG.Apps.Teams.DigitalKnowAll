// <copyright file="ISearchService.cs" company="Games For Seva">
// Copyright (c) Games For Seva. All rights reserved.
// </copyright>

namespace YICG.Apps.Teams.DigitalKnowBot.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using YICG.Apps.Teams.DigitalKnowBot.Common.Models;

    /// <summary>
    /// This is the interface for the Azure Search Service.
    /// </summary>
    public interface ISearchService
    {
        /// <summary>
        /// This method provides the search results from the table to be used in the SME team based on the Azure Search Service.
        /// </summary>
        /// <param name="searchScope">Scope of the search.</param>
        /// <param name="searchQuery">The query that is provided by the messaging extension.</param>
        /// <param name="count">The number of search results to return.</param>
        /// <param name="skip">The number of search results to skip.</param>
        /// <returns>A list of the search results as defined by the query.</returns>
        Task<IList<TicketEntity>> SearchTicketsAsync(TicketSearchScope searchScope, string searchQuery, int? count = null, int? skip = null);
    }
}
