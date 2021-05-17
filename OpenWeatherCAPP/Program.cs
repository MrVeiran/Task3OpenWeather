using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace OpenWeatherCAPP
{
    class Program
    {
        static void Main(string[] args)
        {
            string URL = "http://api.openweathermap.org/data/2.5/forecast?q=Moscow&cnt=40&units=metric&appid=1b8c15d47aa4079eb8670e8703b97ba4";
            List<WeatherResponse> list = new List<WeatherResponse>();
            
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(URL);
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            string response;
            int maxDay = 5; //Максимальное давление за предстоящие 5 дней (включая текущий);
            int cycleStep = 8;//OpenWeather will sent you the statistic every 3 hours. we need one value per day. 24/3 = 8, cycle step = 8 

            using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                response = streamReader.ReadToEnd();
            }

            Forecast weatherResponse = JsonConvert.DeserializeObject<Forecast>(response);

            int maxValuePressure = weatherResponse.List[0].Main.Pressure;//need to find max value of pressures
            int indexPressure = 0;//index for searching this day

            float minValueTemperature = 0f; // 9a.m.- morning 3a.m. - night
            int indexTemp = 0;

            List<float> MinT = new List<float>();// pressures each day in 9.a.m.
            List<float> MaxT = new List<float>();// pressures each day in 3.a.m.



            for (int i = 0; i < weatherResponse.List.Count; i += cycleStep)
            {
                if (maxValuePressure <= weatherResponse.List[i].Main.Pressure)
                {
                    indexPressure = i;
                    maxValuePressure = weatherResponse.List[i].Main.Pressure;
                }

            }

            FillMinMaxList(weatherResponse,ref  MinT,ref MaxT);
            
            minValueTemperature = MaxT[0] - MinT[0];

            for (int i = 0; i < maxDay; i++)
            {
                if (minValueTemperature > (MaxT[i] - MinT[i]))
                {
                    indexTemp = i;
                    minValueTemperature = MaxT[i] - MinT[i];
                }
            }
            

            for (int i = 0; i < weatherResponse.List.Count; i += cycleStep)
            {
                Console.WriteLine($"Temperature in {weatherResponse.City.Name} is {weatherResponse.List[i].Main.Temp} °C and pressure is {weatherResponse.List[i].Main.Pressure}. Date is {weatherResponse.List[i].dt_txt}");
            }

            Console.WriteLine($"\nThe max pressure will be in {CutTheDate(weatherResponse, indexPressure)} and will be equal {weatherResponse.List[indexPressure].Main.Pressure}. ");//second task
            
            Console.WriteLine($"The minimum difference will be in {CutTheDate(weatherResponse,indexTemp)} and it will be {minValueTemperature}°C.");//first task
            Console.ReadLine();
        }

        static string CutTheDate(Forecast weatherResponse, int minValueCoor)
        {
            string str1 = weatherResponse.List[minValueCoor].dt_txt;
            string str2 = str1.Remove(10, 9);
            return str2;
        }

        static void FillMinMaxList(Forecast weatherResponse, ref List<float> MinT,ref  List<float> MaxT)
        {
            for (int i = 0; i < weatherResponse.List.Count; i++)
            {
                string str1 = weatherResponse.List[i].dt_txt;
                string str2 = str1.Remove(0, 11);
                string str3 = str2.Replace(":", "");
                if (str3 == "090000")
                {
                    MaxT.Add(weatherResponse.List[i].Main.Temp);
                }
                if (str3 == "030000")
                {
                    MinT.Add(weatherResponse.List[i].Main.Temp);
                }

            }
        }
    }
}
