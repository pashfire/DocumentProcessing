using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DocumentProcessing.DAL;
using DocumentProcessing.Infrastructure.Enums;

namespace DocumentProcessing.Models.User
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        

        public int UserRole { get; set; }
        public string RoleName { get; set; }

        internal static UserModel Map(DAL.User user)
        {
            if (user != null)
            {
                return new UserModel()
                {
                    Id = user.Id,
                    Email = user.AspNetUser.Email,
                    Firstname = user.Firstname,
                    Lastname = user.Lastname,
                    RoleName = user.Role,
                    UserRole = (int)Enum.Parse(typeof(UserRoles),
                user.AspNetUser.AspNetRoles.First().Name)
                };
            }
            else return null;
        }
    }
}