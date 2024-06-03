﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.Application.DTOs
{
    public class PostSummaryDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string ContentPreview { get; set; }
        public DateTime DateCreated { get; set; }
        public UserShortcutDTO CreatedBy { get; set; }
    }
}
