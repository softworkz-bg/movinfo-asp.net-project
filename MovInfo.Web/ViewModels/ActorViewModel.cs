using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovInfo.Web.ViewModels
{
    public class ActorViewModel
    {
        public IDictionary<long, string> AllActorIdsAndNames { get; set; }   
        
        public ICollection<long> ActorIds { get; set; }
    }
}
