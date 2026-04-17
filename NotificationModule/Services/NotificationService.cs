using NotificationModule.Commands;
using NotificationModule.Models;
using NotificationModule.Repositories;
using NotificationModule.SMTP.Models;
using NotificationModule.SMTP.Services;
using Shared.Enums;
using Shared.Other;

namespace NotificationModule.Services;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;
    private readonly IEmailTemplateLoader _loadEmailTemplate;
    private readonly IMailService _mailService;

    public NotificationService(INotificationRepository notificationRepository ,IEmailTemplateLoader loadEmailTemplate , IMailService mailService)
    {
        _notificationRepository = notificationRepository;
        _loadEmailTemplate = loadEmailTemplate;
        _mailService = mailService;
    }
    public async Task<bool> SendConfirmationAsync(SendConfirmationCommand command)
    { 
        string rand_confirm_number = new Random().Next(1000, 9999).ToString();
    
        var replacements = new Dictionary<string, string>
        {
            { "Code", $"{rand_confirm_number}" }
        };
        string emailBody = _loadEmailTemplate.LoadEmailTemplates("confrimcode.html", replacements);
        var mailData = new MailData
        {
            To = new List<string> { command.email },
            From = "PodberuConfirm@yandex.ru",
            DisplayName = "HotelBookPro",
            ReplyTo = "PodberuConfirm@yandex.ru",
            ReplyToName = "Поддержка HotelBookPro",
            Subject = "Код подтверждения бронирования HotelBookPro",
            Body = emailBody
        };
    
        var model = new NotificationModel
        {
            saga_id = command.saga_id,
            booking_id = command.booking_id,
            type = "confirmation",
            channel = "email",
            recipient = command.email,
            content = rand_confirm_number,
            status = NotificationStatus.Pending,
        };

        if (!await _notificationRepository.AddAsync(model))
        {
            return false;
        }

        bool emailSent = false;
        try
        {
            emailSent = await _mailService.SendAsync(mailData, CancellationToken.None);
        }
        catch (Exception ex)
        {
            
        }

        model.status = emailSent ? NotificationStatus.Delivered : NotificationStatus.Failed;
        await _notificationRepository.UpdateAsync(model);
    
        return emailSent;
    }

    public async Task<(bool?,int?)> ConfirmCode(ConfirmCodeCommand data)
    {
        return await _notificationRepository.CheckCodeAsync(data);
    }
}