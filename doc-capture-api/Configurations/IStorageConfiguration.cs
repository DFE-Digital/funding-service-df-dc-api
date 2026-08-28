using System;
namespace DocCapture.API.Configurations
{
    public interface IStorageConfiguration
    {
        public string ConnectionString { get; set; }
        public string Containers  { get; set; }
    }
}

