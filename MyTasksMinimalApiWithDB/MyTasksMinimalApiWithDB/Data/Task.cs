using System.ComponentModel.DataAnnotations.Schema;

namespace MyTasksMinimalApiWithDB.Data;

[Table("Tasks")]
public record Task(int Id, string Name, string IsComplete);
