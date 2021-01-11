using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudService_Data
{
    public class Store : TableEntity
    {
        public string StoreName { get; set; }
        public string StoreAddress { get; set; }
        public string PhoneModel { get; set; }
        public double PhonePrice { get; set; }

        public Store(string storeNO)
        {
            PartitionKey = "Store";
            RowKey = storeNO;
        }
        public Store() { }

    }
}
