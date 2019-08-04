using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentAssistant.Backend.Services
{
    public interface IJwtTokenFactory
    {
        Task<string> CreateJwtToken(string id);
    }
}
