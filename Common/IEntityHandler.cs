using CloudService_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IEntityHandler
    {
        [OperationContract]
        IQueryable<Phone> RetrieveAllPhones();

        [OperationContract]
        void AddPhone(string id, string brand, string model, double price);

        [OperationContract]
        void DeletePhone(string phoneID);

        [OperationContract]
        Phone GetPhone(string phoneID);

        [OperationContract]
        void AddOrReplacePhone(string id, string brand, string model, double price);

        [OperationContract]
        IQueryable<Store> RetrieveAllStores();

        [OperationContract]
        void AddStore(string id, string storeName, string storeAddress, string phoneModel, double phonePrice);

        [OperationContract]
        void DeleteStore(string storeID);

        [OperationContract]
        Store GetStore(string storeID);

        [OperationContract]
        void AddOrReplaceStore(string id, string storeName, string storeAddress, string phoneModel, double phonePrice);
        [OperationContract]
        bool IsAlive();
    }
}
