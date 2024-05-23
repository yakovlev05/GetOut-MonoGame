using GetOut.Controllers;

namespace GetOut.Models;

public interface IEntityInterface
{
    public Hearts HeroHearts { get; set; }
    public Hero Hero { get; set; }
    public MapController MapController { get; set; }
    public bool StaticPosition { get; init; }
    public bool RequireMapController { get; init; }
    public void Update();
    public void Draw();
    public void Init();
}