using SD.Data.Models.Contracts;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SD.Data.Models.DomainModels
{
    public class ApiDataSource : IEntity, IAuditable, IDeletable
    {
        [Key]
        public Guid Id { get; set; }
        [StringLength(50, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string Tag { get; set; }
        [StringLength(300, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string Description { get; set; }
        public measureType MeasureType { get; set; }
        [Range(0, int.MaxValue)]
        public int ApiInterval { get; set; }
        [Range(0, int.MaxValue)]
        public int LastValueApi { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime TimeStamp { get; set; }
        [StringLength(18, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 17)]
        public bool GotOutOfRange { get; set; }
        public int RangeMin { get; set; }
        public int RangeMax { get; set; }


        [DataType(DataType.DateTime)]
        public DateTime? CreatedOn { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? ModifiedOn { get; set; }
        public bool IsDeleted { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? DeletedOn { get; set; }
    }
}
