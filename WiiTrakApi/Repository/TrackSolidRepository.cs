/*
* 06.06.2022
* Copyright (c) 2022 WiiTrak, All Rights Reserved.
*/
using WiiTrakApi.Repository.Contracts;
using WiiTrakApi.DTOs;
using WiiTrakApi.Models;
using WiiTrakApi.SPModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Data.SqlClient;
using WiiTrakApi.Data;
using Microsoft.EntityFrameworkCore;

namespace WiiTrakApi.Repository
{
    public class TrackSolidRepository : ITrackSolidRepository
    {
        private readonly ApplicationDbContext DbContext;

        public TrackSolidRepository(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }
        public async Task<(bool IsSuccess, string? ErrorMessage)> GetDataFromTrackSolidAsync()
        {
            try
            {
                var AccessToken = await GetAccessToken();

                if (!string.IsNullOrEmpty(AccessToken))
                {
                    var devicelist = await GetDeviceList(AccessToken);
                    if (devicelist != null)
                    {
                        foreach (var item in devicelist)
                        {
                            await GetDeviceLocation(AccessToken, item);
                        }
                    }
                }

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        #region GetAccessToken
        private async Task<string> GetAccessToken()
        {
            var AccessToken = string.Empty;
            try
            {
                var date = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss").Replace('.', ':');

                TrackingTrackSolidDto trackingTrackSolidDto = new TrackingTrackSolidDto();
                var tokenparameters = new Dictionary<string, string>();

                tokenparameters.Add("method", "jimi.oauth.token.get");
                tokenparameters.Add("timestamp", date);
                tokenparameters.Add("app_key", trackingTrackSolidDto.app_key);
                tokenparameters.Add("sign", trackingTrackSolidDto.sign);
                tokenparameters.Add("sign_method", trackingTrackSolidDto.sign_method);
                tokenparameters.Add("v", trackingTrackSolidDto.v);
                tokenparameters.Add("format", trackingTrackSolidDto.format);
                tokenparameters.Add("user_id", trackingTrackSolidDto.user_id);
                tokenparameters.Add("user_pwd_md5", trackingTrackSolidDto.user_pwd_md5);
                tokenparameters.Add("expires_in", trackingTrackSolidDto.expires_in);

                var tokenresponse = await GetResultObject(tokenparameters);
                if (tokenresponse != null)
                {
                    AccessToken = tokenresponse["result"]["accessToken"].ToString();
                }
            }
            catch
            {
                AccessToken = string.Empty;
            }
            return AccessToken;
        }
        #endregion

        #region GetDeviceList
        private async Task<List<string>> GetDeviceList(string AccessToken)
        {

            try
            {
                TrackingTrackSolidDto trackingTrackSolidDto = new TrackingTrackSolidDto();
                var devicelistparameters = new Dictionary<string, string>();
                devicelistparameters.Add("method", "jimi.user.device.list");
                devicelistparameters.Add("timestamp", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss").Replace('.', ':'));
                devicelistparameters.Add("app_key", trackingTrackSolidDto.app_key);
                devicelistparameters.Add("sign", trackingTrackSolidDto.sign);
                devicelistparameters.Add("sign_method", trackingTrackSolidDto.sign_method);
                devicelistparameters.Add("v", trackingTrackSolidDto.v);
                devicelistparameters.Add("format", trackingTrackSolidDto.format);
                devicelistparameters.Add("access_token", AccessToken);
                devicelistparameters.Add("target", trackingTrackSolidDto.user_id);

                var devicelistresponse = await GetResultObject(devicelistparameters);
                List<string> DeviceIMEIList = new List<string>();
                if (devicelistresponse != null)
                {

                    foreach (var item in devicelistresponse["result"])
                    {
                        var imei = item["imei"].ToString().Trim();
                        if (!string.IsNullOrEmpty(imei))
                        {
                            DeviceIMEIList.Add(imei);
                        }
                    }
                }
                return DeviceIMEIList;
            }
            catch
            {

                return null;
            }

        }
        #endregion

        #region GetDeviceLocation
        private async Task GetDeviceLocation(string AccessToken, string Deviceimei)
        {
            try
            {
                TrackingTrackSolidDto trackingTrackSolidDto = new TrackingTrackSolidDto();
                var devicelocationparameters = new Dictionary<string, string>();
                devicelocationparameters.Add("method", "jimi.device.location.get");
                devicelocationparameters.Add("timestamp", DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss").Replace('.', ':'));
                devicelocationparameters.Add("app_key", trackingTrackSolidDto.app_key);
                devicelocationparameters.Add("sign", trackingTrackSolidDto.sign);
                devicelocationparameters.Add("sign_method", trackingTrackSolidDto.sign_method);
                devicelocationparameters.Add("v", trackingTrackSolidDto.v);
                devicelocationparameters.Add("format", trackingTrackSolidDto.format);
                devicelocationparameters.Add("access_token", AccessToken);
                devicelocationparameters.Add("target", trackingTrackSolidDto.user_id);
                devicelocationparameters.Add("imeis", Deviceimei);

                var devicelocationresponse = await GetResultObject(devicelocationparameters);
                if (devicelocationresponse != null)
                {
                    foreach (var item in devicelocationresponse["result"])
                    {

                        TrackingDeviceModel trackingDeviceModel = new TrackingDeviceModel();
                        trackingDeviceModel.IMEINumber = item["imei"].ToString();
                        trackingDeviceModel.DeviceName = item["deviceName"].ToString();
                        trackingDeviceModel.Latitude = Convert.ToDouble(item["lat"].ToString());
                        trackingDeviceModel.Longitude = Convert.ToDouble(item["lng"].ToString());
                        await UpdateTrackingDeviceCoOrdinatesAsync(trackingDeviceModel);
                    }
                }
            }
            catch
            {
                //Exception
            }
        }
        #endregion

        #region UpdateTrackingDeviceCoOrdinates to DB
        private async Task UpdateTrackingDeviceCoOrdinatesAsync(TrackingDeviceModel trackingDevice)
        {
            const string sqlquery = "Exec SPUpdateTrackingDeviceByIMEI @IMEINumber,@DeviceName,@Latitude,@Longitude";

            List<SqlParameter> parms;

            parms = new List<SqlParameter>
                {
                     new SqlParameter { ParameterName = "@IMEINumber", Value = trackingDevice.IMEINumber},
                     new SqlParameter { ParameterName = "@DeviceName", Value = trackingDevice.DeviceName },
                     new SqlParameter { ParameterName = "@Latitude", Value = trackingDevice.Latitude },
                     new SqlParameter { ParameterName = "@Longitude", Value = trackingDevice.Longitude }
                };
            await DbContext.Database.ExecuteSqlRawAsync(sqlquery, parms.ToArray());
            await DbContext.SaveChangesAsync();
        }
        #endregion

        #region  GetResultObject
        private async Task<JObject?> GetResultObject(Dictionary<string, string> Parameter)
        {
            try
            {
                var client = new HttpClient();
                var req = new HttpRequestMessage(HttpMethod.Post, TrackingTrackSolidDto.RequestUrl) { Content = new FormUrlEncodedContent(Parameter) };
                var res = await client.SendAsync(req);
                if (res.IsSuccessStatusCode)
                {
                    var responsestring = await res.Content.ReadAsStringAsync();
                    var jsonobj = JsonConvert.DeserializeObject<AccessTokenResponse>(responsestring);
                    if (jsonobj.code.Equals("0") && jsonobj.message.Equals("success"))
                    {
                        return JObject.Parse(responsestring);
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        #endregion
        public async Task<(bool IsSuccess, List<SpGetDeviceForStoreId>? connectedstorelist, string? ErrorMessage)> GetDeviceForStoreIdAsync()
        {
            try
            {
                List<SqlParameter> parms;
                const string sqlquery = "Exec SpGetDeviceForStoreId @StoreId";
                parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@StoreId", Value =DBNull.Value },
                };

                var connectedstorelist = await DbContext.SpGetDeviceForStoreId.FromSqlRaw(sqlquery, parms.ToArray()).ToListAsync();

                if (connectedstorelist != null)
                {
                    return (true, connectedstorelist, null);
                }
                return (false, null, "No Store Found");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }
    }


    public class AccessTokenResponse
    {
        public string code { get; set; } = string.Empty;
        public string message { get; set; } = string.Empty;
    }
}
