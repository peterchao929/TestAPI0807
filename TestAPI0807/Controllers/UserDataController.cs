using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestAPI0807.Models;

namespace TestAPI0807.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDatasController : ControllerBase
    {
        private readonly TodoContext _context;

        public UserDatasController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDataDto>>> GetUserDatas()
        {
            return await _context.UserDatas
                .Select(x => ItemToDto(x))
                .ToListAsync();
        }

        // GET: api/TodoItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDataDto>> GetUserData(long id)
        {
            var userdata = await _context.UserDatas.FindAsync(id);

            if (userdata == null)
            {
                return NotFound();
            }

            return ItemToDto(userdata);
        }

        // PUT: api/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserData(long id, UserDataDto userDataDto)
        {
            if (id != userDataDto.Id)
            {
                return BadRequest();
            }

            var userdata = await _context.UserDatas.FindAsync(id);
            if (userdata == null)
            {
                return NotFound();
            }

            userdata.Firstname = userDataDto.Firstname;
            userdata.Lastname = userDataDto.Lastname;
            userdata.Gender = userDataDto.Gender;
            userdata.Age = userDataDto.Age;
            userdata.RegistorDate = userDataDto.RegistorDate;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!UserDataExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserDataDto>> PostTodoItem(UserDataDto userDTO)
        {
            var userData = new UserData
            {
                Firstname = userDTO.Firstname,
                Lastname = userDTO.Lastname,
                Gender = userDTO.Gender,
                Age = userDTO.Age,
                RegistorDate = DateTime.Now.ToLocalTime(),
                //RegistorDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };

            _context.UserDatas.Add(userData);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetUserData),
                new { id = userData.Id },
                ItemToDto(userData));
        }

        // DELETE: api/TodoItems/5
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

            _context.UserDatas.Remove(userData);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserDataExists(long id)
        {
            return _context.UserDatas.Any(e => e.Id == id);
        }

        private static UserDataDto ItemToDto(UserData userData) =>
            new UserDataDto
            {
                Id = userData.Id,
                Firstname = userData.Firstname,
                Lastname = userData.Lastname,
                Gender = userData.Gender,
                Age = userData.Age,
                RegistorDate = userData.RegistorDate
            };
    }
}
