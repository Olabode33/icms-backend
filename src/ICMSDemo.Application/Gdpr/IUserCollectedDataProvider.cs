using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using ICMSDemo.Dto;

namespace ICMSDemo.Gdpr
{
    public interface IUserCollectedDataProvider
    {
        Task<List<FileDto>> GetFiles(UserIdentifier user);
    }
}
