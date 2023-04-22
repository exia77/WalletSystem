using BusinessObjects;
using DatabaseLayer;

namespace BusinessLayer
{
    public interface IUsers
    {
        Task<bool> Register(UsersModelObject users);
        Task<bool> Verify(UsersModelObject users);
        Task<bool> CheckUserExist(UsersModelObject users);
    }

    public class Users : IUsers
    {
        private readonly UsersDb userdb;
        public Users(string connectionString)
        {
            userdb = new UsersDb(connectionString);
        }

        public async Task<bool> Register(UsersModelObject users)
        {
            return await userdb.Register(users);
        }

        public async Task<bool> Verify(UsersModelObject users)
        {
            return await userdb.VerifyUser(users);
        }

        public async Task<bool> CheckUserExist(UsersModelObject users)
        {
            return await userdb.CheckUserExist(users);
        }
    }
}
