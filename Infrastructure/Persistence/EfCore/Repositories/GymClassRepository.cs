using Domain.Abstractions.Repositories;
using Domain.Aggregates.GymClasses;
using Infrastructure.Persistence.EfCore.Entities;

namespace Infrastructure.Persistence.EfCore.Repositories;

public sealed class GymClassRepository(DataContext context)
    : RepositoryBase<GymClass, string, GymClassEntity, DataContext>(context), IGymClassRepository 
{
    protected override string GetId(GymClass model) => model.Id;

    protected override GymClassEntity ToEntity(GymClass model) => new()
    {
        Id = model.Id,
        Name = model.Name,
        StartTime = model.StartTime,
        Instructor = model.Instructor
    };

    protected override GymClass ToModel(GymClassEntity entity) => GymClass.Rehydrate(
        entity.Id,
        entity.Name,
        entity.StartTime,
        entity.Instructor
    );

    protected override void ApplyUpdates(GymClass model, GymClassEntity entity)
    {
        entity.Name = model.Name;
        entity.StartTime = model.StartTime;
        entity.Instructor = model.Instructor;
    }
}