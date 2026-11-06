using Backend.DTOs;

namespace backend.services;

public interface IBrandService : ICommonService<BrandDto, BrandInsertDto, BrandUpdateDto>
{
    Task<IEnumerable<BeerDto>?> GetBeers(int brandId);
}
