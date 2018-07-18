using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ITSRunning.Models.Models
{
    public class Runner
    {
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        public int? Gender { get; set; }
        public string PhotoUri { get; set; }
        
    }
}
