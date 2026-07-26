using Mediaspot.Application.Common;
using Mediaspot.Application.Titles;
using Mediaspot.Domain.Titles;
using Mediaspot.Domain.Titles.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Mediaspot.Infrastructure.Persistence.Titles;

internal class TitleRepository(MediaspotDbContext db) : ITitleRepository
{
    public async Task AddAsync(Title entity, CancellationToken cancellationToken = default)
    {
        _ = await db.Titles.AddAsync(entity, cancellationToken);
    }

    public Task<Title?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return db.Titles.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<Title?> GetByNameAsync(string name, CancellationToken ct = default)
    {
        var vo = new TitleName(name);
        return db.Titles.AsNoTracking().FirstOrDefaultAsync(x => x.Name == vo, ct);
    }

    public Task RemoveAsync(Title entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Title>> ListAsync(PageSize size, Guid? lastSeen = null, CancellationToken cancellationToken = default)
    {

        var query = db.Titles
               .AsNoTracking()
               .OrderBy(x => x.Id)
               .Where(x=> lastSeen == null || x.Id > lastSeen);


        return await query
            .Take(size.Value)
            .ToListAsync(cancellationToken);

    }

    public Task UpdateAsync(Title entity, CancellationToken cancellationToken = default)
    {
        var entityEntry = db.Titles.Attach(entity);
        entityEntry.State = EntityState.Modified;
        return Task.CompletedTask;

    }
}
