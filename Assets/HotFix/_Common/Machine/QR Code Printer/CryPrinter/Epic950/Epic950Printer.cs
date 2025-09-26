#region Copyright & License

/*
MIT License

Copyright (c) 2017 Pyramid Technologies

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
 */

#endregion

namespace CryPrinter
{
    using System;
    using System.Collections.Generic;
    // using ThermalTalk.Imaging;
    using UnityEngine;
    using SBoxApi;

    /*
     *  使用说明:
     *  1.如果接了JCM的950打印机:
     *  
     *  var printer = new Epic950Printer("COM4",false);//"/dev/ttyS1") ;
     *  printer.printer.PrintTicket("011058314280279645", 512.32, "Crown International Club", "CYTECH", "NORTH", "10600001", 10, "1", System.DateTime.Now);
     *  
     *  2.如果接了TRANSACT的950打印机:
     *  var printer = new Epic950Printer("COM4",true);//"/dev/ttyS1") ;
     *  printer.printer.PrintTicket("011058314280279645", 512.32, "Crown International Club", "CYTECH", "NORTH", "10600001", 10, "1", System.DateTime.Now);
     * 
     */

    /// <inheritdoc />
    public class Epic950Printer : BasePrinter
    {
        // Milliseconds
        private const int DefaultReadTimeout = 1500;
        private const int DefaultBaudRate = 9600;
        private bool mIsTransact = false;
        private List<byte> PrintCmd = new List<byte>();
        #region Constructors

        /// <summary>
        /// Constructs a new instance of Epic950Printer.
        /// </summary>
        /// <param name="serialPortName">OS name of serial port</param>        
        public Epic950Printer(string serialPortName,bool bTransact) : this()
        {
            mIsTransact = bTransact;
            if (string.IsNullOrEmpty(serialPortName))
                return;
            InitPrinterCommand = new byte[] { 0x1B, 0x2A };
            FormFeedCommand = new byte[] { 0x0C };
            PrintSerialReadTimeout = DefaultReadTimeout;
            PrintSerialBaudRate = DefaultBaudRate;
            PrintCmd.Clear();
            Debug.Log("Creating new instance of Epic950Printer Printer on port: " + serialPortName);

            // User wants a serial port
            Connection = new Epic950SerialPort(serialPortName, PrintSerialBaudRate,bTransact ? Handshake.XOnXOff: Handshake.None)
            {
                ReadTimeoutMS = DefaultReadTimeout
            };
        }

        /// <summary>
        /// Constructs a new instance of Epic950Printer.
        /// </summary>
        /// <param name="connection">Serial connection.</param>
        public Epic950Printer(ISerialConnection connection = null)
        {
            InitPrinterCommand = new byte[] { 0x1B, 0x2A };
            FormFeedCommand = new byte[] { 0x0C };
            PrintSerialReadTimeout = DefaultReadTimeout;
            PrintSerialBaudRate = DefaultBaudRate;
            PrintCmd.Clear();
            if (connection == null)
                return;

            Debug.Log("Creating new instance of Phoenix Printer on port: " + connection.Name);

            Connection = connection;
        }

        #endregion

        /// <summary>
        /// Updates the formfeed line count to n.
        /// where 0 lt n lt 200
        /// Units are in lines relative to current font size. The default
        /// value is 20.
        /// </summary>
        /// <param name="n">Count of lines to print before cut</param>
        public void SetFormFeedLineCount(byte n)
        {
            Debug.Log("Setting form feed line count to: " + n);

            FormFeedCommand[2] = n;
        }

        /// <inheritdoc />
        /// <summary>
        /// Encodes the specified string as a center justified 2D barcode. 
        /// This 2D barcode is compliant with the QR Code® specicification and can be read by all 2D barcode readers.
        /// Up to 154 8-bit characters are supported.
        /// f the input string length exceeds the range specified by the k parameter, only the first 154 characters will be 
        /// encoded. The rest of the characters to be encoded will be printed as regular ESC/POS characters on a new line.
        /// </summary>
        /// <param name="encodeThis">String to encode, max length = 154 bytes</param>
        public override ReturnCode Print2DBarcode(string encodeThis)
        {
            return ReturnCode.Success;
          // Debug.Log("Encoding the following string as a barcode: " + encodeThis);

          ////  Use all default values for barcode
          // var barcode = new TwoDBarcode(TwoDBarcode.Flavor.Phoenix)
          // {
          //     EncodeThis = encodeThis
          // };
          // var fullCmd = barcode.Build();
          //  return AppendToDocBuffer(fullCmd);
        }
        public string Getbarcode(string barcode)
        {
            string str = "";
            str = barcode.Substring(0, 2) + "-" + barcode.Substring(2, 4) + "-" +
                barcode.Substring(6, 4) + "-" + barcode.Substring(10, 4) + "-" + barcode.Substring(14, 4);
            return str;
        }

