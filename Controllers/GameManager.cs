using GetOut.Models;

namespace GetOut.Controllers;

public class GameManager
{
    private Hero _hero;

    public void Init()
    {
        _hero = new(new(300, 300), 200f);
    }

    public void Update()
    {
        InputManager.Update();
        _hero.Update();
    }

    public void Draw()
    {
        _hero.Draw();
    }
}