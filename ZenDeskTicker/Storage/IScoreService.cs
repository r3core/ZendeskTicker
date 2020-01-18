using System.Threading.Tasks;

namespace ZenDeskTicker.Storage
{
    public interface IScoreService
    {
        Task<int?> GetHighScoreAsync(int currentDaysSinceSevOne);
    }
}