using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;
using System.Linq;
using TestAPI0807.Models;

namespace TestAPI0807.Services
{
    public interface UserDataService
    {
        IQueryable<UserDataDto> GetUserDatas();

        Task<ActionResult<UserDataDto>> GetUserData(long id);

        Task<IActionResult> PutUserData(long id, UserDataDto userDataDto);

        UserData PostUserData(UserDataDto userDataDto);

        Task<int> DeleteUserData(long id);

        bool UserDataExists(long id);

        UserDataDto UserToDto(UserData userData);
    }
    public class UserDataServiceImpl : UserDataService
    {
        private readonly UserDataContext _userDataContext;

        public UserDataServiceImpl(UserDataContext userDataContext)
        {
            _userDataContext = userDataContext;
        }

        public IQueryable<UserDataDto> GetUserDatas()
        {
            return _userDataContext.UserDatas
            //.Select(x => UserToDto(x));
                .Select(x => new UserDataDto
                {
                    Id = x.Id,
                    Firstname = x.Firstname,
                    Lastname = x.Lastname,
                    Gender = x.Gender,
                    Age = x.Age,
                    RegistorDate = x.RegistorDate
                });
        }

        public async Task<ActionResult<UserDataDto>> GetUserData(long id)
        {
            var userdata = await _userDataContext.UserDatas.FindAsync(id);

            if (userdata == null)
            {
                return new NotFoundResult();
            }

            return UserToDto(userdata);
        }

        public async Task<IActionResult> PutUserData(long id, UserDataDto userDataDto)
        {
            var userdata = await _userDataContext.UserDatas.FindAsync(id);
            if (userdata == null)
            {
                return new NotFoundResult();
            }

            userdata.Firstname = userDataDto.Firstname;
            userdata.Lastname = userDataDto.Lastname;
            userdata.Gender = userDataDto.Gender;
            userdata.Age = userDataDto.Age;
            userdata.RegistorDate = userDataDto.RegistorDate;

            try
            {
                await _userDataContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!UserDataExists(id))
            {
                return new NotFoundResult();
            }

            return new NoContentResult();
        }

        public UserData PostUserData(UserDataDto userDto)
        {
            DateTime dateTime = DateTime.Now;
            UserData userData = new UserData
            {
                Firstname = userDto.Firstname,
                Lastname = userDto.Lastname,
                Gender = userDto.Gender,
                Age = userDto.Age,
                RegistorDate = dateTime
            };

            _userDataContext.UserDatas.Add(userData);
            _userDataContext.SaveChanges();
            return userData;
        }

        public async Task<int> DeleteUserData(long id)
        {
            var userdata = await _userDataContext.UserDatas.FindAsync(id);
            _userDataContext.UserDatas.Remove(userdata);

            return await _userDataContext.SaveChangesAsync();
        }

        public bool UserDataExists(long id)
        {
            return _userDataContext.UserDatas.Any(e => e.Id == id);
        }

        public UserDataDto UserToDto(UserData userData) =>
            new()
            {
                Id = userData.Id,
                Firstname = userData.Firstname,
                Lastname = userData.Lastname,
                Gender = userData.Gender,
                Age = userData.Age,
                RegistorDate = userData.RegistorDate
            };
    }
}
