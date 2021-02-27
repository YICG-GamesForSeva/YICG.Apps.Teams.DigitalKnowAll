// <copyright file="ITicketsProvider.cs" company="Games For Seva">
// Copyright (c) Games For Seva. All rights reserved.
// </copyright>

namespace YICG.Apps.Teams.DigitalKnowBot.Common.Providers
{
    using System.Threading.Tasks;
    using YICG.Apps.Teams.DigitalKnowBot.Common.Models;

    /// <summary>
    /// This interface will be able to provide methods for storing ticket information in Azure table storage.
    /// </summary>
    public interface ITicketsProvider
    {
        /// <summary>
        /// Save or update ticket entity.
        /// </summary>
        /// <param name="ticket">Ticket received from bot based on which appropriate row will replaced or inserted in table storage.</param>
        /// <returns><see cref="Task"/> that resolves successfully if the data was saved successfully.</returns>
        Task SaveOrUpdateTicketAsync(TicketEntity ticket);

        /// <summary>
        /// Get already saved entity detail from storage table.
        /// </summary>
        /// <param name="ticketId">ticket id received from bot based on which appropriate row data will be fetched.</param>
        /// <returns><see cref="Task"/> Already saved entity detail.</returns>
        Task<TicketEntity> GetTicketAsync(string ticketId);
    }
}