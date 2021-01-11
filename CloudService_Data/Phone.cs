using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudService_Data
{
    public class Phone : TableEntity
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public double Price { get; set; }

        public Phone(string phoneNo)
        {
            PartitionKey = "Phone";
            RowKey = phoneNo;
        }

        public Phone() { }
    }
}
