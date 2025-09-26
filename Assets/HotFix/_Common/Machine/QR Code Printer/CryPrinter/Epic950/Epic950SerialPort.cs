﻿#region Copyright & License
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
using System;
using System.Text;
using UnityEngine;

namespace CryPrinter
{
    using System.IO.Ports;

    class Epic950SerialPort : BaseSerialPort
    {
        /*
         * The RS-232C interface default configuration settings are: 
         *   Baud Rate:     9600 BPS
         *   Data Bits:     8 Bits
         *   Parity:        None
         *   Stop Bits:     1
         *   Handshaking:   XON/XOF 
         *   Receive Error: Prints 
         *   Input Buffer   8,192 bytes
         */

        #region Default SerialPort Params
        private const int DefaultBaudRate = 9600;
        private const int DefaultDatabits = 8;
        private const Parity DefaultParity = Parity.None;
        private const StopBits DefaultStopbits = StopBits.One;
        private const Handshake DefaultHandshake = Handshake.XOnXOff;
        #endregion

        #region Constructor

        public Epic950SerialPort(string portName)
            : this(portName, DefaultBaudRate)
        { }

        public Epic950SerialPort(string portName, int baud)
            : base(portName, baud, DefaultDatabits, DefaultParity, DefaultStopbits, DefaultHandshake)
        { }

        public Epic950SerialPort(string portName, int baud, Handshake handshake)
           : base(portName, baud, DefaultDatabits, DefaultParity, DefaultStopbits, handshake)
        { }

        #endregion
    }
    //#if !UNITY_ANDROID
    //    using System.IO.Ports;

    //    class Epic950SerialPort : BaseSerialPort
    //    {

    //        #region Default SerialPort Params
    //        private const int DefaultBaudRate = 9600;
    //        private const int DefaultDatabits = 8;
    //        private const Parity DefaultParity = Parity.None;
    //        private const StopBits DefaultStopbits = StopBits.One;
    //        private const Handshake DefaultHandshake = Handshake.None;
    //        #endregion

    //        #region Constructor

    //        public Epic950SerialPort(string portName)
    //            : this(portName, DefaultBaudRate)
    //        { }

    //        public Epic950SerialPort(string portName, int baud)
    //            : base(portName, baud, DefaultDatabits, DefaultParity, DefaultStopbits, DefaultHandshake)
    //        { }

    //        #endregion
    //    }
    //#elif UNITY_ANDROID//ANDROID平台
    //     class Epic950SerialPort : ISerialConnection
    //    {
    //        private AndroidJavaObject nativeObject;
    //        #region Default SerialPort Params
    //        private const int DefaultBaudRate = 9600;
    //        private const int DefaultDatabits = 8;
    //        private const int DefaultParity = 0;
    //        private const int DefaultStopbits = 1;

    //        #endregion

    //        protected int _mReadTimeout;
    //        protected int _mWriteTimeout;

    //        public string Name { get; private set; }

    //        public int ReadTimeoutMS
    //        {
    //            get { return _mReadTimeout; }
    //            set
    //            {
    //                _mReadTimeout = value;
    //            }
    //        }

    //        /// <summary>
    //        /// Gets or Sets the write timeout in milliseconds
    //        /// </summary>
    //        public int WriteTimeoutMS
    //        {
    //            get { return _mWriteTimeout; }
    //            set
    //            {
    //                _mWriteTimeout = value;
    //            }
    //        }

    //        public void Dispose()
    //        {
    //            GC.SuppressFinalize(this);
    //        }

    //        #region Constructor

    //        public PhoenixSerialPort(string portName)
    //        {
    //            nativeObject = new AndroidJavaObject("com.cryfx.game.libserialport.SerialPortPlugin");
    //            nativeObject.Call("CreateSerialPort", portName, DefaultBaudRate, DefaultParity, DefaultDatabits, DefaultStopbits);
    //        }

    //        public PhoenixSerialPort(string portName, int baud)
    //        {
    //            nativeObject = new AndroidJavaObject("com.cryfx.game.libserialport.SerialPortPlugin");
    //            nativeObject.Call("CreateSerialPort", portName, baud, DefaultParity, DefaultDatabits, DefaultStopbits);
    //            if (nativeObject != null)
    //            {
    //                Debug.Log(" CreateSerialPort Success");

    //            }
    //        }

    //        public ReturnCode Open()
    //        {
    //            if (nativeObject == null) return ReturnCode.ConnectionNotFound;
    //            bool isOpen = nativeObject.Call<bool>("isOpen");
    //            if (isOpen)
    //            {
    //                Debug.Log(" Serial Port is Opened");
    //                return ReturnCode.Success;
    //            }
    //            try
    //            {
    //                isOpen = nativeObject.Call<bool>("Open");

    //                if (isOpen)
    //                {
    //                    Debug.Log(" Serial Port is Opened");
    //                    //return ReturnCode.Success;
    //                }
    //                else
    //                {
    //                    Debug.Log(" Serial Port is not Opened");
    //                }
    //                //isOpen = nativeObject.Call<bool>("isOpen");
    //                return ReturnCode.Success;
    //            }
    //            catch (System.IO.IOException)
    //            {
    //                return ReturnCode.ConnectionNotFound;
    //            }
    //            catch (System.AccessViolationException)
    //            {
    //                return ReturnCode.ConnectionAlreadyOpen;
    //            }
    //        }

    //        public int Write(byte[] payload)
    //        {
    //            if (nativeObject == null) return 0;
    //            bool isOpen = nativeObject.Call<bool>("isOpen");
    //            if (!isOpen)
    //            {
    //                nativeObject.Call<bool>("Open");
    //            }
    //           //string strPrint = Encoding.UTF8.GetString(payload);
    //            bool bRet =  nativeObject.Call<bool>("WriteByte", payload);
    //            return bRet? payload.Length : 0;
    //            // return WritePort(payload);
    //        }

    //        public byte[] Read(int n)
    //        {
    //            return nativeObject.Call<byte[]>("Read",n);
    //        }

    //        public ReturnCode Close()
    //        {
    //            if (nativeObject == null) return ReturnCode.ConnectionNotFound;

    //            bool isOpen = nativeObject.Call<bool>("isOpen");
    //            if (!isOpen) return ReturnCode.Success;

    //            try
    //            {
    //                nativeObject.Call("Close");
    //                return ReturnCode.Success;
    //            }
    //            catch
    //            {
    //                return ReturnCode.ExecutionFailure;
    //            }
    //        }


    //        #endregion
    //    }
    //#endif//UNITY_EDITOR
}
