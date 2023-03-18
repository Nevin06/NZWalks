using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;

        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalksAsync()
        {
            // Fetch data from database - Domain Walks
            var walks = await walkRepository.GetAllAsync();
            // Convert Domain Walks to DTO Walks
            var walksDTO = mapper.Map<List<Models.DTO.Walk>>(walks);
            // Return response
            return Ok(walksDTO);
        }

        [HttpGet]
        [Route("{id:guid}")] //restricting id to take only guid values
        [ActionName("GetWalkAsync")]
        public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            // Get Walk Domain object from database
            var walk = await walkRepository.GetAsync(id);
            if (walk == null)
            {
                return NotFound();
            }

            // Convert Domain object to DTO
            var walkDTO = mapper.Map<Models.DTO.Walk>(walk);
            // Return response
            return Ok(walkDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkAsync([FromBody] AddWalkRequest addWalkRequest)
        {
            // AddWalkRequest DTO to domain model
            var walk = new Models.Domain.Walk()
            {
                Name = addWalkRequest.Name,
                Length = addWalkRequest.Length,
                RegionId = addWalkRequest.RegionId,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId,
            };

            // Pass domain object to repository to persist data
            walk = await walkRepository.AddAsync(walk);

            // Convert the Domain object back to DTO
            var walkDTO = new Models.DTO.Walk()
            {
                Id = walk.Id,
                Name = walk.Name,
                Length = walk.Length,
                RegionId = walk.RegionId,
                WalkDifficultyId = walk.WalkDifficultyId,
            };

            // Send DTO response back to client
            return CreatedAtAction(nameof(GetWalkAsync), new {Id = walkDTO.Id}, walkDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkAsync(Guid id)
        {
            // Get walk from database
            var walk = await walkRepository.DeleteAsync(id);

            // If null NotFound
            if (walk == null)
            {
                return NotFound();
            }

            // Convert response back to DTO model
            var walkDTO = mapper.Map<Models.DTO.Walk>(walk);
            return Ok(walkDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult>UpdateWalkAsync([FromRoute] Guid id, [FromBody] UpdateWalkRequest updateWalkRequest)
        {
            // Convert UpdateWalkRequest DTO to domain model
            var walk = new Models.Domain.Walk()
            {
                Name = updateWalkRequest.Name,
                Length = updateWalkRequest.Length,
                RegionId = updateWalkRequest.RegionId,
                WalkDifficultyId = updateWalkRequest.WalkDifficultyId,
                //Region = updateWalkRequest.Region,
                //WalkDifficulty = updateWalkRequest.WalkDifficulty
            };

            // Update Walk using repository
            walk = await walkRepository.UpdateAsync(id, walk);

            // If null NotFound
            if (walk == null)
            {
                return NotFound();
            }

            // Convert Domain back to DTO model
            var walkDTO = mapper.Map<Models.DTO.Walk>(walk);

            // return Ok response
            return Ok(walkDTO);
        }
    }
}
