// Ruta: CFA.Business/Validators/Cliente/ClienteUpdateDtoValidator.cs
using FluentValidation;
using CFA.Entities.DTOs.Cliente;

namespace CFA.Business.Validators.Cliente;

public class ClienteUpdateDtoValidator : AbstractValidator<ClienteUpdateDto>
{
    public ClienteUpdateDtoValidator()
    {
        RuleFor(x => x.TipoDocumento)
            .NotEmpty().WithMessage("El tipo de documento es requerido")
            .Length(2).WithMessage("El tipo de documento debe tener 2 caracteres")
            .Must(tipo => new[] { "CC", "TI", "RC" }.Contains(tipo))
                .WithMessage("Tipo de documento inválido. Debe ser CC, TI o RC");

        RuleFor(x => x.NumeroDocumento)
            .NotEmpty().WithMessage("El número de documento es requerido")
            .MaximumLength(11).WithMessage("El número de documento no puede exceder 11 caracteres")
            .Matches(@"^\d+$").WithMessage("El número de documento solo debe contener dígitos");

        RuleFor(x => x.Nombres)
            .NotEmpty().WithMessage("Los nombres son requeridos")
            .MaximumLength(30).WithMessage("Los nombres no pueden exceder 30 caracteres")
            .Matches(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$")
                .WithMessage("Los nombres solo deben contener letras y espacios");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El email es requerido")
            .MaximumLength(100).WithMessage("El email no puede exceder 100 caracteres")
            .EmailAddress().WithMessage("El formato del email no es válido");
    }
}