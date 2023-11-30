namespace Core.Entities
{
    public record PaginationResult<TEntity>(List<TEntity> Entities, int TotalCount, int TotalPages, int CurrentPage);

}
