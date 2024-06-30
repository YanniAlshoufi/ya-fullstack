using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Shared.Data.Models;

[PrimaryKey(nameof(Id))]
public class User
{
    [Required] public Guid Id { get; set; } = Guid.NewGuid();
    [Required] [MaxLength(50)] public required string FirstName { get; set; }
    [Required] [MaxLength(50)] public required string LastName { get; set; }
}