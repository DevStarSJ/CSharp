using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace LunaStar.Util
{
    public class UT_EXCEL : IDisposable
    {
        
        private Excel.Application Mxl_Excel;
        private Excel._Workbook Mwb_Book;
        private Excel._Worksheet Mws_Sheet;

        public Font Mf_Font;

        public UT_EXCEL()
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
            Le_Range = Mws_Sheet.get_Range(Mws_Sheet.Cells[Pi_Row, Pi_Col] as Excel.Range, Mws_Sheet.Cells[Pi_Row + Po_Value.GetLength(0) - 1, Pi_Col + Po_Value.GetLength(1) - 1] as Excel.Range);
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
            Le_Range = Mws_Sheet.get_Range(Mws_Sheet.Cells[Pi_Row, Pi_Col] as Excel.Range, Mws_Sheet.Cells[Pi_Row + Po_Value.GetLength(0) - 1, Pi_Col + Po_Value.GetLength(1) - 1] as Excel.Range);
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

        public void SetImage(int Pi_Row, int Pi_Col, Image Po_Image)
        {
            Excel.Range Le_Range = null;
            try
            {
                Clipboard.SetDataObject(Po_Image);

                Le_Range = Mws_Sheet.get_Range(Mws_Sheet.Cells[Pi_Row, Pi_Col] as Excel.Range, Mws_Sheet.Cells[Pi_Row, Pi_Col] as Excel.Range);
                Mws_Sheet.Paste(Le_Range, Po_Image);
                //Le_Range.Select();
                //Mws_Sheet.Paste(Missing.Value, Missing.Value);
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
            Le_Range = Mws_Sheet.get_Range(Mws_Sheet.Cells[Pi_Row1, Pi_Col2] as Excel.Range, Mws_Sheet.Cells[Pi_Row2, Pi_Col2] as Excel.Range);
            Le_Range.EntireRow.AutoFit();
            Le_Range.EntireColumn.AutoFit();
        }


        public void MergeCells(int Pi_Row1, int Pi_Col1, int Pi_Row2, int Pi_Col2)
        {
            Excel.Range Le_Range = null;
            try
            {
                Le_Range = Mws_Sheet.get_Range(Mws_Sheet.Cells[Pi_Row1, Pi_Col1] as Excel.Range, Mws_Sheet.Cells[Pi_Row2, Pi_Col2] as Excel.Range);
                Le_Range.MergeCells = true;
                Le_Range.Borders.LineStyle = Excel.XlLineStyle.xlContinuous;
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
                Le_Range = Mws_Sheet.get_Range(Mws_Sheet.Cells[Pi_Row1, Pi_Col1] as Excel.Range, Mws_Sheet.Cells[Pi_Row2, Pi_Col2] as Excel.Range);
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
        public void SetLineStyle(int Pi_Row1, int Pi_Col1, int Pi_Row2, int Pi_Col2, Excel.XlLineStyle Po_LineStyle)
        {
            Excel.Range Le_Range = null;

            try
            {
                Le_Range = Mws_Sheet.get_Range(Mws_Sheet.Cells[Pi_Row1, Pi_Col1] as Excel.Range, Mws_Sheet.Cells[Pi_Row2, Pi_Col2] as Excel.Range);
                Le_Range.Borders.LineStyle = Po_LineStyle;
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
            SetText(Pi_Row, Pi_Col, Po_Value, Color.Transparent, Color.Black, EM_EXCEL_HALIGN.xlHAlignCenter, EM_EXCEL_VALIGN.xlVAlignCenter);
        }

        public void SetText(int Pi_Row, int Pi_Col, object Po_Value, Color Pc_BackColor, Color Pc_ForeColor, EM_EXCEL_HALIGN Pe_HAlign, EM_EXCEL_VALIGN Pe_VAlign)
        {
            Excel.Range Le_Range = null;
            try
            {
                Le_Range = (Excel.Range)Mws_Sheet.get_Range(Mws_Sheet.Cells[Pi_Row, Pi_Col] as Excel.Range, Mws_Sheet.Cells[Pi_Row, Pi_Col] as Excel.Range);
                 
                Le_Range.Font.Bold = Mf_Font.Bold;
                Le_Range.Font.Size = Mf_Font.Size;

                //Le_Range.Interior.Color = ColorTranslator.ToOle(Pc_BackColor);
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
            Le_Range = Mws_Sheet.get_Range(Mws_Sheet.Cells[Pi_Row, 1] as Excel.Range, Mws_Sheet.Cells[Pi_Row, 1] as Excel.Range);
            Le_Range.RowHeight = Pi_Height;
        }

        public void SetColumnWidth(int Pi_Col, int Pi_Width)
        {
            Excel.Range Le_Range = null;
            Le_Range = Mws_Sheet.get_Range(Mws_Sheet.Cells[1, Pi_Col] as Excel.Range, Mws_Sheet.Cells[1, Pi_Col] as Excel.Range);
            Le_Range.ColumnWidth = 34.00;
        }

        #region ExcelExportVet(ComponentFactory.Krypton.Toolkit.KryptonDataGridView Pgv_List, ...
        //public void ExcelExportVet(ComponentFactory.Krypton.Toolkit.KryptonDataGridView Pgv_List,
        //    string Ps_SheetName, string Ps_FileName)
        //{
        //    SheetInit();
        //    SelectSheet();
        //    SetSheetName(Ps_SheetName);


        //    object[,] Lo_Value = new object[Pgv_List.Rows.Count, 1];

        //    for (int Li_Cnt = 0; Li_Cnt < Pgv_List.Rows.Count; Li_Cnt++)
        //    {
        //        Lo_Value[Li_Cnt, 0] = Pgv_List.Rows[Li_Cnt].HeaderCell.Value;
        //    }

        //    SetRangeDataVet(1, 1, Lo_Value, EM_EXCEL_HALIGN.xlHAlignCenter, EM_EXCEL_VALIGN.xlVAlignCenter);
        //    SetColorCells(1, 1, Lo_Value.Length, 1, Color.Orange);
        //    SetRowHeight(1, 30);

        //    Lo_Value = new object[Pgv_List.Rows.Count, Pgv_List.Columns.Count];

        //    for (int Li_Cnt2 = 0; Li_Cnt2 < Pgv_List.Rows.Count; Li_Cnt2++)
        //    {
        //        for (int Li_Cnt = 0; Li_Cnt < Pgv_List.Columns.Count; Li_Cnt++)
        //        {
        //            if (Pgv_List.Columns[Li_Cnt].CellType == Type.GetType("System.Double"))
        //            {
        //                Lo_Value[Li_Cnt2, Li_Cnt] = Convert.ToDouble(Pgv_List.Rows[Li_Cnt2].Cells[Li_Cnt].Value).ToString("###0.###");
        //            }
        //            else
        //            {
        //                Lo_Value[Li_Cnt2, Li_Cnt] = "'" + Pgv_List.Rows[Li_Cnt2].Cells[Li_Cnt].Value.ToString();
        //            }
        //        }
        //    }

        //    SetRangeDataVet(1, 2, Lo_Value);

        //    string Ls_FileName = Ps_FileName;
        //    if (!Ls_FileName.Contains(".xls"))
        //    {
        //        Ls_FileName += ".xls";
        //    }

        //    SaveCopyAs(Ls_FileName);

        //    Dispose();
        //}


        //public void ExcelExport(ComponentFactory.Krypton.Toolkit.KryptonDataGridView Pgv_List,
        //    string Ps_SheetName, string Ps_FileName)
        //{
        //    SheetInit();
        //    SelectSheet();
        //    SetSheetName(Ps_SheetName);
            
        //    int ExportColumnCount = 0;
        //    int ColumnCount = 0;
            
        //    foreach (DataGridViewColumn dgvc in Pgv_List.Columns)
        //    {
        //        if(dgvc.Visible == false)continue;
        //        ExportColumnCount++;
        //    }

        //    object[,] Lo_Value = new object[1, ExportColumnCount];

        //    for (int Li_Cnt = 0; Li_Cnt < Pgv_List.Columns.Count; Li_Cnt++)
        //    {
        //        if (Pgv_List.Columns[Li_Cnt].Visible == true)
        //        {
        //            Lo_Value[0, ColumnCount] = Pgv_List.Columns[Li_Cnt].HeaderText;
        //            ColumnCount++;
        //        }
        //    }
            
        //    SetRangeData(1, 1, Lo_Value, EM_EXCEL_HALIGN.xlHAlignCenter, EM_EXCEL_VALIGN.xlVAlignCenter);
        //    SetColorCells(1, 1, 1, Lo_Value.Length, Color.Orange);
        //    SetRowHeight(1, 30);

        //    Lo_Value = new object[Pgv_List.Rows.Count, ExportColumnCount];
			
        //    for (int Li_Cnt2 = 0; Li_Cnt2 < Pgv_List.Rows.Count; Li_Cnt2++)
        //    {
        //        ColumnCount = 0;
        //        for (int Li_Cnt = 0; Li_Cnt < Pgv_List.Columns.Count; Li_Cnt++)
        //        {
        //            if (Pgv_List.Columns[Li_Cnt].Visible == false) continue;
					
        //            if (Pgv_List.Columns[Li_Cnt].CellType == Type.GetType("System.Double"))
        //            {
        //                Lo_Value[Li_Cnt2, ColumnCount] = Convert.ToDouble(Pgv_List.Rows[Li_Cnt2].Cells[Li_Cnt].Value).ToString("###0.###");
        //            }
        //            else //if (Pgv_List.Columns[Li_Cnt].ValueType == Type.GetType("System.String"))
        //            {
        //                Lo_Value[Li_Cnt2, ColumnCount] = "'" + Pgv_List.Rows[Li_Cnt2].Cells[Li_Cnt].Value;//.ToString();
        //            }
        //            ColumnCount++;
        //        }
        //    }

        //    SetRangeData(2, 1, Lo_Value);

        //    string Ls_FileName = Ps_FileName;
        //    if (!Ls_FileName.Contains(".xls"))
        //    {
        //        Ls_FileName += ".xls";
        //    }

        //    SaveCopyAs(Ls_FileName);

        //    Dispose();
        //}
        #endregion

        public DataTable ExcelImport(string Ps_FileName)
        {
			try
			{
				string ExcelConn ="Provider = Microsoft.ACE.OLEDB.12.0;Data Source=" + Ps_FileName +
											";Extended Properties='Excel 12.0;HDR=YES'";
				OleDbConnection excelConn =	new OleDbConnection(string.Format(ExcelConn,Ps_FileName));
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
        public double GetRowHeight(int Pi_Row)
        {
            Excel.Range Le_Range = null;
            Le_Range = Mws_Sheet.get_Range(Mws_Sheet.Cells[Pi_Row, 1] as Excel.Range, Mws_Sheet.Cells[Pi_Row, 1] as Excel.Range);
            return Le_Range.Rows.Height;
        }
        public double GetColumnWidth(int Pi_Col)
        {
            Excel.Range Le_Range = null;
            Le_Range = Mws_Sheet.get_Range(Mws_Sheet.Cells[1, Pi_Col] as Excel.Range, Mws_Sheet.Cells[1, Pi_Col] as Excel.Range);
            return Le_Range.Columns.Width;
        }
    }
}




