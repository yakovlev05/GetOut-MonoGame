using GetOut.Controllers;
using GetOut.Models.Statistic;

namespace GetOut.Models;

public interface IEntityInterface
{
    public Hero Hero { get; set; }
    public MapController MapController { get; set; }
    public ScoreStatistic ScoreStatistic { get; set; }
    public void Update();
    public void Draw();
    public void Init();
    public int Score { get; }
}