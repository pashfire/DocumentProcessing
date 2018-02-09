using DocumentProcessing.DAL;
using DocumentProcessing.Infrastructure.Enums;
using DocumentProcessing.Models;
using DocumentProcessing.Models.Document;
using DocumentProcessing.Models.User;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DocumentProcessing.Service
{
    public class UserService
    {
        private RoleManager<IdentityRole> _roleManager;
        private GenericRepository<User> _userRepo;

        public UserService(DocumentProcessingEntities dataContext)
        {
            _userRepo = new GenericRepository<User>(dataContext);
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(dataContext));
        }

        public UsersModel GetAllUsers()
        {
            return UsersModel.Map(_userRepo.Get(u => u.Role != "Admin")
                        ?.Select(UserModel.Map).ToList());
        }

        public User GetUser(int id)
            => _userRepo.GetById(id);

        public User GetUserByEmail(string email)
            => _userRepo.Get(a => a.AspNetUser.Email.Equals(email))
                    ?.FirstOrDefault();

        public async Task<(IdentityResult, ApplicationUser)> RegisterUserAsync(RegisterModel model, ApplicationUserManager userManager)
        {
            var appUser = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await userManager.CreateAsync(appUser, model.Password);

            if (result.Succeeded)
            {
                appUser = userManager.FindByEmail(model.Email);
                var roleName = Enum.GetName(typeof(UserRoles), (UserRoles)model.UserRole);
                result = await userManager.AddToRoleAsync(appUser.Id, roleName);

                if (result.Succeeded)
                {
                    var user = new User()
                    {
                        AspnetUserId = appUser.Id,
                        Lastname = model.Lastname,
                        Firstname = model.Firstname,
                        Role = roleName
                    };

                    _userRepo.Insert(ref user);
                }
                else
                {
                    await userManager.DeleteAsync(appUser);
                }
            }

            return (result, appUser);
        }
    }
}