using System;
using System.Collections.Generic;
using System.Text;

namespace OpenWeatherCAPP
{
    class Info
    {
        public float Temp { get; set; }
        
        private int _pressure;
            public int Pressure 
            {
                get 
                {
                    return _pressure;
                }
                set
                {
                    _pressure = Convert.ToInt32(value / 1.33322f);
                }
            }

    }
}
