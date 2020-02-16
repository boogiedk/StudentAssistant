using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentAssistant.Backend.Interfaces;
using StudentAssistant.Backend.Models.Account.Requests;
using StudentAssistant.Backend.Models.Account.Responses;
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
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountService(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager,
            IJwtTokenFactory jwtTokenFactory, ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _jwtTokenFactory = jwtTokenFactory;
            _context = context;
            _roleManager = roleManager;
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
                    result.IdentityRole = IdentityRoles.Student;
                    result.ProfileInfo = await _context.Students
                        .Include(w => w.StudyGroupModel)
                        .Include(w => w.IdentityUser)
                        .FirstOrDefaultAsync(w => w.IdentityUser.Id == requestUserId);
                    break;

                case IdentityRoles.Teacher:
                    result.IdentityRole = IdentityRoles.Teacher;
                    result.ProfileInfo = await _context.Teachers
                        .Include(w => w.IdentityUser)
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

        public async Task<AccountRegisterResponse> Register(AccountRegisterRequest model, CancellationToken cancellationToken)
        {
             var user = new IdentityUser(model.Login);
             var result = await _userManager.CreateAsync(user, model.Password);

             if (!result.Succeeded) return new AccountRegisterResponse() {IdentityResult = result};
            cancellationToken.ThrowIfCancellationRequested();

            await _signInManager.SignInAsync(user, true);

            if (model.ApplicationRoles == IdentityRoles.Administrator)
                return new AccountRegisterResponse { IdentityResult = IdentityResult.Failed(new IdentityError
                {
                    Code = "IdentityRoleError",
                    Description = "Can't registered with role Administrator"
                })};

            await _roleManager.CreateAsync(new IdentityRole(model.ApplicationRoles.Humanize()));
            await _userManager.AddToRoleAsync(user, model.ApplicationRoles.Humanize());

            switch (model.ApplicationRoles)
            {
                case IdentityRoles.Student:
                    await _context.Students.AddAsync(new StudentModel
                    {
                        Id = Guid.NewGuid(),
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        IdentityUser = user,
                        StudyGroupModel = await _context.StudyGroups.FirstOrDefaultAsync(w => w.Name == model.GroupName,
                                              cancellationToken: cancellationToken) ??
                                          throw new NullReferenceException()
                    }, cancellationToken);
                    _context.SaveChanges();
                    break;

                case IdentityRoles.Teacher:
                    await _context.Teachers.AddAsync(new TeacherModel()
                    {
                        Id = Guid.NewGuid(),
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        IdentityUser = user
                    }, cancellationToken);
                    _context.SaveChanges();
                    break;
                case IdentityRoles.Administrator:
                    break;
                case IdentityRoles.User:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var token = await _jwtTokenFactory.CreateJwtToken(user.Id);

            return new AccountRegisterResponse
            {
                IdentityResult = IdentityResult.Success,
                Token = token
            };
        }
    }
}