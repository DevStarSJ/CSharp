using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SmartXLS;

namespace LunaStar.Util
{
    public class Ut_SmartXLS
    {
        public WorkBook Mw_WorkBook = null;
        private RangeStyle Mr_RangeStyle = null;

        public Ut_SmartXLS()
        {
            Mw_WorkBook = new WorkBook();
        }

        public void SetNumSheets(int SheetCount)
        {
            Mw_WorkBook.NumSheets = SheetCount;
        }

        public void SetSheetIndex(int SheetIndex)
        {
            Mw_WorkBook.Sheet = SheetIndex;
        }

        public void SetSheetName(int SheetIndex, string SheetName)
        {
            Mw_WorkBook.setSheetName(SheetIndex, SheetName);
        }

        public void SetSheetHidden(int SheetIndex)
        {
            Mw_WorkBook.SheetHidden = SheetIndex;
        }

        public Int32 GetNumSheet()
        {
            return Mw_WorkBook.NumSheets;
        }

        public Int32 GetSheetIndex()
        {
            return Mw_WorkBook.Sheet;
        }

        public void SetSheetDelete(int SheetIndex, int Sheet)
        {
            Mw_WorkBook.deleteSheets(SheetIndex, Sheet);
        }

        public void SetRangeDelete(int Row1, int Col1, int Row2, int Col2, short Shift)
        {
            Mw_WorkBook.deleteRange(Row1, Col1, Row2, Col2, Shift);
        }

        public void SetAutoCalc(bool Lb_Check)
        {
            Mw_WorkBook.AutoCalc = Lb_Check;
        }

        public void ExcelSave(string FilePath, string FileExtension)
        {
            switch (FileExtension)
            {
                case "1":   //XLSX
                    Mw_WorkBook.writeXLSX(FilePath);
                    break;
                case "2":   //XLS
                    Mw_WorkBook.write(FilePath);
                    break;
                case "3":   //CSV
                    Mw_WorkBook.writeCSV(FilePath);
                    break;
            }
        }

        public void ExcelSaveXLSX(string FilePath)
        {
            Mw_WorkBook.writeXLSX(FilePath);
        }

        public void SetText(string AreaName, string Value)
        {
            Mw_WorkBook.setText(AreaName, Value);
        }

        public void SetText(int Row, int Col, string Value)
        {
            Mw_WorkBook.setText(Row, Col, Value);
        }

        public void SetNumber(string AreaName, double Value)
        {
            Mw_WorkBook.setNumber(AreaName, Value);
        }

        public void SetNumber(int Row, int Col, double Value)
        {
            Mw_WorkBook.setNumber(Row, Col, Value);
        }

        public void GetRangeStyle()
        {
            Mr_RangeStyle = Mw_WorkBook.getRangeStyle();
        }

        public void GetRangeStyle(int Row1, int Col1, int Row2, int Col2)
        {
            Mr_RangeStyle = Mw_WorkBook.getRangeStyle(Row1, Col1, Row2, Col2);
        }

        public void SetRangeStyle()
        {
            Mw_WorkBook.setRangeStyle(Mr_RangeStyle);
        }

        public void SetRangeStyle(int Row1, int Col1, int Row2, int Col2)
        {
            Mw_WorkBook.setRangeStyle(Mr_RangeStyle, Row1, Col1, Row2, Col2);
        }

        public void SetMergeCells(bool Pb_Check)
        {
            Mr_RangeStyle.MergeCells = Pb_Check;
        }

        public void SetSolidPattern(short Ps_Pattern, int fcolor, int bcolor)
        {
            Mr_RangeStyle.Pattern = Ps_Pattern;
            Mr_RangeStyle.PatternFG = fcolor;
            Mr_RangeStyle.PatternBG = bcolor;
        }

        public void SetAllCenter()
        {
            Mr_RangeStyle.HorizontalAlignment = RangeStyle.HorizontalAlignmentCenter;
            Mr_RangeStyle.VerticalAlignment = RangeStyle.VerticalAlignmentCenter;
            Mr_RangeStyle.WordWrap = false;
        }

        public void SetAllLeft()
        {
            Mr_RangeStyle.HorizontalAlignment = RangeStyle.HorizontalAlignmentLeft;
            Mr_RangeStyle.VerticalAlignment = RangeStyle.VerticalAlignmentCenter;
            Mr_RangeStyle.WordWrap = false;
        }

        public void SetAllRight()
        {
            Mr_RangeStyle.HorizontalAlignment = RangeStyle.HorizontalAlignmentRight;
            Mr_RangeStyle.VerticalAlignment = RangeStyle.VerticalAlignmentCenter;
            Mr_RangeStyle.WordWrap = false;
        }

        public void SetAlign(short Ps_HAlig, short Ps_VAlig)
        {
            Mr_RangeStyle.HorizontalAlignment = Ps_HAlig;
            Mr_RangeStyle.VerticalAlignment = Ps_VAlig;
            Mr_RangeStyle.WordWrap = false;
        }

