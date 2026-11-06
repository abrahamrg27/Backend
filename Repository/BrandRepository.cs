using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace repository;

public class BrandRepository : IBrandRepository
{
    private readonly StoreContext _context;

    public BrandRepository(StoreContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Brand>> Get() =>
        await _context.Brands.ToListAsync();

    public async Task<Brand?> GetById(int id) =>
        await _context.Brands.FindAsync(id);

    public async Task Add(Brand entity) =>
        await _context.Brands.AddAsync(entity);

    public void Update(Brand entity) =>
        _context.Brands.Attach(entity);

    public void Delete(Brand entity) =>
        _context.Brands.Remove(entity);

    public async Task Save() =>
        await _context.SaveChangesAsync();

    public async Task<bool> ExistsByName(
        string name,
        int? excludedId,
        CancellationToken cancellationToken = default)
    {
        var query = _context.Brands.Where(brand => brand.Name == name);

        if (excludedId.HasValue)
        {
            query = query.Where(brand => brand.BrandId != excludedId.Value);
        }

        return await query.AnyAsync(cancellationToken);
    }

    public async Task<IEnumerable<Beer>> GetBeers(int brandId) =>
        await _context.Beers
            .Where(beer => beer.BrandId == brandId)
            .ToListAsync();
}
