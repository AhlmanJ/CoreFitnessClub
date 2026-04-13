
// This will become its own table in the Database and is the user account itself.

using Domain.Entities.Members;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{

}
