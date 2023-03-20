using System.Data;

namespace WebApplication1.Services;

public interface IDatabaseTransactionsManagementService
{
    void BeginTransaction(IsolationLevel isolationLevel);
    void CommitTransaction();
    void RollbackTransaction();
}
