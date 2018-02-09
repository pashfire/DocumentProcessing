using DocumentProcessing.Models.Document;
using DocumentProcessing.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DocumentProcessing.Models.ViewModels.User
{
    public class UserViewModel
    {
        public bool IsUserExists { get; set; }
        public UserModel UserModel { get; set; }
        public UsersModel AllUsersModel { get; set; }
    }
}