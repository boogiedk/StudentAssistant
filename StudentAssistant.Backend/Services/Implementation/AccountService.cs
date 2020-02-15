using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentAssistant.Backend.Interfaces;
using StudentAssistant.Backend.Models.Account.Requests;
using StudentAssistant.DbLayer;
using StudentAssistant.DbLayer.Models;

namespace StudentAssistant.Backend.Services.Implementation
{
    public class AccountService : IAccountService
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IJwtTokenFactory _jwtTokenFactory;
        private readonly ApplicationDbContext _context;

        public AccountService(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager,
            IJwtTokenFactory jwtTokenFactory, ApplicationDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtTokenFactory = jwtTokenFactory;
            _context = context;
        }    

        public async Task<ProfileViewModel> Get(string requestUserId)
        {
            var user = await _userManager.FindByIdAsync(requestUserId);

            if (user == null)
                throw new NullReferenceException();
            
            Enum.TryParse<IdentityRoles>((await _userManager.GetRolesAsync(user)).FirstOrDefault(), out var userRole);

            var result = new ProfileViewModel();

            switch (userRole)
            {
               case IdentityRoles.Student:
               result.ProfileInfo = await _context.Students
                   .Include(w=>w.StudyGroupModel)
                   .Include(w=>w.IdentityUser)
                   .FirstOrDefaultAsync(w => w.IdentityUser.Id == requestUserId);
                    break;
               
               case IdentityRoles.Teacher:
                   result.ProfileInfo = await _context.Teachers
                       .Include(w=>w.IdentityUser)
                       .FirstOrDefaultAsync(w => w.IdentityUser.Id == requestUserId);
                   break;

               case IdentityRoles.Administrator:
                   break;
               case IdentityRoles.User:
                   break;
               default:
                   throw new InvalidEnumArgumentException();
            }

            return result;
        }
    }
}