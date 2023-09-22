using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TestAPI0807.Models;

namespace TestAPI0807.Services
{
    public interface UserDataService
    {
        Task<List<UserDataDto>> GetUserDatas();

        Task<UserDataDto> GetUserData(long id);

        Task<int> PutUserData(long id, UserDataDto userDataDto);

        Task<UserData> PostUserData(UserDataDto userDataDto);

        Task<int> DeleteUserData(long id);

        Task<UserDataDto> UserToDto(UserData userData);
    }
    public class UserDataServiceImpl : UserDataService
    {
        private readonly UserDataContext _userDataContext;

        public UserDataServiceImpl(UserDataContext userDataContext)
        {
            _userDataContext = userDataContext;
        }

        public async Task<List<UserDataDto>> GetUserDatas()
        {
            var userdata = await _userDataContext.UserDatas.ToListAsync();
            return userdata
                .Select(x => new UserDataDto
                {
                    Id = x.Id,
                    Firstname = x.Firstname,
                    Lastname = x.Lastname,
                    Gender = x.Gender,
                    Age = x.Age,
                    RegistorDate = x.RegistorDate
                })
                .ToList();
        }

        public async Task<UserDataDto> GetUserData(long id)
        {
            var userdata = await _userDataContext.UserDatas.FindAsync(id);

            if (userdata == null)
            {
                return null;
            }

            return await UserToDto(userdata);
        } 

        public async Task<int> PutUserData(long id, UserDataDto userDataDto)
        {
            //if (id != userDataDto.Id)
            //{
            //    return 2;
            //}

            var userdata = await _userDataContext.UserDatas.FindAsync(id);

            if (userdata == null)
            {
                return 0;
            }

            userdata.Firstname = userDataDto.Firstname;
            userdata.Lastname = userDataDto.Lastname;
            userdata.Gender = userDataDto.Gender;
            userdata.Age = userDataDto.Age;
            userdata.RegistorDate = DateTime.Now;

            try
            {
                await _userDataContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!UserDataExists(id))
            {
                return 0;
            }
            return 3;
        }

        public async Task<UserData> PostUserData(UserDataDto userDto)
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
            await _userDataContext.SaveChangesAsync();
            return userData;
        }

        public async Task<int> DeleteUserData(long id)
        {
            if (_userDataContext.UserDatas == null)
            {
                return 0;
            }

            var userdata = await _userDataContext.UserDatas.FindAsync(id);

            if (userdata == null)
            {
                return 0;
            }

            _userDataContext.UserDatas.Remove(userdata);

            return await _userDataContext.SaveChangesAsync();
        }

        public bool UserDataExists(long id)
        {
            return _userDataContext.UserDatas.Any(e => e.Id == id);
        }

        public async Task<UserDataDto> UserToDto(UserData userData) =>
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
