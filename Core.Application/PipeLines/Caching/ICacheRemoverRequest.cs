using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.PipeLines.Caching;

public interface ICacheRemoverRequest
{
    string CacheKey { get; }
    bool ByPassCache { get; }
}
