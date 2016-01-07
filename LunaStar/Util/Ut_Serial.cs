using System;
using System.Collections.Generic;
using System.IO.Ports;
using Globals;
using LunaStar.Globals;

namespace LunaStar.Util
{
    public class Ut_Serial
    {
        private SerialPort Msp_Serial = null;



        public Ut_Serial()
        {
        }

        public void Open(int Pi_Port, int Pi_BaudRate)
        {
            Open(Pi_Port, Pi_BaudRate, Parity.None, 8, StopBits.One);
        }

        public void Open(int Pi_Port, int Pi_BaudRate, Parity Po_Parity, int Pi_DataBit, StopBits Po_StopBit)
        {
            Msp_Serial = new SerialPort();
            Msp_Serial.PortName = "COM" + Pi_Port.ToString();
            Msp_Serial.BaudRate = Pi_BaudRate;
            Msp_Serial.Parity = Po_Parity;
            Msp_Serial.DataBits = Pi_DataBit;
            Msp_Serial.StopBits = Po_StopBit;

            Msp_Serial.ReceivedBytesThreshold = 1;
            Msp_Serial.Handshake = Handshake.None;

        }

        public void Close()
        {
            if (Msp_Serial != null)
            {
                Msp_Serial.Close();
            }
        }

        public string Send(string Ps_Address)
        {
            string Ls_Ret = "";
            string Ls_Command = "";

            //Ls_Command = ENQ + "00RSS01" + "07%MX0000" + EOT;

            Ls_Command = Consts.ENQ
                + Ps_Address
                + Consts.EOT;

            Msp_Serial.Write(Ls_Command);
            do
            {
            } while (Msp_Serial.WriteBufferSize == 0);


            Ls_Ret = Read();

            return Ls_Ret;
        }

        private string Read()
        {
            string Ls_Ret = "";
            bool Lb_IsExit = false;
            string Ls_Data = string.Empty;
            DateTime Lt_start = DateTime.Now;

            do
            {
                string Ls_Msg = Msp_Serial.ReadExisting();
                Ls_Data += Ls_Msg;

                if (Ls_Msg.IndexOf(Consts.ETX) > 0)
                {
                    if (Ls_Data[0] == Consts.ACK)
                    {
                        //들어오는 데이타를 분석..[ETX(1)+국번(2)+비트읽기(3)+블륵수(2)]

                        Ls_Ret = Ls_Data.Substring(8, Ls_Data.Length - 9); //실제Data

                        Lb_IsExit = true;
                    }
                    else if (Ls_Data[0] == Consts.NAK)
                    {
                        Ls_Ret = "NAK";

                        Lb_IsExit = true;
                    }
                }

                DateTime Lt_End = DateTime.Now;

                int Li_Diff = (Lt_End - Lt_start).Milliseconds;

                //if (Li_Diff > Statics.TIME_OUT_TICKS)
                //{
                //    Ls_Ret = "TimeOut";
                //    Lb_IsExit = true;
                //}

            } while (!Lb_IsExit);

            return Ls_Ret;
        }
    }

    public class IndicatorManager
    {
        #region Const
        private const int DRS_STX_LEN = 1;          // STX
        private const int DRS_CFLF_LEN = 1;         // 종료 문자
        private const int DRS_CMD_LEN = 3;          // Command
        private const int DRS_ADDRESS_LEN = 2;      // Address                    

        private const byte STX = 0x02;
        private const byte ETX = 0x03;
        private const byte CR = 0x0D;
        private const byte LF = 0x0A;

        //수신 Frame
        private const int DRS_R_STATUS_LEN = 2;     // 응답 상태
        private const int DRS_R_DATA_LEN = 2;       // 데이터

        //송신 Frame
        private const int DRS_S_TOTAL_LEN = 14;     // 전체 길이
        private const int DRS_S_DATACNT_LEN = 2;    // 수신 받을 데이터 겟수
        private const int DRS_S_DREG_LEN = 4;       // 시작 D Register
        #endregion

        public enum IndicatorType
        {
            NX4 = 0,
            MP3 = 1
        }

        public delegate void dele_Recv(object sender, SerialDataReceivedEventArgs e);

        private List<IndicatorInfo> Mmi_ItemList;
        private IndicatorComm Mic_Comm;

        public IndicatorManager()
        {
            Mic_Comm = new IndicatorComm(DataReceived);
            Mmi_ItemList = new List<IndicatorInfo>();
            Init_Indicator();
        }

        private void Init_Indicator()
        {
            Mmi_ItemList.Add(new IndicatorInfo("A", 1, IndicatorType.NX4));
            Mmi_ItemList.Add(new IndicatorInfo("A", 2, IndicatorType.MP3));
            Mmi_ItemList.Add(new IndicatorInfo("A", 3, IndicatorType.NX4));
            Mmi_ItemList.Add(new IndicatorInfo("A", 4, IndicatorType.MP3));
            Mmi_ItemList.Add(new IndicatorInfo("A", 5, IndicatorType.NX4));
            Mmi_ItemList.Add(new IndicatorInfo("A", 6, IndicatorType.MP3));
            Mmi_ItemList.Add(new IndicatorInfo("A", 7, IndicatorType.NX4));
            Mmi_ItemList.Add(new IndicatorInfo("A", 8, IndicatorType.NX4));
            Mmi_ItemList.Add(new IndicatorInfo("A", 9, IndicatorType.MP3));
            Mmi_ItemList.Add(new IndicatorInfo("A", 10, IndicatorType.NX4)); 
        }

