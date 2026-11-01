using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SharedPipelines.DockerSample.Tests;

[TestClass]
public sealed class ServiceProbeTest
{
    [TestMethod]
    public async Task Get_ForRunningDockerService_ReturnsOk()
    {
        using var probe = new ServiceProbe();

        using var response = await probe.GetAsync();

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadAsStringAsync();
        Assert.IsFalse(string.IsNullOrWhiteSpace(body), "The service returned an empty body.");
    }
}
