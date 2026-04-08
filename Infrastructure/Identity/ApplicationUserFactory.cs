// NOTE! ChatGPT helped med with this. ( It was the first entity i created so i needed help with an explanation of how to work with the code. )

using Domain.Entities.Members;
using Infrastructure.Factories;

namespace Infrastructure.Identity;

public class ApplicationUserFactory(MemberEntityFactory memberEntityFactory)
{

    private readonly MemberEntityFactory _memberEntityFactory = memberEntityFactory;

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
