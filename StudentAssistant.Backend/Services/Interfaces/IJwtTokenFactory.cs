using System.Threading.Tasks;

namespace StudentAssistant.Backend.Services.Interfaces
{
    public interface IJwtTokenFactory
    {
        Task<string> CreateJwtToken(string id);
    }
}
