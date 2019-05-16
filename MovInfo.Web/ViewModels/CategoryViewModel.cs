using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovInfo.Web.ViewModels
{
    public class CategoryViewModel
    {
        public IDictionary<long, string> AllCategoryIdsAndNames { get; set; }

        public ICollection<long> AllCategoryIds { get; set; }
    }
}
