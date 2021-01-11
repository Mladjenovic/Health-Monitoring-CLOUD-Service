
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudService_Data
{
    public class PhoneStoreDataRepository
    {
        private CloudStorageAccount _storageAccount;
        private CloudTable _table;

        public PhoneStoreDataRepository()
        {
            _storageAccount =
            CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("DataConnectionString"));
            CloudTableClient tableClient = new CloudTableClient(new
            Uri(_storageAccount.TableEndpoint.AbsoluteUri), _storageAccount.Credentials);
            _table = tableClient.GetTableReference("PhoneStoreTable");
            _table.CreateIfNotExists();
        }

        #region Phone Operations
        public IQueryable<Phone> RetrieveAllPhones()
        {
            var results = from g in _table.CreateQuery<Phone>()
                          where g.PartitionKey == "Phone"
                          select g;
            return results;
        }
        public void AddPhone(Phone newPhone)
        {
            TableOperation insertOperation = TableOperation.InsertOrReplace(newPhone);
            _table.Execute(insertOperation);
        }
        public bool DeletePhone(string phoneID)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<Phone>("Phone", phoneID);
            TableResult retrievedResult = _table.Execute(retrieveOperation);
            Phone delete = (Phone)retrievedResult.Result;
            if(delete != null)
            {
                TableOperation deleteOperation = TableOperation.Delete(delete);
                _table.Execute(deleteOperation);
                return true;
            }
            return false;
        }
        public Phone GetPhone(string phoneID)
        {
            var results = from g in _table.CreateQuery<Phone>()
                          where g.PartitionKey == "Phone" && g.RowKey == phoneID
                          select g;
            return results.ToArray()[0];
        }
        public void AddOrReplacePhone(Phone newPhone) //This will be the edit CRUD operation, later..
        {
            // Samostalni rad: izmestiti tableName u konfiguraciju servisa.
            TableOperation insertOperation = TableOperation.InsertOrReplace(newPhone);
            _table.Execute(insertOperation);
        }
        #endregion

        #region Store Operations
        public IQueryable<Store> RetrieveAllStores()
        {
            var results = from g in _table.CreateQuery<Store>()
                          where g.PartitionKey == "Store"
                          select g;
            return results;
        }
        public void AddStore(Store newStore)
        {
            TableOperation insertOperation = TableOperation.InsertOrReplace(newStore);
            _table.Execute(insertOperation);
        }
        public bool DeleteStore(string storeID)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<Store>("Store", storeID);
            TableResult retrievedResult = _table.Execute(retrieveOperation);
            Store delete = (Store)retrievedResult.Result;
            if (delete != null)
            {
                TableOperation deleteOperation = TableOperation.Delete(delete);
                _table.Execute(deleteOperation);
                return true;
            }
            return false;
        }
        public Store GetStore(string storeID)
        {
            var results = from g in _table.CreateQuery<Store>()
                          where g.PartitionKey == "Store" && g.RowKey == storeID
                          select g;
            return results.ToArray()[0];
        }
        public void AddOrReplaceStore(Store newStore) //This will be the edit CRUD operation, later..
        {
            // Samostalni rad: izmestiti tableName u konfiguraciju servisa.
            TableOperation insertOperation = TableOperation.InsertOrReplace(newStore);
            _table.Execute(insertOperation);
        }
        #endregion

    }
}
