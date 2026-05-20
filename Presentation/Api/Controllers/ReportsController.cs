using LibraryOpitech.Application.Features.Reports.Queries.GetPopularBooksByCategory;
using LibraryOpitech.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryOpitech.Api.Controllers;

[ApiController]
[Authorize(Roles = nameof(UserRole.Admin))]
[Route("api/[controller]")]
public class ReportsController(GetPopularBooksByCategoryHandler handler) : ControllerBase
{
    [HttpGet("popular-books")]
    public async Task<IActionResult> GetPopularBooks(CancellationToken ct)
    {
        var report = await handler.Handle(new GetPopularBooksByCategoryQuery(), ct);
        return Ok(report);
    }
}
