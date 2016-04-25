﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using SportsStore.WebUI.Models;
using SportsStore.WebUI.HtmlHelpers;

namespace SportsStore.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        private Mock<IProductRepository> GetPagingMock()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductID = 1, Name = "P1", Category = "Cat1" },
                new Product {ProductID = 2, Name = "P2", Category = "Cat2" },
                new Product {ProductID = 3, Name = "P3", Category = "Cat1" },
                new Product {ProductID = 4, Name = "P4", Category = "Cat2" },
                new Product {ProductID = 5, Name = "P5", Category = "Cat3" }
            });

            return mock;

        }

        [TestMethod]
        public void Can_Paginate()
        {
            // Arrange

            Mock<IProductRepository> mock = GetPagingMock();

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            // Act

            ProductsListViewModel result = (ProductsListViewModel)controller.List(null, 2).Model;

            // Assert

            Product[] prodArray = result.Products.ToArray();
            Assert.IsTrue(prodArray.Length == 2);
            Assert.AreEqual(prodArray[0].Name, "P4");
            Assert.AreEqual(prodArray[1].Name, "P5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            // Arrange

            HtmlHelper myHelper = null;

            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            Func<int, string> pageUrlDelegate = i => "Page" + i;

            // Act

            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            // Assert
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
            result.ToString());
        }

        [TestMethod]
        public void Can_Send_Pagination_ViewModel()
        {
            // Arrange

            Mock<IProductRepository> mock = GetPagingMock();

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            // Act

            ProductsListViewModel result = (ProductsListViewModel)controller.List(null, 2).Model;

            // Assert

            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }

        [TestMethod]
        public void Can_Filter_Products()
        {
            // Arrange

            Mock<IProductRepository> mock = GetPagingMock();

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            // Action

            Product[] result = ((ProductsListViewModel)controller.List("Cat2", 1).Model).Products.ToArray();

            // Assert

            Assert.AreEqual(result.Length, 2);
            Assert.IsTrue(result[0].Name == "P2" && result[0].Category == "Cat2");
            Assert.IsTrue(result[1].Name == "P4" && result[1].Category == "Cat2");
        }

        [TestMethod]
        public void Can_Create_Categories()
        {
            // Arrage

            Mock<IProductRepository> mock = GetPagingMock();

            NavController target = new NavController(mock.Object);

            // Action

            string[] result = ((IEnumerable<string>)target.Menu().Model).ToArray();

            // Assert

            Assert.AreEqual(result.Length, 3);
            Assert.AreEqual(result[0], "Cat1");
            Assert.AreEqual(result[1], "Cat2");
            Assert.AreEqual(result[2], "Cat3");
        }

        [TestMethod]
        public void Indicates_Selected_Category()
        {
            // Arrage

            Mock<IProductRepository> mock = GetPagingMock();

            NavController target = new NavController(mock.Object);
            string categoryToSelect = "Cat2";

            // Action

            string result = target.Menu(categoryToSelect).ViewBag.SelectedCategory;

            // Assert

            Assert.AreEqual(categoryToSelect, result);
        }

        [TestMethod]
        public void Generate_Category_Specific_Product_Count()
        {
            // Arrange

            Mock<IProductRepository> mock = GetPagingMock();

            ProductController target = new ProductController(mock.Object);
            target.PageSize = 3;

            // Action

            int res1 = ((ProductsListViewModel)target.List("Cat1").Model).PagingInfo.TotalItems;
            int res2 = ((ProductsListViewModel)target.List("Cat2").Model).PagingInfo.TotalItems;
            int res3 = ((ProductsListViewModel)target.List("Cat3").Model).PagingInfo.TotalItems;
            int resAll = ((ProductsListViewModel)target.List(null).Model).PagingInfo.TotalItems;

            // Assert

            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 1);
            Assert.AreEqual(resAll, 5);
        }
    }
}
