using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReportingServices.WebApi;
using ReportingServices.WebApi.Data;
using System.Linq;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Threading.Tasks;
using ReportingServices.WebApi.SeedData;
using Newtonsoft.Json;
using System.Text;
using Xunit;
using FluentAssertions;
using System.Net.Http.Headers;

namespace ReportingServices.Tests
{
    [TestClass]
    public class IntegrationTests
    {
        private TestServer _server;
        public HttpClient Client { get; private set; }

        public IntegrationTests()
        {
            SetUpClient();
        }

        private async Task SeedData()
        {
            var createForm0 = GenerateReportCreateForm("Report Name 1");
            var response0 = await Client.PostAsync("/api/reports", new StringContent(JsonConvert.SerializeObject(createForm0), Encoding.UTF8, "application/json"));

            var createForm1 = GenerateReportCreateForm("Report Name 2");
            var response1 = await Client.PostAsync("/api/reports", new StringContent(JsonConvert.SerializeObject(createForm1), Encoding.UTF8, "application/json"));

            var createForm2 = GenerateReportCreateForm("Report Name 3");
            var response2 = await Client.PostAsync("/api/reports", new StringContent(JsonConvert.SerializeObject(createForm2), Encoding.UTF8, "application/json"));

            var createForm3 = GenerateReportCreateForm("Report Name 4");
            var response3 = await Client.PostAsync("/api/reports", new StringContent(JsonConvert.SerializeObject(createForm3), Encoding.UTF8, "application/json"));
        }

        private ReportForm GenerateReportCreateForm(string reportName)
        {
            return new ReportForm
            {
                Name = reportName,
            };
        }

        [Fact]
        public async Task Test1()
        {
            await SeedData();

            Client.DefaultRequestHeaders.Clear();
            var response = await Client.GetAsync("/api/reports/1");
            response.StatusCode.Should().BeEquivalentTo(200);

            var reportDefault = JsonConvert.DeserializeObject<Report>(response.Content.ReadAsStringAsync().Result);
            reportDefault.Name.Should().Be("Report Name 1");

            reportDefault.Rows.Should().HaveCount(3);
            reportDefault.Rows[0].Should().Be("Header 1");
        }

        [Fact]
        public async Task Test2()
        {
            await SeedData();

            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("it"));
            var response1 = await Client.GetAsync("/api/reports/1");
            response1.StatusCode.Should().BeEquivalentTo(200);

            var reportIt = JsonConvert.DeserializeObject<Report>(response1.Content.ReadAsStringAsync().Result);
            reportIt.Name.Should().Be("Report Name 1");

            reportIt.Rows.Should().HaveCount(3);
            reportIt.Rows[0].Should().Be("Intestazione 1");
        }

        [Fact]
        public async Task Test3()
        {
            await SeedData();

            Client.DefaultRequestHeaders.Clear();
            Client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("ru"));
            var response0 = await Client.GetAsync("/api/reports/1");
            response0.StatusCode.Should().BeEquivalentTo(200);

            var report = JsonConvert.DeserializeObject<Report>(response0.Content.ReadAsStringAsync().Result);
            report.Name.Should().Be("Report Name 1");

            report.Rows.Should().HaveCount(3);
            report.Rows[0].Should().Be("Заголовок 1");
        }
        private void SetUpClient()
        {
            var builder = new WebHostBuilder()
                .UseStartup<Startup>()
                .ConfigureServices(services =>
                {
                    var context = new ReportingContext(new DbContextOptionsBuilder<ReportingContext>()
                        .UseSqlite("DataSource=:memory:")
                        .EnableSensitiveDataLogging()
                        .Options);

                    services.RemoveAll(typeof(ReportingContext));
                    services.AddSingleton(context);

                    context.Database.OpenConnection();
                    context.Database.EnsureCreated();

                    context.SaveChanges();

                    // Clear local context cache
                    foreach (var entity in context.ChangeTracker.Entries().ToList())
                    {
                        entity.State = EntityState.Detached;
                    }
                });

            _server = new TestServer(builder);

            Client = _server.CreateClient();
        }
    }
}