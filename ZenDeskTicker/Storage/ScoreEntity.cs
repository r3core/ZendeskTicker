using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;

namespace ZenDeskTicker.Storage
{
    public class ScoreEntity : TableEntity
    {
        public int HighScore { get; set; }

        public ScoreEntity(string partitionKey, string rowKey) : base(partitionKey, rowKey)
        {
        }

        public ScoreEntity()
        {

        }
    }
}
