using System;
using System.Collections.Generic;
using System.Text;
using GodsAmongSheep.Shared.Models;

namespace GodsAmongSheep
{
    public class GAS_Repos
    {
        public List<GasUser> _users = new List<GasUser>()
        {
            new GasUser{UserId = 1, Username="Sao", Password="asuna", 
                Workouts = new List<Workout>()
                {
                    new Workout
                    {
                        WorkoutId = 1, 
                        Date = new DateTime(2019, 9, 30),
                        Workouts = new List<WorkoutType>()
                        {
                            new WorkoutType{WorkoutTypeId = 1, Exercise = ExerciseType.Cardio, Length = 25},
                            new WorkoutType{WorkoutTypeId = 2, Exercise = ExerciseType.Sports, Length = 30}
                        },
                        Description = "Tred Mill at lvl 6, Basketball 2v2"
                    },
                    new Workout
                    {
                        WorkoutId = 2,
                        Date = new DateTime(2019, 10, 30),
                        Workouts = new List<WorkoutType>()
                        {
                            new WorkoutType{WorkoutTypeId = 1, Exercise = ExerciseType.Cardio, Length = 25},
                            new WorkoutType{WorkoutTypeId = 2, Exercise = ExerciseType.Sports, Length = 30},
                            new WorkoutType{WorkoutTypeId = 3, Exercise = ExerciseType.Arms, Length = 30}
                        },
                        Description = "Tred Mill at lvl 5, Basketball 21, Bench 135 lbs 3 sets of 10"
                    }
                }},
            new GasUser{UserId = 2, Username="ElectricTaco", Password="test"},
            new GasUser{UserId = 3, Username="hestia", Password="danmachi"},
            new GasUser{UserId = 4, Username="aiz", Password="danmachi"}
        };

        public List<GasUser> GetUsers()
        {
            return _users;
        }

        public List<Workout> _workouts = new List<Workout>()
        {
            new Workout{WorkoutId = 1,  Date = DateTime.Now, 
                Workouts = new List<WorkoutType>()
                {
                    new WorkoutType{WorkoutTypeId = 1, Exercise = ExerciseType.Cardio, Length = 25},
                    new WorkoutType{WorkoutTypeId = 1, Exercise = ExerciseType.Sports, Length = 30}
                }
            },
            new Workout{WorkoutId = 2,  Date = DateTime.Now,
                Workouts = new List<WorkoutType>()
                {
                    new WorkoutType{WorkoutTypeId = 1, Exercise = ExerciseType.Arms, Length = 15},
                    new WorkoutType{WorkoutTypeId = 1, Exercise = ExerciseType.Cardio, Length = 30}
                }
            },
            new Workout{WorkoutId = 3,  Date = DateTime.Now,
                Workouts = new List<WorkoutType>()
                {
                    new WorkoutType{WorkoutTypeId = 1, Exercise = ExerciseType.Abs, Length = 15},
                    new WorkoutType{WorkoutTypeId = 1, Exercise = ExerciseType.Back, Length = 20}
                }
            }
        };

        public List<Workout> GetWorkouts()
        {
            return _workouts;
        }
    }
}
