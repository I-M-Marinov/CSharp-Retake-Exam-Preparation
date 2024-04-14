namespace SmartDevice.Tests
{
    using NUnit.Framework;
    using System;
    using System.Text;

    public class Tests
    {
        private Device device;
        private int memoryCapacity = 1000;
        private int photos = 0;
        private int applicationsCount = 0;
        private int photoMemorySize = 200;
        private string appName = "na baba ti hvyrchiloto";
        private int appSize = 300;
        private object exception;

        [SetUp]
        public void Setup()
        {
            device = new Device(memoryCapacity);

        }

        [Test]
        public void Test_Constructor_Is_Saving_Correctly()
        {

            Assert.AreEqual(memoryCapacity, device.MemoryCapacity);
            Assert.AreEqual(memoryCapacity, device.AvailableMemory);
            Assert.AreEqual(photos, device.Photos);
            Assert.AreEqual(applicationsCount, device.Applications.Count);
        }


        [Test]
        public void Test_TakePhoto_Method_Is_Working_Correctly()
        {

            device.TakePhoto(photoMemorySize);


            Assert.AreEqual(memoryCapacity - photoMemorySize, device.AvailableMemory);
            Assert.AreEqual(1, device.Photos);
            Assert.AreEqual(applicationsCount, device.Applications.Count);
        }

        [Test]
        public void Test_TakePhoto_Method_Is_Returning_False()
        {

            var result = device.TakePhoto(photoMemorySize * 6);

            Assert.AreEqual(memoryCapacity, device.MemoryCapacity);
            Assert.AreEqual(memoryCapacity, device.AvailableMemory);
            Assert.AreEqual(photos, device.Photos);
            Assert.AreEqual(applicationsCount, device.Applications.Count);
            Assert.AreEqual(false, result);
        }

        [Test]
        public void Test_InstallApp_Method_Is_Working_Properly()
        {
            var result = device.InstallApp(appName, appSize);

            Assert.AreEqual(memoryCapacity - appSize, device.AvailableMemory);
            Assert.AreEqual(photos, device.Photos);
            Assert.AreEqual(1, device.Applications.Count);
            Assert.AreEqual($"{appName} is installed successfully. Run application?", result);
        }

        [Test]
        public void Test_InstallApp_Method_Is_Throwing_An_Exception()
        {

            Assert.Throws<InvalidOperationException>(() =>
            {
                device.InstallApp(appName, appSize * 6);
            });
        }

        [Test]
        public void Test_That_FormatDevice_Is_Works()
        {
            device.TakePhoto(200);
            device.InstallApp("chudo", 200);

            device.FormatDevice();

            Assert.AreEqual(memoryCapacity, device.MemoryCapacity);
            Assert.AreEqual(memoryCapacity, device.AvailableMemory);
            Assert.AreEqual(photos, device.Photos);
            Assert.AreEqual(applicationsCount, device.Applications.Count);

        }

        [Test]
        public void Test_That_GetDeviceStatus_Works()
        {
            device = new Device(2000);
            device.TakePhoto(200);
            device.InstallApp("somethingApp", 200);

            var result = device.GetDeviceStatus();

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Memory Capacity: {device.MemoryCapacity} MB, Available Memory: {device.AvailableMemory} MB");
            sb.AppendLine($"Photos Count: {device.Photos}");
            sb.AppendLine($"Applications Installed: {string.Join(", ", device.Applications)}");

            Assert.AreEqual(sb.ToString().Trim(), result);
        }


    }
}