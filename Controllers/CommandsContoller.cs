using System;
using System.Collections.Generic;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/c/platform/{platformId}/commands")]
    [ApiController]
    public class CommandsContoller : ControllerBase
    {
        private readonly ICommandRepos _repositery;
        private readonly IMapper _mapper;

        public CommandsContoller(ICommandRepos repositery, IMapper mapper)
        {
            _repositery = repositery;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId)
        {
            Console.WriteLine($"--> Hit GetCommandForPlatform: {platformId}");

            if(!_repositery.PlatformExists(platformId))
            {
                return NotFound();
            }

            var commands = _repositery.GetCommandsForPlatfrom(platformId);

            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));
        }

        [HttpGet("{commandId}", Name = "GetCommandsForPlatformIdAndCommandId")]
        public ActionResult<CommandReadDto> GetCommandsForPlatformIdAndCommandId(int platformId, int commandId)
        {
            Console.WriteLine($"--> Hit GetCommandsForPlatformIdAndCommandId: {platformId} - {commandId}");

            if (!_repositery.PlatformExists(platformId))
                return NotFound();

            var command = _repositery.GetCommand(platformId, commandId);

            if(command == null)
                return NotFound();

            return Ok(_mapper.Map<CommandReadDto>(command));
        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommandForPlatform(int platformId, 
                                                            CommandCreateDto commandDto)
        {
            Console.WriteLine($"--> Hit CreateCommandForPlatform and platformId = {platformId}");
            
            if(!_repositery.PlatformExists(platformId))
            {
                return NotFound();
            }

            var command = _mapper.Map<Command>(commandDto);
            try
            {
                _repositery.CreateCommand(platformId, command);
                _repositery.SaveChanges();

                var commandReadDto = _mapper.Map<CommandReadDto>(command);
                return CreatedAtRoute(nameof(GetCommandsForPlatformIdAndCommandId),
                        new
                        {
                            platformId = platformId,
                            CommandId = commandReadDto.Id
                        },
                            commandReadDto
                        );
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"--> ArgumentNullException - Failing to create command for platform {ex.Message}");
                return Conflict();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Failing to create command for platform {ex.Message}");
                return Conflict();
            }
            
        }
    }
}
