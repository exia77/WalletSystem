using BusinessLayer;
using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using WalletSystem.Controllers;

namespace WalletSystemTest
{
    public class AccountControllerTest
    {
        private readonly AccountController _controller;
        private readonly Mock<IAccounts> _mockAccounts;

        public AccountControllerTest()
        {
            _mockAccounts = new Mock<IAccounts>();
            _controller = new AccountController(_mockAccounts.Object);
        }

        //Deposit
        [Fact]
        public async Task Deposit_ReturnsSuccess_WhenDepositSuccessful()
        {
            var controller = new AccountController(_mockAccounts.Object);
            var accountObjects = new AccountsModelObject
            {
                AccountNumber = "123456789012",
                Balance = 100
            };

            _mockAccounts.Setup(x => x.UpdateBalance(accountObjects)).ReturnsAsync(accountObjects);

            var result = await controller.Deposit(accountObjects);
            Assert.IsType<OkObjectResult>(result);

            var responseContent = result is ObjectResult objectResult ? objectResult.Value!.ToString() : "";

            Assert.Contains("successful", responseContent);
        }

        [Fact]
        public async Task Deposit_ReturnsBadRequest_WhenAccountNumberIsMissing()
        {
            var controller = new AccountController(_mockAccounts.Object);
            var accountObjects = new AccountsModelObject
            {
                AccountNumber = "",
                Balance = 100
            };

            var result = await controller.Deposit(accountObjects);
            Assert.IsType<BadRequestObjectResult>(result);

            var badRequestResult = result as BadRequestObjectResult;
            Assert.Equal("Account number is required.", badRequestResult!.Value!.GetType().GetProperty("Message")!.GetValue(badRequestResult.Value));
        }

        [Fact]
        public async Task Deposit_ReturnsBadRequest_WhenAmountIsMissing()
        {
            var controller = new AccountController(_mockAccounts.Object);
            var accountObjects = new AccountsModelObject
            {
                AccountNumber = "123",
                Balance = 0
            };

            var result = await controller.Deposit(accountObjects);
            Assert.IsType<BadRequestObjectResult>(result);

            var badRequestResult = result as BadRequestObjectResult;
            Assert.Equal("Amount is required.", badRequestResult!.Value!.GetType().GetProperty("Message")!.GetValue(badRequestResult.Value));
        }


        //Withdraw
        [Fact]
        public async Task Withdraw_ReturnsSuccess_WhenWithdrawSuccessful()
        {
            var controller = new AccountController(_mockAccounts.Object);
            var accountObjects = new AccountsModelObject
            {
                AccountNumber = "123456789012",
                Balance = 100,
            };

            _mockAccounts.Setup(x => x.UpdateBalance(accountObjects)).ReturnsAsync(accountObjects);

            var result = await controller.Withdraw(accountObjects);
            Assert.IsType<OkObjectResult>(result);

            var responseContent = result is ObjectResult objectResult ? objectResult.Value!.ToString() : "";

            Assert.Contains("successful", responseContent);
        }

        [Fact]
        public async Task Withdraw_ReturnsBadRequest_WhenAccountNumberIsMissing()
        {
            var controller = new AccountController(_mockAccounts.Object);
            var accountObjects = new AccountsModelObject
            {
                AccountNumber = "",
                Balance = 100
            };

            var result = await controller.Withdraw(accountObjects);
            Assert.IsType<BadRequestObjectResult>(result);

            var badRequestResult = result as BadRequestObjectResult;
            Assert.Equal("Account number is required.", badRequestResult!.Value!.GetType().GetProperty("Message")!.GetValue(badRequestResult.Value));
        }

        [Fact]
        public async Task Withdraw_ReturnsBadRequest_WhenAmountIsMissing()
        {
            var controller = new AccountController(_mockAccounts.Object);
            var accountObjects = new AccountsModelObject
            {
                AccountNumber = "123",
                Balance = 0
            };

            var result = await controller.Withdraw(accountObjects);
            Assert.IsType<BadRequestObjectResult>(result);

            var badRequestResult = result as BadRequestObjectResult;
            Assert.Equal("Amount is required.", badRequestResult!.Value!.GetType().GetProperty("Message")!.GetValue(badRequestResult.Value));
        }

