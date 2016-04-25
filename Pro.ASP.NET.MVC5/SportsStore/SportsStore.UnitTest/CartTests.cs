using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStore.Domain.Entities;
using System.Linq;

namespace SportsStore.UnitTest
{
    [TestClass]
    public class CartTests
    {
        Product p1;
        Product p2;
        Product p3;
        Cart cart;

        private void ArrangeBeforeTest()
        {
            p1 = new Product { ProductID = 1, Name = "P1", Price = 100 };
            p2 = new Product { ProductID = 2, Name = "P2", Price = 50 };
            p3 = new Product { ProductID = 3, Name = "P3", Price = 10 };

            cart = new Cart();
        }

        [TestMethod]
        public void Can_Add_New_Lines()
        {
            // Arrange
            ArrangeBeforeTest();

            // Act

            cart.AddItem(p1, 1);
            cart.AddItem(p2, 1);
            CartLine[] results = cart.Lines.ToArray();

            // Assert

            Assert.AreEqual(results.Length, 2);
            Assert.AreEqual(results[0].Product, p1);
            Assert.AreEqual(results[1].Product, p2);
        }

        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            // Arrange
            ArrangeBeforeTest();

            // Act

            cart.AddItem(p1, 1);
            cart.AddItem(p2, 1);
            cart.AddItem(p1, 10);
            CartLine[] results = cart.Lines.OrderBy(l => l.Product.ProductID).ToArray();

            // Assert
            Assert.AreEqual(results.Length, 2);
            Assert.AreEqual(results[0].Quantity, 11);
            Assert.AreEqual(results[1].Quantity, 1);
        }

        [TestMethod]
        public void Can_Remove_Line()
        {
            // Arrange
            ArrangeBeforeTest();

            // Act

            cart.AddItem(p1, 1);
            cart.AddItem(p2, 3);
            cart.AddItem(p3, 5);
            cart.AddItem(p2, 1);

            cart.RemoveLine(p2);

            // Assert

            Assert.AreEqual(cart.Lines.Where(l => l.Product == p2).Count(), 0);
            Assert.AreEqual(cart.Lines.Count(), 2);
        }

        [TestMethod]
        public void Calculate_Cart_Total()
        {
            // Arrange
            ArrangeBeforeTest();

            // Act

            cart.AddItem(p1, 1);
            cart.AddItem(p2, 1);
            cart.AddItem(p1, 3);
            decimal result = cart.ComputeTotalValue();

            // Assert
            Assert.AreEqual(result, 450);
        }

        [TestMethod]
        public void Can_Clear_Contents()
        {
            // Arrange
            ArrangeBeforeTest();

            // Act

            cart.AddItem(p1, 1);
            cart.AddItem(p2, 1);
            cart.AddItem(p1, 3);

            cart.Clear();

            // Assert

            Assert.AreEqual(cart.Lines.Count(), 0);
        }
    }
}
