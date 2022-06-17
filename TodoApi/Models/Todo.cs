using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models;
public class Todo
{
    public Todo()
    {
        Id = Guid.NewGuid();
    }

    public Guid Id { get; set; }

    [Required]
    public string Title { get; set; }

    public bool Done { get; set; }

    public bool IsValid()
    {
        return Validator.TryValidateObject(this, new ValidationContext(this), null, true);
    }
}