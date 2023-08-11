using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestAPI0807.Models;
using TestAPI0807.Services;

namespace TodoApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodoItemsController : ControllerBase
{
    private readonly UserDataContext _context;
    private readonly TodoItemService _todoItemService;

    // DI，Dependency Injection
    public TodoItemsController(UserDataContext context, TodoItemService todoItemService)
    {
        _context = context;
        _todoItemService = todoItemService;
    }

    // GET: api/TodoItems
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItemDto>>> GetTodoItems()
    {
        var result = await _todoItemService.GetTodoItems().ToListAsync();
          
        if (result == null || result.Count() <= 0)
        {
            return NotFound();
        }

        return result;
    }

    // GET: api/TodoItems/5
    // <snippet_GetByID>
    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItemDto>> GetTodoItem(long id)
    {
        var result = await _todoItemService.GetTodoItem(id);

        if (result == null)
        {
            return NotFound();
        }
        return result;
    }
    // </snippet_GetByID>

    // PUT: api/TodoItems/5
    // <snippet_Update>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTodoItem(long id, TodoItemDto todoDTO)
    {
        var result = await _todoItemService.PutTodoItem(id, todoDTO);

        if (result == 0)
        {
            return NotFound();
        }
        else if(result == 2)
        {
            return BadRequest();
        }
        else if( result == 3)
        {
            return NoContent();
        }

        return Ok();
    }
    // </snippet_Update>

    // POST: api/TodoItems
    // <snippet_Create>
    [HttpPost]
    public IActionResult PostTodoItem(TodoItemDto todoDTO)
    {
        var result =  _todoItemService.PostTodoItem(todoDTO);

        return CreatedAtAction(
            nameof(GetTodoItem),
            new { id = result.Id },
            _todoItemService.ItemToDTO(result));
    }
    // </snippet_Create>

    // DELETE: api/TodoItems/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoItem(long id)
    {
        var result = await _todoItemService.DeleteTodoItem(id);

        if (result == 0)
        {
            return NotFound();
        }

        return NoContent();
    }
}