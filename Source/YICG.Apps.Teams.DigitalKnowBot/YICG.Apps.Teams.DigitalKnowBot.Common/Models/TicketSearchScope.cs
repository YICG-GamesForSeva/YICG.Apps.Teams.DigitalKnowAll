// <copyright file="TicketSearchScope.cs" company="Games For Seva">
// Copyright (c) Games For Seva. All rights reserved.
// </copyright>

namespace YICG.Apps.Teams.DigitalKnowBot.Common.Models
{
    /// <summary>
    /// This enumeration represents the ticket search scope.
    /// </summary>
    public enum TicketSearchScope
    {
        /// <summary>
        /// Recent tickets
        /// </summary>
        RecentTickets,

        /// <summary>
        /// Open tickets
        /// </summary>
        OpenTickets,

        /// <summary>
        /// Tickets assigned to a subject-matter expert
        /// </summary>
        AssignedTickets,
    }
}