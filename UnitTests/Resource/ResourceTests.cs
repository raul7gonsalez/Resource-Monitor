namespace UnitTests.Resource
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using AutoFixture;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using ResourceMonitor.Controllers;
    using ResourceMonitor.Entities;
    using ResourceMonitor.Interfaces;

    /// <summary>Тесты для ресурсов</summary>
    [TestClass]
    public class ResourceTests
    {
        /// <summary>Вывод списка ресурсов</summary>
        [TestMethod]
        public void ListResources()
        {
            var mock = new Mock<IResourceRepository>();
            var fixture = new Fixture();
            var firstEntity = fixture.Build<Resource>()
                                     .With(r => r.Name, "Msdn")
                                     .With(r => r.HostAddress, "https://msdn.microsoft.com")
                                     .Create();

            var secondEntity = fixture.Build<Resource>()
                                      .With(r => r.Name, "Mail")
                                      .With(r => r.HostAddress, "https://msdn.mail.ru")
                                      .Create();

            var entities = new List<Resource>
                           {
                               firstEntity,
                               secondEntity
                           };

            mock.Setup(m => m.Resources)
                .Returns(entities.AsQueryable());

            var controller = new AdminController
                             {
                                 ResourceRepository = mock.Object
                             };

            var viewResult = controller.List() as ViewResult;
            var entityForEdit = viewResult?.ViewData.Model as IReadOnlyList<Resource>;

            Assert.IsNotNull(entityForEdit);
            Assert.IsTrue(entityForEdit.Count == mock.Object.Resources.Count());
        }

        /// <summary>Добавление ресурса</summary>
        [TestMethod]
        public void AddResource()
        {
            var mock = new Mock<IResourceRepository>();
            var fixture = new Fixture();
            var firstEntity = fixture.Build<Resource>()
                                     .With(r => r.Name, "Msdn")
                                     .With(r => r.HostAddress, "https://msdn.microsoft.com")
                                     .Create();

            mock.Setup(m => m.Resources)
                .Returns(new[] { firstEntity }.AsQueryable());

            var controller = new AdminController
                             {
                                 ResourceRepository = mock.Object
                             };

            controller.Create(firstEntity);

            mock.Verify(m => m.Create(firstEntity));
        }

        /// <summary>Успешное получение ресурса для редактирования</summary>
        [TestMethod]
        public void SuccessGetResourceForEdit()
        {
            var mock = new Mock<IResourceRepository>();
            var fixture = new Fixture();
            var firstEntity = fixture.Build<Resource>()
                                     .With(r => r.Name, "Msdn")
                                     .With(r => r.HostAddress, "https://msdn.microsoft.com")
                                     .Create();

            var entities = new List<Resource>
                           {
                               firstEntity
                           };

            mock.Setup(m => m.Resources)
                .Returns(entities.AsQueryable());

            mock.Setup(m => m.Get(It.IsAny<long>()))
                .Returns((long id) => entities.Single(e => e.Id.Equals(id)));

            var controller = new AdminController
                             {
                                 ResourceRepository = mock.Object
                             };

            var viewResult = controller.Edit(firstEntity.Id) as ViewResult;
            var entityForEdit = viewResult?.ViewData.Model as Resource;

            Assert.IsNotNull(entityForEdit);
            Assert.IsTrue(entityForEdit.Id == firstEntity.Id);
        }

        /// <summary>Ошибка при получении несуществующего ресурса для редактирования</summary>
        [TestMethod]
        public void ErrorGetResourceForEdit()
        {
            var mock = new Mock<IResourceRepository>();
            var fixture = new Fixture();
            var firstEntity = fixture.Build<Resource>()
                                     .With(r => r.Name, "Msdn")
                                     .With(r => r.HostAddress, "https://msdn.microsoft.com")
                                     .Create();

            var secondEntity = fixture.Build<Resource>()
                                      .With(r => r.Name, "Mail")
                                      .With(r => r.HostAddress, "https://msdn.mail.ru")
                                      .Create();

            var entities = new List<Resource>
                           {
                               firstEntity
                           };

            mock.Setup(m => m.Resources)
                .Returns(entities.AsQueryable());

            mock.Setup(m => m.Get(It.IsAny<long>()))
                .Returns((long id) => entities.SingleOrDefault(e => e.Id.Equals(id)));

            var controller = new AdminController
                             {
                                 ResourceRepository = mock.Object
                             };

            var viewResult = controller.Edit(secondEntity.Id) as ViewResult;
            var entityForEdit = viewResult?.ViewData.Model as Resource;

            Assert.IsNull(entityForEdit);
        }

        /// <summary>Успешное редактирование ресурса</summary>
        [TestMethod]
        public void SuccessEditResource()
        {
            var mock = new Mock<IResourceRepository>();
            var fixture = new Fixture();
            var firstEntity = fixture.Build<Resource>()
                                     .With(r => r.Name, "Msdn")
                                     .With(r => r.HostAddress, "https://msdn.microsoft.com")
                                     .Create();

            mock.Setup(m => m.Resources)
                .Returns(new[] { firstEntity }.AsQueryable());

            var newName = "Test";

            firstEntity.Name = newName;

            var controller = new AdminController
                             {
                                 ResourceRepository = mock.Object
                             };

            controller.Edit(firstEntity);

            mock.Verify(s => s.Update(It.Is<Resource>(rs => rs.Name == newName)));
        }

        /// <summary>Успешное получение ресурса для удаления</summary>
        [TestMethod]
        public void SuccessGetResourceForDelete()
        {
            var mock = new Mock<IResourceRepository>();
            var fixture = new Fixture();
            var firstEntity = fixture.Build<Resource>()
                                     .With(r => r.Name, "Msdn")
                                     .With(r => r.HostAddress, "https://msdn.microsoft.com")
                                     .Create();

            var entities = new List<Resource>
                          {
                              firstEntity
                          };

            mock.Setup(m => m.Resources)
                .Returns(entities.AsQueryable());

            mock.Setup(m => m.Get(It.IsAny<long>()))
                .Returns((long id) => entities.Single(e => e.Id.Equals(id)));

            var controller = new AdminController
                             {
                                 ResourceRepository = mock.Object
                             };

            controller.Delete(firstEntity.Id);

            mock.Verify(m => m.Get(firstEntity.Id));
        }

        /// <summary>Ошибка при попытке получить несуществующий ресурс для удаления</summary>
        [TestMethod]
        public void ErrorGetResourceForDelete()
        {
            var mock = new Mock<IResourceRepository>();
            var fixture = new Fixture();
            var firstEntity = fixture.Build<Resource>()
                                     .With(r => r.Name, "Msdn")
                                     .With(r => r.HostAddress, "https://msdn.microsoft.com")
                                     .Create();

            var secondEntity = fixture.Build<Resource>()
                                      .With(r => r.Name, "Mail")
                                      .With(r => r.HostAddress, "https://msdn.mail.ru")
                                      .Create();

            var entities = new List<Resource>
                           {
                               firstEntity
                           };

            mock.Setup(m => m.Resources)
                .Returns(entities.AsQueryable());

            mock.Setup(m => m.Get(It.IsAny<long>()))
                .Returns((long id) => entities.SingleOrDefault(e => e.Id.Equals(id)));

            var controller = new AdminController
                             {
                                 ResourceRepository = mock.Object
                             };

            var viewResult = controller.Delete(secondEntity.Id) as ViewResult;
            var entityForDelete = viewResult?.ViewData.Model as Resource;

            Assert.IsNull(entityForDelete);
        }

        /// <summary>Успешное удаление ресурса</summary>
        [TestMethod]
        public void SuccessDeleteResource()
        {
            var mock = new Mock<IResourceRepository>();
            var fixture = new Fixture();
            var firstEntity = fixture.Build<Resource>()
                                     .With(r => r.Name, "Msdn")
                                     .With(r => r.HostAddress, "https://msdn.microsoft.com")
                                     .Create();

            var entities = new List<Resource>
                           {
                               firstEntity
                           };

            mock.Setup(m => m.Resources)
                .Returns(entities.AsQueryable());

            mock.Setup(m => m.Get(It.IsAny<long>()))
                .Returns((long id) => entities.SingleOrDefault(e => e.Id.Equals(id)));

            var controller = new AdminController
                             {
                                 ResourceRepository = mock.Object
                             };

            controller.DeleteConfirmed(firstEntity.Id);

            mock.Verify(m => m.Get(firstEntity.Id));
            mock.Verify(m => m.Delete(firstEntity));
        }
    }
}