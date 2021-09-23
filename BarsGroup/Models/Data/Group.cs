﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BarsGroup.Models
{
    public class Group
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(200)]
        [Display(Name = "Название")]
        public string Name { get; set; }
        [Display(Name = "Студенты")]
        public List<Student> Students { get; set; } = new List<Student>();
        [Display(Name = "Преподаватель")]
        public int TeacherId { get; set; }
        [Display(Name = "Преподаватель")]
        public Teacher Teacher { get; set; }
        [Display(Name = "Курс")]
        public int CourseId { get; set; }
        [Display(Name = "Курс")]
        public Course Course { get; set; }
    }
}
