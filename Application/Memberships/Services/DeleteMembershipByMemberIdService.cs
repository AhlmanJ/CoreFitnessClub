using Application.Abstraction;
using Application.Abstraction.MembershipInterface;
using Application.Common.Results;
using Domain.Abstractions.Repositories.Memberships;

namespace Application.Memberships.Services;

public class DeleteMembershipByMemberIdService(IMembershipRepository membershipRepository, IUnitOfWork _unitOfWork) : IDeleteMembershipByMemberIdService
{
    public async Task<Result<string?>> ExecuteAsync(Guid memberId, CancellationToken ct = default)
    {
        if (memberId == Guid.Empty)
            return Result<string?>.BadRequest("Member Id cannot be null.");

        try
        {
            var membershipToDelete = await membershipRepository.RemoveByMemberIdAsync(memberId, ct);
            if (!membershipToDelete)
                return Result<string?>.NotFound("Could not delete the membership");

            await _unitOfWork.CommitAsync(ct);

            return Result<string?>.Ok("Membership was deleted successfully.");
        }
        catch (Exception ex)
        {
            return Result<string?>.InternalServerError(ex.Message);
        }
    }
}
