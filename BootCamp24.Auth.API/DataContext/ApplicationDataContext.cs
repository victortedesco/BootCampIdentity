using BootCamp24.Auth.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BootCamp24.Auth.API.DataContext;

public class ApplicationDataContext(DbContextOptions<ApplicationDataContext> options) : IdentityDbContext<User>(options)
{
}
