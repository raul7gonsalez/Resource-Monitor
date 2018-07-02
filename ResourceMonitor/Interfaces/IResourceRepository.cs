namespace ResourceMonitor.Interfaces
{
    using System.Linq;
    using ResourceMonitor.Entities;

    /// <summary> Репозиторий с ресурсами </summary>
    public interface IResourceRepository
    {
        /// <summary> Ресурсы </summary>
        IQueryable<Resource> Resources { get; }

        /// <summary>Получить</summary>
        Resource Get(long id);

        /// <summary>Создать</summary>
        void Create(Resource resource);

        /// <summary>Обновить</summary>
        void Update(Resource resource);

        /// <summary>Удалить</summary>
        Resource Delete(Resource resource);
    }
}