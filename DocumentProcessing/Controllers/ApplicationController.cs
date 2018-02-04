using DocumentProcessing.DAL;
using DocumentProcessing.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocumentProcessing.Controllers
{
    public abstract class ApplicationController : Controller
    {
        private DocumentProcessingEntities dataContext;
        private User user;


        protected DocumentProcessingEntities DataContext
        {
            get
            {
                return dataContext ?? (dataContext = new DocumentProcessingEntities());
            }
        }

        protected User CurrentUser
        {
            get
            {
                if (User.Identity.IsAuthenticated)
                {
                    return user == null
                        ? new UserService(DataContext).GetUserByEmail(User.Identity.Name)
                        : null;
                }
                return null;
            }
        }

        public ApplicationController() { }

    }
}