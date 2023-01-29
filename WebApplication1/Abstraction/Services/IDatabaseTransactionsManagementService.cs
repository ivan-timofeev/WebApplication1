using System.Data;

namespace WebApplication1.Abstraction.Services;

public interface IDatabaseTransactionsManagementService
{
    void BeginTransaction(IsolationLevel isolationLevel);
    void CommitTransaction();
    void RollbackTransaction();
}