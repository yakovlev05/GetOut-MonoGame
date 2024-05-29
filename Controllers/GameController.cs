using System.Collections.Generic;
using GetOut.Models;
using Microsoft.Xna.Framework;

namespace GetOut.Controllers;

public class GameController
{
    public Hero Hero { get; private set; } // Героя вынесли отдельно, так как он будет везде, он независим от всего остального, исключительная личность
    private Hearts Hearts { get; set; }
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
        Hero = new(new(900, 500), 200f); // Центр Экрана, присутсвует везде
        Hearts = new Hearts(new Vector2(640+10, 360+10), 3); // Всегда есть, с эти классом взаимодействуют другие сущности
        Hero.Hearts = Hearts;

        foreach (var entity in Entities)
        {
            entity.MapController = mapController;
            entity.Hero = Hero;
            entity.Init();
        }
    }

    public void Update()
    {
        InputController.Update();
        Hero.Update();
        Hearts.Update();
        foreach (var entity in Entities)
        {
            entity.Update();
        }
    }

    public void Draw() // Отрисовка статических объектов
    {
        Hero.Draw();
        Hearts.Draw();
    }

    public void DrawDynamic()
    {
        foreach (var entity in Entities)
        {
            entity.Draw();
        }
    }
}