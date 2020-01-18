using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;

namespace ZenDeskTicker.Storage
{
    public class TableHelper : ITableHelper
    {
        private readonly IConfiguration _configuration;

        public TableHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private CloudStorageAccount CreateStorageAccount()
        {
            CloudStorageAccount storageAccount;
            try
            {
                storageAccount = CloudStorageAccount.Parse(_configuration["storageConnectionString"]);
            }
            catch
            {
                // TODO: Add Logging.
                throw;
            }

            return storageAccount;
        }

        public async Task<CloudTable> CreateTableAsync(string tableName)
        {
            var storageAccount = CreateStorageAccount();
            var tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());
            var table = tableClient.GetTableReference(tableName);
            await table.CreateIfNotExistsAsync();
            return table;
        }

        public async Task<T> RetrieveEntityUsingPointQueryAsync<T>(CloudTable table, string partitionKey, string rowKey) where T : TableEntity
        {
            try
            {
                var retrieveOperation = TableOperation.Retrieve<T>(partitionKey, rowKey);
                var result = await table.ExecuteAsync(retrieveOperation);
                T entity = result.Result as T;

                return entity;
            }
            catch
            {
                // TODO: Add Logging.
                throw;
            }
        }

        public async Task<T> InsertOrMergeEntityAsync<T>(CloudTable table, T entity) where T : TableEntity
        {
            try
            {
                var insertOrMergeOperation = TableOperation.InsertOrMerge(entity);
                var result = await table.ExecuteAsync(insertOrMergeOperation);
                T insertedEntity = result.Result as T;

                return insertedEntity;
            }
            catch
            {
                // TODO: Add Logging.
                throw;
            }
        }
    }
}
