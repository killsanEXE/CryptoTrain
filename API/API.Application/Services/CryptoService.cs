using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Application.Helpers;
using API.Application.Interfaces;

namespace API.Application.Services
{
    public class CryptoService : BackgroundService
    {
        readonly IServiceProvider _serviceProvider;
        readonly HttpClient client = new();
        readonly string _token;

        public CryptoService(string token, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _token = token;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                using(var scope = _serviceProvider.CreateScope())
                {
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    string url = $"https://min-api.cryptocompare.com/data/price?fsym=BTC&tsyms=USD&api_key={_token}";
                    var response = await client.GetAsync(url);
                    var result = await response.Content.ReadFromJsonAsync<CryptoCompareRequestModel>();
                    if(result?.USD != null)
                    {
                        unitOfWork.CryptoRepository.AddNewBTC(new()
                        {
                            Price = result.USD,
                            Date = DateTime.Now
                        });

                        await unitOfWork.Complete();
                    }
                    await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
                }
            }
        }
    }
}