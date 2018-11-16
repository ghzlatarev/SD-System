using System;

namespace SD.Data.Models.Contracts
{
    public interface IEntity
    {
        Guid Id { get; set; }
    }
}
