using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SD.Data.Models.Contracts;

namespace SD.Data.Models.Abstract
{
    public abstract class BaseEntity : IEntity, IAuditable, IDeletable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        
        [DataType(DataType.DateTime)]
        public DateTime? CreatedOn { get; set; }
        
        [DataType(DataType.DateTime)]
        public DateTime? ModifiedOn { get; set; }
        
        public bool IsDeleted { get; set; }
        
        [DataType(DataType.DateTime)]
        public DateTime? DeletedOn { get; set; }
    }
}
