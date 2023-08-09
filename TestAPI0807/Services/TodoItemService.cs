using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;
using TestAPI0807.Models;

namespace TestAPI0807.Services
{
    public interface TodoItemService
    {
        Task<IEnumerable<TodoItemDTO>> GetTodoItems();
        Task<ActionResult<TodoItemDTO>> GetTodoItem(long id);
        Task<IActionResult> PutTodoItem(long id, TodoItemDTO todoDTO);
        Task<ActionResult<TodoItemDTO>> PostTodoItem(TodoItemDTO todoDTO);
        Task<IActionResult> DeleteTodoItem(long id);
        bool TodoItemExists(long id);
        TodoItemDTO ItemToDTO(TodoItem todoItem);
    }

    public class TodoItemServiceImpl : TodoItemService
    {
        private readonly UserDataContext _userDataContext;
        public TodoItemServiceImpl(UserDataContext userDataContext)
        {
            _userDataContext = userDataContext;
        }

        // GET: api/TodoItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems()
        {
            return await _userDataContext.TodoItems
                .Select(x => ItemToDTO(x))
                .ToListAsync();
        }

        // GET: api/TodoItems/5
        // <snippet_GetByID>
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id)
        {
            var todoItem = await _userDataContext.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                //return NotFound();
                return new NotFoundResult();
            }
            return ItemToDTO(todoItem);
        }
        // </snippet_GetByID>

        // PUT: api/TodoItems/5
        // <snippet_Update>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItemDTO todoDTO)
        {
            if (id != todoDTO.Id)
            {
                return new BadRequestResult();
            }

            var todoItem = await _userDataContext.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return new NotFoundResult();
            }

            todoItem.Name = todoDTO.Name;
            todoItem.IsComplete = todoDTO.IsComplete;

            try
            {
                await _userDataContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!TodoItemExists(id))
            {
                return new NotFoundResult();
            }
            return new NoContentResult();
        }
        // </snippet_Update>

        // POST: api/TodoItems
        // <snippet_Create>
        [HttpPost]
        public async Task<ActionResult<TodoItemDTO>> PostTodoItem(TodoItemDTO todoDTO)
        {
            var todoItem = new TodoItem
            {
                IsComplete = todoDTO.IsComplete,
                Name = todoDTO.Name
            };

            _userDataContext.TodoItems.Add(todoItem);   
            await _userDataContext.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetTodoItem),
                new { id = todoItem.Id },
                ItemToDTO(todoItem));
        }
        // </snippet_Create>

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var todoItem = await _userDataContext.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return new NotFoundResult();
            }

            _userDataContext.TodoItems.Remove(todoItem);
            await _userDataContext.SaveChangesAsync();

            return new NoContentResult();
        }
        public bool TodoItemExists(long id)
        {
            return _userDataContext.TodoItems.Any(e => e.Id == id);
        }
        public TodoItemDTO ItemToDTO(TodoItem todoItem) =>
            new()
            {
                Id = todoItem.Id,
                Name = todoItem.Name,
                IsComplete = todoItem.IsComplete
            };

    }
}
