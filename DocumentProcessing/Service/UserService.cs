using DocumentProcessing.DAL;
using DocumentProcessing.Infrastructure.Enums;
using DocumentProcessing.Models;
using DocumentProcessing.Models.User;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DocumentProcessing.Service
{
    public class UserService
    {
        private GenericRepository<User> userRepo;

        public UserService(DocumentProcessingEntities dataContext)
        {
            userRepo = new GenericRepository<User>(dataContext);
        }

        public User GetUser(int id)
            => userRepo.GetById(id);

        public User GetUserByEmail(string email)
            => userRepo.Get(a => a.AspNetUser.Email.Equals(email))
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
                        Firstname = model.Firstname
                    };

                    userRepo.Insert(ref user);
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