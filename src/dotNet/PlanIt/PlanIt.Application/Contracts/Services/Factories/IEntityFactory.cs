namespace PlanIt.Application.Contracts.Services.Factories;
public interface IEntityFactory<TEntity, TDto>
    where TEntity : class
    where TDto : class
{
    TEntity Create(TDto dto);
}
