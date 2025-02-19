using Gameserver.Angel;


namespace Gameserver.Interfaces
{
    public interface IRotation
    {
        Angle moveangle { get; set; }
        Angle speedangle { get; }
    }
}
