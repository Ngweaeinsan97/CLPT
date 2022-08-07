using Internal.Services.Movements.Data.Contexts;
using Internal.Services.Movements.Data.Models;
using Internal.Services.Movements.Data.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Internal.Services.Movements.IntegrationTests.Utilities
{
    public class DbHelper
	{
		private readonly MovementsDataContext _movementsDb;

		public DbHelper(MovementsDataContext movementsDb)
		{
			_movementsDb = movementsDb;
		}

		public void InitializeDbForTests()
		{
			var customerAccount = new Customer
			{
				CustomerId = 11111,
				CustomerFirstName = "John",
				CustomerLastName = "Wick",
				CustomerEmail = "john@gmail.com"
			};
			var fisicalAccount = new Customer
			{
				CustomerId = 22222,
				CustomerFirstName = "Jack",
				CustomerLastName = "Son",
				CustomerEmail = "jackson@gmail.com"
			};
			var interestAccount = new Customer
			{
				CustomerId = 33333,
				CustomerFirstName = "Jame",
				CustomerLastName = "Wilson",
				CustomerEmail = "jamewilson@gmail.com"
			};
			var fiscalProduct = new Product
			{
				ProductId = 2,
				ProductType = EnumProductType.SavingsRetirement,
				ExternalAccount = AccountHelper.FiscalTransferAccount
			};
			var incomingProduct = new Product
			{
				ProductId = 1,
				ProductType = EnumProductType.Unknown,
				ExternalAccount = AccountHelper.CustomerAccount
			};
			var interestProduct = new Product
			{
				ProductId = 3,
				ProductType = EnumProductType.Unknown,
				ExternalAccount = AccountHelper.InterestAccount
			};

			MockIncomingData();
			MockFiscalTransferData();
			MockInterestData();

			_movementsDb.Customers.Add(customerAccount);
			_movementsDb.Customers.Add(fisicalAccount);
			_movementsDb.Customers.Add(interestAccount);
			_movementsDb.Products.Add(fiscalProduct);
			_movementsDb.Products.Add(interestProduct);
			_movementsDb.Products.Add(incomingProduct);
			_movementsDb.SaveChanges();
		}

		public void MockIncomingData()
		{
			var incoming = new ProductCustomer
			{
				ProductCustomerId = 1,
				CustomerId = 11111,
				ProductId = 1
			};
			_movementsDb.ProductsCustomers.Add(incoming);
			_movementsDb.SaveChanges();
		}

		public void MockInterestData()
		{
			var interest = new ProductCustomer
			{
				ProductCustomerId = 3,
				CustomerId = 33333,
				ProductId = 3
			};
			_movementsDb.ProductsCustomers.Add(interest);
			_movementsDb.SaveChanges();
		}

		public void MockFiscalTransferData()
		{
			var fiscal = new ProductCustomer
			{
				ProductCustomerId = 2,
				CustomerId = 22222,
				ProductId = 2
			};
			_movementsDb.ProductsCustomers.Add(fiscal);
			_movementsDb.SaveChanges();
		}

	}
}
