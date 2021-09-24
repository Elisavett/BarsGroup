using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BarsGroup.Models
{
    public class Employee
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(300)]
        [Display(Name = "ФИО")]
        public string FullName { get; set; }
        [Display(Name = "Группы")]
        public List<Group> Groups { get; set; } = new List<Group>();
        [Display(Name = "Организация")]
        public int OrganizationId { get; set; }
        [Display(Name = "Организация")]
        public Organization Organization { get; set; }
    }
}
