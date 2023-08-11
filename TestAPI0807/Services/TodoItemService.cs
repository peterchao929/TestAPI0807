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
        IQueryable<TodoItemDto> GetTodoItems();

        Task<TodoItemDto> GetTodoItem(long id);

        Task<int> PutTodoItem(long id, TodoItemDto todoDTO);

        TodoItem PostTodoItem(TodoItemDto todoItemDto);

        Task<int> DeleteTodoItem(long id);

        bool TodoItemExists(long id);

        TodoItemDto ItemToDTO(TodoItem todoItem);
    }

    public class TodoItemServiceImpl : TodoItemService
    {
        private readonly UserDataContext _userDataContext;

        public TodoItemServiceImpl(UserDataContext userDataContext)
        {
            _userDataContext = userDataContext;
        }

        public IQueryable<TodoItemDto> GetTodoItems()
        {
            return  _userDataContext.TodoItems
                .Select(x => new TodoItemDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    IsComplete = x.IsComplete
                });
        }

        public async Task<TodoItemDto> GetTodoItem(long id)
        {
            var todoItem = await _userDataContext.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return null;
            }

            return ItemToDTO(todoItem);
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

        public TodoItem PostTodoItem(TodoItemDto todoDTO)
        {
            TodoItem todoItem = new()
            {
                Name = todoDTO.Name,
                IsComplete = todoDTO.IsComplete
            };

            _userDataContext.TodoItems.Add(todoItem);
            _userDataContext.SaveChanges();
            //_userDataContext.SaveChangesAsync();

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
            return _userDataContext.TodoItems.Any(e => e.Id == id);
        }

        public TodoItemDto ItemToDTO(TodoItem todoItem) =>
            new()
            {
                Id = todoItem.Id,
                Name = todoItem.Name,
                IsComplete = todoItem.IsComplete
            };
    }
}
