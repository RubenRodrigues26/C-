using System;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;

namespace AsyncReturnTypeLibrary
{
    public class AsyncReturnTypeManager
    {     
        public async void PauseSixSecondsAsync()
        {
            await Task.Delay(6000);
        }

        public async Task CallWebServiceThatDoesNotReturnResults()
        {
            var uri = new Uri($"https://asynccoinfunction.azurewebsites.net/api/asynccoin/3");
            var client = new HttpClient();
            await client.GetAsync(uri);
            return;
        }

        public async Task<string> CallWebServiceThatReturnsString()
        {
            var uri = new Uri($"https://asynccoinfunction.azurewebsites.net/api/asynccoin/3");
            var client = new HttpClient();
            Task<string> webTask = client.GetStringAsync(uri);
            string result = await webTask;
            return result;
        }

        private DateTime LastCacheRefreshUtc { get; set; }

        private double CachePrice {get;set;}

        public ValueTask<double> GetCurrentCoinPrice()
        {
            if(DateTime.UtcNow < LastCacheRefreshUtc.AddSeconds(3))
            {
                return new ValueTask<double>(CachePrice);
            }
            return new ValueTask<double>(PollSeveralServersForCoinPrice());
        }

        public async Task<double> PollSeveralServersForCoinPrice()
        {
            await Task.Delay(2000);
            var coinPrice = new Random().NextDouble() * (250 - 50) + 50;
            if (coinPrice == CachePrice)
            {
                coinPrice += 2;
            }
            CachePrice = coinPrice;
            LastCacheRefreshUtc = DateTime.UtcNow;
            return coinPrice;
        }

    }

}