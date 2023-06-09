using MassTransit;
using Moq;
using Workshop.Services;
using Workshop.Services.TransferObjects;

namespace TestProject1
{
  [TestClass]
  public class UnitTest1
  {
    [TestMethod]
    public void TestMethod1()
    {
      var dashboardServiceMock = new Mock<IDashboardService>();

      dashboardServiceMock.Setup(e => e.AddDashboard(It.IsAny<DashboardDto>()))
        .Returns(Task.CompletedTask);


      // dashboardServiceMock.Object.DeleteDashboard(string, string, string, string);



    }
  }
}