/*
 * NOTE! Help from AI.
 * 
 * Here i encountered a problem with how to map output data to the response output when my output DTO includes data from muliple database tables.
 * Because my mapper that maps to DomainModel in the repository and my aggregate does not contain properties from related database tables,
 * i could not map this directly into my repository.
 * 
 * Instead, i had to create a repository that handles Read data. But since my Domain layer does not have ( and should not have ) a dependency on the Application layer and
 * i have my interfaces for repositorys there, i had to add the interface for this read repository to the Application layer in the "Abstractions" folder.
 * 
 * Ai has helped me undestand how to work around this problem and explained different ways to do this. But i thought this solution was a good compromise between Clean Architecture
 * and semi DDD style. If i strictly follow DDD i would have to rebuild my Domain layer too much because i didn't have this knowledge when i started this project.
 */

using Application.Abstraction.MembershipInterface;
using Application.Abstraction.MembershipReadInterface;
using Application.Common.Results;
using Application.Memberships.Outputs;

namespace Application.Memberships.Services;

public class GetAllMembershipsService(IMembershipQueryService membershipQueryService) : IGetAllMembershipsService
{
    public async Task<Result<List<MembershipResponseOutput>>> ExecuteAsync(CancellationToken ct = default)
    {
        try
        {
            var output = await membershipQueryService.GetAllMembershipsAsync(ct);
            return Result<List<MembershipResponseOutput>>.Ok(output);
        }
        catch (Exception ex)
        {
            return Result<List<MembershipResponseOutput>>.InternalServerError(ex.Message);
        }
    }
}
