﻿using GaEpd.EmailService;
using GaEpd.EmailService.EmailLogRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MyApp.AppServices.ErrorLogging;
using MyApp.Domain.Entities.WorkEntries;

namespace MyApp.AppServices.Notifications;

public class NotificationService(
    IEmailService emailService,
    IEmailLogRepository emailLogRepository,
    IHostEnvironment environment,
    IConfiguration configuration,
    IErrorLogger errorLogger) : INotificationService
{
    private const string FailurePrefix = "Notification email not sent:";

    public async Task<NotificationResult> SendNotificationAsync(Template template, string recipientEmail,
        WorkEntry workEntry, CancellationToken token = default)
    {
        var subjectPrefix = environment.EnvironmentName switch
        {
            "Development" => "[DEV] ",
            "Staging" => "[UAT] ",
            _ => "",
        };

        var subject = $"{subjectPrefix} {template.Subject}";
        var textBody = string.Format(template.TextBody + Template.TextSignature, workEntry.Id.ToString());
        var htmlBody = string.Format(template.HtmlBody + Template.HtmlSignature, workEntry.Id.ToString());

        var settings = new EmailServiceSettings();
        configuration.GetSection(nameof(EmailServiceSettings)).Bind(settings);

        if (string.IsNullOrEmpty(recipientEmail))
            return NotificationResult.FailureResult($"{FailurePrefix} A recipient could not be determined.");

        Message message;
        try
        {
            message = Message.Create(subject, recipientEmail, textBody, htmlBody);
        }
        catch (Exception e)
        {
            await errorLogger.LogErrorAsync(e, subject).ConfigureAwait(false);
            return NotificationResult.FailureResult($"{FailurePrefix} An error occurred when generating the email.");
        }

        await emailLogRepository.InsertAsync(message, token).ConfigureAwait(false);

        if (settings is { EnableEmail: false, EnableEmailAuditing: false })
        {
            return NotificationResult.FailureResult($"{FailurePrefix} Emailing is not enabled on the server.");
        }

        try
        {
            _ = emailService.SendEmailAsync(message,  token).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            await errorLogger.LogErrorAsync(e, subject).ConfigureAwait(false);
            return NotificationResult.FailureResult($"{FailurePrefix} An error occurred when sending the email.");
        }

        return NotificationResult.SuccessResult();
    }
}
