using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GodsAmongSheep.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace GodsAmongSheep.Shared.Controllers
{
    public class GasWorkoutGroupsController
    {
        private readonly GasContext _gasContext;
        public GasWorkoutGroupsController(GasContext gasContext)
        {
            _gasContext = gasContext;
        }

        private IEnumerable<WorkoutGroup> GetWorkoutGroups => _gasContext.WorkoutGroups;

        private IEnumerable<WorkoutGroup_User> GetWorkoutGroups_Users => _gasContext.WorkoutGroups_Users;

        public WorkoutGroup FindWorkoutGroup(int id) => GetWorkoutGroups.FirstOrDefault(wg => wg.WorkoutGroupId == id);

        public WorkoutGroup_User FindWorkoutGroup_User(int workoutGroupId, int userId) => 
            GetWorkoutGroups_Users.FirstOrDefault(
                wgu => wgu.GasUserId == userId && 
                       wgu.WorkoutGroupId == workoutGroupId);

        // This method ignores workoutgroups that the user hasn't yet accepted...
        /// <summary>
        /// Pass FALSE to return UNACCEPTED workout groups, Pass TRUE to return ACCEPTED workout groups 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="isAccepted"></param>
        /// <returns></returns>
        public IEnumerable<WorkoutGroup> FindUsersWorkoutGroups(int userId, bool isAccepted)
        {
            using (var context = new GasContext())
            {
                context.SetupServer();
                var wgus = GetWorkoutGroups_Users.Where(wgu => wgu.GasUserId == userId).ToArray();
                var wgs = new List<WorkoutGroup>();
                for (var i = 0; i < wgus.Count(); ++i)
                {
                    if(isAccepted && wgus[i].IsAccepted)
                    {
                        // accepted workout groups
                        wgs.Add(FindWorkoutGroup(wgus[i].WorkoutGroupId));
                    }
                    else if (!isAccepted && (wgus[i].IsAccepted == false))
                    {
                        // unaccapted workout groups
                        wgs.Add(FindWorkoutGroup(wgus[i].WorkoutGroupId));
                    }
                    //var id = wgus[i].WorkoutGroupId;
                    //var result = FindWorkoutGroup(id);
                    //wgs.Add(result);
                }
                return wgs;
            }
        }

        /// <summary>
        /// Returns all the workoutgroup_users, need to provide a workoutgroup
        /// </summary>
        /// <param name="workoutGroup"></param>
        /// <returns></returns>
        public IEnumerable<WorkoutGroup_User> FindWorkoutGroupWorkoutGroup_Users(WorkoutGroup workoutGroup)
        {
            using (var context = new GasContext())
            {
                context.SetupServer();
                return GetWorkoutGroups_Users.Where(wgu => wgu.WorkoutGroupId == workoutGroup.WorkoutGroupId).ToArray();
            }
        }

        private void InsertWorkoutGroup(WorkoutGroup wg)
        {
            using (var context = new GasContext())
            {
                context.SetupServer();
                context.WorkoutGroups.Add(wg);
                context.SaveChanges();
            }
        }

        private async Task InsertWorkoutGroup_User(WorkoutGroup_User wgu)
        {
            using (var context = new GasContext())
            {
                try
                {
                    context.SetupServer();
                    context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
                    for (int i = 0; i < 5; ++i)
                    {
                        Debug.WriteLine($"{i} - Find the error!");
                    }
                    await Task.Run(() => context.WorkoutGroups_Users.Add(wgu));
                    //await Task.Run(() => _gasContext.WorkoutGroups_Users.Add(wgu));
                    for (int i = 0; i < 5; ++i)
                    {
                        Debug.WriteLine($"{i} - Find the error!");
                    }

                    //await context.SaveChangesAsync();
                    context.SaveChanges();
                    //_gasContext.SaveChanges();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.InnerException.Message);
                }
            }
        }

        private void DeleteWorkoutGroup(WorkoutGroup wg)
        {
            using (var context = new GasContext())
            {
                context.SetupServer();
                context.WorkoutGroups.Remove(wg);
                context.SaveChanges();
            }
        }

        private void DeleteWorkoutGroup_User(WorkoutGroup_User wgu)
        {
            using (var context = new GasContext())
            {
                context.SetupServer();
                context.WorkoutGroups_Users.Remove(wgu);
                context.SaveChanges();
            }
        }

        private void UpdateWorkoutGroup(WorkoutGroup updatedWorkoutGroup)
        {
            using (var context = new GasContext())
            {
                context.SetupServer();
                var oldWorkoutGroup = FindWorkoutGroup(updatedWorkoutGroup.WorkoutGroupId);
                DeleteWorkoutGroup(oldWorkoutGroup);
                InsertWorkoutGroup(updatedWorkoutGroup);
                context.SaveChanges();
            }
        }

        private async void UpdateWorkoutGroup_User(WorkoutGroup_User updatedWorkoutGroup_User)
        {
            using (var context = new GasContext())
            {
                context.SetupServer();
                var oldWorkoutGroupUser = FindWorkoutGroup_User(updatedWorkoutGroup_User.WorkoutGroupId,
                    updatedWorkoutGroup_User.GasUserId);
                DeleteWorkoutGroup_User(oldWorkoutGroupUser);
                await InsertWorkoutGroup_User(updatedWorkoutGroup_User);
                context.SaveChanges();
            }
        }

        public void CreateGasWorkoutGroup(WorkoutGroup wg)
        {
            try
            {
                if (wg == null) throw new Exception("!!! Cannot add null workout group !!!");
                InsertWorkoutGroup(wg);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public async Task CreateGasWorkoutGroup_User(WorkoutGroup_User wgu)
        {
            try
            {
                if (wgu == null) throw new Exception("!!! Cannot add null workout group user !!!");
                await InsertWorkoutGroup_User(wgu);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public void UpdateGasWorkoutGroup(WorkoutGroup wg)
        {
            try
            {
                if (wg == null) throw new Exception("!!! Cannot update null workout group !!!");
                var wgExists = FindWorkoutGroup(wg.WorkoutGroupId);
                if (wgExists == null) throw new Exception("!!! A workout group does not exist with that id !!!");
                UpdateWorkoutGroup(wg);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public void UpdateGasWorkoutGroup_User(WorkoutGroup_User wgu)
        {
            try
            {
                if (wgu == null) throw new Exception("!!! Cannot update null workout group !!!");
                var wguExists = FindWorkoutGroup_User(wgu.WorkoutGroupId, wgu.GasUserId);
                if (wguExists == null) throw new Exception("!!! A workout group user does not exist with that id !!!");
                UpdateWorkoutGroup_User(wgu);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public void DeleteGasWorkoutGroup(int wgId)
        {
            try
            {
                var wgExists = FindWorkoutGroup(wgId);
                if (wgExists == null) throw new Exception("!!! Cannot delete a workout group that does not exist!!!");
                DeleteWorkoutGroup(wgExists);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public void DeleteGasWorkoutGroup_User(WorkoutGroup_User wgu)
        {
            try
            {
                var wguExists = FindWorkoutGroup_User(wgu.WorkoutGroupId, wgu.GasUserId);
                if (wguExists == null) throw new Exception("!!! Cannot delete a workout group user that does not exist!!!");
                DeleteWorkoutGroup_User(wguExists);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
    }
}
