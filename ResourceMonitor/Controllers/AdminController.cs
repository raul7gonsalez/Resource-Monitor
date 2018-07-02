namespace ResourceMonitor.Controllers
{
    using System.Linq;
    using System.Net;
    using System.Web.Mvc;

    using ResourceMonitor.Entities;
    using ResourceMonitor.Interfaces;

    /// <summary>Контроллер страницы администратора</summary>
    [Authorize]
    public class AdminController : Controller
    {
        public IResourceRepository ResourceRepository { get; set; }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Resource resource)
        {
            if (ModelState.IsValid)
            {
                ResourceRepository.Create(resource);

                return RedirectToAction("List");
            }

            return View(resource);
        }

        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var resource = ResourceRepository.Get(id.Value);
            if (resource == null)
            {
                return HttpNotFound();
            }

            return View(resource);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            var resource = ResourceRepository.Get(id);

            ResourceRepository.Delete(resource);

            return RedirectToAction("List");
        }

        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var resource = ResourceRepository.Get(id.Value);
            if (resource == null)
            {
                return HttpNotFound();
            }

            return View(resource);
        }

        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var resource = ResourceRepository.Get(id.Value);
            if (resource == null)
            {
                return HttpNotFound();
            }

            return View(resource);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Resource resource)
        {
            if (ModelState.IsValid)
            {
                ResourceRepository.Update(resource);

                return RedirectToAction("List");
            }
            return View(resource);
        }

        public ActionResult List()
        {
            return View(ResourceRepository.Resources.ToList());
        }
    }
}