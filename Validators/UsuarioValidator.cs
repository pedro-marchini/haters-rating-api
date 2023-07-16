using FluentValidation;
using HatersRating.Models;

namespace HatersRating.Validators
{
    public class UsuarioValidator : AbstractValidator<Usuario>
    {
        public UsuarioValidator()
        {
            RuleFor(r => r.Id).NotEmpty().WithMessage("Campo Id não pode estar vazio!");

            RuleFor(r => r.Email)
                .NotEmpty().WithMessage("Campo Email não pode estar vazio!")
                .EmailAddress().WithMessage("Formato de Email inválido.")
                .MaximumLength(200).WithMessage("Campo email deve ter no maximo 200 caracteres.");

            RuleFor(r => r.Nome)
                .NotEmpty().WithMessage("Campo Nome não pode estar vazio!")
                .MaximumLength(200).WithMessage("Campo Nome deve ter no maximo 200 caracteres");

            RuleFor(r => r.Senha)
                .NotEmpty().WithMessage("Campo Senha não pode estar vazio!")
                .MinimumLength(8).WithMessage("Campo Senha deve ter no minimo 8 caracteres")
                .MaximumLength(100).WithMessage("Campo Senha deve ter no maximo 100 caracteres")
                .Matches("[A-Z]").WithMessage("Campo Senha deve conter caracteres maiúsculos")
                .Matches("[a-z]").WithMessage("Campo Senha deve conter caracteres minúsculos")
                .Matches("[0-9]").WithMessage("Campo Senha deve conter numeros")
                .Matches("[^a-zA-Z0-9]").WithMessage("Campo Senha deve conter caracteres especiais");

            RuleFor(r => r.Ativo)
                .Must(o => o == false || o == true).WithMessage("Campo Ativo deve ser true ou false");

            RuleFor(r => r.CreatedDate)
                .NotEmpty().WithMessage("Campo CreatedDate não pode estar vazio!");

            RuleFor(r => r.UpdatedDate)
                .NotEmpty().WithMessage("Campo UpdatedDate não pode estar vazio!");
        }
    }
}