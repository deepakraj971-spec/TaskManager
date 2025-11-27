// TaskManager.Domain/Entities/AppUser.cs
using Microsoft.AspNetCore.Identity;

namespace TaskManager.Domain.Entities;

public class AppUser : IdentityUser<Guid> { }
