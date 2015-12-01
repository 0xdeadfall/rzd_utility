using System;
using System.Text;
using Newtonsoft.Json;
using System.Threading;

using RussianRailwaysUtility.Entities;

namespace RussianRailwaysUtility.Utils {
    public static class RzdUtility {
        private const int MAX_ATTEMPTS_COUNT = 10;
        private const int DELAY_TIME = 1000;

        private static readonly string urlPat = "http://pass.rzd.ru/timetable/public/ru?" +
                                                "STRUCTURE_ID=735&layer_id=5371&dir=0&tfl=1&checkSeats=1&" +
                                                "st0={0}&code0={1}&dt0={4}&st1={2}&code1={3}&dt1={5}";
                                                        // САНКТ-ПЕТЕРБУРГ
        private static readonly string station0_name = "%D0%A1%D0%90%D0%9D%D0%9A%D0%A2-%D0%9F%D0%95%D0%A2%D0%95%D0%A0%D0%91%D0%A3%D0%A0%D0%93";
        private static readonly string station0_code = "2004000";
                                                        // МОСКВА
        private static readonly string station1_name = "%D0%9C%D0%9E%D0%A1%D0%9A%D0%92%D0%90";
        private static readonly string station1_code = "2000000";
        private static readonly string date = "01.01.2016";
        public static ResponseObject AskSite() {
            string url = string.Format(urlPat,
                                       station0_name, station0_code,
                                       station1_name, station1_code,
                                       date, DateTime.Parse(date).AddMonths(1).ToString("dd.MM.yyyy"));

            using (WebClientWithCookies cli = new WebClientWithCookies() { Encoding = Encoding.UTF8 }) {

                string ridResponseStr = cli.DownloadString(url);
                RidResponseObject ridObj = JsonConvert.DeserializeObject<RidResponseObject>(ridResponseStr);

                if (ridObj.ResultCode.Equals("RID", StringComparison.OrdinalIgnoreCase)) {

                    ResponseObject resultData;
                    int attempts = 0;
                    do { // Спрашиваем сервер до тех пор, пока не будет готов ответ
                        attempts++;
                        Thread.Sleep(DELAY_TIME);

                        string finalResponseStr = cli.DownloadString(url + "&rid=" + ridObj.RidValue);
                        resultData = JsonConvert.DeserializeObject<ResponseObject>(finalResponseStr);

                        if (resultData.ResultCode.Equals("Error", StringComparison.OrdinalIgnoreCase)) {
                            throw new Exception(string.Format("Ошибка запроса, возвращенный код: \"{0}\", сырой ответ: \n\"{1}\"", ridObj.ResultCode, ridResponseStr));
                        }

                    } while ( !resultData.ResultCode.Equals("OK", StringComparison.OrdinalIgnoreCase)
                                && attempts < MAX_ATTEMPTS_COUNT );

                    return resultData;
                } else {
                    throw new Exception(string.Format("Ошибка запроса, возвращенный код: \"{0}\", сырой ответ: \n\"{1}\"", ridObj.ResultCode, ridResponseStr));
                }
            }
        }
    }
}
