using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace LunaStar.Util
{
    /// <summary>
    /// Image 처리를 위한 Class
    ///   - 2011-12-01
    ///   - 윤석준
    /// </summary>
    public class Ut_Image
    {
        /// <summary>
        /// 이미지를 압축하여 jpg 형식으로 저장합니다.
        /// </summary>
        /// <param name="Ps_src">원본 이미지</param>
        /// <param name="Ps_tgt">저장할 파일 이름</param>
        /// <param name="Pb_CompressRate">압축 % (1~100) 바이트형이라서 끝에 L 붙여야함</param>
        public static void ToJpg(Image P_src, String Ps_tgt, long Pb_CompressRate)
        {
            try
            {
                ImageCodecInfo codec = getEncoderInfo("image/jpeg");
                EncoderParameters param = new EncoderParameters(1);
                param.Param[0] = new EncoderParameter(Encoder.Quality, Pb_CompressRate);
                P_src.Save(Ps_tgt, codec, param);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                P_src.Dispose();
            }
        }

        /// <summary>
        /// 이미지를 압축하여 jpg 형식으로 저장합니다.
        /// </summary>
        /// <param name="Ps_src">원본 이미지</param>
        /// <param name="Ps_tgt">저장할 파일 이름</param>
        /// <param name="Pb_CompressRate">압축 % (1~100) 바이트형이라서 끝에 L 붙여야함</param>
        public static void ToJpg(String Ps_src, String Ps_tgt, long Pb_CompressRate)
        {
            try
            {
                Image L_Image = Image.FromFile(Ps_src);
                ToJpg(L_Image, Ps_tgt, Pb_CompressRate);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        private static ImageCodecInfo getEncoderInfo(String mimeType)
        {
            // Get image codecs for all image formats
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            // Find the correct image codec
            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == mimeType)
                    return codecs[i];
            return null;
        }
    }
}
