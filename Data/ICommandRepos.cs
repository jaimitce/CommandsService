using CommandsService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommandsService.Data
{
    public interface ICommandRepos
    {
        bool SaveChanges();

        //Platforms
        public IEnumerable<Platform> GetAllPlatforms();
        public void CreatePlatform(Platform plat);
        public bool PlatformExists(int platfromId);

        //Commands
        public IEnumerable<Command> GetCommandsForPlatfrom(int platformId);
        Command GetCommand(int platformId, int commandId);
        void CreateCommand(int platfromId, Command command);
    }
}
