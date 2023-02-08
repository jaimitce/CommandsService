using CommandsService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommandsService.Data
{
    public class CommandRepo : ICommandRepos
    {
        private readonly AppDbContext _dbContext;

        public CommandRepo(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void CreateCommand(int platfromId, Command command)
        {
            if(command == null)
                throw new ArgumentNullException(nameof(command));

            command.PlatformId = platfromId;
            _dbContext.Commands.Add(command);
        }

        public void CreatePlatform(Platform plat)
        {
            if (plat == null)
                throw new ArgumentNullException(nameof(plat));

            _dbContext.Platforms.Add(plat);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            return _dbContext.Platforms.ToList();
        }

        public Command GetCommand(int platformId, int commandId)
        {
            return _dbContext.Commands
                    .Where(c => c.Id == commandId && c.PlatformId == platformId)
                    .FirstOrDefault();
        }

        public IEnumerable<Command> GetCommandsForPlatfrom(int platformId)
        {
            return _dbContext.Commands
                    .Where(c => c.PlatformId == platformId)
                    .OrderBy(c => c.Platform.Name);
        }

        public bool PlatformExists(int platfromId)
        {
            return _dbContext.Platforms.Any(p => p.Id == platfromId);
        }

        public bool SaveChanges()
        {
            return (_dbContext.SaveChanges() >= 0 );
        }
    }
}
