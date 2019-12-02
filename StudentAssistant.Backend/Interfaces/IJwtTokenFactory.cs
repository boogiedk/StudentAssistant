using System.Threading.Tasks;

namespace StudentAssistant.Backend.Interfaces
{
    public interface IJwtTokenFactory
    {
        Task<string> CreateJwtToken(string id);
    }
}
