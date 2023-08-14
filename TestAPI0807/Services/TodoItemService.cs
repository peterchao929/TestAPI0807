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
        Task<List<TodoItemDto>> GetTodoItems();

        Task<TodoItemDto> GetTodoItem(long id);

        Task<int> PutTodoItem(long id, TodoItemDto todoDTO);

        Task<TodoItem> PostTodoItem(TodoItemDto todoItemDto);

        Task<int> DeleteTodoItem(long id);

        Task<TodoItemDto> ItemToDTO(TodoItem todoItem);
    }

    public class TodoItemServiceImpl : TodoItemService
    {
        private readonly UserDataContext _userDataContext;

        public TodoItemServiceImpl(UserDataContext userDataContext)
        {
            _userDataContext = userDataContext;
        }

        public async Task<List<TodoItemDto>> GetTodoItems()
        {
            var todoitem = await _userDataContext.TodoItems.ToListAsync();
            return todoitem
                .Select(x => new TodoItemDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsComplete = x.IsComplete
                })
                .ToList();
        }

        public async Task<TodoItemDto> GetTodoItem(long id)
        {
            var todoItem = await _userDataContext.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return null;
            }

            return await ItemToDTO(todoItem);
        }

        public async Task<int> PutTodoItem(long id, TodoItemDto todoDTO)
        {
            if (id != todoDTO.Id)
            {
                return 2;
            }

            var todoItem = await _userDataContext.TodoItems.FindAsync(id);
            
            if (todoItem == null)
            {
                return 0;
            }

            todoItem.Name = todoDTO.Name;
            todoItem.IsComplete = todoDTO.IsComplete;

            try
            {
                await _userDataContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!TodoItemExists(id))
            {
                return 0;
            }
            return 3;
        }

        public async Task<TodoItem> PostTodoItem(TodoItemDto todoDTO)
        {
            TodoItem todoItem = new()
            {
                Name = todoDTO.Name,
                IsComplete = todoDTO.IsComplete
            };

            _userDataContext.TodoItems.AddAsync(todoItem);
            await _userDataContext.SaveChangesAsync();

            return todoItem;
        }

        public async Task<int> DeleteTodoItem(long id)
        {
            if (_userDataContext.TodoItems == null)
            {
                return 0;
            }

            var todoitem = await _userDataContext.TodoItems.FindAsync(id);

            if (todoitem == null)
            {
                return 0;
            }

            _userDataContext.TodoItems.Remove(todoitem);

            return await _userDataContext.SaveChangesAsync();
        }

        public bool TodoItemExists(long id)
        {
            return  _userDataContext.TodoItems.Any(e => e.Id == id);
        }

        public async Task<TodoItemDto> ItemToDTO(TodoItem todoItem) =>
            new()
            {
                Id = todoItem.Id,
                Name = todoItem.Name,
                IsComplete = todoItem.IsComplete
            };
    }
}
