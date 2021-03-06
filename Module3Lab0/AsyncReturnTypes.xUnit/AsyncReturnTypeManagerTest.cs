using System;
using Xunit;
using AsyncReturnTypeLibrary;
using System.Threading.Tasks;

namespace AsyncReturnTypes.xUnit
{
    public class AsyncReturnTypeManagerTest
    {
        private AsyncReturnTypeManager _returnTypeMgr;

        public AsyncReturnTypeManagerTest()
        {
            _returnTypeMgr = new AsyncReturnTypeManager();
        }

        [Fact]
        public void Test1()
        {

        }

        [Fact]
        public void AsyncManagerCantPause6SecondsWithVoid()
        {
            var startTime = DateTime.UtcNow;
            _returnTypeMgr.PauseSixSecondsAsync();
            var diffInSeconds = (DateTime.UtcNow - startTime).TotalSeconds;
            Assert.True(diffInSeconds < 5, "The void method paused for five seconds or more");
        }
        
        [Fact]
        public async Task AsyncManagerCanCallWebService()
        {
            var startTime = DateTime.UtcNow;
            await _returnTypeMgr.CallWebServiceThatDoesNotReturnResults();
            var diffInSeconds = (DateTime.UtcNow - startTime).TotalSeconds;
            Assert.True(diffInSeconds >= 2, "Did not mine for at least two seconds");
        }

        [Fact]
        public async Task AsyncManagerCanCallWebServiceWithResult()
        {
            string resultString = await _returnTypeMgr.CallWebServiceThatReturnsString();
            Assert.True(resultString.Contains("AsyncCoin"), "Did not return correct results");
        }
        
        [Fact]
        public async Task AsyncManagerCanCaptureChangedCoinPrice()
        {
            var initialPrice = await _returnTypeMgr.GetCurrentCoinPrice();
            var cachedPrice = await _returnTypeMgr.GetCurrentCoinPrice();
            await Task.Delay(3500);
            var changedPrice = await _returnTypeMgr.GetCurrentCoinPrice();
            Assert.Equal(initialPrice, cachedPrice);
            Assert.NotEqual(cachedPrice, changedPrice);
        }

    }
}