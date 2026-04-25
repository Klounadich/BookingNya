using AuthModule.Commands;
using AuthModule.Repositories;
using FluentValidation;

namespace BookingNya.Validators;

public class RegisterRequestValidator: AbstractValidator<RegisterRequestCommand>
{
    private readonly IAuthRepository _authRepository;
    public RegisterRequestValidator(IAuthRepository authRepository)
    {
        _authRepository = authRepository;
        RuleFor(x=> x.dislay_name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.dislay_name).Length(3, 50).Matches(@"^[a-zA-Zа-яА-ЯёЁ\s\']+$")
            .WithMessage("Display name must be between 3 and 50 characters");
        RuleFor(x=>x.email).NotNull().NotEmpty().EmailAddress().WithMessage("Email is required");
        RuleFor(x => x.email).MustAsync(_authRepository.IsEmailUnique).WithMessage("Email already exists");
        RuleFor(x=> x.password).NotNull().NotEmpty().WithMessage("Password is required");
        RuleFor(x=> x.password).Length(6, 40).Matches(@".*[a-zA-Z].*").WithMessage("Password must be between 3 and 50 characters and contains 1 letter");
        
    }
}