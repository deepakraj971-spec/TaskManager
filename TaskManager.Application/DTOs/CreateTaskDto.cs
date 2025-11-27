// TaskManager.Application/DTOs/CreateTaskDto.cs
using FluentValidation;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.DTOs;

public class CreateTaskDto
{
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public DateTime? DueDate { get; set; }
    public Priority Priority { get; set; } = Priority.Medium;
    public Status Status { get; set; } = Status.Pending;
}

public class CreateTaskDtoValidator : AbstractValidator<CreateTaskDto>
{
    public CreateTaskDtoValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(1);        // e.g. 200
        RuleFor(x => x.Description).MaximumLength(1).When(x => x.Description != null);
        RuleFor(x => x.DueDate)
            .Must(d => d is null || d.Value.Date >= DateTime.UtcNow.Date)
            .WithMessage("Due date cannot be in the past.");
    }
}
