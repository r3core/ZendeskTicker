using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;

namespace ZenDeskTicker.Storage
{
    public class ScoreService : IScoreService
    {
        private const string TableName = "sevonescores";
        private const string PartitionKey = "scores";

        private readonly ITableHelper _tableHelper;
        private readonly IConfiguration _configuration;

        public ScoreService(ITableHelper tableHelper, IConfiguration configuration)
        {
            _tableHelper = tableHelper;
            _configuration = configuration;
        }

        public async Task<int?> GetHighScoreAsync(int currentDaysSinceSevOne)
        {
            try
            {
                var dataTable = await _tableHelper.CreateTableAsync(TableName);
                string rowKey = _configuration["storageRowKey"];
                var scoreEntity = await _tableHelper.RetrieveEntityUsingPointQueryAsync<ScoreEntity>(dataTable, PartitionKey, rowKey);

                if (scoreEntity == null)
                {
                    var insertedScoreEntity = await InsertScore(currentDaysSinceSevOne, dataTable, rowKey);
                    return insertedScoreEntity.HighScore;
                }
                else if (currentDaysSinceSevOne > scoreEntity.HighScore)
                {
                    var updatedScoreEntity = await UpdateScore(currentDaysSinceSevOne, dataTable, scoreEntity);
                    return updatedScoreEntity.HighScore;
                }
                else
                {
                    return scoreEntity.HighScore;
                }
            }
            catch
            {
                // TODO: Add Logging
                return null;
            }
        }

        private async Task<ScoreEntity> UpdateScore(int currentDaysSinceSevOne, CloudTable dataTable, ScoreEntity scoreEntity)
        {
            scoreEntity.HighScore = currentDaysSinceSevOne;
            var updatedScoreEntity = await _tableHelper.InsertOrMergeEntityAsync(dataTable, scoreEntity);
            return updatedScoreEntity;
        }

        private async Task<ScoreEntity> InsertScore(int currentDaysSinceSevOne, CloudTable dataTable, string rowKey)
        {
            return await _tableHelper.InsertOrMergeEntityAsync(dataTable, new ScoreEntity(PartitionKey, rowKey)
            {
                HighScore = currentDaysSinceSevOne
            });
        }
    }
}
