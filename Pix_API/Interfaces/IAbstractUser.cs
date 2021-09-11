using System;
using PixBlocks.Server.DataModels.DataModels;

namespace Pix_API.Interfaces
{
    public interface IAbstractUser:ICommandRepository
    {
        string password { get; }
        string login { get; }
        User GenerateUser(int Id);
    }
}
