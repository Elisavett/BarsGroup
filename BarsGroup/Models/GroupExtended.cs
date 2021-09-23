using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BarsGroup.Models
{
    public class GroupExtended
    {
        public int Id { get; set; }
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Display(Name = "Преподаватель")]
        public string Teacher { get; set; }
        [Display(Name = "Количество студентов")]
        public int StudentsCount { get; set; }

    }
}
