using System.Collections.Generic;
using GetOut.Models;
using GetOut.Models.Statistic;

namespace GetOut.Controllers;

public class GameController
{
    // Героя вынесли отдельно, так как он будет везде, он независим от всего остального, исключительная личность
    public Hero Hero { get; private set; }
    private Hearts Hearts { get; set; }
    public TimeStatistic TimeStatistic { get; private set; }
    public ScoreStatistic ScoreStatistic { get; private set; }
    private List<IEntityInterface> Entities { get; set; } = new List<IEntityInterface>();

    public GameController()
    {
    }

    public GameController(List<IEntityInterface> entities)
    {
        Entities = entities;
    }

    public void Init(MapController mapController)
    {
        Hero = new(new(900, 500), 200f, 3); // Центр Экрана, присутсвует везде
        Hearts = Hero.Hearts; // Всегда есть, с эти классом взаимодействуют другие сущности;
        TimeStatistic = new TimeStatistic();
        ScoreStatistic = new ScoreStatistic();

        foreach (var entity in Entities)
        {
            entity.MapController = mapController;
            entity.Hero = Hero;
            entity.ScoreStatistic = ScoreStatistic;
            entity.Init();
        }
    }

    public void Update()
    {
        InputController.Update();
        Hero.Update();
        Hearts.Update();
        TimeStatistic.Update();
        foreach (var entity in Entities)
        {
            entity.Update();
        }
    }

    public void Draw() // Отрисовка статических объектов
    {
        Hero.Draw();
        Hearts.Draw();
        TimeStatistic.Draw();
        ScoreStatistic.Draw();
    }

    public void DrawDynamic()
    {
        foreach (var entity in Entities)
        {
            entity.Draw();
        }
    }
}