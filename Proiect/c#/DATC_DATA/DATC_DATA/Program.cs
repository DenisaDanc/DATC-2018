using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DATC_DATA
{
    class Program
    {
        private const string URL = "http://34.73.166.92/v1/parking-lot/";
        private const int NR_SENSORS = 4;

        static void Main(string[] args)
        {
            List<Sensor> sensors = new List<Sensor>();
            for (int i = 1; i < NR_SENSORS; i++)
            {
                sensors.Add(new Sensor("P" + i));
            }
            SerialPort port = new SerialPort("COM10", 9600, Parity.None, 8, StopBits.One);
            port.Open();
            while (true)
            {
                string message = port.ReadExisting();
                if (!message.Equals(""))
                {
                    message = message.Substring(0, message.Length -3);
                    string[] data = message.Split(' ');
                    for (int i = 0; i < data.Length; i++)
                    {
                        string[] values = data[i].Split(':');
                        Sensor sensor = sensors.Find(x => x.Name.Equals(values[0]));
                        int state = Int32.Parse(values[1]);
                        if (sensor != null)
                        {
                            if (sensor.Occupied != state)
                            {
                                sensor.Occupied = state;
                                makeRequest(sensor.Name, state);
                            }
                        }
                    }
                }
                Thread.Sleep(1000);
            }


        }

        private static void makeRequest(string sensorName, int value)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(URL + sensorName);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "PATCH";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = "{\"sensorValue\":" + value +
                              "}";

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
            }
        }
    }
}
