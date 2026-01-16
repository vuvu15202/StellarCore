using CategoryService.Domain.Entities;
using CategoryService.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CategoryService.Application.Services
{
    public interface ICategoryRepositoryProvider
    {
        dynamic GetRepository(string mapping);
        Type GetEntityType(string mapping);
    }

    public class CategoryRepositoryProvider : ICategoryRepositoryProvider
    {
        private readonly IServiceProvider _serviceProvider;
        private static readonly Dictionary<string, Type> _entityTypeMap;

        static CategoryRepositoryProvider()
        {
            _entityTypeMap = typeof(BaseCategory).Assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(BaseCategory)))
                .ToDictionary(
                    t => t.Name.Replace("Category", "").ToLower(),
                    t => t,
                    StringComparer.OrdinalIgnoreCase
                );
        }

        public CategoryRepositoryProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public dynamic GetRepository(string mapping)
        {
            var entityType = GetEntityType(mapping);
            var repoType = typeof(IBaseCategoryRepository<>).MakeGenericType(entityType);
            return _serviceProvider.GetRequiredService(repoType);
        }

        public Type GetEntityType(string mapping)
        {
            if (_entityTypeMap.TryGetValue(mapping, out var type))
            {
                return type;
            }
            throw new Exception($"Unsupported category mapping: {mapping}");
        }
    }
}
