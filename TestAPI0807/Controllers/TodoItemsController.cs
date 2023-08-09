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

}