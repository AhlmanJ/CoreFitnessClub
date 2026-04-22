// NOTE! ChatGPT helped med with this. ( It was the first entity i created so i needed help with an explanation of how to work with the code. )

using Infrastructure.Entities.Members;

namespace Infrastructure.Identity;

public class ApplicationUserFactory()
{
    public ApplicationUser Create(string email, bool confirmedEmail = true)
    {

        var applicationUser = new ApplicationUser
        { 
            UserName = email,
            Email = email,
            EmailConfirmed = confirmedEmail
        };

        return applicationUser;
    }
}
