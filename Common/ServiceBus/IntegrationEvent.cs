﻿using System;

namespace Common.ServiceBus
{
    public abstract class IntegrationEvent
    {
        public Guid Id { get; }
        public DateTime CreatedAt { get; }

        protected IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }
    }
}
