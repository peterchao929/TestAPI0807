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
            var result = await _userDataService.GetUserDatas().ToListAsync();

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
        public async Task<IActionResult> PutUserData(long id, UserDataDto userDataDto)
        {
            if (id != userDataDto.Id)
            {
                return BadRequest();
            }
            await _userDataService.PutUserData(id, userDataDto);
            return Ok();
        }

        // POST: api/UserDatas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserDataDto>> PostUserData(UserDataDto userDTO)
        {
            var result = _userDataService.PostUserData(userDTO);

            return CreatedAtAction(
                nameof(GetUserData),
                new { id = result.Id },
                _userDataService.UserToDto(result));
        }

        // DELETE: api/UserDatas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserData(long id)
        {
            if (_context.UserDatas == null)
            {
                return NotFound();
            }
            var userData = await _context.UserDatas.FindAsync(id);
            if (userData == null)
            {
                return NotFound();
            }

            var result = await _userDataService.DeleteUserData(id);
            if (result == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
