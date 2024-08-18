namespace IOLab.Core.Assets
{
    public class FolderNames
    {
        public static string Slash = @"\";
        public static string AssetsFolder = "Assets";
        public static string AssetsServicesLocalFile = "local_services.json";
        public static string AssetsExperimentLocalFolder = "LocalExperiment";
        public static string AssetsExperimentNetworkFolder = "NetworkExperiment";

        // 尽量不要使用网络服务做采集器
        public static string AssetsServicesNetworkFolder = "NetworkServices";

        public static string RecordFolder = "Record";
    }
}
