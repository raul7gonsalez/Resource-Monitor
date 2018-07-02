namespace ResourceMonitor.Repositories
{
    using System.Data.Entity;
    using System.Linq;

    using ResourceMonitor.DbContexts;
    using ResourceMonitor.Entities;
    using ResourceMonitor.Interfaces;

    /// <inheritdoc />
    public class ResourceRepository : IResourceRepository
    {
        private readonly EFDbContext _context = new EFDbContext();

        public IQueryable<Resource> Resources => _context.Resources;

        public Resource Get(long id)
        {
            var entity = _context.Resources.Find(id);

            return entity;
        }

        public void Create(Resource resource)
        {
            _context.Resources.Add(resource);

            _context.SaveChanges();
        }

        public Resource Delete(Resource resource)
        {
            var entity = _context.Resources.Find(resource.Id);
            if (entity != null)
            {
                _context.Resources.Remove(entity);
                _context.SaveChanges();
            }

            return entity;
        }

        public void Update(Resource resource)
        {
            var entity = _context.Resources.Find(resource.Id);
            if (entity != null)
            {
                entity.Name = resource.Name;
                entity.HostAddress = resource.HostAddress;
                _context.Entry(entity).State = EntityState.Modified;
            }

            _context.SaveChanges();
        }
    }
}