using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.BusinessInterface
{

    public interface IUsers
    {
        Task<bool> Register(UsersModelObject users);
        Task<bool> Verify(UsersModelObject users);
        Task<bool> CheckUserExist(UsersModelObject users);
    }
}
