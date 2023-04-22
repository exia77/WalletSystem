using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace WalletSystemTest
{
    public class ActualDatabaseTest
    {
        private readonly HttpClient _client;

        public ActualDatabaseTest()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("https://localhost:5001/");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6InRlc3RVc2VyMSIsImV4cCI6MTY4Mjc2Njg1NCwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzIzMyIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjcyMzMifQ.U3ZBhip3BL9xCFMWZo1QnpAZsZErcN811n9iru7aJpQ");
        }
        [Fact]
        public async Task Withdraw_ReturnsBadRequest_WhenNotEnoughBalance()
        {
            var credentials = new
            {
                AccountNumber = "542502500676",
                Amount = 100.00
            };

            string json = JsonSerializer.Serialize(credentials);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync("Account/Withdraw", content);

            Assert.False(response.IsSuccessStatusCode);

            string responseBody = await response.Content.ReadAsStringAsync();
            Assert.Contains("Not enough balance.", responseBody);

        }
        [Fact]
        public async Task Withdraw_ReturnsBadRequest_WhenAccountNumberIsNotValid()
        {
            var credentials = new
            {
                AccountNumber = "5425025006767777",
                Amount = 100.00
            };

            string json = JsonSerializer.Serialize(credentials);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync("Account/Withdraw", content);

            Assert.False(response.IsSuccessStatusCode);

            string responseBody = await response.Content.ReadAsStringAsync();
            Assert.Contains("Invalid account number.", responseBody);

        }

        [Fact]
        public async Task Transfer_ReturnsBadRequest_WhenNotEnoughBalance()
        {
            var credentials = new
            {
                AccountNumber = "542502500676",
                AccountNumberTo = "794540851215",
                Amount = 100.00
            };

            string json = JsonSerializer.Serialize(credentials);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync("Account/Transfer", content);

            Assert.False(response.IsSuccessStatusCode);

            string responseBody = await response.Content.ReadAsStringAsync();
            Assert.Contains("Not enough balance.", responseBody);

        }

        [Fact]
        public async Task Transfer_ReturnsBadRequest_WhenAccountNumberIsNotValid()
        {
            var credentials = new
            {
                AccountNumber = "5425025006767777",
                AccountNumberTo = "794540851215",
                Amount = 100.00
            };

            string json = JsonSerializer.Serialize(credentials);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync("Account/Transfer", content);

            Assert.False(response.IsSuccessStatusCode);

            string responseBody = await response.Content.ReadAsStringAsync();
            Assert.Contains("Invalid account number.", responseBody);

        }
        [Fact]
        public async Task Transfer_ReturnsBadRequest_WhenAccountNumberToIsNotValid()
        {
            var credentials = new
            {
                AccountNumber = "542502500676",
                AccountNumberTo = "7945408512157777",
                Amount = 100.00
            };

            string json = JsonSerializer.Serialize(credentials);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync("Account/Transfer", content);

            Assert.False(response.IsSuccessStatusCode);

            string responseBody = await response.Content.ReadAsStringAsync();
            Assert.Contains("Invalid account number to.", responseBody);

        }

    }
}
