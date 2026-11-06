using Backend.Models;

namespace repository;

public interface IBrandRepository : IRepository<Brand>
{
    Task<bool> ExistsByName(string name, int? excludedId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Beer>> GetBeers(int brandId);
}
