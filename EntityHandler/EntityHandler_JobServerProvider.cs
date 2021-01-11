using CloudService_Data;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityHandler
{
    class EntityHandler_JobServerProvider : IEntityHandler
    {
        PhoneStoreDataRepository repository = new PhoneStoreDataRepository();
        public void AddOrReplacePhone(string id, string brand, string model, double price)
        {
            Phone phone = new Phone(id)
            {
                Brand = brand,
                Model = model,
                Price = price
            };
            repository.AddOrReplacePhone(phone);
        } //EDIT Phone

        public void AddOrReplaceStore(string id, string storeName, string storeAddress, string phoneModel, double phonePrice)
        {
            Store store = new Store(id)
            {
                StoreName = storeName,
                StoreAddress = storeAddress,
                PhoneModel = phoneModel,
                PhonePrice = phonePrice
            };
            repository.AddOrReplaceStore(store);
        } //EDIT Store

        public void AddPhone(string id, string brand, string model, double price)
        {
            Phone newPhone = new Phone(id)
            {
                Brand = brand,
                Model = model,
                Price = price
            };
            repository.AddPhone(newPhone);
        } // ADD Phone

        public void AddStore(string id, string storeName, string storeAddress, string phoneModel, double phonePrice)
        {
            Store newStore = new Store(id)
            {
                StoreName = storeName,
                StoreAddress = storeAddress,
                PhoneModel = phoneModel,
                PhonePrice = phonePrice
            };
            repository.AddStore(newStore);
        } // ADD Store

        public void DeletePhone(string phoneID)
        {
            repository.DeletePhone(phoneID);
        } // Delete Phone

        public void DeleteStore(string storeID)
        {
            repository.DeleteStore(storeID);
        } // Delete Store

        public Phone GetPhone(string phoneID)
        {
            return repository.GetPhone(phoneID);

        } // Get Phone
        public Store GetStore(string storeID)
        {
            return repository.GetStore(storeID);
        } // Get Store

        public bool IsAlive()
        {
            return true;
        }

        public IQueryable<Phone> RetrieveAllPhones() // Retreive All Phones
        {
            return repository.RetrieveAllPhones();
        }
        public IQueryable<Store> RetrieveAllStores() // Retreive All Stores
        {
            return repository.RetrieveAllStores();
        }
    }
}
