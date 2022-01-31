using Backend.Core.Modell.Entities;
using FluentValidation;

namespace Backend.Validation
{
    public class DigimonValidation : AbstractValidator<Digimon>
    {
        public DigimonValidation()
        {
            RuleFor(x => x.Id).NotEmpty()
                              .NotNull()
                              .GreaterThan(0).WithMessage("Nem megfelelő az objektum azonosítójja!");

            RuleFor(x => x.Img).NotEmpty()
                               .NotNull()
                               .Must(CustomValidators.ValidateUri).WithMessage("Nem valós URL adott meg!");

            RuleFor(x => x.Level).NotEmpty()
                                 .NotNull();
        }
    }
}
