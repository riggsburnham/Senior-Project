using System;
using System.Collections.Generic;
using System.Text;

namespace GodsAmongSheep.Shared.Controllers
{
    public class GasContextController
    {
        private GasContext _gasContext;
        public GasContextController(GasContext gasContext)
        {
            _gasContext = gasContext;
            _gasContext.Database.EnsureCreated();
        }

        public GasContext GasContext => _gasContext;

        public GasUsersController GasUsersController => new GasUsersController(_gasContext);

        public GasWorkoutsController GasWorkoutsController => new GasWorkoutsController(_gasContext);

        public GasFriendsController GasFriendsController => new GasFriendsController(_gasContext);

        public GasWorkoutGroupsController GasWorkoutGroupsController => new GasWorkoutGroupsController(_gasContext);

        public GasGroupChatsController GasGroupChatsController => new GasGroupChatsController(_gasContext);
    }
}
