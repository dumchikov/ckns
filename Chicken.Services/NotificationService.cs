using System.Linq;
using Chicken.Domain.Interfaces;
using Chicken.Domain.Models;
using PushSharp;
using PushSharp.Android;

namespace Chicken.Services
{
    public class NotificationService
    {
        private readonly IRepository<Device> _devices;

        public NotificationService(IRepository<Device> devices)
        {
            _devices = devices;
        }

        public void AddDevice(string registrationId)
        {
            var device = new Device
                {
                    RegistrationId = registrationId
                };
            _devices.Add(device);
            _devices.Save();
        }

        public void Notify(int newPostsCount)
        {
            var devices = _devices.Query().ToList();

            var push = new PushBroker();
            push.RegisterGcmService(new GcmPushChannelSettings("AIzaSyBwZSF2O70VxltPsZSDjkqTYn-JpkccPi0"));

            foreach (var device in devices)
            {
                push
                    .QueueNotification(new GcmNotification().ForDeviceRegistrationId(device.RegistrationId)
                    .WithJson(string.Format("{{\"newPostsCount\":{0}}}", newPostsCount)));
            }
        }
    }
}
