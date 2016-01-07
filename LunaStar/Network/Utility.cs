using System;


namespace LunaStar.Network
{
    /* Class : Utility
      * Content :    유용한 함수들
      *
      * Author : 윤석준 (JS-System)
      * Date : 2011. 7.11
      */

    public class Utility
    {
        public static Int32 ConvertEndian(Int32 pInt32Value)
        {
            Byte[] temp = BitConverter.GetBytes(pInt32Value);
            Array.Reverse(temp);
            return BitConverter.ToInt32(temp, 0);
        }

        public static Int16 ConvertEndian(Int16 pInt16Value)
        {
            Byte[] temp = BitConverter.GetBytes(pInt16Value);
            Array.Reverse(temp);
            return BitConverter.ToInt16(temp, 0);
        }

        public static void ByteCopy(Byte[] src, int srcIndex, Byte[] dst, int dstIndex, int count)
        {
            if (src == null || srcIndex < 0 ||
                dst == null || dstIndex < 0 || count < 0)
            {
                throw new System.ArgumentException();
            }

            int srcLen = src.Length;
            int dstLen = dst.Length;
            if (srcLen - srcIndex < count || dstLen - dstIndex < count)
            {
                throw new System.ArgumentException();
            }

            for (int i = 0; i < count; i++)
            {
                dst[dstIndex + i] = src[srcIndex + i];
            }
        }

        public static Byte[] SubByte(Byte[] src, int srcIndex, int count)
        {
            if (src == null ||
                srcIndex < 0 ||
                count < 1 ||
                src.Length < 1 ||
                srcIndex >= src.Length ||
                srcIndex + count > src.Length)
            {
                throw new System.ArgumentException();
            }

            Byte[] Result = new Byte[count];
            for (int i = 0; i < count; i++)
            {
                Result[i] = src[srcIndex + i];
            }

            return Result;
        }

        public static void ByteResize(ref Byte[] src, int count)
        {
            if (count < 0)
            {
                throw new System.ArgumentException();
            }

            Byte[] Result = new Byte[count];
            if (src != null)
            {
                int end;
                if (src.Length > count)
                {
                    end = count;
                }
                else
                {
                    end = src.Length;
                }
                for (int i = 0; i < end; i++)
                {
                    Result[i] = src[i];
                }
            }
            src = Result;
        }


        //// The unsafe keyword allows pointers to be used within the following method:
        //public static unsafe void ByteCopy(byte[] src, int srcIndex, byte[] dst, int dstIndex, int count)
        //{
        //    if (src == null || srcIndex < 0 ||
        //        dst == null || dstIndex < 0 || count < 0)
        //    {
        //        throw new System.ArgumentException();
        //    }

        //    int srcLen = src.Length;
        //    int dstLen = dst.Length;
        //    if (srcLen - srcIndex < count || dstLen - dstIndex < count)
        //    {
        //        throw new System.ArgumentException();
        //    }

        //    // The following fixed statement pins the location of the src and dst objects
        //    // in memory so that they will not be moved by garbage collection.
        //    fixed (byte* pSrc = src, pDst = dst)
        //    {
        //        byte* ps = pSrc;
        //        byte* pd = pDst;

        //        // Loop over the count in blocks of 4 bytes, copying an integer (4 bytes) at a time:
        //        for (int i = 0; i < count / 4; i++)
        //        {
        //            *((int*)pd) = *((int*)ps);
        //            pd += 4;
        //            ps += 4;
        //        }

        //        // Complete the copy by moving any bytes that weren't moved in blocks of 4:
        //        for (int i = 0; i < count % 4; i++)
        //        {
        //            *pd = *ps;
        //            pd++;
        //            ps++;
        //        }
        //    }
        //}
    }
}
