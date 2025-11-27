// TaskManager.Application/DTOs/UpdateTaskDto.cs
using FluentValidation;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.DTOs;

public class UpdateTaskDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public Priority? Priority { get; set; }
    public Status? Status { get; set; }
}

public class UpdateTaskDtoValidator : AbstractValidator<UpdateTaskDto>
{
    public UpdateTaskDtoValidator()
    {
        RuleFor(x => x.Title).MaximumLength(200);
        RuleFor(x => x.Description).MaximumLength(1000);
        RuleFor(x => x.DueDate).Must(d => d is null || d.Value.Date >= DateTime.UtcNow.Date)
            .WithMessage("Due date cannot be in the past.");
    }
}
