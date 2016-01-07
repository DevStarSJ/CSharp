using System;

namespace LunaStar.Globals
{
    public class Consts
    {
        public const string VersionInfo = "1.0.0.2";

        /*
         *  * 2013-01-18 - 1.0.0.2
         *  - Util.FileIO.GetBytes() : 최적화 작업
         *  - Util.Ut_String : VersionCompare(), GetContainsCount() 추가, Enums.EM_StringCompare 에 ERROR 추가 
         * 
         * 
         */

        // 시리얼통신
        public const char STX = (char)0x02;
        public const char ETX = (char)0x03; //End Text [응답용Asc]
        public const char EOT = (char)0x04; //End of Text[요구용 Asc]
        public const char ENQ = (char)0x05; //Enquire[프레임시작코드]
        public const char ACK = (char)0x06; //Acknowledge[응답 시작]
        public const char NAK = (char)0x15; //Not Acknoledge[에러응답시작]

        public const char CR = (char)0x0D;
        public const char LF = (char)0x0A;

        /// <summary>
        /// SPC+
        /// </summary>
        public const String HeaderMsg = "SPC+";


        public const double SDT_N3SIGMA = 0.0014;
        public const double SDT_N2SIGMA = 0.0228;
        public const double SDT_N1SIGMA = 0.1587;
        public const double SDT_MEAN    = 0.5;
        public const double SDT_P1SIGMA = 0.8443;
        public const double SDT_P2SIGMA = 0.9772;
        public const double SDT_P3SIGMA = 0.9986;

    }
}
