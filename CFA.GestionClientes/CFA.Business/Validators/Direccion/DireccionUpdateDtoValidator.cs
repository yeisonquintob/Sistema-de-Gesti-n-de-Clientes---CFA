
using FluentValidation;
using CFA.Entities.DTOs.Direccion;

namespace CFA.Business.Validators.Direccion;

public class DireccionUpdateDtoValidator : AbstractValidator<DireccionUpdateDto>
{
    public DireccionUpdateDtoValidator()
    {
        RuleFor(x => x.Direccion)
            .NotEmpty().WithMessage("La dirección es requerida")
            .MaximumLength(100).WithMessage("La dirección no puede exceder 100 caracteres")
            .Matches(@"^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑ\s\-\#\,\.]+$")
                .WithMessage("La dirección contiene caracteres no válidos");

        RuleFor(x => x.TipoDireccion)
            .NotEmpty().WithMessage("El tipo de dirección es requerido")
            .MaximumLength(20).WithMessage("El tipo de dirección no puede exceder 20 caracteres")
            .Must(tipo => new[] { "Casa", "Trabajo", "Otro" }.Contains(tipo))
                .WithMessage("Tipo de dirección inválido. Debe ser Casa, Trabajo u Otro");
    }
}