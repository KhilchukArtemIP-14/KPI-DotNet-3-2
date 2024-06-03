using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.BAL.DTO
{
    public class UserSummaryDTO
    {
        public string Id { get; set; }
        public string Name {  get; set; }
        public string Email { get; set; }
        public string[] Roles {  get; set; }
        public string Bio {  get; set; }
    }
}
