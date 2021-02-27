// <copyright file="TicketsProvider.cs" company="Games For Seva">
// Copyright (c) Games For Seva. All rights reserved.
// </copyright>

namespace YICG.Apps.Teams.DigitalKnowBot.Common.Providers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Table;
    using YICG.Apps.Teams.DigitalKnowBot.Common.Exceptions;
    using YICG.Apps.Teams.DigitalKnowBot.Common.Models;

    /// <summary>
    /// This class will implement the methods defined in <see cref="ITicketsProvider"/>.
    /// </summary>
    public class TicketsProvider : ITicketsProvider
    {
        private const string PartitionKey = "TicketInfo";

        private readonly Lazy<Task> initializeTask;
        private CloudTable ticketCloudTable;

        /// <summary>
        /// Initializes a new instance of the <see cref="TicketsProvider"/> class.
        /// </summary>
        /// <param name="connectionString">connection string of storage provided by DI.</param>
        public TicketsProvider(string connectionString)
        {
            this.initializeTask = new Lazy<Task>(() => this.InitializeAsync(connectionString));
        }

        /// <summary>
        /// This method will get a ticket from the table storage.
        /// </summary>
        /// <param name="ticketId">The ticket ID.</param>
        /// <returns>A unit of execution that contains a type of <see cref="TicketEntity"/>.</returns>
        public async Task<TicketEntity> GetTicketAsync(string ticketId)
        {
            await this.EnsureInitializedAsync().ConfigureAwait(false);

            var searchOperation = TableOperation.Retrieve<TicketEntity>(PartitionKey, ticketId);
            var searchResult = await this.ticketCloudTable.ExecuteAsync(searchOperation).ConfigureAwait(false);

            return (TicketEntity)searchResult.Result;
        }

        /// <summary>
        /// Store or update the ticket entity in table storage.
        /// </summary>
        /// <param name="ticket">The ticket entity.</param>
        /// <returns>A unit of execution.</returns>
        public Task SaveOrUpdateTicketAsync(TicketEntity ticket)
        {
            if (ticket is null)
            {
                throw new ArgumentNullException(nameof(ticket));
            }

            ticket.PartitionKey = PartitionKey;
            ticket.RowKey = ticket.TicketId;

            if (ticket.Status > (int)TicketState.MaxValue)
            {
                throw new TicketValidationException($"The ticket status ({ticket.Status}) is not valid.");
            }

            return this.StoreOrUpdateTicketEntityAsync(ticket);
        }

        /// <summary>
        /// Store or update ticket entity in table storage.
        /// </summary>
        /// <param name="entity">entity.</param>
        /// <returns><see cref="Task"/> that represents configuration entity is saved or updated.</returns>
        private async Task<TableResult> StoreOrUpdateTicketEntityAsync(TicketEntity entity)
        {
            await this.EnsureInitializedAsync().ConfigureAwait(false);
            TableOperation addOrUpdateOperation = TableOperation.InsertOrReplace(entity);
            return await this.ticketCloudTable.ExecuteAsync(addOrUpdateOperation).ConfigureAwait(false);
        }

        /// <summary>
        /// Create tickets table if it doesn't exists.
        /// </summary>
        /// <param name="connectionString">storage account connection string.</param>
        /// <returns><see cref="Task"/> representing the asynchronous operation task which represents table is created if its not existing.</returns>
        private async Task InitializeAsync(string connectionString)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudTableClient cloudTableClient = storageAccount.CreateCloudTableClient();
            this.ticketCloudTable = cloudTableClient.GetTableReference(StorageInfo.TicketTableName);

            await this.ticketCloudTable.CreateIfNotExistsAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Initialization of InitializeAsync method which will help in creating table.
        /// </summary>
        /// <returns>A unit of execution.</returns>
        private async Task EnsureInitializedAsync()
        {
            await this.initializeTask.Value;
        }
    }
}