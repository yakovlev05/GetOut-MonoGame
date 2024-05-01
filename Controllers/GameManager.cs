using GetOut.Models;

namespace GetOut.Controllers;

public class GameManager
{
    private Hero _hero;

    public void Init()
    {
        _hero = new(new(1920/2, 1080/2), 200f);
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