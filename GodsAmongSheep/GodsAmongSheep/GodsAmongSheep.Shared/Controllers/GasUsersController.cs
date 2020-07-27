using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using GodsAmongSheep.Shared.Models;

namespace GodsAmongSheep.Shared.Controllers
{
    public class GasUsersController
    {
        private readonly GasContext _gasContext;
        public GasUsersController(GasContext gasContext)
        {
            _gasContext = gasContext;
        }

        public IEnumerable<GasUser> GetGasUsers => _gasContext.Users;

        public GasUser FindGasUser(int id) => GetGasUsers.FirstOrDefault(user => user.UserId == id);
        public GasUser FindGasUser(string username) => GetGasUsers.FirstOrDefault(user => user.Username == username);

        private void InsertUser(GasUser user)
        {
            _gasContext.Users.Add(user);
            _gasContext.SaveChanges();
        } 
        private void UpdateUser(GasUser updatedUser)
        {
            var oldUser = FindGasUser(updatedUser.UserId);
            _gasContext.Users.Remove(oldUser);
            _gasContext.Users.Add(updatedUser);
            _gasContext.SaveChanges();
        }

        private void DeleteUser(GasUser user)
        {
            _gasContext.Users.Remove(user);
            _gasContext.SaveChanges();
        }

        public void CreateGasUser(GasUser user)
        {
            try
            {
                if (user == null)
                {
                    throw new Exception("!!! Cannot add null user !!!");
                }

                var userExists = FindGasUser(user.Username);
                if (userExists != null)
                {
                    throw new Exception("!!! A user already exists with that username !!!");
                }
                InsertUser(user);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        // TODO: think about what the user is specifically updating, here since the entire user is being updated 
        // it causes a loss of workouts unless the workouts are provided when updating the user
        // for now this is alright but when making GAS you are going to want specific update function not a generic
        public void UpdateGasUser(GasUser user)
        {
            try
            {
                if (user == null)
                {
                    throw new Exception("!!! Cannot update null user !!!");
                }

                var userExists = FindGasUser(user.UserId);
                if (userExists == null)
                {
                    throw new Exception("!!! A user does not exist with that id !!!");
                }
                UpdateUser(user);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public void DeleteGasUser(int userId)
        {
            try
            {
                var user = FindGasUser(userId);
                if (user == null)
                {
                    throw new Exception("!!! Cannot delete a user that doesn't exist !!!");
                }
                DeleteUser(user);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
    }
}
