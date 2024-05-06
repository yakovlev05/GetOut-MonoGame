using GetOut.Models;
using GetOut.Program;

namespace GetOut.Controllers;

public class GameController
{
    public Hero Hero { get; private set; }

    public void Init()
    { // 900*500
        Hero = new(new(900, 500), 200f);
        // Hero = new(new(600, 400), 200f);
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