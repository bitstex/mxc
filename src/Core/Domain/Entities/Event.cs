using System.ComponentModel.DataAnnotations;
using Domain.Contract;

namespace Domain.Entities {
    public class Event : BaseEntity{
        [Required]
        public string Name {get;set;}
        [Required]
        [MaxLength(100)]
        public string Location {get;set;}

        public string Country {get;set;}

        public uint? Capacity{get;set;}
    }
}