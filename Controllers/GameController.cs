using System.Collections.Generic;
using GetOut.Models;

namespace GetOut.Controllers;

public class GameController
{
    public Hero Hero { get; private set; } // Героя вынесли отдельно, так как он будет везде, он независим от всего остального, исключительная личность
    public List<IEntityInterface> Entities { get; private set; }
    
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
    }

    public void Update()
    {
        InputController.Update();
        Hero.Update();
    }

    public void Draw()
    {
        Hero.Draw();
    }
}