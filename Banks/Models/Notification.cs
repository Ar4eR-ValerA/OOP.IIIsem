using System;
using Banks.Tools;

namespace Banks.Models
{
    public class Notification
    {
        public Notification(Guid clientId, string message, DateTime sendDate)
        {
            ClientId = clientId;
            Message = message ?? throw new BanksException("Message is null");
            SendDate = sendDate;
            Id = Guid.NewGuid();
        }

        public Notification()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; private set; }
        public string Message { get; private set; }
        public DateTime SendDate { get; private set; }
        public Guid ClientId { get; private set; }
    }
}