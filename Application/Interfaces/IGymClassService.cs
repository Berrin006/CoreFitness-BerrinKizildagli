using Application.Common.Results;
using Domain.Aggregates.GymClasses;

namespace Application.Services;

public interface IGymClassService
{
    Task<Result<IEnumerable<GymClass>>> GetAllAsync();
    Task<Result> BookClassAsync(string userId, string classId);
    Task<Result> UnbookClassAsync(string userId, string classId);
    Task<Result> CreateClassAsync(GymClass model);
    Task<Result> DeleteClassAsync(string id);

    Task<List<string>> GetBookedClassIdsForUserAsync(string userId);
    Task<IEnumerable<GymClass>> GetBookedClassesForUserAsync(string userId);
}