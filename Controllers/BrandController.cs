using Backend.DTOs;
using backend.services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BrandController : ControllerBase
{
    private readonly IValidator<BrandInsertDto> _insertValidator;
    private readonly IValidator<BrandUpdateDto> _updateValidator;
    private readonly ICommonService<BrandDto, BrandInsertDto, BrandUpdateDto> _brandService;
    private readonly IBrandService _brandSpecificService;

    public BrandController(
        IValidator<BrandInsertDto> insertValidator,
        IValidator<BrandUpdateDto> updateValidator,
        [FromKeyedServices("brandService")]
        ICommonService<BrandDto, BrandInsertDto, BrandUpdateDto> brandService,
        [FromKeyedServices("brandService")] IBrandService brandSpecificService)
    {
        _insertValidator = insertValidator;
        _updateValidator = updateValidator;
        _brandService = brandService;
        _brandSpecificService = brandSpecificService;
    }

    [HttpGet]
    public async Task<IEnumerable<BrandDto>> Get() => await _brandService.Get();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BrandDto>> GetById(int id)
    {
        var brand = await _brandService.GetById(id);
        return brand == null ? NotFound() : Ok(brand);
    }

    [HttpGet("{id:int}/beers")]
    public async Task<ActionResult<IEnumerable<BeerDto>>> GetBeers(int id)
    {
        var beers = await _brandSpecificService.GetBeers(id);
        return beers == null ? NotFound() : Ok(beers);
    }

    [HttpPost]
    public async Task<ActionResult<BrandDto>> Add(BrandInsertDto brandInsertDto)
    {
        var validationResult = await _insertValidator.ValidateAsync(brandInsertDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var brand = await _brandService.Add(brandInsertDto);
        return CreatedAtAction(nameof(GetById), new { id = brand.Id }, brand);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<BrandDto>> Update(int id, BrandUpdateDto brandUpdateDto)
    {
        brandUpdateDto.Id = id;
        var validationResult = await _updateValidator.ValidateAsync(brandUpdateDto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        var brand = await _brandService.Update(id, brandUpdateDto);
        return brand == null ? NotFound() : Ok(brand);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<BrandDto>> Delete(int id)
    {
        try
        {
            var brand = await _brandService.Delete(id);
            return brand == null ? NotFound() : Ok(brand);
        }
        catch (BrandHasBeersException exception)
        {
            return Conflict(new { message = exception.Message });
        }
    }
}
