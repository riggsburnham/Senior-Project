using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using GodsAmongSheep.Shared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace GodsAmongSheep.Shared.Controllers
{
    public class GasWorkoutsController
    {
        private readonly GasContext _gasContext;

        public GasWorkoutsController(GasContext gasContext)
        {
            _gasContext = gasContext;
        }

        public IEnumerable<Workout> GetAllWorkouts => _gasContext.Workouts;

        public IEnumerable<WorkoutType> GetAllWorkoutsTypes => _gasContext.WorkoutTypes;

        public IEnumerable<Workout> GetUsersWorkouts(int id) => _gasContext.Workouts.Where(w => w.GasUserId == id);
        //public IEnumerable<Workout> GetUsersWorkouts(int id)
        //{
        //    var workouts = _gasContext.Workouts.Where(w => w.GasUserId == id);
        //    foreach (var workout in workouts)
        //    {
        //        var types = _gasContext.WorkoutTypes.Where(wt => wt.WorkoutId == workout.WorkoutId);
        //        workout.Workouts.AddRange(types);
        //    }
        //    return workouts;
        //}

        // TODO: try making the GetUsersWorkouts function to return the workout types along with it, so your doing only 1 call,
        // TODO: this may fix your issue of opening up a second reader...

        public List<WorkoutType> GetWorkoutsWorkoutTypes(int workoutId)
        {
            var workoutTypes = new List<WorkoutType>();
            //var types1 = _gasContext.WorkoutTypes.Include(w => w.WorkoutId == workoutId);
            var types = _gasContext.WorkoutTypes.Where(wt => wt.WorkoutId == workoutId);
            workoutTypes = types.ToList();

            //foreach (var type in types)
            //{
            //    if (type.WorkoutId == workoutId)
            //    {
            //        workoutTypes.Add(type);
            //    }
            //}
            return workoutTypes;
        }

        //public IEnumerable<WorkoutType> GetWorkoutsWorkoutTypes(int id) => _gasContext.WorkoutTypes.Where(wt => wt.WorkoutId == id);

        //public List<WorkoutType> GetWorkoutsWorkoutTypes(int id) =>
            //new List<WorkoutType>(_gasContext.WorkoutTypes.Where(w => w.WorkoutId == id));

        public Workout FindWorkout(int id) => _gasContext.Workouts.FirstOrDefault(w => w.WorkoutId == id);

        public WorkoutType FindWorkoutType(int id) => _gasContext.WorkoutTypes.FirstOrDefault(w => w.WorkoutTypeId == id);

        private void InsertWorkout(Workout workout)
        {
            _gasContext.Workouts.Add(workout);
            _gasContext.SaveChanges();
        }

        private void InsertWorkoutType(WorkoutType workoutType)
        {
            _gasContext.WorkoutTypes.Add(workoutType);
            _gasContext.SaveChanges();
        }

        private void UpdateWorkout(Workout updatedWorkout)
        {
            var oldWorkout = FindWorkout(updatedWorkout.WorkoutId);
            _gasContext.Workouts.Remove(oldWorkout);
            _gasContext.Workouts.Add(updatedWorkout);
            _gasContext.SaveChanges();
        }

        private void UpdateWorkoutType(WorkoutType updatedWorkoutType)
        {
            var oldWorkoutType = FindWorkoutType(updatedWorkoutType.WorkoutTypeId);
            _gasContext.WorkoutTypes.Remove(oldWorkoutType);
            _gasContext.WorkoutTypes.Add(updatedWorkoutType);
            _gasContext.SaveChanges();
        }

        private void DeleteWorkout(Workout workout)
        {
            _gasContext.Workouts.Remove(workout);
            _gasContext.SaveChanges();
        }

        private void DeleteWorkoutType(WorkoutType workoutType)
        {
            _gasContext.WorkoutTypes.Remove(workoutType);
            _gasContext.SaveChanges();
        }

        public void CreateGasWorkout(Workout workout)
        {
            try
            {
                if (workout == null)
                {
                    throw new Exception("!!! Cannot add null workout !!!");
                }
                InsertWorkout(workout);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public void CreateGasWorkoutType(WorkoutType workoutType)
        {
            try
            {
                if (workoutType == null)
                {
                    throw new Exception("!!! Cannot add null workout type !!!");
                }
                InsertWorkoutType(workoutType);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public void UpdateGasWorkout(Workout workout)
        {
            try
            {
                if (workout == null)
                {
                    throw new Exception("!!! Cannot update null workout !!!");
                }

                var workoutExists = FindWorkout(workout.WorkoutId);
                if (workoutExists == null)
                {
                    throw new Exception("!!! A workout does not exist with that id !!!");
                }
                UpdateWorkout(workout);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public void UpdateGasWorkoutType(WorkoutType workoutType)
        {
            try
            {
                if (workoutType == null)
                {
                    throw new Exception("!!! Cannot update null workout type !!!");
                }

                var workoutTypeExists = FindWorkoutType(workoutType.WorkoutTypeId);
                if (workoutTypeExists == null)
                {
                    throw new Exception("!!! A workout type does not exist with that id !!!");
                }
                UpdateWorkoutType(workoutType);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public void DeleteGasWorkout(int workoutId)
        {
            try
            {
                var workout = FindWorkout(workoutId);
                if (workout == null)
                {
                    throw new Exception("!!! Cannot delete a workout that doesn't exist !!!");
                }
                DeleteWorkout(workout);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public void DeleteGasWorkoutType(int workoutTypeId)
        {
            try
            {
                var workoutType = FindWorkoutType(workoutTypeId);
                if (workoutType == null)
                {
                    throw new Exception("!!! Cannot delete a workout that doesn't exist !!!");
                }
                DeleteWorkoutType(workoutType);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
    }
}
