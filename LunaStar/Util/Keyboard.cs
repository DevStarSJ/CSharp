using System;

namespace LunaStar.Util
{
    public class Keyboard
    {
        public static Boolean IsCtrlV(Char Pc_Key)
        {
            return (Pc_Key == (char)22) ? true : false;
        }

        public static Boolean IsCtrlC(Char Pc_Key)
        {
            return (Pc_Key == (char)3) ? true : false;
        }
    }
}
