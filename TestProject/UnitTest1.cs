using BusinessLogicLayer.DTO;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Text;

namespace TestProject
{
    public class UnitTest1: IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _httpClient;

        public UnitTest1(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _httpClient = _factory.CreateClient();
        }

        [Fact]
        public async Task AddNewUser_ReturnsCreatedSuccess()
        {
            // Arrange
            var userDTO = new NewUserDTO { Email = "test@yahoo.com", FirstName = "John", LastName = "Smith", MarketingConsent = true };
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(userDTO), Encoding.UTF8, "application/json");

            // Act
            var response = await _httpClient.PostAsync("/User", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            var item = System.Text.Json.JsonSerializer.Deserialize<CreateUserResponseDTO>(responseContent, new System.Text.Json.JsonSerializerOptions(System.Text.Json.JsonSerializerDefaults.Web));

            //Assert
            Assert.NotNull(item.Id);
            Assert.NotNull(item.AccessToken);

        }
    }
}