        //Transfer

        [Fact]
        public async Task Transfer_ReturnsSuccess_WhenTransferSuccessful()
        {
            var controller = new AccountController(_mockAccounts.Object);
            var accountObjects = new AccountsModelObject
            {
                AccountNumber = "123456789012",
                AccountNumberTo = "12345",
                Balance = 100,
            };

            _mockAccounts.Setup(x => x.UpdateBalance(accountObjects)).ReturnsAsync(accountObjects);

            var result = await controller.Transfer(accountObjects);
            Assert.IsType<OkObjectResult>(result);

            var responseContent = result is ObjectResult objectResult ? objectResult.Value!.ToString() : "";

            Assert.Contains("successful", responseContent);
        }

        [Fact]
        public async Task Transfer_ReturnsBadRequest_WhenAccountNumberIsMissing()
        {
            var controller = new AccountController(_mockAccounts.Object);
            var accountObjects = new AccountsModelObject
            {
                AccountNumber = "",
                AccountNumberTo = "12345",
                Balance = 100
            };

            var result = await controller.Transfer(accountObjects);
            Assert.IsType<BadRequestObjectResult>(result);

            var badRequestResult = result as BadRequestObjectResult;
            Assert.Equal("Account number is required.", badRequestResult!.Value!.GetType().GetProperty("Message")!.GetValue(badRequestResult.Value));
        }

        [Fact]
        public async Task Transfer_ReturnsBadRequest_WhenAccountNumberToIsMissing()
        {
            var controller = new AccountController(_mockAccounts.Object);
            var accountObjects = new AccountsModelObject
            {
                AccountNumber = "12345",
                AccountNumberTo = "",
                Balance = 100
            };

            var result = await controller.Transfer(accountObjects);
            Assert.IsType<BadRequestObjectResult>(result);

            var badRequestResult = result as BadRequestObjectResult;
            Assert.Equal("Account number to is required.", badRequestResult!.Value!.GetType().GetProperty("Message")!.GetValue(badRequestResult.Value));
        }

        [Fact]
        public async Task Transfer_ReturnsBadRequest_WhenAmountIsMissing()
        {
            var controller = new AccountController(_mockAccounts.Object);
            var accountObjects = new AccountsModelObject
            {
                AccountNumber = "123",
                AccountNumberTo = "12345",
                Balance = 0
            };

            var result = await controller.Transfer(accountObjects);
            Assert.IsType<BadRequestObjectResult>(result);

            var badRequestResult = result as BadRequestObjectResult;
            Assert.Equal("Amount is required.", badRequestResult!.Value!.GetType().GetProperty("Message")!.GetValue(badRequestResult.Value));
        }

        //GetTransactionHistory

        [Fact]
        public async Task Account_ReturnSuccess_WhenGetTransactionHistory()
        {
            var controller = new AccountController(_mockAccounts.Object);
            var accountObjects = new AccountsModelObject
            {
                AccountNumber = "123456789012"
            };
            var transactionHistoryList = new List<TransactionHistoryModelObjects>();

            var transactionModelObject = new TransactionHistoryModelObjects
            {
                TransactionType = "Deposit",
                Amount = 1000.00M,
                AccountNumberFrom = "123456789012",
                AccountNumberTo = "12345",
                TransactionDate = DateTime.Now,
                EndBalance = 0.00M

            };
            transactionHistoryList.Add(transactionModelObject);

            _mockAccounts.Setup(x => x.GetTransactionHistory(accountObjects)).ReturnsAsync(transactionHistoryList);

            var result = await controller.GetTransactionHistory(accountObjects);
            Assert.IsType<OkObjectResult>(result);
        }
    }
}
