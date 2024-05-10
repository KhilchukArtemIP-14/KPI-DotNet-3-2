﻿using NetBlog.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.DAL.Specifications.Implementations
{
    public class PostWithCommentsSpecification:BaseSpecification<Post>
    {
        public PostWithCommentsSpecification() {
            AddInclude(p => p.Comments);
        }
    }
}
