using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestAPI0807.Models;
using TestAPI0807.Services;

namespace TestAPI0807.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDatasController : ControllerBase
    {
        private readonly UserDataContext _context;
        private readonly UserDataService _userDataService;

        public UserDatasController(UserDataContext context,UserDataService userDataService)
        {
            _context = context;
            _userDataService = userDataService;
        }

        // GET: api/UserDatas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDataDto>>> GetUserDatas()
        {
            var result = await _userDataService.GetUserDatas();

            if (result == null || result.Count() <= 0)
            {
                return NotFound();
            }

            return result;
        }

        // GET: api/UserDatas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDataDto>> GetUserData(long id)
        {
            var result = await _userDataService.GetUserData(id);

            if (result == null)
            {
                return NotFound();
            }
            return result;
        }

        // PUT: api/UserDatas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserData([FromRoute] long id, [FromBody] UserDataDto userDataDto)
        {
            var result = await _userDataService.PutUserData(id, userDataDto);

            if (result == 0)
            {
                return NotFound();
            }
            else if (result == 2)
            {
                return BadRequest();
            }
            else if (result == 3)
            {
                return NoContent();
            }

            return Ok();
        }

        // POST: api/UserDatas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostUserData(UserDataDto userDTO)
        {
            var result = await _userDataService.PostUserData(userDTO);

            return CreatedAtAction(
                nameof(GetUserData),
                new { id = result.Id },
                _userDataService.UserToDto(result));
        }

        // DELETE: api/UserDatas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserData(long id)
        {
            var result = await _userDataService.DeleteUserData(id);

            if (result == 0)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
