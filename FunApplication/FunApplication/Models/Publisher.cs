using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FunApplication.Models
{
    public class Publisher
    {
        [Key]
        public int ID { get; set; }
        [Required(ErrorMessage = "The Publisher name cannot be blank")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Please enter a Publisher name between 3 and 50 characters in length")]
        [Display(Name = "Publisher")]

        public string Name { get; set; }
        [Required(ErrorMessage = "The Publisher Description cannot be blank")]
        [Display(Description = "Publisher Description")]
        public string Description { get; set; }
        public virtual ICollection<Game> Games { get; set; }
    }
}