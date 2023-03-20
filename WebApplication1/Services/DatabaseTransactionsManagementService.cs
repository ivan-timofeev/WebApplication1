using System.Data;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Services;

namespace WebApplication1.Services;

public class DatabaseTransactionsManagementService : IDatabaseTransactionsManagementService
{
    private readonly DbContext _dbContext;

    public DatabaseTransactionsManagementService(DbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public void BeginTransaction(IsolationLevel isolationLevel)
    {
        _dbContext.Database.BeginTransaction(isolationLevel);
    }

    public void CommitTransaction()
    {
        _dbContext.Database.CommitTransaction();
    }

    public void RollbackTransaction()
    {
        _dbContext.Database.RollbackTransaction();
    }
}
