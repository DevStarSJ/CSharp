using System;
using System.Collections;

namespace LunaStar.Util
{
    public class Ut_Controls
    {
        public static void getControls(System.Windows.Forms.Control.ControlCollection Ocontrol, ref ArrayList Space)
        {
            try
            {
                for (int i = 0; i < Ocontrol.Count; i++)
                {
                    Space.Add(Ocontrol[i]);

                    if (Ocontrol[i].Controls.Count > 0)
                        getControls(Ocontrol[i].Controls, ref Space);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
