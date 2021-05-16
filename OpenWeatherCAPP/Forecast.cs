using System;
using System.Collections.Generic;
using System.Text;

namespace OpenWeatherCAPP
{
    class Forecast
    {
        public IList<WeatherResponse> List { get; set; }
        
        public City City { get; set; }
    }
}
