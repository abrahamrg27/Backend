using Backend.DTOs;
using FluentValidation;
using repository;

namespace Backend.Validators;

public class BrandInsertValidator : AbstractValidator<BrandInsertDto>
{
    public BrandInsertValidator(IBrandRepository brandRepository)
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("El nombre es obligatorio")
            .Length(2, 20)
            .WithMessage("El nombre debe medir de 2 a 20 caracteres")
            .MustAsync(async (name, cancellationToken) =>
                !await brandRepository.ExistsByName(name, null, cancellationToken))
            .WithMessage("Ya existe una marca con ese nombre");
    }
}
