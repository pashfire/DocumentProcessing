using DocumentProcessing.Models.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DocumentProcessing.Models.Document
{
    public class UsersModel
    {
        public int Id { get; set; }
        public UserModel User { get; set; }
        public List<UserModel> AllUsers { get; set; }
        
        public static UsersModel Map(List<UserModel> allUsers)
        {
            if (allUsers.Count > 0)
            {
                return new UsersModel()
                {
                    AllUsers = allUsers,
                    Id = allUsers.First().Id,
                    User = allUsers.First()
                };
            }
            else return new UsersModel();

        }
    }
}