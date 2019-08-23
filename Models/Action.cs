using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace wedding.Models
{
    public class Action
    {
        [Key]
        [Column("id")]
        public int ActionId {get;set;}
   
        [Column("wed_id")]
        public int WedId {get;set;}
        [Column("user_id")]
        public int UserId {get;set;}
        [Column("is_rsvp")]
        public bool RSVP {get;set;}



        // Navigation Properties
        public User yes {get;set;}
        public Wedding no {get;set;}
    }
}