/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
namespace WiiTrakApi.DTOs
{
    public class TrackingTrackSolidDto
    {
        public static string RequestUrl { get; set; } = "https://us-open.tracksolidpro.com/route/rest";
        public string timestamp { get; set; } = DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:sss");//UTC Time (Format 2022-05-20 05:57:003)
        public string app_key { get; set; } = "8FB345B8693CCD0090F9BB773BFECBEF";
        public string sign { get; set; } = "15F71EB259DBA10498405C0DBD0390C8";
        public string sign_method { get; set; } = "md5";
        public string v { get; set; } = "0.9";//Default version 1.0
        public string format { get; set; } = "json";
        public string user_id { get; set; } = "fritz test";
        public string user_pwd_md5 { get; set; } = "f8824b734b7c7255659d64375e8f3146";
        public string expires_in { get; set; } = "7200";
             
    }
}
