using FluentValidation;
using MyApp.AppServices.WorkEntries.CommandDto;

namespace MyApp.AppServices.WorkEntries.Validators;

public class WorkEntryUpdateValidator : AbstractValidator<WorkEntryUpdateDto>
{
    public WorkEntryUpdateValidator()
    {
        RuleFor(dto => dto.Notes).NotEmpty();
    }
}
