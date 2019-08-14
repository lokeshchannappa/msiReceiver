//-----------------------------------------------------------------------
// <copyright file="Received.cs" company="WEIR">
//    © 2019 WEIR All Rights Reserved
// </copyright>
//-----------------------------------------------------------------------
namespace Msireceiver.UDP
{
    using System.Net;

    /// <summary>
    /// Received class holds sender and message properties
    /// </summary>
    public struct Received
    {
        /// <summary>
        /// Gets or sets sender
        /// </summary>
        public IPEndPoint Sender { get; set; }

        /// <summary>
        /// Gets or sets message
        /// </summary>
        public string Message { get; set; }
    }
}