        public string ConvertToWords(decimal amount)
        {
            if (amount < 0)
                return "MINUS " + ConvertToWords(-amount);
            if (amount == 0)
                return "ZERO DOLLARS";
            string words = "";
            // 分离元和分
            long dollars = (long)amount;
            int cents = (int)((amount - dollars) * 100);
            if (dollars > 0)
            {
                words += NumberToWords(dollars) + " DOLLAR" + (dollars > 1 ? "S" : "");
            }
            if (cents > 0)
            {
                if (words != "")
                    words += " AND ";
                words += NumberToWords(cents) + " CENT" + (cents > 1 ? "S" : "");
            }
            else
            {
                if (words != "")
                    words += " AND ";
                words += "NO CENTS";
            }
            return words;
        }

        public string NumberToWords(long number)
        {
            if (number == 0)
                return "";
            if (number < 0)
                return "MINUS " + NumberToWords(-number);
            string words = "";
            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " MILLION ";
                number %= 1000000;
            }
            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " THOUSAND ";
                number %= 1000;
            }
            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " HUNDRED ";
                number %= 100;
            }
            if (number > 0)
            {
                string[] unitsMap = { "", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN",
                                  "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN",
                                  "EIGHTEEN", "NINETEEN" };
                string[] tensMap = { "", "", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY" };
                if (number < 20)
                {
                    words += unitsMap[number];
                }
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                    {
                        words += " " + unitsMap[number % 10];
                    }
                }
            }
            return words.Trim();
        }
        /// <summary>
        /// 打印ticket
        /// </summary>
        /// <param name="barcode">票号，18位，唯一
        /// 此处要注意，如果系统配置了system validation ,则这个票号是从sas host端获取，否则的话则是从算法卡中获取
        /// </param>
        /// <param name="money">金额</param>
        /// <param name="companyname">公司名称</param>
        /// <param name="addr1">地址1</param>
        /// <param name="addr2">地址2</param>
        /// <param name="machineid">机器ID</param>
        /// <param name="voiddays">过期天数</param>
        /// <param name="ticketindex">票序列号，累加</param>
        /// <param name="printTime">打印时间</param>
        public ReturnCode PrintTicket(string barcode, double money, string companyname, string addr1,
            string addr2, string machineid, int voiddays, string ticketindex, DateTime printTime)
        {
            PrintCmd.Clear();
            string code = barcode.Trim();
            string voids = voiddays.ToString();
            string index = ticketindex;

            if(mIsTransact)
            {
                FormFeedCommand[0] = 0;
                //<0xFF>[SOH]J[EOT][NULL][GS]C[SOH]
                /*
                 * [SOH]:0x01 Start of heading
                 * [EOT]:0x04 End of xmit
                 * [GS]0x1D Group separator
                 */

                //<0xFF>[SOH]J[EOT][NULL][GS]C[SOH]12-3456-7890-1234-5678
                PrintCmd.AddRange(new byte[] { 0xFF, 0x01, 0x4A, 0x04, 0x00, 0x1D, 0x43, 0x01 });
                PrintCmd.AddRange(System.Text.Encoding.ASCII.GetBytes(Getbarcode(code)));

                //[CR][GS]C[STX]
                /*
                 * [CR]:0x0D Carriage feed
                 * [STX]:0x02 Start of text
                 * [ETX]:0x03 End of text
                 * [ENQ]:0x05 Enquiry
                 */
                //[CR][GS]C[STX]Casino Name (EGM Config)
                PrintCmd.AddRange(new byte[] { 0x0D, 0x1D, 0x43, 0x02 });
                PrintCmd.AddRange(System.Text.Encoding.ASCII.GetBytes(companyname));

                //[CR][GS]C[ETX]Address 1 (EGM Config)
                PrintCmd.AddRange(new byte[] { 0x0D, 0x1D, 0x43, 0x03 });
                PrintCmd.AddRange(System.Text.Encoding.ASCII.GetBytes(addr1));

                //[CR][GS]C[EOT]Address 2 (EGM Config)
                PrintCmd.AddRange(new byte[] { 0x0D, 0x1D, 0x43, 0x04 });
                PrintCmd.AddRange(System.Text.Encoding.ASCII.GetBytes(addr2));

                //[CR][GS]C[ENQ]08/31/2009 ?  31/08/2009
                PrintCmd.AddRange(new byte[] { 0x0D, 0x1D, 0x43, 0x05 });
                PrintCmd.AddRange(System.Text.Encoding.ASCII.GetBytes(DateTime.Now.ToString("dd/MM/yyyy")));

                //[CR][GS]C[ACK]23:59:59
                PrintCmd.AddRange(new byte[] { 0x0D, 0x1D, 0x43, 0x06 });
                PrintCmd.AddRange(System.Text.Encoding.ASCII.GetBytes(DateTime.Now.ToString("HH:mm:ss")));

                //[CR][GS]C[BEL]TICKET# 0001
                PrintCmd.AddRange(new byte[] { 0x0D, 0x1D, 0x43, 0x07 });
                PrintCmd.AddRange(System.Text.Encoding.ASCII.GetBytes("TICKET #" + index));

                //[CR][GS]C[BS]
                PrintCmd.AddRange(new byte[] { 0x0D, 0x1D, 0x43, 0x08 });
                PrintCmd.AddRange(System.Text.Encoding.ASCII.GetBytes(ConvertToWords(new Decimal(money))));

                //[GS]C[HT]*** Cashout Ticket, Template #78 ***
                // PrintCmd.AddRange(new byte[] { 0x1D, 0x43, 0x09 });
                // PrintCmd.AddRange(System.Text.Encoding.ASCII.GetBytes("*** " +  + " ***"));

                //[CR][GS]C[LF]$1,234.56 
                PrintCmd.AddRange(new byte[] { 0x0D, 0x1D, 0x43, 0x0A });
                PrintCmd.AddRange(System.Text.Encoding.ASCII.GetBytes("$" + money.ToString("N2")));

                //[CR][GS]C[VT]
                PrintCmd.AddRange(new byte[] { 0x0D, 0x1D, 0x43, 0x0B });
                PrintCmd.AddRange(System.Text.Encoding.ASCII.GetBytes(voids + " Days"));

                //[CR][GS]C[FF]
                PrintCmd.AddRange(new byte[] { 0x0D, 0x1D, 0x43, 0x0C });
                PrintCmd.AddRange(System.Text.Encoding.ASCII.GetBytes("MACHINE#" + machineid));

                //[CR][GS]C[CR] 
                PrintCmd.AddRange(new byte[] { 0x0D, 0x1D, 0x43, 0x0D });
                PrintCmd.AddRange(System.Text.Encoding.ASCII.GetBytes(code));

                //[CR][GS]ON[DC2]<0xD0> 
                PrintCmd.AddRange(new byte[] { 0x0D, 0x1D, 0x4F, 0x4E, 0x12, 0xD0 });

               
            }
            else
            {
                String printstr = "^P|0|1|" + Getbarcode(code) + "|" + companyname +
                "|" + addr1 + "|" + addr2 + "|Cashout Ticket|Validation|";
                printstr = printstr + Getbarcode(code) + "|" + DateTime.Now.ToString("dd/MM/yyyy") + "|" +
                    DateTime.Now.ToString("HH:mm:ss") +
                    "|" + "TICKET #" + index + "|" + ConvertToWords(new Decimal(money)) + "||";
                printstr = printstr + "$" + money.ToString("N2") + "||" + voids + "days|MACHINE ID#: "
                    + machineid + "|" + code + "|^";
                Debug.Log("print str = " + printstr);

                Epic950Code39 epic950Code39 = new Epic950Code39()
                {
                    EncodeThis = printstr
                };
                var fullCmd = epic950Code39.Build();
                PrintCmd.AddRange(fullCmd);
            }
            var toPrint = PrintCmd.ToArray();
            AppendToDocBuffer(toPrint);
            return FormFeed();
        }

        /// <summary>
        /// Sets the active font to this
        /// </summary>
        /// <param name="font">Font to use</param>
        /// <returns>ReturnCode.Success if successful, ReturnCode.ExecutionFailure otherwise.</returns>
        public override ReturnCode SetFont(ThermalFonts font)
        {
            Debug.Log("Setting thermal fonts . . .");

            //if (font == ThermalFonts.NOP)
            //{
            //    Debug.Log("No change selected");

            //    return ReturnCode.Success;
            //}

            //var result = ReturnCode.ExecutionFailure;

            //switch (font)
            //{
            //    case ThermalFonts.A:
            //        Debug.Log("Attempting to set font to font A.");
            //        result = AppendToDocBuffer(FontACmd);
            //        break;
            //    case ThermalFonts.B:
            //        Debug.Log("Attempting to set font to font B.");
            //        result = AppendToDocBuffer(FontBCmd);
            //        break;
            //    case ThermalFonts.C:
            //        Debug.Log("Attempting to set font to font C.");
            //        result = AppendToDocBuffer(FontCCmd);
            //        break;
            //}

            return ReturnCode.Success;
        }

        /// <inheridoc/>
        public override ReturnCode SetImage(PrinterImage image, IDocument doc, int index)
        {
            //while (index >= doc.Sections.Count)
            //{
            //    doc.Sections.Add(new Placeholder());
            //}

            //doc.Sections[index] = new PhoenixImageSection()
            //{
            //    Image = image,
            //};

            return ReturnCode.Success;
        }

        /// <summary>
        /// Phoenix support normal and double scalars. All other scalar values will
        /// be ignored.
        /// </summary>
        /// <param name="w">New scalar (1x, 2x, nop)</param>
        /// <param name="h">New scalar (1x, 2x, nop)</param>
        /// /// <returns>ReturnCode.Success if successful, ReturnCode.ExecutionFailure otherwise.</returns>
        public override ReturnCode SetScalars(FontWidthScalar w, FontHeighScalar h)
        {
            //var newWidth = Width;
            //if (w == FontWidthScalar.NOP || w == FontWidthScalar.w1 || w == FontWidthScalar.w2)
            //{
            //    newWidth = w;
            //}

            //var newHeight = Height;
            //if (h == FontHeighScalar.NOP || h == FontHeighScalar.h1 || h == FontHeighScalar.h2)
            //{
            //    newHeight = h;
            //}

            //// Only apply update if either property has changed
            //if (newWidth != Width || newHeight != Height)
            //{
            //    return base.SetScalars(newWidth, newHeight);
            //}

            return ReturnCode.Success;
        }

        /// <inheritdoc />
        /// <summary>
        /// This command is processed in real time. The reply to this command is sent
        /// whenever it is received and does not wait for previous ESC/POS commands to be executed first.
        /// If there is no response or an invalid response, IsValidReport will be set to false
        /// </summary>
        /// <remarks>Phoenix does not support Error or Movement status request type</remarks>
        /// <param name="type">StatusRequest type</param>
        /// <returns>Instance of PhoenixStatus</returns>
        public override StatusReport GetStatus(StatusTypes type)
        {
            ReturnCode ret;

            // Translate generic status to phoenix status
            //PhoenixStatusRequests r;
            //switch (type)
            //{
            //    case StatusTypes.PrinterStatus:
            //        r = PhoenixStatusRequests.Status;
            //        break;

            //    case StatusTypes.OfflineStatus:
            //        r = PhoenixStatusRequests.OffLineStatus;
            //        break;

            //    case StatusTypes.ErrorStatus:
            //        return StatusReport.Invalid();
            //        break;

            //    case StatusTypes.PaperStatus:
            //        r = PhoenixStatusRequests.PaperRollStatus;
            //        break;

            //    case StatusTypes.MovementStatus:
            //        // Not supported on Phoenix
            //        return StatusReport.Invalid();

            //    case StatusTypes.FullStatus:
            //        r = PhoenixStatusRequests.FullStatus;
            //        break;

            //    default:
            //        // Unknown status type
            //        return StatusReport.Invalid();
            //}

            var rts = new StatusReport();

            //if (r == PhoenixStatusRequests.FullStatus)
            //{
            //    ret = internalGetStatus(PhoenixStatusRequests.Status, rts);
            //    ret = ret != ReturnCode.Success ? ret : internalGetStatus(PhoenixStatusRequests.PaperRollStatus, rts);
            //    ret = ret != ReturnCode.Success ? ret : internalGetStatus(PhoenixStatusRequests.OffLineStatus, rts);

            //    // Not supported PP-82
            //    //ret = ret != ReturnCode.Success ? ret : internalGetStatus(PhoenixStatusRequests.ErrorStatus, rts);
            //}
            //else
            //{
            //    ret = internalGetStatus(r, rts);
            //}

            ret = internalGetStatus(PhoenixStatusRequests.FullStatus, rts);

            // Return null status object on error
            return ret == ReturnCode.Success ? rts : StatusReport.Invalid();
        }

        /// <summary>
        /// Write specified report type to target and fill rts with parsed response
        /// </summary>
        /// <param name="r">Report type, Phoenix does not support the error status command</param>
        /// <param name="rts">Target</param>
        /// <returns>ReturnCode.Success if successful, and ReturnCode.ExecutionFailure if there
        /// is an issue with the response received. Returns ReturnCode.Unsupported command
        /// if r == PhoenixStatusRequests.ErrorStatus. </returns>
        private ReturnCode internalGetStatus(PhoenixStatusRequests r,  StatusReport rts)
        {
            //Debug.Log("Attempting to get status of printer . . .");

            //return ReturnCode.Success;

            // PP-82 : Phoenix does not support the error status command
            if (r == PhoenixStatusRequests.ErrorStatus)
            {
                return ReturnCode.UnsupportedCommand;
            }

            // Send the real time status command, r is the argument
           // var command = new byte[] { 0x05 };
            //int respLen = 1;

            byte data = 0;
            //string str = "";
            try
            {
                //Debug.Log("Attempting to open connection . .. ");
                //Connection.Open();

                //var written = Connection.Write(command);

                //System.Threading.Thread.Sleep(350);

                //// Collect the response
                //data = Connection.Read(respLen);
                data = SBoxSandbox.PrinterStateEx();
            }
            catch (Exception e)
            {
                //Debug.LogError("The following exception was thrown while attempting to write the status:");
                //Debug.LogError(e.Message);
                //Debug.LogError(e.StackTrace);

                return ReturnCode.ExecutionFailure;
            }
            // finally
            // {
            //     Connection.Close();
            //  }

            // Invalid response
            //if (data.Length != respLen)
            //{
            //    //Debug.Log("Data received is the incorrect length, returning execution failure . . . ");
            //    //Connection.Close();
            //    return ReturnCode.ExecutionFailure;
            //}

            //Debug.Log("PhoenixStatusRequests--------->" + r);
            //switch (r)
            //{
            //    case PhoenixStatusRequests.Status:
            //        // bit 3: 0- online, 1- offline        
            //        //rts.IsOnline = (data[0] & 0x08) == 0;
            //        rts.IsOnline = (data & 0x01) == 0;
            //        break;

            //    case PhoenixStatusRequests.OffLineStatus:
            //        // bit 6: 0- no error, 1- error
            //        //rts.HasError = (data[0] & 0x40) != 0;
            //        //rts.HasError =str != "0"&&str != "18";
            //        rts.HasError = (data & 0x40) != 0;
            //        break;

            //    case PhoenixStatusRequests.ErrorStatus:
            //        rts.IsInvalidReport = true;
            //        break;

            //    case PhoenixStatusRequests.PaperRollStatus:
            //        // bit 5,6: 0- okay, 96- Not okay
            //        rts.IsPaperPresent = (data & 0x60) == 0;
            //        //rts.IsPaperPresent = str == "0";
            //        //rts.IsPaperLevelOkay = str == "18";
            //        break;

            //    default:
            //        rts.IsInvalidReport = true;
            //        break;
            //}

            rts.IsOnline = (data & 0x01) == 0;

            rts.HasError = (data & 0x04) != 0;

            rts.IsPaperPresent = (data & 0x10) == 0;

            return ReturnCode.Success;
        }
    }
}