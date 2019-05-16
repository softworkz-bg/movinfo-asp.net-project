using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovInfo.Web.Mappers
{
    public interface IViewModelMapper<TEntity, TViewModel>
    {
        TViewModel MapFrom(TEntity entity);
    }
}
