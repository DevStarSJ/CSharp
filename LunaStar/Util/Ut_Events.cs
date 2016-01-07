using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LunaStar.Util
{
    public class Ut_Events
    {
        public static void TextBox_IgnoreQuotation(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\''|| e.KeyChar == '\"' )
            {
                e.Handled = true;
            }
        }
    }
}
