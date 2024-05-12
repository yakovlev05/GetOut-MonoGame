using System.Collections.Generic;
using GetOut.Models;

namespace GetOut.Controllers;

public class GameController
{
    public Hero
        Hero
    {
        get;
        private set;
    } // Героя вынесли отдельно, так как он будет везде, он независим от всего остального, исключительная личность

    public List<IEntityInterface> Entities { get; private set; } = new List<IEntityInterface>();

    public GameController()
    {
    }

    public GameController(List<IEntityInterface> entities)
    {
        Entities = entities;
    }

    public void Init()
    {
        Hero = new(new(900, 500), 200f); // Центр Экрана, присутсвует везде

        foreach (var entity in Entities)
        {
            entity.Hero = Hero;
        }
    }

    public void Update()
    {
        InputController.Update();
        Hero.Update();
        foreach (var entity in Entities)
        {
            entity.Update();
        }
    }

    public void Draw()
    {
        Hero.Draw();

        foreach (var entity in Entities)
        {
            if (!entity.StaticPosition) continue;
            entity.Draw();
        }
    }
    
    public void DrawDynamic()
    {
        foreach (var entity in Entities)
        {
            if (entity.StaticPosition) continue;
            entity.Draw();
        }
    }
}