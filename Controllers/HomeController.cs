using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebScale.Models;
using DOT_NET_COMS_LIB;
using System.IO.Ports;
using System.Globalization;
using System.Threading;

namespace WebScale.Controllers
{
    public class HomeController : Controller
    {

        private static SerialPort Port;
        private static HexaInterface HexaInter;

        private byte DestinationID, SourceID;
        private byte Options = 0x22;
        private int Code;

        private byte channel = 0x01;
        private byte modulePort = 0x03;
        private byte module = 0x01;

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public ActionResult Connect(string COM, int BaudRate)
        {
            Port = new SerialPort("COM" + COM, BaudRate, Parity.None, 8, StopBits.One);
            HexaInter = new HexaInterface(COM, BaudRate);
            return Json(new { COM, BaudRate });
        }

        [HttpPost]
        public void Disconnect()
        {
            Code = (int)HexaInterface.Message_Codes.CODE_H26R0_STOP;
            byte[] Payload = new byte[0];

            HexaInter.SendMessage(DestinationID, SourceID, Code, Payload);
            Port.Dispose();
        }

        [HttpPost]
        public void SendData (string SourceID, string DestinationID, string [] relayValues)
        {
            Code = (int)HexaInterface.Message_Codes.CODE_H0FR6_TOGGLE;
            HexaInter.Opt2_16_BIT_Code = "1"; 
            byte[] Payload = new byte[0];
            HexaInter.SendMessage(byte.Parse(DestinationID), byte.Parse(SourceID), Code, Payload);

        }

        [HttpPost]
        public void ToggleAll(string RelaysCount)
        {
            Code = (int)HexaInterface.Message_Codes.CODE_H0FR6_TOGGLE;
            HexaInter.Opt2_16_BIT_Code = "1";
            byte[] Payload = new byte[0];
            HexaInter.SendMessage(255, 1, Code, Payload);


            //for (int i = 2; i <= int.Parse(RelaysCount); i++)
            //{
            //    Thread.Sleep(200);
            //    HexaInter.SendMessage((byte)i, 1, Code, Payload);
            //}

        }

    }
}
