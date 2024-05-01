using GetOut.Models;
using GetOut.Program;

namespace GetOut.Controllers;

public class GameManager
{
    public Hero Hero { get; private set; }

    public void Init()
    {
        Hero = new(new(1920 / 2, 1080 / 2), 200f);
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