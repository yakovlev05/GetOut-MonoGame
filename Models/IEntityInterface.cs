namespace GetOut.Models;

public interface IEntityInterface
{
    public bool StaticPosition { get; init; }
    public void Update();
    public void Draw();
}