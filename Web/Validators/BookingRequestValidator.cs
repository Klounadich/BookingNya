using System.Runtime.InteropServices.JavaScript;
using BookingModule.Commands;
using FluentValidation;

namespace BookingNya.Validators;

public class BookingRequestValidator: AbstractValidator<BookingRequestCommand>
{
    public BookingRequestValidator()
    {
        RuleFor(x=>x.room_id).NotNull().NotEmpty().WithMessage("Room ID is required");
        RuleFor(x=> x.guest_name).NotNull().NotEmpty().WithMessage("Guest Name is required");
        RuleFor(x=> x.guest_name).Length(3,50).Matches(@"^[a-zA-Zа-яА-ЯёЁ\s\-']+$").WithMessage("Guest Name must be between 3 and 50 characters");
        RuleFor(x => x.guest_email).NotNull().NotEmpty().EmailAddress().WithMessage("Email is required");
        RuleFor(x=> x.guest_phone).NotNull().NotEmpty().WithMessage("Phone Number is required");
        RuleFor(x => x.guest_phone).MaximumLength(12).Matches(@"^[\+\-\s\(\)0-9]{7,20}$").WithMessage("Phone Number must be 12 characters");
        RuleFor(x => x.check_in).NotNull().NotEmpty().WithMessage("Check In Date is required");
        RuleFor(x => x.check_in).GreaterThanOrEqualTo(DateTime.UtcNow.Date).WithMessage("Check-in date cannot be in the past");
        RuleFor(x => x.check_out).NotNull().NotEmpty().WithMessage("Check Out Date is required");
        RuleFor(x => x.check_out).GreaterThanOrEqualTo(x => x.check_in.AddDays(1)).WithMessage("Minimum stay is 1 night");
        RuleFor(x => x.total_price).NotEmpty().NotNull().GreaterThan(0).WithMessage("Total price must be greater than 0");
        RuleFor(x => x.currency).NotEmpty().NotNull().MaximumLength(3).WithMessage("Currency is required");
        RuleFor(x => x.payment_method).NotEmpty().NotNull().MaximumLength(15).WithMessage("Payment Method is required");
        RuleFor(x => x.payment_reservation_id).NotEmpty().NotNull().WithMessage("Payment Reservation id is required");
        

    }
}