using LibraryOpitech.Application.DTOs.Auth;
using LibraryOpitech.Domain.Entities;

namespace LibraryOpitech.Application.Interfaces;

public interface IJwtTokenGenerator
{
    AuthResponse Generate(User user);
}
