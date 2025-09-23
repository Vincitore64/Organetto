using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organetto.Core.Shared.Services
{
    public interface IReadByIdAndDeleteRepository<TEntity, in TKey> : IReadByIdRepository<TEntity, TKey>, IDeleteRepository<TKey>
    {
    }
}
