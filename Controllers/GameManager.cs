using GetOut.Models;
using GetOut.Program;

namespace GetOut.Controllers;

public class GameManager
{
    public Hero Hero { get; private set; }

    public void Init()
    {
        Hero = new(new(900, 500), 200f);
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