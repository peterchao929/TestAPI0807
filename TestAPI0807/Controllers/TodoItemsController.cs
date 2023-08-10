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
    public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems()
    {
        var result = await _todoItemService.GetTodoItems();

        if (result == null || result.Count() <= 0)
        {
            return NotFound();
        }

        return Ok();
    }

    // GET: api/TodoItems/5
    // <snippet_GetByID>
    [HttpGet("{id}")]
    public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
    {
        var result = await _todoItemService.GetTodoItem(id);

        if (result == null)
        {
            return NotFound();
        }
        return Ok();
    }
    // </snippet_GetByID>

    // PUT: api/TodoItems/5
    // <snippet_Update>
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTodoItem(long id, TodoItemDTO todoDTO)
    {
        if (id != todoDTO.Id)
        {
            return BadRequest();
        }
        await _todoItemService.PutTodoItem(id, todoDTO);
        return Ok();
    }
    // </snippet_Update>

    // POST: api/TodoItems
    // <snippet_Create>
    [HttpPost]
    public Task<ActionResult<TodoItemDTO>> PostTodoItem(TodoItemDTO todoDTO)
    {
        var result = _todoItemService.PostTodoItem(todoDTO);

        return CreatedAtAction(
            nameof(_todoItemService.GetTodoItem),
            new { id = result.Id },
            _todoItemService.ItemToDTO(result));
    }
    // </snippet_Create>

    // DELETE: api/TodoItems/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodoItem(long id)
    {
        if (_context.TodoItems == null)
        {
            return NotFound();
        }
        var todoitem = await _context.TodoItems.FindAsync(id);
        if (todoitem == null)
        {
            return NotFound();
        }

        var result = await _todoItemService.DeleteTodoItem(id);
        if (result == null)
        {
            return NotFound();
        }
        return NoContent();
    }

}