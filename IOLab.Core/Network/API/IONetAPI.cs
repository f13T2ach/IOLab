/**
 * IOLab - A Full-Stack Experimentation Platform
 *
 * Copyright (C) 2024 Redmaple. All rights reserved.
 *
 * IONetAPI.cs is part of the IOLab project.
 *
 * IOLab is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * IOLab is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with IOLab. If not, see <https://www.gnu.org/licenses/>.
 */
namespace IOLab.Network.API
{
    public class IONetAPI
    {
        public static Dictionary<string, string> Servers = new Dictionary<string, string>
        {
            { "[*]测试服务器_广州主线1","http://139.9.177.204:9000" },
            { "IOLab Network Global","http://iolab.us.kg:9000" },
            { "[*]本地测试服务器","http://127.0.0.1:9000" },
        };

        public static string TestServerPerfix = "[*]";

        public static int delay = 5000;

        public static string AuthLogin = "/api/auth/login";

        public static string AuthRegist = "/api/auth/regist";

        public static string AuthLogout = "/api/auth/logout";

        public static string AuthFetchPrivately = "/api/auth/privateFetch";

        public static string AuthCheckIn = "/api/auth/checkin";

        public static string WorkshopUpload = "/api/workshop/upload";

        public static string WorkshopBrowse = "/api/workshop/browse";

        public static string WorkshopDownload = "/api/workshop/download/{item_id}";
    }
}
