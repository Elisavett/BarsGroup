using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BarsGroup.Models
{
    public class Teacher
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(300)]
        [Display(Name = "ФИО")]
        public string FullName { get; set; }
        [Required]
        [StringLength(200)]
        [Display(Name = "E-mail")]
        [EmailAddress]
        public string Email { get; set; }
        [Display(Name = "Группы")]
        public List<Group> Groups { get; set; } = new List<Group>();
        [Display(Name = "Организации")]
        public List<Organization> Organizations { get; set; } = new List<Organization>();
    }
}
