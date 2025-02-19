

namespace Gameserver.Interfaces
{
    public interface IUObject
    {
        void SetProperty(string key, object value);
        object GetProperty(string key);
        void DeleteProperty(string name);
    }
}
