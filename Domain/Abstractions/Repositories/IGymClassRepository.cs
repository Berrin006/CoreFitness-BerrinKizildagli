using Domain.Aggregates.GymClasses;

namespace Domain.Abstractions.Repositories;

public interface IGymClassRepository : IRepositoryBase<GymClass, string>
{
}