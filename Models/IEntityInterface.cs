namespace GetOut.Models;

public interface IEntityInterface
{
    public Hero Hero { get; set; }
    public bool StaticPosition { get; init; }
    public void Update();
    public void Draw();
}