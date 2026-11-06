using Backend.DTOs;
using FluentValidation;
using repository;

namespace Backend.Validators;

public class BrandUpdateValidator : AbstractValidator<BrandUpdateDto>
{
    public BrandUpdateValidator(IBrandRepository brandRepository)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("El Id debe ser mayor a 0");
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("El nombre es obligatorio")
            .Length(2, 20)
            .WithMessage("El nombre debe medir de 2 a 20 caracteres")
            .MustAsync(async (dto, name, cancellationToken) =>
                !await brandRepository.ExistsByName(name, dto.Id, cancellationToken))
            .WithMessage("Ya existe una marca con ese nombre");
    }
}
