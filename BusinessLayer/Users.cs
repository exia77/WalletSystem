using BusinessObjects;
using DatabaseLayer;
using DatabaseLayer.DatabaseContext;
using BusinessLayer.BusinessInterface;

namespace BusinessLayer
{
  
    public class Users : IUsers
    {
        private readonly UsersDb userdb;
        private readonly UsersEF? usersEf;
        private readonly DbLayerContext? _dbLayerContext;
        public Users(string connectionString, DbLayerContext dbLayerContext)
        {
            userdb = new UsersDb(connectionString);
            _dbLayerContext = dbLayerContext;
            usersEf = new(_dbLayerContext!);
        }

        public async Task<bool> Register(UsersModelObject users)
        {
            //return await userdb.Register(users);

            return await usersEf!.Register(users);
        }

        public async Task<bool> Verify(UsersModelObject users)
        {
            //return await userdb.VerifyUser(users);

            return await usersEf!.Verify(users);

        }

        public async Task<bool> CheckUserExist(UsersModelObject users)
        {
            //return await userdb.CheckUserExist(users);

            return await usersEf!.CheckUserExist(users);
        }
    }
}
