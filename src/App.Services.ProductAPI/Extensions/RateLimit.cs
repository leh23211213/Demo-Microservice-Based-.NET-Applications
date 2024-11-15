using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace App.Services.ProductAPI.Extensions
{
    /// <summary>
    /// 4. Test Case: Đảm bảo rằng một IP cụ thể bị giới hạn theo đúng cấu hình
    ///Mục tiêu: Xác minh rate limiting áp dụng cho một địa chỉ IP cụ thể.
    ///Mô tả: Sử dụng cùng một IP và xác minh rằng nó bị giới hạn khi vượt quá số lượng requests quy định.
    /// </summary>
    public class RateLimit
    {
        [Fact]
        public async Task LimitsRequestsPerIp()
        {
            // Arrange
            var client = new HttpClient { BaseAddress = new Uri("http://localhost:5000") };
            int requestLimit = 10;

            // Act
            for (int i = 0; i < requestLimit + 1; i++)
            {
                var response = await client.GetAsync("/api/your-endpoint");

                if (i < requestLimit)
                {
                    Assert.True(response.IsSuccessStatusCode, $"Request #{i + 1} should be within limit.");
                }
                else
                {
                    Assert.Equal((int)HttpStatusCode.TooManyRequests, (int)response.StatusCode);
                }
            }
        }

        /// <summary>
        /// 3. Test Case: Đảm bảo rằng requests được reset sau khoảng thời gian quy định
        /// Mục tiêu: Xác minh rằng giới hạn được reset sau khoảng thời gian quy định.
        /// Mô tả: Gửi requests đạt giới hạn, đợi khoảng thời gian giới hạn trôi qua, sau đó thử lại để đảm bảo các requests tiếp theo được chấp nhận.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ResetsLimitAfterTimePeriod()
        {
            // Arrange
            var client = new HttpClient { BaseAddress = new Uri("http://localhost:5000") };
            int requestLimit = 10;
            string timePeriod = "10s"; // Giới hạn 10 requests mỗi 10 giây

            // Act
            for (int i = 0; i < requestLimit; i++)
            {
                var response = await client.GetAsync("/api/your-endpoint");
                Assert.True(response.IsSuccessStatusCode, $"Request #{i + 1} should be within limit.");
            }

            // Đợi thời gian reset giới hạn
            await Task.Delay(TimeSpan.Parse(timePeriod));

            // Sau khoảng thời gian giới hạn, các requests mới sẽ được chấp nhận lại
            var resetResponse = await client.GetAsync("/api/your-endpoint");

            // Assert
            Assert.True(resetResponse.IsSuccessStatusCode, "Request should succeed after rate limit reset.");
        }

        /// <summary>
        ///     2. Test Case: Xác minh rằng requests vượt quá giới hạn sẽ bị từ chối
        /// Mục tiêu: Đảm bảo rằng khi số lượng requests vượt quá giới hạn, các requests sau sẽ bị từ chối.
        /// Mô tả: Gửi số lượng requests nhiều hơn giới hạn và xác minh rằng các requests bị từ chối (HTTP status 429).
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task RejectsRequestsExceedingLimit()
        {
            // Arrange
            var client = new HttpClient { BaseAddress = new Uri("http://localhost:5000") };
            int requestLimit = 10; // Giới hạn requests, ví dụ là 10 requests trong 10 giây

            // Act
            for (int i = 0; i < requestLimit + 5; i++) // Gửi vượt quá giới hạn
            {
                var response = await client.GetAsync("/api/your-endpoint");

                if (i < requestLimit)
                {
                    // Assert rằng các requests trong giới hạn được chấp nhận
                    Assert.True(response.IsSuccessStatusCode, $"Request #{i + 1} should be within limit.");
                }
                else
                {
                    // Assert rằng các requests vượt quá giới hạn bị từ chối
                    Assert.Equal((int)HttpStatusCode.TooManyRequests, (int)response.StatusCode);
                }
            }
        }

        /// <summary>
        /// 1. Test Case: Xác minh rằng rate limiting cho phép số lượng requests trong giới hạn
        /// Mục tiêu: Đảm bảo rằng ứng dụng cho phép số lượng requests dưới hoặc bằng giới hạn quy định.
        /// Mô tả: Gửi requests với số lượng dưới giới hạn quy định và xác minh rằng tất cả các requests đều được xử lý thành công.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task AllowsRequestsWithinLimit()
        {
            // Arrange
            var client = new HttpClient { BaseAddress = new Uri("http://localhost:5000") };
            int allowedRequests = 10; // Giới hạn trong 10 giây, giả sử đã cấu hình giới hạn là 10 requests

            // Act
            for (int i = 0; i < allowedRequests; i++)
            {
                var response = await client.GetAsync("/api/your-endpoint");

                // Assert
                Assert.True(response.IsSuccessStatusCode, "Request should be successful within rate limit.");
            }
        }
    }
}