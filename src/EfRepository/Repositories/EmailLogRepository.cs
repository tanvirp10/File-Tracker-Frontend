using GaEpd.EmailService;
using GaEpd.EmailService.EmailLogRepository;
using Microsoft.Extensions.Configuration;
using MyApp.EfRepository.DbContext;

namespace MyApp.EfRepository.Repositories;

public sealed class EmailLogRepository(AppDbContext dbContext, IConfiguration configuration) : IEmailLogRepository
{
    public async Task InsertAsync(Message message, CancellationToken token = default)
    {
        var settings = new EmailServiceSettings();
        configuration.GetSection(nameof(EmailServiceSettings)).Bind(settings);
        if (!settings.EnableEmailLog) return;

        var emailLog = EmailLog.Create(message);
        await dbContext.EmailLogs.AddAsync(emailLog, token).ConfigureAwait(false);
        await dbContext.SaveChangesAsync(token).ConfigureAwait(false);
    }

    #region IDisposable,  IAsyncDisposable

    public void Dispose() => dbContext.Dispose();
    public ValueTask DisposeAsync() => dbContext.DisposeAsync();

    #endregion
}
