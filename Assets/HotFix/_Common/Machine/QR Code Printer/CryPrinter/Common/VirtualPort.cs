#if UNITY_ANDROID
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
    using System.Text;
#if !UNITY_ANDROID
    using System.IO.Ports;
#endif
    using System.Collections.Generic;
    using System.Threading;
    using System.Collections;
    using SBoxApi;

    public class VirtualPort
    {
        protected int _BaudRate = 9600;
        protected int _DataBits = 8;
        protected StopBits _StopBits = StopBits.One;
        protected Parity _Parity = Parity.None;
        protected Handshake _Handshake = Handshake.None;
        protected bool _DiscardNull;
        protected bool _DtrEnable;
        protected bool _RtsEnable;
        protected int _WriteTimeout;
        protected int _WriteBufferSize;
        protected int _ReadTimeout;
        protected int _ReadBufferSize;
        protected bool _IsOpen = false;
        //protected Thread _Thread = null;

        //private Queue<byte> _ReadQueue = new Queue<byte>();

        public int BaudRate
        {
            get { return _BaudRate; }
            set
            {
                _BaudRate = value;
            }
        }

        public int DataBits
        {
            get { return _DataBits; }
            set
            {
                _DataBits = value;
            }
        }

        public bool DiscardNull
        {
            get { return _DiscardNull; }
            set
            {
                _DiscardNull = value;
            }
        }

        public bool DtrEnable
        {
            get { return _DtrEnable; }
            set
            {
                _DtrEnable = value;
            }
        }

        public Handshake Handshake
        {
            get { return _Handshake; }
            set
            {
                _Handshake = value;
            }
        }

        public bool IsOpen
        {
            get
            {
                return _IsOpen;
            }
        }

        public Parity Parity
        {
            get
            {
                return _Parity;
            }
            set
            {
                _Parity = value;
            }
        }

        public int ReadBufferSize
        {
            get
            {
                return _ReadBufferSize;
            }
            set
            {
                _ReadBufferSize = value;
            }
        }

        public int ReadTimeout
        {
            get
            {
                return _ReadTimeout;
            }
            set
            {
                _ReadTimeout = value;
            }
        }

        public bool RtsEnable
        {
            get
            {
                return _RtsEnable;
            }
            set
            {
                _RtsEnable = value;
            }
        }

        public StopBits StopBits
        {
            get
            {
                return _StopBits;
            }
            set
            {
                _StopBits = value;
            }
        }

        public int WriteBufferSize
        {
            get
            {
                return _WriteBufferSize;
            }
            set
            {
                _WriteBufferSize = value;
            }
        }

        public int WriteTimeout
        {
            get
            {
               return _WriteTimeout;
            }
            set
            {
                _WriteTimeout = value;
            }
        }


        public VirtualPort(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {
            _BaudRate = baudRate;
            _Parity = parity;
            _DataBits = dataBits; 
            _StopBits = stopBits;
            _IsOpen = false;
        }
        public void Dispose()
        {
        }

        public void Close()
        {
            //_IsOpen = false;
            //SBoxSandbox.PrinterBridgeSetup(-1, _BaudRate, _DataBits, (int)_Parity, (int)_StopBits);
        }

        public void Open()
        {
            _IsOpen = true;

            //if (_Thread == null)
            //{
            //    _Thread = new Thread(new ThreadStart(MyThreadMethod));
            //    _Thread.Start();
            //}

            SBoxSandbox.PrinterBridgeSetup(1, _BaudRate, _DataBits, (int)_Parity, (int)_StopBits);
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            int size = 0;
            //while(size < count)
            //{
            //    if(_ReadQueue.Count > 0)
            //    {
            //        buffer[size++] = _ReadQueue.Dequeue();
            //    }
            //    else
            //    {
            //        break;
            //    }
            //}

            return size;
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            byte[] bytes = new byte[count];
            for(int i = 0; i < count; i++)
            {
                bytes[i] = buffer[offset + i];
            }
            SBoxSandbox.PrinterBridgeWrite(bytes);
        }

        //protected void MyThreadMethod()
        //{
        //    while(_IsOpen == true)
        //    {
        //        byte[] buff = SBoxSandbox.PrinterBridgeRead();
        //        if(buff.Length > 0)
        //        {
        //            for(int i = 0 ; i < buff.Length; i++)
        //            {
        //                _ReadQueue.Enqueue(buff[i]);
        //            }
        //        }
        //        else
        //        {
        //            System.Threading.Thread.Sleep(20);
        //        }
        //    }
        //}
    }
}
#endif
