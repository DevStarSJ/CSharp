using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace LunaStar.Network.TCP
{


    /// <summary>
    /// 비동기적으로 Client의 Data를 읽어오기 위한 State Object
    /// </summary>
    public class StateObject
    {
        // Size of receive buffer.
        public const int BufferSize = 327680;

        public StateObject() { }
        public StateObject(Socket sock) { this.sock = sock; }

        // Client socket.
        private Socket _sock = null;
        // Receive buffer.
        private byte[] _buffer = new byte[BufferSize];
        // Received data string.
        private StringBuilder _sb = new StringBuilder();

        public Socket sock { get { return _sock; } set { _sock = value; } }
        public byte[] buffer { get { return _buffer; } set { _buffer = value; } }
        public StringBuilder sb { get { return _sb; } set { _sb = value; } }
    } // end of class StateObject

    #region 사용하지 않음 - 인터넷 긁어옴

    /// <summary>
    /// 비동기 소켓에서 발생한 에러 처리를 위한 이벤트 Argument Class
    /// </summary>
    public class AsyncSocketErrorEventArgs : EventArgs
    {
        public int Id { get; private set; } = 0;
        public Exception AsyncSocketException { get; private set; }

        public AsyncSocketErrorEventArgs(int id, Exception exception)
        {
            Id = id;
            AsyncSocketException = exception;
        }
    }

    /// <summary>
    /// 비동기 소켓의 연결 및 연결해제 이벤트 처리를 위한 Argument Class
    /// </summary>
    public class AsyncSocketConnectionEventArgs : EventArgs
    {
        public int Id { get; private set; }

        public AsyncSocketConnectionEventArgs(int id)
        {
            Id = id;
        }
    }

    /// <summary>
    /// 비동기 소캣의 데이터 전송 이벤트 처리를 위한 Argument Class
    /// </summary>
    public class AsyncSocketSendEventArgs : EventArgs
    {
        public int SendBytes { get; private set; }
        public int Id { get; private set; }

        public AsyncSocketSendEventArgs(int id, int sendBytes)
        {
            Id = id;
            SendBytes = sendBytes;
        }
    }

    /// <summary>
    /// 비동기 소켓의 데이터 수신 이벤트 처리를 위한 Argument Class
    /// </summary>
    public class AsyncSocketReceiveEventArgs : EventArgs
    {
        public int Id { get; private set; } = 0;
        public int ReceiveBytes { get; private set; }
        public byte[] ReceiveData { get; private set; }

        public AsyncSocketReceiveEventArgs(int id, int receiveBytes, byte[] receiveData)
        {
            Id = id;
            ReceiveBytes = receiveBytes;
            ReceiveData = receiveData;
        }
    }

    /// <summary>
    /// 비동기 서버의 Accept 이벤트를 위한 Argument Class
    /// </summary>
    public class AsyncSocketAcceptEventArgs : EventArgs
    {
        public Socket Conn { get; private set; }

        public AsyncSocketAcceptEventArgs(Socket conn)
        {
            Conn = conn;
        }
    }

    ///
    /// delegate 정의
    ///
    public delegate void AsyncSocketErrorEventHandler(object sender, AsyncSocketErrorEventArgs e);
    public delegate void AsyncSocketConnectEventHandler(object sender, AsyncSocketConnectionEventArgs e);
    public delegate void AsyncSocketCloseEventHandler(object sender, AsyncSocketConnectionEventArgs e);
    public delegate void AsyncSocketSendEventHandler(object sender, AsyncSocketSendEventArgs e);
    public delegate void AsyncSocketReceiveEventHandler(object sender, AsyncSocketReceiveEventArgs e);
    public delegate void AsyncSocketAcceptEventHandler(object sender, AsyncSocketAcceptEventArgs e);

    public class AsyncSocketClass
    {
        public int Id { get; private set; } = -1;

        // Event Handler
        public event AsyncSocketErrorEventHandler OnError;
        public event AsyncSocketConnectEventHandler OnConnet;
        public event AsyncSocketCloseEventHandler OnClose;
        public event AsyncSocketSendEventHandler OnSend;
        public event AsyncSocketReceiveEventHandler OnReceive;
        public event AsyncSocketAcceptEventHandler OnAccept;

        public AsyncSocketClass()
        {
        }

        public AsyncSocketClass(int id)
        {
            Id = id;
        }

        protected virtual void ErrorOccured(AsyncSocketErrorEventArgs e)
        {
            AsyncSocketErrorEventHandler handler = OnError;

            if (handler != null)
                handler(this, e);
        }

        protected virtual void Connected(AsyncSocketConnectionEventArgs e)
        {
            AsyncSocketConnectEventHandler handler = OnConnet;

            if (handler != null)
                handler(this, e);
        }

        protected virtual void Closed(AsyncSocketConnectionEventArgs e)
        {
            AsyncSocketCloseEventHandler handler = OnClose;

            if (handler != null)
                handler(this, e);
        }

        protected virtual void Sent(AsyncSocketSendEventArgs e)
        {
            AsyncSocketSendEventHandler handler = OnSend;

            if (handler != null)
                handler(this, e);
        }

        protected virtual void Received(AsyncSocketReceiveEventArgs e)
        {
            AsyncSocketReceiveEventHandler handler = OnReceive;

            if (handler != null)
                handler(this, e);
        }

        protected virtual void Accepted(AsyncSocketAcceptEventArgs e)
        {
            AsyncSocketAcceptEventHandler handler = OnAccept;

            if (handler != null)
                handler(this, e);
        }

    }
    #endregion

     /// <summary>
     /// 비동기 TCP Socket을 이용하여 Non-blocking 으로 Client로 부터의 요청을 처리
     /// Accept, Receive, Disconnect 3부분의 Method를 delegate를 이용하여 외부에서 구현
     /// </summary>
    public class TcpAsynServer : IDisposable
    {
        public delegate String Delegate_WorkString(Socket sock, String str, Object obj);
        public delegate Byte[] Delegate_WorkByte(Socket sock, Byte[] Pb_Receive, Object obj);
        public delegate void Delegate_Accept_Work(Socket sock, Object obj);
        public delegate void Delegate_Disconnect_Work(String key, Object obj);

        // Thread signal
        // 쓰레드를 동작시키고 멈추게 하는 역할. Multi Thread에서 동기화를 맞추기 위한 용도라는데 그까진 모르겠고.
        private ManualResetEvent allDone = new ManualResetEvent(false);
        private ManualResetEvent recvDone = new ManualResetEvent(false);

        /// <summary>
        /// Accept를 위해 열어놓을 Port
        /// </summary>
        public int Port { set; get; }

        public string Ip { set; get; } = null;

        /// <summary>
        /// 접속한 Client들의 Socket을 저장
        /// </summary>
        public Hashtable ClientList { set; get; } = new Hashtable();

        /// <summary>
        /// Data를 Send할 Client들의 Key
        /// </summary>
        public ArrayList SendID { set; get; } = new ArrayList();

        /// <summary>
        /// Receive 했을때 처리해야하는 Method의 대리자
        /// </summary>
        public Delegate_WorkByte ReceiveWork { set; private get; }

        /// <summary>
        /// Accept 했을때 처리해야하는 Method의 대리자
        /// </summary>
        public Delegate_Accept_Work AcceptWork { set; private get; }

        /// <summary>
        /// Disconnect 했을때 처리해야하는 Method의 대리자
        /// </summary>
        public Delegate_Disconnect_Work DisconnectWork { set; private get; }


        /// <summary>
        /// Socket을 이용하여 Key를 찾음
        /// </summary>
        /// <param name="ht">Hashtable의 이름</param>
        /// <param name="value">찾아야 할 Key의 Socket</param>
        /// <returns>찾은 Key</returns>
        public Object FindKey(Hashtable ht, Object value)
        {
            foreach (DictionaryEntry de in ht)
            {
                if (de.Value.Equals(value))
                {
                    return de.Key;
                }
            }
            return null;
        }

        // Key값을 이용하여 접속 종료
        private void RemoveClient(String key)
        {
            if (key == null) return;
            Socket sock = ClientList[key] as Socket;
            DisconnectWork(key, this);
            Disconnect(sock);
        }

        // Socket을 이용하여 접속종료
        private void RemoveClient(Object value)
        {
            RemoveClient(FindKey(ClientList, value) as String);
        }

        // Accept 동작의 Default 대리자 Method
        private void Default_Accept_Work(Socket sock, Object obj)
        {
            ClientList.Add(sock.RemoteEndPoint.ToString(), sock);
            Console.WriteLine("Connected : {0}", sock.RemoteEndPoint.ToString());
        }

        // Disconnect 동작의 Default 대리자 Method
        private void Default_Disconnect_Work(String key, Object obj)
        {
            Socket sock = ClientList[key] as Socket;
            Console.WriteLine("Disconnected : {0}", sock.RemoteEndPoint.ToString());
            ClientList.Remove(key);
        }


        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="port">Accept때 사용할 Port</param>
        public TcpAsynServer(int port)
        {
            CommonConstructor(port, null);
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="port">Accept때 사용할 Port</param>
        public TcpAsynServer(String ip, int port, Delegate_WorkByte Work)
        {
            Ip = ip;
            CommonConstructor(port, Work);
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="port">Accept때 사용할 Port</param>
        /// <param name="Work">Receive 때 사용할 Method 대리자</param>
        public TcpAsynServer(int port, Delegate_WorkByte Work)
        {
            CommonConstructor(port, Work);
        }

        private void CommonConstructor(int port, Delegate_WorkByte Work)
        {
            Port = port;
            ReceiveWork = Work;
            AcceptWork = Default_Accept_Work;
            DisconnectWork = Default_Disconnect_Work;
        }

        /// <summary>
        /// Thread를 이용한 Server 구동
        /// </summary>
        public void Start()
        {
            Thread th = new Thread(new ThreadStart(StartListening));
            th.Start();
        }

        // Server 구동
        private void StartListening()
        {
            byte[] bytes = new byte[1024];
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.
            try
            {
                // Bind 와 Listen은 짝으로 사용해야함. Bind로 End Point를 연결해놓고 Listen으로 다른 Client들의 연결을 기다림.
                if (Ip == null)
                {
                    Ip = Dns.GetHostName();
                }

                listener.Bind(new IPEndPoint(Dns.GetHostAddresses(Ip)[0], Port));    // 현재의 IP주소를 가져와서 11000 Port로 Bind
                listener.Listen(100);                                                               // 최대 Clinet 갯수
                //Console.WriteLine("Local address and port : {0}", listener.LocalEndPoint.ToString());

                while (true)
                {
                    allDone.Reset();    // Thread 동작 못하게

                    //Console.WriteLine("Waiting for a connection...");
                    listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);      // 비동기적 연결시도. 연결되면 AcceptCallback 호출

                    allDone.WaitOne();  // Thread가 Set 될때까지 대기
                }
            }
            catch (Exception e) { Console.WriteLine(e.ToString()); }
        }

        // Accept 작업이 끝난후 호출되는 Callback
        private void AcceptCallback(IAsyncResult ar)
        {
            allDone.Set();  // Thread가 동작하도록 On

            Socket sock = ((Socket)ar.AsyncState).EndAccept(ar);
            StateObject state = new StateObject(sock); // ar에서부터 socket을 가져옴
            state.sock.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);

            AcceptWork(sock, null);
        }

        // Read이후에 호출되는 callback 함수
        private void ReadCallback(IAsyncResult ar)
        {
            StateObject state = (StateObject)ar.AsyncState; // ar에서부터 state와 그의 Socket을 가져옴
            Socket handler = state.sock;
            Byte[] Lb_Read;

            int bytesRead;

            try
            {
                bytesRead = handler.EndReceive(ar); // socket에서 데이타를 읽음
            }
            catch (SocketException)
            {
                RemoveClient(handler);
                return;
            }
            catch (ObjectDisposedException)
            {
                RemoveClient(handler);
                return;
            }

            if (bytesRead > 0)
            {
                Lb_Read = new Byte[bytesRead];
                Utility.ByteCopy(state.buffer, 0, Lb_Read, 0, bytesRead);
                //for (int i = 0; i < bytesRead; i++){
                //    Lb_Read[i] = state.buffer[i];
                //}




                //state.sb.Append(Encoding.Default.GetString(state.buffer, 0, bytesRead)); // 버퍼에서 data를 읽어서 sb 뒤에다가 붙임. (혹시 저장된 데이타가 있을까바서리)
                //state.sb.Append(Encoding.GetEncoding(51949).GetString(state.buffer, 0, bytesRead)); // 버퍼에서 data를 읽어서 sb 뒤에다가 붙임. (혹시 저장된 데이타가 있을까바서리)

                //string content = state.sb.ToString();  // 51949 : euc-kr

                //String content="";
                //for(int i=0; i<bytesRead; i++)
                //{
                //       content += String.Format("{0:C}",(Char)state.buffer[i]);
                //}
                //Lb_Read = Encoding.Default.GetBytes(content);
                //String content = (Encoding.Default.GetString(state.buffer, 0, bytesRead));

                //string content = buffer.ToString();



                //Console.WriteLine("Read {0} bytes from socket. \n Data : {1}", content.Length, content); // 읽어온 Data를 출력
                Byte[] result = ReceiveWork(handler, Lb_Read, this);

                //if (result.IndexOf("<EOF>") > -1)
                //{
                //    RemoveClient(handler.RemoteEndPoint.ToString());

                //    return;
                //}
                /*
                if (ListClient.Count > 0)
                {
                    ArrayList socks = new ArrayList();
                    foreach (object obj in ListClient.Values)
                    {
                        socks.Add(obj);
                    }
                    foreach (object obj in socks)
                    {
                        Send((Socket)obj, result);
                    }
                }
                */

                Send(handler, result); // Client Socket으로 읽어온 Data 전송
            }
        }

        /// <summary>
        /// Data를 Client로 전송
        /// </summary>
        /// <param name="handler">Client의 Socket</param>
        /// <param name="data">보낼 내용</param>
        public void Send(Socket handler, String data)
        {
            byte[] byteData = Encoding.GetEncoding(51949).GetBytes(data); // 스트링을 바이트배열로

            try
            {
                handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), handler);

                recvDone.WaitOne();

                StateObject state = new StateObject(handler);

                handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
            }
            catch (SocketException)
            {
                RemoveClient(handler);
            }
        }

        /// <summary>
        /// Data를 Client로 전송
        /// </summary>
        /// <param name="handler">Client의 Socket</param>
        /// <param name="data">보낼 내용</param>
        public void Send(Socket handler, Byte[] data)
        {
            //if (data == null) return;

            byte[] byteData = data; // 스트링을 바이트배열로

            try
            {
                if (data != null)
                {
                    handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), handler);
                    recvDone.WaitOne();
                }


                StateObject state = new StateObject(handler);

                handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
            }
            catch (SocketException)
            {
                RemoveClient(handler);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Send 이후에 호출되는 callback 함수
        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                if (!((Socket)ar.AsyncState).Connected) { return; }
                // Retrieve the socket from the state object.
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = handler.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to client.", bytesSent);

                recvDone.Set();

            }
            catch (Exception e) { Console.WriteLine(e.ToString()); }
        }

        /// <summary>
        /// 해당 Client의 접속을 종료
        /// </summary>
        /// <param name="sock">접속 종료할 Client의 Socket</param>
        public void Disconnect(Socket sock)
        {
            if (sock.Connected)
            {
                sock.Shutdown(SocketShutdown.Both);
            }
            sock.Close();
        }

        #region IDisposable 멤버

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
