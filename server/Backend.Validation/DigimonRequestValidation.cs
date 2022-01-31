using Backend.Core.Modell.Request;
using FluentValidation;

namespace Backend.Validation
{
    public class DigimonRequestValidation : AbstractValidator<DigimonRequest>
    {
        public DigimonRequestValidation()
        {
            RuleFor(x => x.Name).NotEmpty()
                                .NotNull();

            RuleFor(x => x.Img).NotEmpty()
                               .NotNull()
                               .Must(CustomValidators.ValidateUri).WithMessage("Nem valós URL adott meg!");

            RuleFor(x => x.Level).NotEmpty()
                                 .NotNull();
        }
    }
}
