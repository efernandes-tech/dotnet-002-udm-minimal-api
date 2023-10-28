using System.Data;

namespace MyTasksMinimalApiWithDB.Data;

public class TaskContext
{
    public delegate Task<IDbConnection> GetConnection();
}
