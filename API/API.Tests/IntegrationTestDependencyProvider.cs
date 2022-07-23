using System.Net.Http.Headers;
using API.Application.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using API.Application.DTOs;
using System.Net.Http.Json;
using API.Application.Interfaces;
using API.Application.Helpers;
using API.Application.Services;

namespace API.Tests
{
    public class FakeEmailService : IEmailService
    {
        public Task<bool> SendEmail(EmailMessage message)
        {
            return Task.FromResult(true);
        }
    }

    public class IntegrationTestDependencyProvider
    {
        protected readonly HttpClient _client;

        public IntegrationTestDependencyProvider()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "TESTING");
            var appFactory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder => 
                {
                    builder.ConfigureServices(services => 
                    {
                        var emailService = services   
                            .FirstOrDefault(f => f.ServiceType == typeof(IEmailService));
                        services.Remove(emailService!);
                        services.AddScoped<IEmailService, FakeEmailService>();
                    });
                });
            _client = appFactory.CreateClient();
        }

        protected async Task AuthenticateAsync(string username)
        {
            _client.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", await GetTokenAsync(username));
        }

        private async Task<string> GetTokenAsync(string username)
        {
            var response = await _client.PostAsJsonAsync("/api/account/login", new LoginDTO
            {
                Username = username,
                Password = "pass",
            });

            var loginResponse = await response.Content.ReadFromJsonAsync<UserDTO>();
            return loginResponse!.Token!;
        }

        protected async Task _output(string text)
        {
            using (FileStream fs = new("..\\..\\..\\output.txt", FileMode.Append, FileAccess.Write))
            using (StreamWriter textWriter = new(fs))
            {
                await textWriter.WriteLineAsync(text);
            }
        }
    }
}