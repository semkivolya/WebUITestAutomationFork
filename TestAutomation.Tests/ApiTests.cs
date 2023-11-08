using Microsoft.Extensions.Configuration;
using RestSharp;
using System.Net;
using TestAutomation.Business.Api;

namespace TestAutomation.Tests
{
    [Category("API")]
    public class ApiTests
    {
        private IConfiguration configuration;
        private RestRequestsProcessor _requestsProcessor;
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        [OneTimeSetUp]
        public void OnTimeSetUp()
        {
            configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            Logger.Info("Created configuration");
            _requestsProcessor = new RestRequestsProcessor(configuration["apiapplication"]);
        }

        [Test]
        public async Task ListOfUsersCanBeReceivedSuccessfully()
        {
            var response = await _requestsProcessor.GetUsers();
            Assert.That(_requestsProcessor.UsersDataIsValid(response.Content), Is.True);
            Assert.That(response.ResponseStatus, Is.EqualTo(ResponseStatus.Completed));
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.ErrorMessage, Is.Null);
        }

        [Test]
        public async Task ResponseHeaderForAListOfUsersIsValid()
        {
            var response = await _requestsProcessor.GetUsers();
            var contentTypeHeader = response.ContentHeaders?.FirstOrDefault(h => h.Name.Equals("Content-Type"));
            Assert.That(contentTypeHeader, Is.Not.Null);
            Assert.That(contentTypeHeader.Value, Is.EqualTo("application/json; charset=utf-8"));
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.ErrorMessage, Is.Null);
        }

        [Test]
        public async Task UsersAreValid()
        {
            var response = await _requestsProcessor.GetUsers();
            var users = _requestsProcessor.ConvertUsers(response.Content);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.ErrorMessage, Is.Null);
            Assert.That(users.Count, Is.EqualTo(10));
            Assert.That(users.Select(u => u.Id).ToArray(), Is.Unique);
            Assert.That(users.Where(u => string.IsNullOrEmpty(u.Name) || string.IsNullOrEmpty(u.Username)).ToArray(),
                Is.Empty);
            Assert.That(users.Where(u => u.Company == null || string.IsNullOrEmpty(u.Company.Name)), Is.Empty);
        }

        [Test]
        public async Task UserCanBeCreated()
        {
            var time = DateTime.UtcNow.ToString("yyyy-MM-dd_hh-mm-ss-fff");
            var user = new User() { Name = $"User {time}", Username = $"Username{time}" };
            var response = await _requestsProcessor.CreateUser(user);
            var createdUser = _requestsProcessor.ConvertUser(response.Content);

            Assert.That(response.ErrorMessage, Is.Null);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(response.Content, Is.Not.Null.Or.Empty);
            Assert.That(createdUser.Id, Is.Not.Null);
        }

        [Test]
        public async Task UserIsNotifiedIfResourceDoesNotExist()
        {
            var response = await _requestsProcessor.SendInvalidRequest();

            Assert.That(response.ErrorMessage, Is.Null);
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _requestsProcessor?.Dispose();
        }
    }
}
