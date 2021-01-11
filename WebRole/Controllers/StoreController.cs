using CloudService_Data;
using Common;
using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Mvc;

namespace WebRole.Controllers
{
    public class StoreController : Controller
    {
        public static List<EndpointAddress> RetreiveAllInstances()
        {
            string internalEndpointName = "InternalRequest";
            List<EndpointAddress> internalEndpoints = new List<EndpointAddress>();
            foreach (RoleInstance instance in RoleEnvironment.Roles["EntityHandler"].Instances)
            {
                internalEndpoints.Add(new EndpointAddress($"net.tcp://{instance.InstanceEndpoints[internalEndpointName].IPEndpoint.ToString()}/{internalEndpointName}"));
            }

            return internalEndpoints;
        }
        // GET: Store
        public ActionResult Index()
        {
            List<EndpointAddress> internalEndpoints = RetreiveAllInstances();
            IEntityHandler proxy = new ChannelFactory<IEntityHandler>(new NetTcpBinding(), internalEndpoints[1]).CreateChannel();

            List<Store> stores = proxy.RetrieveAllStores().ToList<Store>();

            return View(stores);
        }

        // Get Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Store Store)
        {
            List<EndpointAddress> internalEndpoints = RetreiveAllInstances();
            IEntityHandler proxy = new ChannelFactory<IEntityHandler>(new NetTcpBinding(), internalEndpoints[1]).CreateChannel();

            proxy.AddStore(Store.RowKey, Store.StoreName, Store.StoreAddress, Store.PhoneModel, Store.PhonePrice);

            return RedirectToAction("Index");
        }

        // Get Edit
        public ActionResult Edit(string id)
        {
            List<EndpointAddress> internalEndpoints = RetreiveAllInstances();
            IEntityHandler proxy = new ChannelFactory<IEntityHandler>(new NetTcpBinding(), internalEndpoints[1]).CreateChannel();

            Store store = proxy.GetStore(id);

            return View(store);
        }

        [HttpPost]
        public ActionResult Edit(Store store)
        {
            List<EndpointAddress> internalEndpoints = RetreiveAllInstances();
            IEntityHandler proxy = new ChannelFactory<IEntityHandler>(new NetTcpBinding(), internalEndpoints[1]).CreateChannel();

            proxy.AddOrReplaceStore(store.RowKey, store.StoreName, store.StoreAddress, store.PhoneModel, store.PhonePrice);

            return RedirectToAction("Index");
        }

        //Get Delete?id
        public ActionResult Delete(string id)
        {
            List<EndpointAddress> internalEndpoints = RetreiveAllInstances();
            IEntityHandler proxy = new ChannelFactory<IEntityHandler>(new NetTcpBinding(), internalEndpoints[0]).CreateChannel();

            proxy.DeleteStore(id);
            List<Store> stores = proxy.RetrieveAllStores().ToList<Store>();

            return View("Index", stores);
        }
    }
}