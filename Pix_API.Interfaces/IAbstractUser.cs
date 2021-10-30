using Pix_API.Base.Utills;
using PixBlocks.Server.DataModels.DataModels;

namespace Pix_API.PixBlocks.Interfaces
{
    public interface IAbstractUser:ICommandRepository
    {
        string password { get; }
        string login { get; }
        User GenerateUser(int Id);
    }
}
