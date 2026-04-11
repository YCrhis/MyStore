using MyStore.Entities;
using MyStore.Models;
using MyStore.Repositories;
using System.Linq.Expressions;

namespace MyStore.Services
{
    public class UserService
    {
        private readonly GenericRepository<User> _genericRepository;

        public UserService(GenericRepository<User> genericRepository)
        {
            _genericRepository = genericRepository;
        }

            
        public async Task<UserVM> Login(LoginVM loginVM)
        {
            var conditions = new List<Expression<Func<User, bool>>>()
            {
                x => x.Email == loginVM.Email,
                x => x.Password == loginVM.Password,
            };

            var found = await _genericRepository.GetByFilter(conditions.ToArray());

            var userVM = new UserVM();

            if(found != null)
            {
                userVM.Email = found.Email;
                userVM.FullName = found.FullName;
                userVM.UsertId = found.UserId;
                userVM.Type = found.Type;
            };
            return userVM;
        }


        public async Task Register(UserVM userVM)
        {
            if(userVM.Password != userVM.Password2)
            {
                throw new Exception("Passwords do not match");
            }

            var conditions = new List<Expression<Func<User, bool>>>()
            {
                x => x.Email == userVM.Email,
            };

            var foundEmail = await _genericRepository.GetByFilter(conditions.ToArray());

            if (foundEmail != null)
            {
                throw new Exception("The email already exist");
            }

            var user = new User()
            {
                Email = userVM.Email,
                FullName = userVM.FullName,
                Password = userVM.Password,
                Type = "Client"
            };

            await _genericRepository.AddAsync(user);
        }
    }
}
