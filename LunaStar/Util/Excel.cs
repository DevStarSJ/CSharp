using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace LunaStar.Util
{
    public enum EM_EXCEL_HALIGN
    {
        xlHAlignRight = -4152,
        xlHAlignLeft = -4131,
        xlHAlignJustify = -4130,
        xlHAlignDistributed = -4117,
        xlHAlignCenter = -4108,
        xlHAlignGeneral = 1,
        xlHAlignFill = 5,
        xlHAlignCenterAcrossSelection = 7,
    }

    public enum EM_EXCEL_VALIGN
    {
        xlVAlignTop = -4160,
        xlVAlignJustify = -4130,
        xlVAlignDistributed = -4117,
        xlVAlignCenter = -4108,
        xlVAlignBottom = -4107,
    }

    public class EXCEL : IDisposable
    {
        protected Excel.Application Mxl_Excel;
        protected Excel._Workbook Mwb_Book;
        protected Excel._Worksheet Mws_Sheet;

        public Font Mf_Font;

        public EXCEL()
        {
            Mxl_Excel = new Excel.Application();
            Mwb_Book = (Excel._Workbook)(Mxl_Excel.Workbooks.Add(Missing.Value));
            Mws_Sheet = (Excel._Worksheet)Mwb_Book.ActiveSheet;

            Mf_Font = new Font("굴림체", 10, FontStyle.Regular);
        }

        public void ExcePrint()
        {
            Mwb_Book.PrintPreview(true);
        }

        public void Dispose()
        {
            if (Mws_Sheet != null)
            {
                Marshal.ReleaseComObject(Mws_Sheet);
                Mws_Sheet = null;
            }

            if (Mwb_Book != null)
            {
                Mwb_Book.Close(false, null, null);
                Marshal.ReleaseComObject(Mwb_Book);
                Mwb_Book = null;
            }

            if (Mxl_Excel != null)
            {
                Mxl_Excel.Workbooks.Close();
                Mxl_Excel.Quit();
                Marshal.ReleaseComObject(Mxl_Excel);
                Mxl_Excel = null;
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public void SheetInit()
        {
            //sheet 한개부터 시작하기 위해 삭제
            Mws_Sheet = (Excel.Worksheet)Mwb_Book.Worksheets.get_Item(Mwb_Book.Worksheets.Count);
            Mws_Sheet.Delete();
            Mws_Sheet = (Excel.Worksheet)Mwb_Book.Worksheets.get_Item(Mwb_Book.Worksheets.Count);
            Mws_Sheet.Delete();
        }

        public void DeleteSheet()
        {
            Mws_Sheet = (Excel.Worksheet)Mwb_Book.Worksheets.get_Item(Mwb_Book.Worksheets.Count);
            Mws_Sheet.Delete();
        }

        public void AddSheet()
        {
            Mwb_Book.Worksheets.Add(Type.Missing, Mwb_Book.Worksheets[Mwb_Book.Worksheets.Count], Type.Missing, Type.Missing);
        }

        public void SelectSheet(int Pi_Index)
        {
            Mws_Sheet = (Excel.Worksheet)Mwb_Book.Worksheets.get_Item(Pi_Index);
        }

        public void SelectSheet()
        {
            Mws_Sheet = (Excel.Worksheet)Mwb_Book.Worksheets.get_Item(Mwb_Book.Worksheets.Count);
        }


        public void SetSheetName(string Ps_Name)
        {
            Mws_Sheet.Name = Ps_Name;
        }

        public void SaveCopyAs(string Ps_FileName)
        {
            try
            {
                Mwb_Book.SaveCopyAs(Ps_FileName);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public void SetRangeData(int Pi_Row, int Pi_Col, object[,] Po_Value)
        {
            SetRangeData(Pi_Row, Pi_Col, Po_Value, EM_EXCEL_HALIGN.xlHAlignGeneral, EM_EXCEL_VALIGN.xlVAlignCenter);
        }


        public void SetRangeData(int Pi_Row, int Pi_Col, object[,] Po_Value, EM_EXCEL_HALIGN Pe_HAlign, EM_EXCEL_VALIGN Pe_VAlign)
        {
            Excel.Range Le_Range = null;
            Le_Range = Mws_Sheet.get_Range(Mws_Sheet.Cells[Pi_Row, Pi_Col], Mws_Sheet.Cells[Pi_Row + Po_Value.GetLength(0) - 1, Pi_Col + Po_Value.GetLength(1) - 1]);
            Le_Range.set_Value(Missing.Value, Po_Value);

            Le_Range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

            Le_Range.HorizontalAlignment = Pe_HAlign;
            Le_Range.VerticalAlignment = Pe_VAlign;


            Le_Range.EntireRow.AutoFit();
            Le_Range.EntireColumn.AutoFit();

            Le_Range.WrapText = false;

            Le_Range.Font.Bold = Mf_Font.Bold;
            Le_Range.Font.Size = Mf_Font.Size;
        }

        public void SetRangeDataVet(int Pi_Row, int Pi_Col, object[,] Po_Value)
        {
            SetRangeDataVet(Pi_Row, Pi_Col, Po_Value, EM_EXCEL_HALIGN.xlHAlignGeneral, EM_EXCEL_VALIGN.xlVAlignCenter);
        }

        public void SetRangeDataVet(int Pi_Row, int Pi_Col, object[,] Po_Value, EM_EXCEL_HALIGN Pe_HAlign, EM_EXCEL_VALIGN Pe_VAlign)
        {
            Excel.Range Le_Range = null;
            Le_Range = Mws_Sheet.get_Range(Mws_Sheet.Cells[Pi_Row, Pi_Col], Mws_Sheet.Cells[Pi_Row + Po_Value.GetLength(0) - 1, Pi_Col + Po_Value.GetLength(1) - 1]);
            Le_Range.set_Value(Missing.Value, Po_Value);

            Le_Range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;

            Le_Range.HorizontalAlignment = Pe_HAlign;
            Le_Range.VerticalAlignment = Pe_VAlign;


            Le_Range.EntireRow.AutoFit();
            Le_Range.EntireColumn.AutoFit();

            Le_Range.WrapText = false;

            Le_Range.Font.Bold = Mf_Font.Bold;
            Le_Range.Font.Size = Mf_Font.Size;
        }

        public void Paste(Int32 Pi_Row, Int32 Pi_Col)
        {
            Mws_Sheet.get_Range(Mws_Sheet.Cells[Pi_Row, Pi_Col], Mws_Sheet.Cells[Pi_Row, Pi_Col]).Select();


            Mws_Sheet.Paste(Missing.Value, Missing.Value);
        }

        public void SetImage(int Pi_Row, int Pi_Col, Image Po_Image)
        {
            Excel.Range Le_Range = null;
            try
            {
                Clipboard.SetDataObject(Po_Image);

                Le_Range = Mws_Sheet.get_Range(Mws_Sheet.Cells[Pi_Row, Pi_Col], Mws_Sheet.Cells[Pi_Row, Pi_Col]);

                Le_Range.Select();
                Mws_Sheet.Paste(Missing.Value, Missing.Value);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (Le_Range != null)
                {
                    Marshal.ReleaseComObject(Le_Range);
                }
            }
        }


        public void AutoFit(int Pi_Row1, int Pi_Col1, int Pi_Row2, int Pi_Col2)
        {
            Excel.Range Le_Range = null;
            Le_Range = Mws_Sheet.get_Range(Mws_Sheet.Cells[Pi_Row1, Pi_Col2], Mws_Sheet.Cells[Pi_Row2, Pi_Col2]);
            Le_Range.EntireRow.AutoFit();
            Le_Range.EntireColumn.AutoFit();
        }


        public void MergeCells(int Pi_Row1, int Pi_Col1, int Pi_Row2, int Pi_Col2)
        {
            Excel.Range Le_Range = null;
            try
            {
                Le_Range = Mws_Sheet.get_Range(Mws_Sheet.Cells[Pi_Row1, Pi_Col1], Mws_Sheet.Cells[Pi_Row2, Pi_Col2]);
                Le_Range.MergeCells = true;
                //Le_Range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (Le_Range != null)
                {
                    Marshal.ReleaseComObject(Le_Range);
                }
            }
        }

        public void MergeCells(int Pi_Row1, int Pi_Col1, int Pi_Row2, int Pi_Col2, Excel.XlLineStyle LineStyle)
        {
            Excel.Range Le_Range = null;
            try
            {
                Le_Range = Mws_Sheet.get_Range(Mws_Sheet.Cells[Pi_Row1, Pi_Col1], Mws_Sheet.Cells[Pi_Row2, Pi_Col2]);
                Le_Range.MergeCells = true;
                Le_Range.Borders.LineStyle = LineStyle;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (Le_Range != null)
                {
                    Marshal.ReleaseComObject(Le_Range);
                }
            }
        }

        public void SetColorCells(int Pi_Row1, int Pi_Col1, int Pi_Row2, int Pi_Col2, Color Po_Color)
        {
            Excel.Range Le_Range = null;

            try
            {
                Le_Range = Mws_Sheet.get_Range(Mws_Sheet.Cells[Pi_Row1, Pi_Col1], Mws_Sheet.Cells[Pi_Row2, Pi_Col2]);
                Le_Range.Interior.Color = ColorTranslator.ToOle(Po_Color);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (Le_Range != null)
                {
                    Marshal.ReleaseComObject(Le_Range);
                }
            }
        }

        public void SetText(int Pi_Row, int Pi_Col, object Po_Value, EM_EXCEL_HALIGN Pe_HAlign)
        {
            SetText(Pi_Row, Pi_Col, Po_Value, Color.Transparent, Color.Black, Pe_HAlign, EM_EXCEL_VALIGN.xlVAlignCenter);
        }

        public void SetText(int Pi_Row, int Pi_Col, object Po_Value, Color Pc_ForeColor)
        {
            SetText(Pi_Row, Pi_Col, Po_Value, Color.Transparent, Pc_ForeColor, EM_EXCEL_HALIGN.xlHAlignCenter, EM_EXCEL_VALIGN.xlVAlignCenter);
        }


        public void SetText(int Pi_Row, int Pi_Col, object Po_Value)
        {
            Excel.Range Le_Range = null;
            try
            {
                Le_Range = Mws_Sheet.get_Range(Mws_Sheet.Cells[Pi_Row, Pi_Col], Mws_Sheet.Cells[Pi_Row, Pi_Col]);

                Le_Range.Font.Bold = Mf_Font.Bold;
                Le_Range.Font.Size = Mf_Font.Size;

                //Le_Range.Interior.Color = ColorTranslator.ToOle(Pc_BackColor);
                //Le_Range.Font.Color = ColorTranslator.ToOle(Pc_ForeColor);

                //Le_Range.HorizontalAlignment = Pe_HAlign;
                //Le_Range.VerticalAlignment = Pe_VAlign;

                Le_Range.Value2 = Po_Value;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (Le_Range != null)
                {
                    Marshal.ReleaseComObject(Le_Range);
                }
            }

            //SetText(Pi_Row, Pi_Col, Po_Value, Color.Transparent, Color.Black, EM_EXCEL_HALIGN.xlHAlignCenter, EM_EXCEL_VALIGN.xlVAlignCenter);
        }

        public void SetText(int Pi_Row, int Pi_Col, object Po_Value, Color Pc_BackColor, Color Pc_ForeColor, EM_EXCEL_HALIGN Pe_HAlign, EM_EXCEL_VALIGN Pe_VAlign)
        {
            Excel.Range Le_Range = null;
            try
            {
                Le_Range = Mws_Sheet.get_Range(Mws_Sheet.Cells[Pi_Row, Pi_Col], Mws_Sheet.Cells[Pi_Row, Pi_Col]);

                Le_Range.Font.Bold = Mf_Font.Bold;
                Le_Range.Font.Size = Mf_Font.Size;

                Le_Range.Interior.Color = ColorTranslator.ToOle(Pc_BackColor);
                Le_Range.Font.Color = ColorTranslator.ToOle(Pc_ForeColor);

                Le_Range.HorizontalAlignment = Pe_HAlign;
                Le_Range.VerticalAlignment = Pe_VAlign;

                Le_Range.Value2 = Po_Value;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (Le_Range != null)
                {
                    Marshal.ReleaseComObject(Le_Range);
                }
            }
        }

        public void SetRowHeight(int Pi_Row, int Pi_Height)
        {
            Excel.Range Le_Range = null;
            Le_Range = Mws_Sheet.get_Range(Mws_Sheet.Cells[Pi_Row, 1], Mws_Sheet.Cells[Pi_Row, 1]);
            Le_Range.RowHeight = Pi_Height;
        }

        public void SetColumnWidth(int Pi_Col, int Pi_Width)
        {
            Excel.Range Le_Range = null;
            Le_Range = Mws_Sheet.get_Range(Mws_Sheet.Cells[1, Pi_Col], Mws_Sheet.Cells[1, Pi_Col]);
            Le_Range.ColumnWidth = 34.00;
        }

        public static DataTable CsvImport(string Ps_FileName)
        {
            try
            {
                DataTable result = new DataTable();

                StreamReader FileStr = new StreamReader(Ps_FileName);

                //Split the first line into the columns  
                string[] columns = null;

                columns = FileStr.ReadLine().Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string col in columns)
                {
                    bool added = false;
                    string next = "";
                    int i = 0;
                    while (!added)
                    {
                        string columnname = col + next;

                        if (!result.Columns.Contains(columnname))
                        {
                            result.Columns.Add(columnname);
                            added = true;
                        }
                        else
                        {
                            i++;
                            next = "_" + i.ToString();
                        }
                    }
                }

                string AllData = FileStr.ReadToEnd();
                string[] rows = AllData.Split(new Char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                DataRow Dr = null;
                foreach (string r in rows)
                {
                    string[] items = r.Split(new Char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    Dr = result.NewRow();
                    for (int i = 0; i < items.Length; i++)
                        Dr[i] = items[i];

                    result.Rows.Add(Dr);
                    Dr = null;
                }
                //Return the imported data.
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static DataTable ExcelImport(string Ps_FileName)
        {
            try
            {
                //var connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", ofd.FileName);
                //                string ExcelConn = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", Ps_FileName);
                string ExcelConn = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\";", Ps_FileName);




                OleDbConnection excelConn = new OleDbConnection(string.Format(ExcelConn, Ps_FileName));
                excelConn.Open();
                //OleDbDataAdapter excelAdapter = new OleDbDataAdapter(ExcelSql,excelConn);
                //DataSet excelDs = new DataSet();
                //excelAdapter.Fill(excelDs);
                //DataTable excelTable =excelDs.Tables[0];

                //return excelTable;
                if (excelConn.State != ConnectionState.Open)
                {
                    MessageBox.Show("엑셀파일에 연결할 수 없습니다", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                var worksheets = excelConn.GetSchema("Tables");
                string Query = string.Empty;

                Query += " select A.* ";
                Query += string.Format(" from [{0}] as A ", worksheets.Rows[0]["TABLE_NAME"]);
                OleDbCommand cmd = new OleDbCommand(Query, excelConn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);

                DataSet ds = new DataSet();

                da.Fill(ds);

                excelConn.Close();

                return ds.Tables[0];
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static DataTable XlsxImport(string Ps_FileName)
        {
            try
            {
                string ExcelConn = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\";", Ps_FileName);

                //string query = String.Format("select * from [{0}]", "Sheet1");

                //OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, ExcelConn);

                //DataSet dataSet = new DataSet();

                //dataAdapter.Fill(dataSet);
                OleDbConnection excelConn = new OleDbConnection(string.Format(ExcelConn, Ps_FileName));
                excelConn.Open();
                //OleDbDataAdapter excelAdapter = new OleDbDataAdapter(ExcelSql,excelConn);
                //DataSet excelDs = new DataSet();
                //excelAdapter.Fill(excelDs);
                //DataTable excelTable =excelDs.Tables[0];

                //return excelTable;
                if (excelConn.State != ConnectionState.Open)
                {
                    MessageBox.Show("엑셀파일에 연결할 수 없습니다", "에러", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                var worksheets = excelConn.GetSchema("Tables");
                string Query = string.Empty;

                Query += " select A.* ";
                Query += string.Format(" from [{0}] as A ", worksheets.Rows[0]["TABLE_NAME"]);
                OleDbCommand cmd = new OleDbCommand(Query, excelConn);
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                DataSet ds = new DataSet();

                da.Fill(ds);

                excelConn.Close();

                return ds.Tables[0];
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ExcelExport(DataTable Pdt_Data, String Ps_SheetName, String Ps_FileName)
        {
            SheetInit();
            SelectSheet();
            SetSheetName(Ps_SheetName);

            int ExportColumnCount = Pdt_Data.Columns.Count;
            int ColumnCount = Pdt_Data.Columns.Count;

            object[,] Lo_Value = new object[1, ExportColumnCount];

            for (int Li_Cnt = 0; Li_Cnt < Pdt_Data.Columns.Count; Li_Cnt++)
            {
                Lo_Value[0, Li_Cnt] = Pdt_Data.Columns[Li_Cnt].ColumnName;
            }

            SetRangeData(1, 1, Lo_Value, EM_EXCEL_HALIGN.xlHAlignCenter, EM_EXCEL_VALIGN.xlVAlignCenter);
            SetColorCells(1, 1, 1, Lo_Value.Length, Color.Orange);
            SetRowHeight(1, 30);

            Lo_Value = new object[Pdt_Data.Rows.Count, ExportColumnCount];

            for (int Li_Cnt2 = 0; Li_Cnt2 < Pdt_Data.Rows.Count; Li_Cnt2++)
            {
                ColumnCount = 0;
                for (int Li_Cnt = 0; Li_Cnt < Pdt_Data.Columns.Count; Li_Cnt++)
                {
                    if (Pdt_Data.Columns[Li_Cnt].DataType == Type.GetType("System.Double"))
                    {
                        Lo_Value[Li_Cnt2, ColumnCount] = Convert.ToDouble(Pdt_Data.Rows[Li_Cnt2][Li_Cnt]).ToString("###0.###");
                    }
                    else //if (Pdt_Data.Columns[Li_Cnt].DataType == Type.GetType("System.String"))
                    {
                        Lo_Value[Li_Cnt2, ColumnCount] = Pdt_Data.Rows[Li_Cnt2][Li_Cnt] as String;//"'" + Pdt_Data.Rows[Li_Cnt2][Li_Cnt] as String ;
                    }
                    ColumnCount++;
                }
            }
            SetRangeData(2, 1, Lo_Value);

            String Ls_FileName = Ps_FileName;
            if (!Ls_FileName.Contains(".xls"))
            {
                Ls_FileName += ".xls";
            }

            SaveCopyAs(Ls_FileName);

            Dispose();
        }

        /// <summary>
        /// DataTable 엑셀로 내보내기
        /// </summary>
        /// <param name="Pdt_Data"></param>
        public void ExcelExport(string Ps_FileName, DataTable Pdt_Data)
        {
            Excel.Range Le_Range = null;
            int Li_Row = 2;
            try
            {

                if (Pdt_Data.Rows.Count > 0)
                {
                    //headers 
                    for (int headCell = 0; headCell < Pdt_Data.Columns.Count; headCell++)
                        Mws_Sheet.Cells[1, headCell + 1] = Pdt_Data.Columns[headCell].ColumnName;


                    //Data Insert
                    for (int Li_R = 0; Li_R < Pdt_Data.Rows.Count; Li_R++)
                    {
                        for (int Li_C = 0; Li_C < Pdt_Data.Columns.Count; Li_C++)
                        {
                            // in each column
                            if (Pdt_Data.Rows[Li_R][Li_C].ToString().StartsWith("0"))
                                Mws_Sheet.Cells[Li_Row, Li_C + 1] = "'" + Pdt_Data.Rows[Li_R][Li_C].ToString();
                            else
                                Mws_Sheet.Cells[Li_Row, Li_C + 1] = Pdt_Data.Rows[Li_R][Li_C].ToString();
                        }
                        Li_Row++;

                    }

                }

                Le_Range = Mws_Sheet.get_Range("A1", "IV1");
                Le_Range.EntireColumn.AutoFit();

                Mxl_Excel.UserControl = false;
                //path declaration
                string strFile = Ps_FileName;

                // to view Excel sheet...
                //oAppln.Visible = true;

                // to save the excel sheet....
                Mwb_Book.SaveCopyAs(strFile);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Dispose();
            }
        }

    }
}