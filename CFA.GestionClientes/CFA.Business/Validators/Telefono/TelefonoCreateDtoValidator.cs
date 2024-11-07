
using FluentValidation;
using CFA.Entities.DTOs.Telefono;

namespace CFA.Business.Validators.Telefono;

public class TelefonoCreateDtoValidator : AbstractValidator<TelefonoCreateDto>
{
    public TelefonoCreateDtoValidator()
    {
        RuleFor(x => x.CodigoCliente)
            .NotEmpty().WithMessage("El código del cliente es requerido")
            .GreaterThan(0).WithMessage("El código del cliente debe ser mayor a 0");

        RuleFor(x => x.NumeroTelefono)
            .NotEmpty().WithMessage("El número de teléfono es requerido")
            .MaximumLength(15).WithMessage("El número de teléfono no puede exceder 15 caracteres")
            .MinimumLength(7).WithMessage("El número de teléfono debe tener al menos 7 dígitos")
            .Matches(@"^\d+$").WithMessage("El número de teléfono solo debe contener dígitos");

        RuleFor(x => x.TipoTelefono)
            .NotEmpty().WithMessage("El tipo de teléfono es requerido")
            .MaximumLength(20).WithMessage("El tipo de teléfono no puede exceder 20 caracteres")
            .Must(tipo => new[] { "Casa", "Celular", "Trabajo", "Otro" }.Contains(tipo))
                .WithMessage("Tipo de teléfono inválido. Debe ser Casa, Celular, Trabajo u Otro");
    }
}
