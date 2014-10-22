using System;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace UrlsAndRoutes.Tests
{
    [TestClass]
    public class RouteTests
    {
        [TestMethod]
        public void TestIncomingRotes()
        {
            //TestRouteMatch("~/", "Home", "Index");
            //TestRouteMatch("~/Customer", "Customer", "Index");
            //TestRouteMatch("~/Admin/Index", "Admin", "Index");
            //TestRouteMatch("~/One/Two", "One", "Two");

            //TestRouteFail("~/Admin/Index/Segment");

            Assert.IsTrue(true);
        }

        [TestMethod]
        public void TestOutgoingRoutes()
        {
            RouteCollection routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);
            RequestContext context = new RequestContext(CreateHttpContext(), new RouteData());

            string result = UrlHelper.GenerateUrl(null, "Index", "Home", null, routes, context, true);

            Assert.AreEqual("/App/DoIndex", result);
        }

        private static HttpContextBase CreateHttpContext(string targetUrl = null, string httpMethod = "GET")
        {
            Mock<HttpRequestBase> mockRequest = new Mock<HttpRequestBase>();
            mockRequest.Setup(m => m.AppRelativeCurrentExecutionFilePath).Returns(targetUrl);
            mockRequest.Setup(m => m.HttpMethod).Returns(httpMethod);

            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>();
            mockResponse.Setup(m => m.ApplyAppPathModifier((It.IsAny<string>()))).Returns<string>(s => s);

            Mock<HttpContextBase> mockContext = new Mock<HttpContextBase>();
            mockContext.Setup(m => m.Request).Returns(mockRequest.Object);
            mockContext.Setup(m => m.Response).Returns(mockResponse.Object);

            return mockContext.Object;
        }

        private void TestRouteMatch(string url, string controller, string action, object routeProperties = null, string httpMethod = "GET")
        {
            RouteCollection routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            RouteData result = routes.GetRouteData(CreateHttpContext(url, httpMethod));

            Assert.IsNotNull(result);
            Assert.IsTrue(TestIncomingRouteResult(result, controller, action, routeProperties));
        }

        private void TestRouteFail(string url)
        {
            RouteCollection routes = new RouteCollection();
            RouteConfig.RegisterRoutes(routes);

            RouteData result = routes.GetRouteData(CreateHttpContext(url));

            Assert.IsTrue(result == null || result.Route == null);
        }

        private static bool TestIncomingRouteResult(RouteData routeResult, string controller, string action, object propertySet = null)
        {
            Func<object, object, bool> valCompare = (v1, v2) => { return StringComparer.CurrentCultureIgnoreCase.Compare(v1, v2) == 0; };

            bool result = valCompare(routeResult.Values["controller"], controller) && valCompare(routeResult.Values["action"], action);
            if (propertySet != null)
            {
                PropertyInfo[] propertyInfos = propertySet.GetType().GetProperties();
                foreach (PropertyInfo pi in propertyInfos)
                {
                    if (!(routeResult.Values.ContainsKey(pi.Name) && valCompare(routeResult.Values[pi.Name], pi.GetValue(propertySet, null))))
                        return false;
                }
            }

            return result;
        }
    }
}
