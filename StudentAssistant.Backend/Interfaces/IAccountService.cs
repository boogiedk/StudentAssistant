using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using StudentAssistant.Backend.Models.Account.Requests;
using StudentAssistant.DbLayer;

namespace StudentAssistant.Backend.Interfaces
{
    public interface IAccountService
    {
       Task<ProfileViewModel> Get(string requestUserId);
    }
}