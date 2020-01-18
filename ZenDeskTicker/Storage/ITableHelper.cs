using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;

namespace ZenDeskTicker.Storage
{
    public interface ITableHelper
    {
        Task<CloudTable> CreateTableAsync(string tableName);
        Task<T> InsertOrMergeEntityAsync<T>(CloudTable table, T entity) where T : TableEntity;
        Task<T> RetrieveEntityUsingPointQueryAsync<T>(CloudTable table, string partitionKey, string rowKey) where T : TableEntity;
    }
}