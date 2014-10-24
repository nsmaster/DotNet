using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.HtmlHelpers;
using SportsStore.WebUI.Models;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CanPaginate()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new[]
                                                {
                                                    new Product{ ProductId = 1, Name = "P1"},
                                                    new Product{ ProductId = 2, Name = "P2"},
                                                    new Product{ ProductId = 3, Name = "P3"},
                                                    new Product{ ProductId = 4, Name = "P4"},
                                                    new Product{ ProductId = 5, Name = "P5"},
                                                }.AsQueryable());

            ProductController controller = new ProductController(mock.Object) { PageSize = 3 };

            var result = (ProductListViewModal)controller.List(null, 2).Model;

            Product[] productArray = result.Products.ToArray();

            Assert.IsTrue(productArray.Length == 2);
            Assert.AreEqual(productArray[0].Name, "P4");
            Assert.AreEqual(productArray[1].Name, "P5");
        }

        [TestMethod]
        public void CanGeneratePageLinks()
        {
            HtmlHelper myHelper = null;

            PagingInfo pagingInfo = new PagingInfo
                                    {
                                        CurrentPage = 2,
                                        TotalItems = 28,
                                        ItemsPerPage = 10
                                    };

            Func<int, string> pageUrlDelegate = i => "Page" + i;

            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            Assert.AreEqual(result.ToString(), @"<a href=""Page1"">1</a>"
                + @"<a class=""selected"" href=""Page2"">2</a>"
                + @"<a href=""Page3"">3</a>");
        }

        [TestMethod]
        public void CanFilterProducts()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new[]
                                                {
                                                    new Product{ ProductId = 1, Name = "P1", Category = "Cat1"},
                                                    new Product{ ProductId = 2, Name = "P2", Category = "Cat2"},
                                                    new Product{ ProductId = 3, Name = "P3", Category = "Cat1"},
                                                    new Product{ ProductId = 4, Name = "P4", Category = "Cat2"},
                                                    new Product{ ProductId = 5, Name = "P5", Category = "Cat3"},
                                                }.AsQueryable());

            ProductController controller = new ProductController(mock.Object) { PageSize = 3 };

            var result = (ProductListViewModal)controller.List("Cat2").Model;

            Product[] productArray = result.Products.ToArray();

            Assert.IsTrue(productArray.Length == 2);
            Assert.IsTrue(productArray[0].Name == "P2" && productArray[0].Category == "Cat2");
            Assert.IsTrue(productArray[1].Name == "P4" && productArray[1].Category == "Cat2");
        }

        [TestMethod]
        public void CanCreateCategories()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new[]
                                                {
                                                    new Product{ ProductId = 1, Name = "P1", Category = "Cat1"},
                                                    new Product{ ProductId = 2, Name = "P2", Category = "Cat2"},
                                                    new Product{ ProductId = 3, Name = "P3", Category = "Cat1"},
                                                    new Product{ ProductId = 4, Name = "P4", Category = "Cat2"},
                                                    new Product{ ProductId = 5, Name = "P5", Category = "Cat3"},
                                                }.AsQueryable());

            NavController controller = new NavController(mock.Object);

            var result = ((IEnumerable<string>)controller.Menu().Model).ToArray();

            Assert.AreEqual(result[2], "Cat3");
            Assert.AreEqual(result.Length, 3);
        }

        [TestMethod]
        public void IndicatesSelectedCategory()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new[]
                                                {
                                                    new Product{ ProductId = 1, Name = "P1", Category = "Cat1"},
                                                    new Product{ ProductId = 2, Name = "P2", Category = "Cat2"},
                                                    new Product{ ProductId = 3, Name = "P3", Category = "Cat1"},
                                                    new Product{ ProductId = 4, Name = "P4", Category = "Cat2"},
                                                    new Product{ ProductId = 5, Name = "P5", Category = "Cat3"},
                                                }.AsQueryable());

            NavController controller = new NavController(mock.Object);

            string categoryToSelect = "Cat2";
            string result = controller.Menu(categoryToSelect).ViewBag.SelectedCategory;

            Assert.AreEqual(result, categoryToSelect);
        }
    }
}
