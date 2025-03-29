﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Lesson
{
    public class LessonRequest
    {
        public Guid Id { get; set; }
        public Guid? ClassId { get; set; }
        public DateTime Date { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
