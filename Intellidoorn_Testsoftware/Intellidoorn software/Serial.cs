using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace Intellidoorn_software
{
    class Serial
    {
        private System.IO.Ports.SerialPort SerialPortLaser;
        
        public Serial()
        {
            this.SerialPortLaser = new SerialPort();
            SerialPortLaser.PortName = MainMenu.COMPort;
            SerialPortLaser.BaudRate = 19200;
        }

        public double ReadData()
        {
            double Output = 0;
            
            try
            {
                SerialPortLaser.Open();
                SerialPortLaser.WriteLine("F");
                string input = SerialPortLaser.ReadLine();
                int index = input.IndexOf("m");
                if (index > 0)
                    input = input.Substring(0, index);

                index = input.LastIndexOf(":") + 2;
                input = (input.Substring(index, input.Length - index));
                input = input.Replace(".", ",");
                Output = Double.Parse(input);
            }
            catch (Exception ex)
            {
                //Ignore
            }
            SerialPortLaser.Close();
            return Output;
        }
    }
}
