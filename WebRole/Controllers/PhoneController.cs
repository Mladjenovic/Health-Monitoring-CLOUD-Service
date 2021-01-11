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
    public class PhoneController : Controller
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

        // GET: Phone
        public ActionResult Index()
        {
            List<EndpointAddress> internalEndpoints = RetreiveAllInstances();
            IEntityHandler proxy = new ChannelFactory<IEntityHandler>(new NetTcpBinding(), internalEndpoints[0]).CreateChannel();

            List<Phone> phones = proxy.RetrieveAllPhones().ToList<Phone>();

            return View(phones);
        }

        // Get Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Phone phone)
        {
            List<EndpointAddress> internalEndpoints = RetreiveAllInstances();
            IEntityHandler proxy = new ChannelFactory<IEntityHandler>(new NetTcpBinding(), internalEndpoints[0]).CreateChannel();

            proxy.AddPhone(phone.RowKey, phone.Brand, phone.Model, phone.Price);

            return RedirectToAction("Index");
        }

        // Get Edit
        public ActionResult Edit(string id)
        {
            List<EndpointAddress> internalEndpoints = RetreiveAllInstances();
            IEntityHandler proxy = new ChannelFactory<IEntityHandler>(new NetTcpBinding(), internalEndpoints[0]).CreateChannel();

            Phone phone = proxy.GetPhone(id);

            return View(phone);
        }

        [HttpPost]
        public ActionResult Edit(Phone phone)
        {
            List<EndpointAddress> internalEndpoints = RetreiveAllInstances();
            IEntityHandler proxy = new ChannelFactory<IEntityHandler>(new NetTcpBinding(), internalEndpoints[0]).CreateChannel();

            proxy.AddOrReplacePhone(phone.RowKey, phone.Brand, phone.Model, phone.Price);

            return RedirectToAction("Index");
        }


        //Get Delete
        public ActionResult Delete(string id)
        {
            List<EndpointAddress> internalEndpoints = RetreiveAllInstances();
            IEntityHandler proxy = new ChannelFactory<IEntityHandler>(new NetTcpBinding(), internalEndpoints[0]).CreateChannel();

            proxy.DeletePhone(id);
            List<Phone> phones = proxy.RetrieveAllPhones().ToList<Phone>();

            return View("Index", phones);
        }
    }
}