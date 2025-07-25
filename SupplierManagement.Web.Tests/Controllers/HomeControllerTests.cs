using Microsoft.AspNetCore.Mvc;
using SupplierManagement.Web.Controllers;
using Xunit;

namespace SupplierManagement.Web.Tests.Controllers
{
    public class HomeControllerTests
    {
        private readonly HomeController _controller;

        public HomeControllerTests()
        {
            _controller = new HomeController();
        }

        [Fact]
        public void Index_ReturnsViewResult()
        {
            // Act
            var result = _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName); // Default view
        }

        [Fact]
        public void Error_ReturnsViewResult()
        {
            // Act
            var result = _controller.Error();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName); // Default view
        }

        [Fact]
        public void Index_ReturnsNonNullResult()
        {
            // Act
            var result = _controller.Index();

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Error_ReturnsNonNullResult()
        {
            // Act
            var result = _controller.Error();

            // Assert
            Assert.NotNull(result);
        }
    }
}
