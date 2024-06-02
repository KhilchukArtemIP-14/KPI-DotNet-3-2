using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.Application.DTOs
{
    public class UserShortcutDTO
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
