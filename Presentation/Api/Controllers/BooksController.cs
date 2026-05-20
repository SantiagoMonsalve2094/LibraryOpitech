using LibraryOpitech.Application.Features.Books.Commands.CreateBook;
using LibraryOpitech.Application.Features.Books.Commands.DeleteBook;
using LibraryOpitech.Application.Features.Books.Commands.UpdateBook;
using LibraryOpitech.Application.Features.Books.Commands.UpdateBookInventory;
using LibraryOpitech.Application.Features.Books.Queries.GetBookById;
using LibraryOpitech.Application.Features.Books.Queries.GetBooks;
using LibraryOpitech.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryOpitech.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class BooksController(
    GetBooksHandler getBooksHandler,
    GetBookByIdHandler getBookByIdHandler,
    CreateBookHandler createBookHandler,
    UpdateBookHandler updateBookHandler,
    UpdateBookInventoryHandler updateBookInventoryHandler,
    DeleteBookHandler deleteBookHandler) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetBooksQuery query, CancellationToken ct)
    {
        var books = await getBooksHandler.Handle(query, ct);
        return Ok(books);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var book = await getBookByIdHandler.Handle(new GetBookByIdQuery(id), ct);
        return book is null ? NotFound() : Ok(book);
    }

    [Authorize(Roles = nameof(UserRole.Admin))]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBookCommand command, CancellationToken ct)
    {
        var book = await createBookHandler.Handle(command, ct);
        return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
    }

    [Authorize(Roles = nameof(UserRole.Admin))]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateBookCommand command, CancellationToken ct)
    {
        var book = await updateBookHandler.Handle(id, command, ct);
        return book is null ? NotFound() : Ok(book);
    }

    [Authorize(Roles = nameof(UserRole.Admin))]
    [HttpPatch("{id:guid}/inventory")]
    public async Task<IActionResult> UpdateInventory(Guid id, [FromBody] UpdateBookInventoryCommand command, CancellationToken ct)
    {
        var book = await updateBookInventoryHandler.Handle(id, command, ct);
        return book is null ? NotFound() : Ok(book);
    }

    [Authorize(Roles = nameof(UserRole.Admin))]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var deleted = await deleteBookHandler.Handle(new DeleteBookCommand(id), ct);
        return deleted ? NoContent() : NotFound();
    }
}
