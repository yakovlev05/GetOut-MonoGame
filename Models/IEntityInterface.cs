namespace GetOut.Models;

public interface IEntityInterface
{
    public Hearts Hearts { get; set; }
    public Hero Hero { get; set; }
    public bool StaticPosition { get; init; }
    public void Update();
    public void Draw();
}