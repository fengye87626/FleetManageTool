using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.Collections.Specialized;
using Microsoft.WindowsAzure.StorageClient.Protocol;

namespace FleetManageToolWebRole.Util
{
    public class UploadFileToStorage
    {
        public static void SaveFile(string fileName, string contentType, byte[] data)
        {
            //获得BlobContainer对象并把文件上传到这个Container
            var blob = GetContainer().GetBlobReference(fileName);
            blob.Properties.ContentType = contentType;

            // 创建元数据信息
            var metadata = new NameValueCollection();
            metadata["Name"] = fileName;

            // 上传图片
            blob.Metadata.Add(metadata);
            blob.UploadByteArray(data);
        }

        public static void  EnsureContainerExists()
        {
            var container = GetContainer();
            // 检查container是否被创建，如果没有，创建container
            container.CreateIfNotExist();
            var permissions = container.GetPermissions();
            //对Storage的访问权限是可以浏览Container
            permissions.PublicAccess = BlobContainerPublicAccessType.Container;

            container.SetPermissions(permissions);
        }

        public static CloudBlobContainer GetContainer()
        {
            //获取ServiceConfiguration.cscfg配置文件的信息
            var account = CloudStorageAccount.FromConfigurationSetting("DataConnectionString");
            var client = account.CreateCloudBlobClient();
            //获得BlobContainer对象
            return client.GetContainerReference(RoleEnvironment.GetConfigurationSettingValue("ContainerName"));
        }
    }
}