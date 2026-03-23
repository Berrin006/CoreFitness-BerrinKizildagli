using Application.Common.Results;
using Domain.Aggregates.GymClasses;

public interface IGymClassService
{
    Task<Result<IEnumerable<GymClass>>> GetAllAsync();

    Task<Result> BookClassAsync(string userId, string gymClassId);

    Task<Result> UnbookClassAsync(string userId, string gymClassId);
}