// <copyright file="TicketState.cs" company="Games For Seva">
// Copyright (c) Games For Seva. All rights reserved.
// </copyright>

namespace YICG.Apps.Teams.DigitalKnowBot.Common.Models
{
    /// <summary>
    /// Represents the current state/status of a ticket.
    /// </summary>
    public enum TicketState
    {
        /// <summary>
        /// Represents an active ticket
        /// </summary>
        Open = 0,

        /// <summary>
        /// Represents a ticket that requires no further action
        /// </summary>
        Closed = 1,

        /// <summary>
        /// Sentinel value
        /// </summary>
        MaxValue = Closed,
    }
}