        public void SetAlign(bool Pb_Wrap, short Ps_HAlig, short Ps_VAlig)
        {
            Mr_RangeStyle.HorizontalAlignment = Ps_HAlig;
            Mr_RangeStyle.VerticalAlignment = Ps_VAlig;
            Mr_RangeStyle.WordWrap = Pb_Wrap;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////
        //BorderNone = 0 (short)0
        //BorderThin = 1 (short)1
        //BorderMedium = 2 (short)2
        //BorderDashed = 3 (short)3
        //BorderDotted = 4 (short)4
        //BorderThick = 5 (short)5
        //BorderDouble = 6 (short)6
        //BorderHair = 7 (short)7
        //BorderMediumDash = 8 (short)8
        //BorderDashDot = 9 (short)9
        //BorderMediumDashDot = 10 (short)10
        //BorderDashDotDot = 11 (short)11
        //BorderMediumDashDotDot = 12 (short)12
        //BorderSlantedDashDot = 13 (short)13
        /////////////////////////////////////////////////////////////////////////////////////////////        

        public void SetRangeTopBorder(short Ps_Line, int Pi_Color)
        {
            Mr_RangeStyle.TopBorder = Ps_Line;
            Mr_RangeStyle.TopBorderColor = Pi_Color;
        }

        public void SetRangeBottomBorder(short Ps_Line, int Pi_Color)
        {
            Mr_RangeStyle.BottomBorder = Ps_Line;
            Mr_RangeStyle.BottomBorderColor = Pi_Color;
        }

        public void SetRangeLeftBorder(short Ps_Line, int Pi_Color)
        {
            Mr_RangeStyle.LeftBorder = Ps_Line;
            Mr_RangeStyle.LeftBorderColor = Pi_Color;
        }

        public void SetRangeRightBorder(short Ps_Line, int Pi_Color)
        {
            Mr_RangeStyle.RightBorder = Ps_Line;
            Mr_RangeStyle.RightBorderColor = Pi_Color;
        }

        public void SetRangeBorder(short Ps_Line, int Pi_Color)
        {
            Mr_RangeStyle.TopBorder = Ps_Line;
            Mr_RangeStyle.TopBorderColor = Pi_Color;
            Mr_RangeStyle.BottomBorder = Ps_Line;
            Mr_RangeStyle.BottomBorderColor = Pi_Color;
            Mr_RangeStyle.LeftBorder = Ps_Line;
            Mr_RangeStyle.LeftBorderColor = Pi_Color;
            Mr_RangeStyle.RightBorder = Ps_Line;
            Mr_RangeStyle.RightBorderColor = Pi_Color;
        }

        public void SetRangeBorder(short Ps_Line, int Pi_Color, short Ps_InSide, int Pi_InSideColor)
        {
            Mr_RangeStyle.TopBorder = Ps_Line;
            Mr_RangeStyle.TopBorderColor = Pi_Color;
            Mr_RangeStyle.BottomBorder = Ps_Line;
            Mr_RangeStyle.BottomBorderColor = Pi_Color;
            Mr_RangeStyle.LeftBorder = Ps_Line;
            Mr_RangeStyle.LeftBorderColor = Pi_Color;
            Mr_RangeStyle.RightBorder = Ps_Line;
            Mr_RangeStyle.RightBorderColor = Pi_Color;

            Mr_RangeStyle.HorizontalInsideBorder = Ps_InSide;
            Mr_RangeStyle.HorizontalInsideBorderColor = Pi_InSideColor;

            Mr_RangeStyle.VerticalInsideBorder = Ps_InSide;
            Mr_RangeStyle.VerticalInsideBorderColor = Pi_InSideColor;
        }

        public void SetRangeBorder(short Ps_Top, short Ps_Bottom, short Ps_Left, short Ps_Right, int color)
        {
            Mr_RangeStyle.TopBorder = Ps_Top;
            Mr_RangeStyle.TopBorderColor = color;
            Mr_RangeStyle.BottomBorder = Ps_Bottom;
            Mr_RangeStyle.BottomBorderColor = color;
            Mr_RangeStyle.LeftBorder = Ps_Left;
            Mr_RangeStyle.LeftBorderColor = color;
            Mr_RangeStyle.RightBorder = Ps_Right;
            Mr_RangeStyle.RightBorderColor = color;
        }

        public void SetFont(int Pi_Size, bool Pb_Bold, string Ps_Name)
        {
            Mr_RangeStyle.FontSize = Pi_Size * 20;
            Mr_RangeStyle.FontBold = Pb_Bold;
            Mr_RangeStyle.FontName = Ps_Name;
        }

        public void SetFontSize(int Pi_Size, string Ps_Plus)
        {
            if (string.IsNullOrEmpty(Ps_Plus))
            {
                Mr_RangeStyle.FontSize = Pi_Size;
            }
            else
            {
                Mr_RangeStyle.FontSize = Pi_Size * Convert.ToInt32(Ps_Plus);
            }
        }

        public void SetFontColor(int Pi_Color)
        {
            Mr_RangeStyle.FontColor = Pi_Color;
        }

        public void SetFontBold(bool Pb_Bold)
        {
            Mr_RangeStyle.FontBold = Pb_Bold;
        }

        public void SetFontName(string Ps_Name)
        {
            Mr_RangeStyle.FontName = Ps_Name;
        }

        public string SetformatRCNr(int Row, int Column, bool DoAbs)
        {
            return Mw_WorkBook.formatRCNr(Row, Column, DoAbs);
        }

        public void SetAddPicture(double ColStart, double RowStart, double ColEnd, double RowEnd, string Ps_File)
        {
            Mw_WorkBook.addPicture(ColStart, RowStart, ColEnd, RowEnd, Ps_File);            
        }

        public void SetRowHeight(int RowIndex, int Height)
        {
            Mw_WorkBook.setRowHeight(RowIndex, Height);
        }

        public void SetRowHeightAuto(int RowIndex, bool Lb_State)
        {
            Mw_WorkBook.setRowHeightAuto(RowIndex, Lb_State);
        }

        public void SetColWidth(int ColIndex, int Width)
        {
            Mw_WorkBook.setColWidth(ColIndex, Width);
        }

        public void SetColWidthAuto(int ColIndex, bool Lb_State)
        {
            Mw_WorkBook.setColWidthAutoSize(ColIndex, Lb_State);
        }
               
    }
}
