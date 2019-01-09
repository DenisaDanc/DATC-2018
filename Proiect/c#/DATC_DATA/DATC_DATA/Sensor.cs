using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATC_DATA
{
    class Sensor
    {
        private string sensorName;
        private int occupied;

        public Sensor(string name)
        {
            this.sensorName = name;
            occupied = 0;
        }

        public int Occupied { get => occupied; set => occupied = value; }
        public string Name { get => sensorName; }
    }
}
