using GetOut.Models;
using GetOut.Program;

namespace GetOut.Controllers;

public class GameManager
{
    public Hero Hero { get; private set; }

    public void Init()
    { // 900*500
        Hero = new(new(950, 600), 200f);
        // Hero = new(new(600, 400), 200f);
    }

    public void Update()
    {
        InputManager.Update();
        Hero.Update();
    }

    public void Draw()
    {
        Hero.Draw();
    }
}