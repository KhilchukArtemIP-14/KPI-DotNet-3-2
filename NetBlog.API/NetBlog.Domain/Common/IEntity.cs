using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBlog.Domain.Common
{
    public interface IEntity
    {
        Guid Id { get; set; }
        bool IsDeleted { get; set; }
    }
}
