using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;

namespace AttendanceTracker.ConsoleApplication
{
    class Program
    {
        //Dictionary variable to store UID and UName
        static Dictionary<string, string> userIdToNameMap;
        //Path of attendance log file
        const string logFile = "LogFile.txt";               

        static void Main(string[] args)
        {
            Console.Title = "Attendance Tracker";
            //Populating dictionary variable
            userIdToNameMap = GetUserMap();
            //File handling - If file doesn't exist create one.
            if (!File.Exists(logFile))                      
            {
                File.Create(logFile);
            }
            //Connect to device on serial port 
            SerialPort serialPort = Connect();
            //When data is received(Event) from device OnDataReceived(Event handler method) is called
            serialPort.DataReceived += OnDataReceived;      

            Console.ReadLine();
        }

        //sender is the (Event triggering) object calling it
        private static void OnDataReceived(object sender, SerialDataReceivedEventArgs e) 
        {
            //Typecasting object to SerialPort class and assigning it to serial port variable
            var serialPort = (SerialPort)sender;

            //Take input from serial port
            var uid = serialPort.ReadExisting();              

            if (uid.Length > 13)
            {   
                // Characters after 13 position might be garbage character and need to be removed.
                uid = uid.Substring(0, 12);
            }

            if (userIdToNameMap.ContainsKey(uid))
            {
                //Returning UName corresponding to UID(key)
                var userName = userIdToNameMap[uid];
                //Getting time stamp of event
                var timeStamp = DateTime.Now;
                //Formatting string
                var log = string.Format("{0} logged in @{1}", userName, timeStamp);
                //Displaying output on console
                Console.WriteLine(log);
                //This method creates a StreamWriter object in append mode, on writing to this object data is written to file
                var fileWriter = File.AppendText(logFile);      
                fileWriter.WriteLine(log);
                //Close file when writing is complete
                fileWriter.Close();
                //Dispose of the StreamWriter object
                fileWriter.Dispose();                           
            }
        }

        private static Dictionary<string, string> GetUserMap()
        {
            Dictionary<string, string> userIdToNameMap = new Dictionary<string, string>();
            //Add UID and UName to dictionary, UID is key
            userIdToNameMap.Add("18008939F55D", "User A"); 
            userIdToNameMap.Add("5600122FF69D", "User B");
            userIdToNameMap.Add("560012456D6C", "User C");
            userIdToNameMap.Add("5600122FF398", "User D");
            userIdToNameMap.Add("330094EE0148", "User E");

            return userIdToNameMap;
        }

        private static SerialPort Connect()
        {
            //Connect to COM3 at 9600 baud rate
            SerialPort serialPort = new SerialPort(portName: "COM3", baudRate: 9600);   
            serialPort.Open();
            Console.WriteLine("Connected to {0}", serialPort.PortName);
            return serialPort;
        }

        private static void Disconnect(SerialPort serialPort)
        {
            serialPort.Close();
            serialPort.Dispose();
        }
    }
}
