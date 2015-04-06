using Chicken.Domain.Interfaces;
using Chicken.Domain.Models;

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
    }
}
