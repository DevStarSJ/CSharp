using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace LunaStar.Util
{
    public class Ut_Stat
    {
        public Ut_Stat()
        {
        }

        public double GetMax(List<double> Pd_Value)
        {
            double Ld_Ret = 0;

            if (Pd_Value.Count > 0)
            {
                Ld_Ret = Pd_Value[0];

                for (int Li_Cnt = 0; Li_Cnt < Pd_Value.Count; Li_Cnt++)
                {
                    if (Ld_Ret < Pd_Value[Li_Cnt])
                    {
                        Ld_Ret = Pd_Value[Li_Cnt];
                    }
                }
            }
            return Ld_Ret;
        }

        public double GetMax(double[] Pd_Value)
        {
            double Ld_Ret = 0;

            if (Pd_Value.Length > 0)
            {
                Ld_Ret = Pd_Value[0];

                for (int Li_Cnt = 0; Li_Cnt < Pd_Value.Length; Li_Cnt++)
                {
                    if (Ld_Ret < Pd_Value[Li_Cnt])
                    {
                        Ld_Ret = Pd_Value[Li_Cnt];
                    }
                }
            }
            return Ld_Ret;
        }


        public double GetMax(DataTable Po_Table, string Ps_ColName)
        {
            double Ld_Ret = 0;

            if (Po_Table != null)
            {

                if (Po_Table.Rows.Count > 0)
                {
                    Ld_Ret = double.Parse(Po_Table.Rows[0][Ps_ColName].ToString());

                    for (int Li_Cnt = 0; Li_Cnt < Po_Table.Rows.Count; Li_Cnt++)
                    {
                        if (Ld_Ret < double.Parse(Po_Table.Rows[Li_Cnt][Ps_ColName].ToString()))
                        {
                            Ld_Ret = double.Parse(Po_Table.Rows[Li_Cnt][Ps_ColName].ToString());
                        }
                    }
                }
            }
            return Ld_Ret;
        }

        public double GetMax(DataTable Po_Table, int Pi_ColIndex)
        {

            double Ld_Ret = 0;
            if (Po_Table != null)
            {

                if (Po_Table.Rows.Count > 0)
                {
                    Ld_Ret = double.Parse(Po_Table.Rows[0][Pi_ColIndex].ToString());


                    for (int Li_Cnt = 0; Li_Cnt < Po_Table.Rows.Count; Li_Cnt++)
                    {
                        if (Ld_Ret < double.Parse(Po_Table.Rows[Li_Cnt][Pi_ColIndex].ToString()))
                        {
                            Ld_Ret = double.Parse(Po_Table.Rows[Li_Cnt][Pi_ColIndex].ToString());
                        }
                    }
                }
            }
            return Ld_Ret;
        }

        public double GetMin(double[] Pd_Value)
        {
            double Ld_Ret = 0;
            if (Pd_Value.Length > 0)
            {
                Ld_Ret = Pd_Value[0];

                for (int Li_Cnt = 0; Li_Cnt < Pd_Value.Length; Li_Cnt++)
                {
                    if (Ld_Ret > Pd_Value[Li_Cnt])
                    {
                        Ld_Ret = Pd_Value[Li_Cnt];
                    }
                }
            }
            return Ld_Ret;
        }


        public double GetMin(List<double> Pd_Value)
        {
            double Ld_Ret = 0;
            if (Pd_Value.Count > 1)
            {
                Ld_Ret = Pd_Value[0];

                for (int Li_Cnt = 0; Li_Cnt < Pd_Value.Count; Li_Cnt++)
                {
                    if (Ld_Ret > Pd_Value[Li_Cnt])
                    {
                        Ld_Ret = Pd_Value[Li_Cnt];
                    }
                }
            }
            return Ld_Ret;
        }

        public double GetMin(DataTable Po_Table, string Ps_ColName)
        {
            double Ld_Ret = 0;
            if (Po_Table != null)
            {
                if (Po_Table.Rows.Count > 0)
                {
                    Ld_Ret = double.Parse(Po_Table.Rows[0][Ps_ColName].ToString());

                    for (int Li_Cnt = 0; Li_Cnt < Po_Table.Rows.Count; Li_Cnt++)
                    {
                        if (Ld_Ret > double.Parse(Po_Table.Rows[Li_Cnt][Ps_ColName].ToString()))
                        {
                            Ld_Ret = double.Parse(Po_Table.Rows[Li_Cnt][Ps_ColName].ToString());
                        }
                    }
                }
            }
            return Ld_Ret;
        }

        public double GetMin(DataTable Po_Table, int Pi_ColIndex)
        {
            double Ld_Ret = 0;
            if (Po_Table != null)
            {
                if (Po_Table.Rows.Count > 0)
                {
                    Ld_Ret = double.Parse(Po_Table.Rows[0][Pi_ColIndex].ToString());

                    for (int Li_Cnt = 0; Li_Cnt < Po_Table.Rows.Count; Li_Cnt++)
                    {
                        if (Ld_Ret > double.Parse(Po_Table.Rows[Li_Cnt][Pi_ColIndex].ToString()))
                        {
                            Ld_Ret = double.Parse(Po_Table.Rows[Li_Cnt][Pi_ColIndex].ToString());
                        }
                    }
                }
            }
            return Ld_Ret;
        }

        public double GetStdev(DataTable Po_Table, int Pi_ColIndex)
        {
            double Ld_Ret = 0;
            double Ld_Avg = 0;

            Ld_Avg = GetAvg(Po_Table, Pi_ColIndex);
            Ld_Ret = GetStdev(Po_Table, Pi_ColIndex, Ld_Avg);

            return Ld_Ret;
        }


        public double GetStdev(DataTable Po_Table, int Pi_ColIndex, double Pd_Avg)
        {
            double Ld_Ret = 0;
            double Ld_tmp = 0;

            if (Po_Table.Rows.Count > 1)
            {
                if (Po_Table != null)
                {
                    for (int Li_Cnt = 0; Li_Cnt < Po_Table.Rows.Count; Li_Cnt++)
                    {
                        Ld_tmp = double.Parse(Po_Table.Rows[Li_Cnt][Pi_ColIndex].ToString()) - Pd_Avg;
                        Ld_Ret += (Ld_tmp * Ld_tmp);
                    }

                    Ld_Ret /= (Po_Table.Rows.Count - 1);

                    Ld_Ret = Math.Sqrt(Ld_Ret);
                }
            }
            return Ld_Ret;
        }



        public double GetStdev(DataTable Po_Table, string Ps_ColName)
        {
            double Ld_Ret = 0;
            double Ld_Avg = 0;

            Ld_Avg = GetAvg(Po_Table, Ps_ColName);
            Ld_Ret = GetStdev(Po_Table, Ps_ColName, Ld_Avg);
            return Ld_Ret;
        }


        public double GetStdev(DataTable Po_Table, string Ps_ColName, double Pd_Avg)
        {
            double Ld_Ret = 0;
            double Ld_tmp = 0;

            if (Po_Table.Rows.Count > 1)
            {
                if (Po_Table != null)
                {
                    for (int Li_Cnt = 0; Li_Cnt < Po_Table.Rows.Count; Li_Cnt++)
                    {
                        Ld_tmp = double.Parse(Po_Table.Rows[Li_Cnt][Ps_ColName].ToString()) - Pd_Avg;
                        Ld_Ret += (Ld_tmp * Ld_tmp);
                    }

                    Ld_Ret /= (Po_Table.Rows.Count - 1);
                    Ld_Ret = Math.Sqrt(Ld_Ret);
                }
            }
            return Ld_Ret;
        }

        public double GetStdev(List<double> Pd_Value)
        {
            double Ld_Ret = 0;
            double Ld_Avg = GetAvg(Pd_Value);

            Ld_Ret = GetStdev(Pd_Value, Ld_Avg);
            return Ld_Ret;
        }

        public double GetStdev(double[] Pd_Value)
        {
            double Ld_Ret = 0;
            double Ld_Avg = GetAvg(Pd_Value);

            Ld_Ret = GetStdev(Pd_Value, Ld_Avg);
            return Ld_Ret;
        }

        public double GetStdev(List<double> Pd_Value, double Pd_Avg)
        {
            double Ld_Ret = 0;
            double Ld_tmp = 0;

            if (Pd_Value.Count > 1)
            {
                for (int Li_Cnt = 0; Li_Cnt < Pd_Value.Count; Li_Cnt++)
                {

                    Ld_tmp = Pd_Value[Li_Cnt] - Pd_Avg;
                    Ld_Ret += (Ld_tmp * Ld_tmp);
                }

                Ld_Ret /= (Pd_Value.Count - 1);
                Ld_Ret = Math.Sqrt(Ld_Ret);
            }
            return Ld_Ret;
        }

        public double GetStdev(double[] Pd_Value, double Pd_Avg)
        {
            double Ld_Ret = 0;
            double Ld_tmp = 0;

            if (Pd_Value.Length > 1)
            {
                for (int Li_Cnt = 0; Li_Cnt < Pd_Value.Length; Li_Cnt++)
                {

                    Ld_tmp = Pd_Value[Li_Cnt] - Pd_Avg;
                    Ld_Ret += (Ld_tmp * Ld_tmp);
                }

                Ld_Ret /= (Pd_Value.Length - 1);
                Ld_Ret = Math.Sqrt(Ld_Ret);
            }
            return Ld_Ret;
        }

        public double GetAvg(List<double> Pd_Value)
        {
            double Ld_Ret = 0;
            if (Pd_Value.Count > 1)
            {
                for (int Li_Cnt = 0; Li_Cnt < Pd_Value.Count; Li_Cnt++)
                {
                    Ld_Ret += Pd_Value[Li_Cnt];
                }
                Ld_Ret /= Pd_Value.Count;
            }
            return Ld_Ret;
        }

        public double GetAvg(double[] Pd_Value)
        {
            double Ld_Ret = 0;
            if (Pd_Value.Length > 1)
            {
                for (int Li_Cnt = 0; Li_Cnt < Pd_Value.Length; Li_Cnt++)
                {
                    Ld_Ret += Pd_Value[Li_Cnt];
                }
                Ld_Ret /= Pd_Value.Length;
            }
            return Ld_Ret;
        }

        public double GetAvg(DataTable Po_Table, int Pi_ColIndex)
        {
            double Ld_Ret = 0;

            if (Po_Table.Rows.Count > 0)
            {
                if (Po_Table != null)
                {
                    Ld_Ret = GetSum(Po_Table, Pi_ColIndex) / Po_Table.Rows.Count;
                }
            }

            return Ld_Ret;
        }

        public double GetAvg(DataTable Po_Table, string Ps_ColName)
        {
            double Ld_Ret = 0;

            if (Po_Table.Rows.Count > 0)
            {
                if (Po_Table != null)
                {
                    Ld_Ret = GetSum(Po_Table, Ps_ColName) / Po_Table.Rows.Count;
                }
            }

            return Ld_Ret;
        }

        public double GetSum(List<double> Pd_Value)
        {
            double Ld_Ret = 0;

            if (Pd_Value.Count > 0)
            {
                for (int Li_Cnt = 0; Li_Cnt < Pd_Value.Count; Li_Cnt++)
                {
                    Ld_Ret += Pd_Value[Li_Cnt];
                }
            }
            return Ld_Ret;
        }

        public double GetSum(double[] Pd_Value)
        {
            double Ld_Ret = 0;

            if (Pd_Value.Length > 0)
            {
                for (int Li_Cnt = 0; Li_Cnt < Pd_Value.Length; Li_Cnt++)
                {
                    Ld_Ret += Pd_Value[Li_Cnt];
                }
            }
            return Ld_Ret;
        }


        public double GetSum(DataTable Po_Table, int Pi_ColIndex)
        {
            double Ld_Ret = 0;
            if (Po_Table != null)
            {
                for (int Li_Cnt = 0; Li_Cnt < Po_Table.Rows.Count; Li_Cnt++)
                {
                    Ld_Ret += double.Parse(Po_Table.Rows[Li_Cnt][Pi_ColIndex].ToString());
                }
            }
            return Ld_Ret;
        }


        public double GetSum(DataTable Po_Table, string Ps_ColName)
        {
            double Ld_Ret = 0;
            if (Po_Table != null)
            {
                for (int Li_Cnt = 0; Li_Cnt < Po_Table.Rows.Count; Li_Cnt++)
                {
                    Ld_Ret += double.Parse(Po_Table.Rows[Li_Cnt][Ps_ColName].ToString());
                }
            }
            return Ld_Ret;
        }

        public double GetQsm(double[] Pd_Value)
        {
            double Ld_Ret = 0;

            if (Pd_Value.Length > 0)
            {
                for (int Li_Cnt = 0; Li_Cnt < Pd_Value.Length; Li_Cnt++)
                {
                    Ld_Ret += (Pd_Value[Li_Cnt] * Pd_Value[Li_Cnt]);
                }
            }
            return Ld_Ret;
        }

        public double GetQsm(List<double> Pd_Value)
        {
            double Ld_Ret = 0;

            if (Pd_Value.Count > 0)
            {
                for (int Li_Cnt = 0; Li_Cnt < Pd_Value.Count; Li_Cnt++)
                {
                    Ld_Ret += (Pd_Value[Li_Cnt] * Pd_Value[Li_Cnt]);
                }
            }
            return Ld_Ret;
        }

        public double GetQsm(DataTable Po_Table, string Ps_ColName)
        {
            double Ld_Ret = 0;

            if (Po_Table != null)
            {
                for (int Li_Cnt = 0; Li_Cnt < Po_Table.Rows.Count; Li_Cnt++)
                {
                    Ld_Ret += double.Parse(Po_Table.Rows[Li_Cnt][Ps_ColName].ToString()) * double.Parse(Po_Table.Rows[Li_Cnt][Ps_ColName].ToString());
                }
            }
            return Ld_Ret;
        }


        // 첨도  (뾰족한 정도)
        public double GetKurt(DataTable Po_Table, string Ps_ColName, double Pd_Avg, double Pd_Stdev)
        {
            double Ld_Ret = 0;
            double Ld_Qsm = 0;
            double Ld_A = 0;
            double Ld_B = 0;

            if ((Po_Table.Rows.Count - 1) > 0)
            {
                if (Po_Table != null)
                {
                    int Li_N = Po_Table.Rows.Count;

                    for (int Li_Cnt = 0; Li_Cnt < Po_Table.Rows.Count; Li_Cnt++)
                    {
                        Ld_Qsm += (((Convert.ToDouble(Po_Table.Rows[Li_Cnt][Ps_ColName]) - Pd_Avg) / Pd_Stdev) *
                                   ((Convert.ToDouble(Po_Table.Rows[Li_Cnt][Ps_ColName]) - Pd_Avg) / Pd_Stdev) *
                                   ((Convert.ToDouble(Po_Table.Rows[Li_Cnt][Ps_ColName]) - Pd_Avg) / Pd_Stdev) *
                                   ((Convert.ToDouble(Po_Table.Rows[Li_Cnt][Ps_ColName]) - Pd_Avg) / Pd_Stdev));
                    }

                    Ld_A = (Li_N * (Li_N + 1) * 1.0) / ((Li_N - 1) * (Li_N - 2) * (Li_N - 3) * 1.0);
                    Ld_B = (3.0 * (Li_N - 1) * (Li_N - 1)) / ((Li_N - 2) * (Li_N - 3) * 1.0);
                    Ld_Ret = Ld_A * Ld_Qsm - Ld_B;
                }
            }

            return Ld_Ret;
        }

        public double GetKurt(DataTable Po_Table, string Ps_ColName)
        {
            double Ld_Ret = 0;
            double Ld_Avg = 0;
            double Ld_Stdev = 0;

            Ld_Avg = GetAvg(Po_Table, Ps_ColName);
            Ld_Stdev = GetStdev(Po_Table, Ps_ColName, Ld_Stdev);
            Ld_Ret = GetKurt(Po_Table, Ps_ColName, Ld_Avg, Ld_Stdev);

            return Ld_Ret;
        }


        public double GetKurt(DataTable Po_Table, int Pi_ColIndex, double Pd_Avg, double Pd_Stdev)
        {
            double Ld_Ret = 0;
            double Ld_Qsm = 0;
            double Ld_A = 0;
            double Ld_B = 0;

            if ((Po_Table.Rows.Count - 1) > 0)
            {
                if (Po_Table != null)
                {
                    int Li_N = Po_Table.Rows.Count;

                    for (int Li_Cnt = 0; Li_Cnt < Po_Table.Rows.Count; Li_Cnt++)
                    {
                        Ld_Qsm += (((Convert.ToDouble(Po_Table.Rows[Li_Cnt][Pi_ColIndex]) - Pd_Avg) / Pd_Stdev) *
                                   ((Convert.ToDouble(Po_Table.Rows[Li_Cnt][Pi_ColIndex]) - Pd_Avg) / Pd_Stdev) *
                                   ((Convert.ToDouble(Po_Table.Rows[Li_Cnt][Pi_ColIndex]) - Pd_Avg) / Pd_Stdev) *
                                   ((Convert.ToDouble(Po_Table.Rows[Li_Cnt][Pi_ColIndex]) - Pd_Avg) / Pd_Stdev));
                    }

                    Ld_A = (Li_N * (Li_N - 1)) / ((Li_N - 1) * (Li_N - 2) * (Li_N - 3));
                    Ld_B = (3 * (Li_N - 1) * (Li_N - 1)) / ((Li_N - 2) * (Li_N - 3));
                    Ld_Ret = Ld_A * Ld_Qsm - Ld_B;
                }
            }

            return Ld_Ret;
        }

        public double GetKurt(DataTable Po_Table, int Pi_ColIndex)
        {
            double Ld_Ret = 0;
            double Ld_Avg = 0;
            double Ld_Stdev = 0;

            Ld_Avg = GetAvg(Po_Table, Pi_ColIndex);
            Ld_Stdev = GetStdev(Po_Table, Pi_ColIndex, Ld_Stdev);
            Ld_Ret = GetKurt(Po_Table, Pi_ColIndex, Ld_Avg, Ld_Stdev);
            return Ld_Ret;
        }



        // 왜도 ( 기울진 정도)
        public double GetSkew(DataTable Po_Table, string Ps_ColName, double Pd_Avg, double Pd_Stdev)
        {
            double Ld_Ret = 0;
            double Ld_Qsm = 0;
            double Ld_A = 0;


            if ((Po_Table.Rows.Count - 1) > 0)
            {
                if (Po_Table != null)
                {
                    int Li_N = Po_Table.Rows.Count;

                    for (int Li_Cnt = 0; Li_Cnt < Po_Table.Rows.Count; Li_Cnt++)
                    {
                        Ld_Qsm += (((Convert.ToDouble(Po_Table.Rows[Li_Cnt][Ps_ColName]) - Pd_Avg) / Pd_Stdev) *
                                   ((Convert.ToDouble(Po_Table.Rows[Li_Cnt][Ps_ColName]) - Pd_Avg) / Pd_Stdev) *
                                   ((Convert.ToDouble(Po_Table.Rows[Li_Cnt][Ps_ColName]) - Pd_Avg) / Pd_Stdev));
                    }

                    Ld_A = (Li_N * 1.0) / ((Li_N - 1) * (Li_N - 2) * 1.0);
                    Ld_Ret = Ld_A * Ld_Qsm;
                }
            }

            return Ld_Ret;
        }

        public double GetSkew(DataTable Po_Table, string Ps_ColName)
        {
            double Ld_Ret = 0;
            double Ld_Avg = 0;
            double Ld_Stdev = 0;

            Ld_Avg = GetAvg(Po_Table, Ps_ColName);
            Ld_Stdev = GetStdev(Po_Table, Ps_ColName, Ld_Stdev);
            Ld_Ret = GetSkew(Po_Table, Ps_ColName, Ld_Avg, Ld_Stdev);

            return Ld_Ret;
        }

        public double GetSkew(DataTable Po_Table, int Pi_ColIndex, double Pd_Avg, double Pd_Stdev)
        {
            double Ld_Ret = 0;
            double Ld_Qsm = 0;
            double Ld_A = 0;


            if ((Po_Table.Rows.Count - 1) > 0)
            {
                if (Po_Table != null)
                {
                    int Li_N = Po_Table.Rows.Count;

                    for (int Li_Cnt = 0; Li_Cnt < Po_Table.Rows.Count; Li_Cnt++)
                    {
                        Ld_Qsm += (((Convert.ToDouble(Po_Table.Rows[Li_Cnt][Pi_ColIndex]) - Pd_Avg) / Pd_Stdev) *
                                   ((Convert.ToDouble(Po_Table.Rows[Li_Cnt][Pi_ColIndex]) - Pd_Avg) / Pd_Stdev) *
                                   ((Convert.ToDouble(Po_Table.Rows[Li_Cnt][Pi_ColIndex]) - Pd_Avg) / Pd_Stdev));

                    }

                    Ld_A = (Li_N * 1.0) / ((Li_N - 1) * (Li_N - 2) * 1.0);
                    Ld_Ret = Ld_A * Ld_Qsm;
                }
            }

            return Ld_Ret;
        }


        public double GetSkew(DataTable Po_Table, int Pi_ColIndex)
        {
            double Ld_Ret = 0;
            double Ld_Avg = 0;
            double Ld_Stdev = 0;

            Ld_Avg = GetAvg(Po_Table, Pi_ColIndex);
            Ld_Stdev = GetStdev(Po_Table, Pi_ColIndex, Ld_Stdev);
            Ld_Ret = GetSkew(Po_Table, Pi_ColIndex, Ld_Avg, Ld_Stdev);
            return Ld_Ret;
        }









        public double GetQuartile(List<double> Plst_Data, double Pd_Rate)
        {
            double Ld_Ret = 0;


            if (Pd_Rate > 0 && Pd_Rate < 1)
            {
                string Ls_tmp = "";
                string[] Ls_Arr;
                int Li_J = 0;
                double Ld_G = 0;

                Ls_tmp = ((Plst_Data.Count - 1) * Pd_Rate).ToString();

                char[] split = { '.' };
                Ls_Arr = Ls_tmp.Split(split);

                Li_J = Convert.ToInt16(Ls_Arr[0]);
                Ld_G = Convert.ToDouble(Ls_Arr[1]);

                Ld_Ret = (1 - Ld_G) * Plst_Data[Li_J + 1] + Ld_G * Plst_Data[Li_J + 1];
            }
            return Ld_Ret;
        }


        /*
        Cp = (USL - LSL)/6*Std.Dev 
        Cp = (관리상한값 -관리하한값)/6*표준편차(Std.dev)
        Cpl = (Mean - LSL)/3*Std.dev
        공정능력 Cpl = (평균 - 관리하한값)/3*표준편차(Std.dev)이다
        Cpu = (USL-Mean)/3*Std.dev
        공정능력 Cpul = (관리상한값-평균)/3*표준편차(Std.dev)이다
        Cpk = Min(Cpl,Cpu)" Ranganadha Kumar
        */
        public double GetCp(double Pd_Stdev, double Pd_USL, double Pd_LSL)
        {
            double Ld_Ret = 0;

            if (Pd_Stdev > 0)
            {
                Ld_Ret = (Pd_USL - Pd_LSL) / (6 * Pd_Stdev);
            }
            return Ld_Ret;
        }

        public double GetCp(string Ps_SpecClass, double Pd_Stdev, double Pd_USL, double Pd_LSL)
        {
            double Ld_Ret = 0;

            if (Pd_Stdev > 0)
            {
                if (Ps_SpecClass == "U" || Ps_SpecClass == "L")
                    Ld_Ret = 0;
                else
                    Ld_Ret = (Pd_USL - Pd_LSL) / (6 * Pd_Stdev);
            }
            return Ld_Ret;
        }

        public double GetCpl(double Pd_Avg, double Pd_Stdev, double Pd_LSL)
        {
            double Ld_Ret = 0;

            if (Pd_Stdev > 0)
            {
                Ld_Ret = (Pd_Avg - Pd_LSL) / (3 * Pd_Stdev);
            }
            return Ld_Ret;
        }

        public double GetCpu(double Pd_Avg, double Pd_Stdev, double Pd_USL)
        {
            double Ld_Ret = 0;

            if (Pd_Stdev > 0)
            {
                Ld_Ret = (Pd_USL - Pd_Avg) / (3 * Pd_Stdev);
            }
            return Ld_Ret;
        }

        public double GetCpk(double Pd_Avg, double Pd_Stdev, double Pd_USL, double Pd_LSL)
        {
            double Ld_Ret = 0;
            double Ld_Cpl = 0;
            double Ld_Cpu = 0;

            if (Pd_Stdev > 0)
            {
                Ld_Cpl = GetCpl(Pd_Avg, Pd_Stdev, Pd_LSL);
                Ld_Cpu = GetCpu(Pd_Avg, Pd_Stdev, Pd_USL);
                Ld_Ret = Math.Min(Ld_Cpl, Ld_Cpu);
            }
            return Ld_Ret;
        }

        public double GetCpk(string Ps_SpecClass, double Pd_Avg, double Pd_Stdev, double Pd_USL, double Pd_LSL)
        {
            double Ld_Ret = 0;
            double Ld_Cpl = 0;
            double Ld_Cpu = 0;

            if (Pd_Stdev > 0)
            {
                Ld_Cpl = GetCpl(Pd_Avg, Pd_Stdev, Pd_LSL);
                Ld_Cpu = GetCpu(Pd_Avg, Pd_Stdev, Pd_USL);

                if (Ps_SpecClass == "U")
                {
                    Ld_Ret = Ld_Cpu;
                }
                else if (Ps_SpecClass == "L")
                {
                    Ld_Ret = Ld_Cpl;
                }
                else
                {
                    Ld_Ret = Math.Min(Ld_Cpl, Ld_Cpu);
                }
            }
            return Ld_Ret;
        }

        public double GetCpkStdev(string Ps_SpecClass, double Pd_Avg, double Pd_Cpk, double Pd_USL, double Pd_LSL)
        {
            double Ld_Ret = 0;
            double Ld_CplStdev = 0;
            double Ld_CpuStdev = 0;

            Ld_CplStdev = (Pd_Avg - Pd_LSL) / (3 * Pd_Cpk);
            Ld_CpuStdev = (Pd_USL - Pd_Avg) / (3 * Pd_Cpk);

            if (Ps_SpecClass == "U")
            {
                Ld_Ret = Ld_CpuStdev;
            }
            else if (Ps_SpecClass == "L")
            {
                Ld_Ret = Ld_CplStdev;
            }
            else
            {
                Ld_Ret = Math.Max(Ld_CplStdev, Ld_CpuStdev);
            }
            
            return Ld_Ret;
        }

        public double GetCIR(double Pd_Avg, double Pd_Stdev, double Pd_USL, double Pd_LSL)
        {
            double Ld_Ret = 0;
            double Ld_Sigma = 0;
            double Ld_Avg = 0;
            double Ld_Stdev = 0;

            if (Pd_Stdev > 0)
            {
                Ld_Sigma = Math.Abs(Pd_USL - Pd_LSL) / 6;
                Ld_Stdev = Pd_Stdev * Pd_Stdev;
                Ld_Avg = (Pd_Avg - ((Pd_USL + Pd_LSL) / 2));
                Ld_Avg *= Ld_Avg;
                Ld_Ret = Math.Sqrt(Ld_Avg + Ld_Stdev) / Ld_Sigma;
            }
            return Ld_Ret;
        }

        public double GetCIR(string Ps_SpecClass, double Pd_Avg, double Pd_Stdev, double Pd_USL, double Pd_LSL)
        {
            double Ld_Ret = 0;
            double Ld_Sigma = 0;
            double Ld_Avg = 0;
            double Ld_Stdev = 0;

            if (Ps_SpecClass == "B")
            {
                if (Pd_Stdev > 0)
                {
                    Ld_Sigma = Math.Abs(Pd_USL - Pd_LSL) / 6;
                    Ld_Stdev = Pd_Stdev * Pd_Stdev;
                    Ld_Avg = (Pd_Avg - ((Pd_USL + Pd_LSL) / 2));
                    Ld_Avg *= Ld_Avg;
                    Ld_Ret = Math.Sqrt(Ld_Avg + Ld_Stdev) / Ld_Sigma;
                }
            }
            else
                Ld_Ret = 0;

            return Ld_Ret;
        }

        public double GetCirStdev(double Pd_Avg, double Pd_Cir, double Pd_USL, double Pd_LSL)
        {
            double Ld_Ret = 0;
            double Ld_Sigma = 0;
            double Ld_Avg = 0;
            double Ld_Csl = 0;

            Ld_Csl = (Pd_USL + Pd_LSL) / 2;
            Ld_Sigma = Math.Abs(Pd_USL - Pd_LSL) / 6;
            
            Ld_Sigma *= Pd_Cir;
            Ld_Sigma *= Ld_Sigma;

            Ld_Avg = (Pd_Avg - Ld_Csl);
            Ld_Avg *= Ld_Avg;

            Ld_Ret = Math.Sqrt(Ld_Sigma - Ld_Avg);

            return Ld_Ret;
        }



        /*            Ld_TKM(Llng_Pivot) = m_SQC.RESULT.POINT(Llng_Cnt).Mean - (Ld_Mean - Ld_StdDev)
            Ld_TKP(Llng_Pivot) = m_SQC.RESULT.POINT(Llng_Cnt).Mean - (Ld_Mean + Ld_StdDev)
            If Ld_TKM(Llng_Pivot) < 0 Then
                Ld_TKMMin(Llng_Pivot) = Ld_TKM(Llng_Pivot)
            Else
                Ld_TKMMin(Llng_Pivot) = 0
            End If
            If Ld_TKP(Llng_Pivot) > 0 Then
                Ld_TKPMax(Llng_Pivot) = Ld_TKP(Llng_Pivot)
            Else
                Ld_TKPMax(Llng_Pivot) = 0
            End If

        */


        public double GetCusum_Min(double Pd_Avg, double Pd_Stdev, double Pd_Avg2)
        {
            double Ld_Ret = 0;
            double Ld_TM = 0;

            if (Pd_Stdev > 0)
            {
                Ld_TM = (Pd_Avg2 - (Pd_Avg - Pd_Stdev));

                if (Ld_TM < 0)
                {
                    Ld_Ret = Ld_TM;
                }
                else
                {
                    Ld_Ret = 0;
                }
            }
            return Ld_Ret;
        }

        public double GetCusum_Max(double Pd_Avg, double Pd_Stdev, double Pd_Avg2)
        {
            double Ld_Ret = 0;
            double Ld_TP = 0;

            if (Pd_Stdev > 0)
            {

                Ld_TP = (Pd_Avg2 - (Pd_Avg + Pd_Stdev));

                if (Ld_TP > 0)
                {
                    Ld_Ret = Ld_TP;
                }
                else
                {
                    Ld_Ret = 0;
                }
            }
            return Ld_Ret;
        }
    }
}