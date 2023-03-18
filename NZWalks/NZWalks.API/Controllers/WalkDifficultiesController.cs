using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkDifficultiesController : Controller
    {
        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        private readonly IMapper mapper;

        public WalkDifficultiesController(IWalkDifficultyRepository walkDifficultyRepository, IMapper mapper)
        {
            this.walkDifficultyRepository = walkDifficultyRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalkDifficultiesAsync()
        {
            // Fetch data from database - Domain WalkDifficulty
            var walkDifficulties = await walkDifficultyRepository.GetAllAsync();
            var walkDifficultiesDTO = mapper.Map<List<Models.DTO.WalkDifficulty>>(walkDifficulties);

            // Return response
            return Ok(walkDifficultiesDTO);
        }

        [HttpGet]
        [Route("{id:guid}")] //restricting id to take only guid values
        [ActionName("GetWalkDifficultyAsync")]
        public async Task<IActionResult> GetWalkDifficultyAsync(Guid id)
        {
            // Get WalkDificulty Domain object from database
            var walkDifficulty = await walkDifficultyRepository.GetAsync(id);
            if (walkDifficulty == null)
            {
                return NotFound();
            }

            // Convert Domain object to DTO
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);
            // Return response
            return Ok(walkDifficultyDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkDifficultyAsync(AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            // AddWalkDifficultyRequest DTO to domain model
            var walkDifficulty = new Models.Domain.WalkDifficulty()
            {
                Code = addWalkDifficultyRequest.Code
            };

            // Pass domain object to repository to persist data
            walkDifficulty = await walkDifficultyRepository.AddAsync(walkDifficulty);

            // Convert the Domain object back to DTO
            //var walkDifficultyDTO = new Models.DTO.WalkDifficulty()
            //{
            //    Id = walkDifficulty.Id,
            //    Code = walkDifficulty.Code
            //};
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);

            // Send DTO response back to client
            return CreatedAtAction(nameof(GetWalkDifficultyAsync), new {Id = walkDifficultyDTO.Id}, walkDifficultyDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkDifficultyAsync(Guid id)
        {
            // Get walkDifficulty from database
            var walkDifficulty = await walkDifficultyRepository.DeleteAsync(id);

            // If null NotFound
            if (walkDifficulty == null)
            {
                return NotFound();
            }

            // Convert response back to DTO model
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);
            return Ok(walkDifficultyDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult>UpdateWalkDifficultyAsync(Guid id, UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            // Convert UpdateWalkDifficultyRequest DTO to domain model
            var walkDifficulty = new Models.Domain.WalkDifficulty()
            {
                Code = updateWalkDifficultyRequest.Code
            };

            // Update WalkDifficulty using repository
            walkDifficulty = await walkDifficultyRepository.UpdateAsync(id, walkDifficulty);

            // If null NotFound
            if (walkDifficulty == null)
            {
                return NotFound();
            }

            // Convert Domain back to DTO model
            var walkDifficultyDTO = mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);

            // return Ok response
            return Ok(walkDifficultyDTO);
        }
    }
}
