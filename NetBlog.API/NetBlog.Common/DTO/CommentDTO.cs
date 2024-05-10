﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.Common.DTO
{
    public class CommentDTO
    {
        public string AuthorName { get; set; }
        public string CommentText { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
