using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using API.Application.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using API.Application.DTOs;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Hosting;
using API.Application.Interfaces;
using API.Application.Helpers;

namespace API.Tests
{

    public class IntegrationTestVariables : IIntegrationTestVariables
    {
        public bool CurrentlyTesting()
        {
            return true;
        }
    }

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
            var appFactory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder => 
                {
                    builder.ConfigureServices(services => 
                    {
                        var context = services.FirstOrDefault(f => f.ServiceType == typeof(ApplicationContext));
                        services.Remove(context!);
                        var integrationTestService = services   
                            .FirstOrDefault(f => f.ServiceType == typeof(IIntegrationTestVariables));
                        services.Remove(integrationTestService!);

                        services.AddTransient<IIntegrationTestVariables, IntegrationTestVariables>();
                        services.AddDbContext<ApplicationContext>(options => 
                        {
                            options.UseInMemoryDatabase("TestDB");
                        });

                        var emailService = services   
                            .FirstOrDefault(f => f.ServiceType == typeof(IEmailService));
                        services.Remove(emailService!);

                        services.AddScoped<IEmailService, FakeEmailService>();
                    });
                });
            _client = appFactory.CreateClient();
        }

        protected async Task AuthenticateAsync()
        {
            _client.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", await GetTokenAsync());
        }

        private async Task<string> GetTokenAsync()
        {
            var response = await _client.PostAsJsonAsync("/api/account/login", new LoginDTO
            {
                Username = "lisa",
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