        public void Write()
        {
            Mic_Comm.Write(Mmi_ItemList[0].Get_WriteMsg());
            Mic_Comm.Write(Mmi_ItemList[1].Get_WriteMsg());
            Mic_Comm.Write(Mmi_ItemList[2].Get_WriteMsg());
            Mic_Comm.Write(Mmi_ItemList[3].Get_WriteMsg());
            Mic_Comm.Write(Mmi_ItemList[4].Get_WriteMsg());
            Mic_Comm.Write(Mmi_ItemList[5].Get_WriteMsg());
            Mic_Comm.Write(Mmi_ItemList[6].Get_WriteMsg());
            Mic_Comm.Write(Mmi_ItemList[7].Get_WriteMsg());
            Mic_Comm.Write(Mmi_ItemList[8].Get_WriteMsg());
            Mic_Comm.Write(Mmi_ItemList[9].Get_WriteMsg());            
        }

        private byte[] ConvertIntToByteArray(int Pi_Value, int Pi_Length)
        {
            string Ls_Value = string.Empty;
            Byte[] Lby_Result = null;
            char[] Lc_HexList = null;
            int Li_CharIndex = 0;

            Lby_Result = new byte[Pi_Length];
            Ls_Value = Pi_Value.ToString("X");

            if (Ls_Value.Length > 0)
            {
                Lc_HexList = Ls_Value.ToCharArray();
                Li_CharIndex = Lc_HexList.Length - 1;

                for (int Li_Index = Pi_Length - 1; Li_Index >= 0; Li_Index--, Li_CharIndex--)
                {
                    if (Lby_Result.Length > Lc_HexList.Length)
                    {
                        if (Li_CharIndex < 0)
                            Lby_Result[Li_Index] = 0x00;
                        else
                            Lby_Result[Li_Index] = (byte)Lc_HexList[Li_CharIndex];
                    }
                    else
                    {

                    }
                }

                //Lby_Result = 
            }

            return Lby_Result;
        }            

        public void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            String Ls_Msg = Mic_Comm.Read();
        }

        //각각의 관리 항목의 인디게이터 정보
        public class IndicatorInfo
        {            
            private string Ms_Group;
            private int Mi_Address;            
            private IndicatorType Me_IndicatorType;

            public IndicatorInfo(string Ps_Group, int Pi_Address, IndicatorType Pe_Type)
            {                
                Ms_Group = Ps_Group;
                Mi_Address = Pi_Address;
                Me_IndicatorType = Pe_Type;
            }

            public byte[] Get_WriteMsg()
            {
                string Ls_Result = "";
                byte[] Lby_Result = null;

                switch (Me_IndicatorType)
                {
                    case IndicatorType.NX4:
                        Ls_Result = string.Format("{0}{1},{2},{3}",Mi_Address.ToString("00"), "DRS", "01","0001");
                        Lby_Result = ConvertStrToByteNX4(Ls_Result);
                        break;
                    case IndicatorType.MP3:
                        Ls_Result = string.Format("{0}{1}{2}", Mi_Address.ToString("00"), "R", "18");
                        Lby_Result = ConvertStrToByteMp3(Ls_Result);
                        break;
                }

                return Lby_Result;
            }

            private byte[] ConvertStrToByteNX4(string Ps_Value)
            {
                char[] Lch_Value = null;
                byte[] Lby_Result = null;

                Lch_Value = Ps_Value.ToCharArray();
                Lby_Result = new byte[Lch_Value.Length + 3];

                Lby_Result[0] = STX;

                for (int Li_Index = 1; Lch_Value.Length >= Li_Index; Li_Index++)
                {
                    Lby_Result[Li_Index] = (byte)Lch_Value[Li_Index - 1];
                }

                Lby_Result[Lby_Result.Length - 1] = CR;
                Lby_Result[Lby_Result.Length - 2] = LF;

                return Lby_Result;
            }

            private byte[] ConvertStrToByteMp3(string Ps_Value)
            {
                char[] Lch_Value = null;
                byte[] Lby_Result = null;

                Lch_Value = Ps_Value.ToCharArray();
                Lby_Result = new byte[Lch_Value.Length + 2];

                Lby_Result[0] = STX;

                for (int Li_Index = 1; Lch_Value.Length >= Li_Index; Li_Index++)
                {
                    Lby_Result[Li_Index] = (byte)Lch_Value[Li_Index - 1];
                }

                Lby_Result[Lby_Result.Length - 1] = ETX;
                return Lby_Result;
            }            
        }        

        //232 통신용
        public class IndicatorComm
        {            
            private SerialPort Msp_Port = null;
            string strz = string.Empty;
            public IndicatorComm(dele_Recv RecvMsg)
            {
                try
                {
                    Msp_Port = new SerialPort("COM1", 9600, Parity.None, 8, StopBits.One);
                    Msp_Port.DataReceived += new SerialDataReceivedEventHandler(RecvMsg);

                    if (!Msp_Port.IsOpen)
                    {
                        Msp_Port.Open();
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.Print(e.Message);
                }            
            }

            public string Read()
            {
                return Msp_Port.ReadExisting();
            }

            public void Write(byte[] Pby_Data)
            {                
                Msp_Port.Write(Pby_Data, 0, Pby_Data.Length);

                for (int Li_Cnt = 0; Pby_Data.Length > Li_Cnt; Li_Cnt++)
                {
                    strz += ((char)Pby_Data[Li_Cnt]).ToString();                    
                }

                System.Diagnostics.Debug.Print(strz);
                strz = "";
            }
        }
    }

}
