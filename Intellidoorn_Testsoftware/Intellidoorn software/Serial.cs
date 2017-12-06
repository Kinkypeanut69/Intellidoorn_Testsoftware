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
        private System.IO.Ports.SerialPort serialPort1;
        
        public Serial()
        {
            this.serialPort1 = new SerialPort();
            serialPort1.PortName = Form1.COMPort;
            serialPort1.BaudRate = 19200;
        }

        public double ReadData()
        {
            double Output = 0;
            serialPort1.Open();

            serialPort1.WriteLine("F");
            try
            {
                string input = serialPort1.ReadLine();
                int index = input.IndexOf("m");
                if (index > 0)
                    input = input.Substring(0, index);

                index = input.LastIndexOf(":") + 1;
                input = (input.Substring(index, input.Length - index));

                Output = Double.Parse(input);

            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex);
            }

            serialPort1.Close();
            return Output;
        }
    }
}
