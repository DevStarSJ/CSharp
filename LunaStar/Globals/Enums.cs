namespace Globals
{

    /// <summary>
    /// Database 접속 모드
    /// </summary>
    public enum EM_MODE
    {
        REAL = 0,
        TEST = 1
    }

    /// <summary>
    /// 함수 return 값
    /// </summary>
    public enum EM_RET
    {
        ERR = -1,
        EOF = 0,
        OK = 1
    }

    /// <summary>
    /// code, name 구분
    /// </summary>
    public enum EM_RET_STR
    {
        CODE = 0,
        NAME = 1,
        CODE_NAME = 2
    }

    public enum EM_DML
    {
        SELECT = 0,
        INSERT = 1,
        UPDATE = 2,
        DELETE = 3,
        SAVE = 4
    }


    public enum EM_ITEM_FLAG
    {
        USE = 1,
        UNUSE = 0,
        NA = -1
    }


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

    public enum EM_ITEM
    {
        TMP_1 = 0,
        TMP_2 = 1,
        TMP_3 = 2
    }

    public enum EM_CONNECT_TYPE
    {
        ADO = 0,
        WEB = 1
    }

    public enum EM_StringCompare
    {
        EQUAL = 0,
        BIG_RIGHT = 1,
        BIG_LEFT = 2,
        ERROR = 3
    }
}


