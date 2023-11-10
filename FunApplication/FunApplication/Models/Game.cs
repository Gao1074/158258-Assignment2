using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Policy;
using System.Web;

namespace FunApplication.Models
{
    public class Game
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "The game name cannot be blank")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Please enter a game name between 3 and 50 characters in length")]
        [Display(Name = "Game Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The game description cannot be blank")]
        [StringLength(2000, MinimumLength = 3, ErrorMessage = "Please enter game description between 3 and 50 characters in length")]
        
        [Display(Name = "Game Description")]
        public string Description { get; set; }
        

        [Required(ErrorMessage = "The Game price cannot be blank")]
        [Range(0, 10000, ErrorMessage = "Please enter a Game price between 0.10 and 10000")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public double Price { get; set; }
        public int? PublisherID { get; set; }

        public virtual Publisher Publisher { get; set; }
        public int? CategoryID { get; set; }
        public virtual Category Category { get; set; }
        public string ImagePath { get; set; }
    }
}