using Mediaspot.Application.Common;
using Mediaspot.Domain.Titles;

namespace Mediaspot.Application.Titles;

public interface ITitleRepository : IRepository<Title>
{
    Task<Title?> GetByNameAsync(string name, CancellationToken ct = default);
}
