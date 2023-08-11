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
        IQueryable<TodoItemDTO> GetTodoItems();
        Task<ActionResult<TodoItemDTO>> GetTodoItem(long id);
        Task<IActionResult> PutTodoItem(long id, TodoItemDTO todoDTO);
        TodoItem PostTodoItem(TodoItemDTO todoDTO);
        Task<int> DeleteTodoItem(long id);
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
        public IQueryable<TodoItemDTO> GetTodoItems()
        {
            return  _userDataContext.TodoItems
                .Select(x => new TodoItemDTO
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsComplete = x.IsComplete
                });
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
        public TodoItem PostTodoItem(TodoItemDTO todoDTO)
        {
            TodoItem todoItem = new()
            {
                Name = todoDTO.Name,
                IsComplete = todoDTO.IsComplete
            };

            _userDataContext.TodoItems.Add(todoItem);   
            _userDataContext.SaveChangesAsync();

            return todoItem;
        }
        // </snippet_Create>

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<int> DeleteTodoItem(long id)
        {
            var todoitem = await _userDataContext.TodoItems.FindAsync(id);
            _userDataContext.TodoItems.Remove(todoitem);

            return await _userDataContext.SaveChangesAsync();
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
