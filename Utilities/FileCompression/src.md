```C#
using System;
using System.IO;
using ICSharpCode.SharpZipLib.Tar;
using ICSharpCode.SharpZipLib.Zip;
using SevenZip;
using System.Collections;

// 솔루션 빌드 후 이벤트에 아래 줄 추가 (단 최종경로 확인 필수 : Project명에 . 들어가는 경우 실제 폴더 구조랑 다를 수 있음
// COPY /Y "$(SolutionDir)$(ProjectName)\7z.dll" "$(SolutionDir)$(SolutionName)\$(OutDir)\7z.dll"

namespace LunaStar.Util
{
    public class FileCompression
    {
        /// <summary>
        /// Unix에서 생성한 Z 파일의 압축해제
        /// </summary>
        /// <param name="zFileName">압축해제할 Z 파일이름</param>
        /// <returns>압축 해제 성공 여부</returns>
        static public bool UnUnixZ(string zFileName)
        {
            try
            {
                FileInfo fi = new FileInfo(zFileName);
                string directoryName = fi.DirectoryName;
                string fileName = Path.GetFileNameWithoutExtension(zFileName);
                string outputFileName = $"{directoryName}\\{fileName}";

                using (Stream inStream = new LzwInputStream(File.OpenRead(zFileName)))
                using (FileStream outStream = File.Create(outputFileName))
                {
                    int bytesRead;
                    byte[] buffer = new byte[4096];

                    while ((bytesRead = inStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        outStream.Write(buffer, 0, bytesRead);
                    }
                }
            }
            catch (Exception e)
            {
                throw new AggregateException("Fail FileCompression.UnUnixZ", e);
            }
            return true;
        }

        /// <summary>
        /// 특정 폴더를 ZIP으로 압축
        /// </summary>
        /// <param name="targetFolderPath">압축 대상 폴더 경로</param>
        /// <param name="zipFilePath">저장할 ZIP 파일 경로</param>
        /// <param name="password">압축 암호</param>
        /// <param name="isDeleteFolder">폴더 삭제 여부</param>
        /// <returns>압축 성공 여부</returns>
        public static bool Zip(string targetFolderPath, string zipFilePath, string password, bool isDeleteFolder)
        {
            if (!Directory.Exists(targetFolderPath)) // 폴더가 존재하는 경우에만 수행
                return false;

            ArrayList fileList = GenerateFileList(targetFolderPath); // 압축 대상 폴더의 파일 목록

            int pathLength = (Directory.GetParent(targetFolderPath)).ToString().Length + 1; // find number of chars to remove. from orginal file path. remove '\'

            using (ZipOutputStream zipOutputStream = new ZipOutputStream(File.Create(zipFilePath))) // ZIP 스트림 생성.
            {
                if (password != null && password != string.Empty) // 패스워드가 있는 경우 패스워드 지정
                    zipOutputStream.Password = password;

                zipOutputStream.SetLevel(9); // 암호화 레벨.(최대 압축)

                ZipEntry zipEntry;
                foreach (string fileName in fileList)
                {
                    zipEntry = new ZipEntry(fileName.Remove(0, pathLength));
                    zipOutputStream.PutNextEntry(zipEntry);

                    try
                    {
                        if (!fileName.EndsWith(@"/")) // 파일인 경우
                        {
                            using (FileStream fileStream = File.OpenRead(fileName))
                            {
                                byte[] buffer = new byte[fileStream.Length];
                                fileStream.Read(buffer, 0, buffer.Length);
                                zipOutputStream.Write(buffer, 0, buffer.Length);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        // 오류가 난 경우 생성 했던 파일을 삭제.
                        if (File.Exists(zipFilePath))
                            File.Delete(zipFilePath);

                        throw new AggregateException("FileCompression.Zip : Fail to ZipOutputStream", e);
                    }
                    finally
                    {
                        zipOutputStream.Finish(); // 압축 종료
                        zipOutputStream.Close();
                    }
                }
            }

            if (isDeleteFolder) // 폴더 삭제를 원할 경우 폴더 삭제
            {
                try
                {
                    Directory.Delete(targetFolderPath, true);
                }
                catch (Exception e)
                {
                    throw new AggregateException("FileCompression.Zip : Fail to delete folder", e);
                }
            }

            return true;
        }

        /// <summary>
        /// 파일, 폴더 목록 생성
        /// </summary>
        /// <param name="directory">폴더 경로</param>
        /// <returns>폴더, 파일 목록(ArrayList)</returns>
        private static ArrayList GenerateFileList(string directory)
        {
            ArrayList fileList = new ArrayList();

            bool isEmpty = true;

            try
            {
                foreach (string fileName in Directory.GetFiles(directory)) // 폴더 내의 파일 추가
                {
                    fileList.Add(fileName);
                    isEmpty = false;
                }
            }
            catch (Exception e)
            {
                throw new AggregateException("FileCompression.GenetateFileList : Fail to add files", e);
            }

            if (isEmpty)
            {
                try
                {
                    if (Directory.GetDirectories(directory).Length == 0) // 파일이 없고, 폴더도 없는 경우 자신의 폴더 추가
                        fileList.Add(directory + @"/");
                }
                catch (Exception e)
                {
                    throw new AggregateException("FileCompression.GenetateFileList : Fail to add self", e);
                }
            }

            try
            {
                foreach (string directoryName in Directory.GetDirectories(directory)) // 폴더 내 폴더 목록
                {
                    foreach (object obj in GenerateFileList(directoryName)) // 해당 폴더로 다시 GenerateFileList 재귀 호출
                    {
                        fileList.Add(obj); // 해당 폴더 내의 파일, 폴더 추가
                    }
                }
            }
            catch (Exception e)
            {
                throw new AggregateException("FileCompression.GenetateFileList : Fail to add directories", e);
            }

            return fileList;
        }

        /// <summary>
        /// ZIP 압축 파일 풀기
        /// </summary>
        /// <param name="zipFilePath">ZIP파일 경로</param>
        /// <param name="unZipTargetFolderPath">압축 풀 폴더 경로</param>
        /// <param name="password">해지 암호</param>
        /// <param name="isDeleteZipFile">zip파일 삭제 여부</param>
        /// <returns>압축 풀기 성공 여부 </returns>
        public static bool Unzip(string zipFilePath, string unZipTargetFolderPath, string password, bool isDeleteZipFile)
        {
            if (!File.Exists(zipFilePath)) // ZIP 파일이 있는 경우만 수행
                return false;

            using (ZipInputStream zipInputStream = new ZipInputStream(File.OpenRead(zipFilePath))) // ZIP 스트림 생성
            {
                if (password != null && password != string.Empty) // 패스워드가 있는 경우 패스워드 지정
                    zipInputStream.Password = password;

                try
                {
                    ZipEntry theEntry;

                    while ((theEntry = zipInputStream.GetNextEntry()) != null) // 반복하며 파일을 가져옴
                    {
                        string directoryName = Path.GetDirectoryName(theEntry.Name); // 폴더 명칭
                        string fileName = Path.GetFileName(theEntry.Name); // 파일 명칭

                        Directory.CreateDirectory(unZipTargetFolderPath + directoryName); // 폴더 생성

                        if (fileName == string.Empty) // 파일 이름이 없으면 Pass
                            continue;

                        using (FileStream streamWriter = File.Create((unZipTargetFolderPath + theEntry.Name))) // 파일 스트림 생성.(파일생성)
                        {
                            byte[] data = new byte[2048];

                            while (true) // 파일 복사
                            {
                                int size = zipInputStream.Read(data, 0, data.Length);

                                if (size > 0)
                                    streamWriter.Write(data, 0, size);
                                else
                                    break;
                            }

                            streamWriter.Close(); // 파일스트림 종료
                        }
                    }
                }
                catch (Exception e)
                {
                    throw new AggregateException("FileCompression.Unzip : Fail to ZipInputStream", e);
                }
                finally
                {
                    zipInputStream.Close(); // ZIP 파일 스트림 종료
                }
            }

            if (isDeleteZipFile) // ZIP파일 삭제를 원할 경우 파일 삭제
            {
                try
                {
                    File.Delete(zipFilePath);
                }
                catch (Exception e)
                {
                    throw new AggregateException("FileCompression.Unzip : Fail to delete zip file", e);
                }
            }

            return true;
        }

        /// <summary>
        /// tgz 파일 압축 풀기
        /// </summary>
        /// <param name="tgzFileName">tgz 파일명</param>
        /// <param name="savePath">저장할 파일 위치</param>
        public static void UnTgz(string tgzFileName, string savePath)
        {
            FileInfo fi = null;
            try
            {
                fi = new FileInfo(tgzFileName);
            }
            catch (Exception e)
            {
                throw new AggregateException("FileCompression.UnTgz : Fail to get fileInfo", e);
            }

            if (fi.Exists == false)
            {
                throw new Exception("파일이 존재하지 않습니다.");
            }

            using (SevenZipExtractor sevenZipExtractor = new SevenZipExtractor(tgzFileName))
            {
                try
                {
                    sevenZipExtractor.EventSynchronization = EventSynchronizationStrategy.AlwaysSynchronous;
                    savePath = savePath.Replace(@"\\", @"\");

                    sevenZipExtractor.ExtractionFinished +=
                        (sender, e) =>
                        {
                            ExtractionFinishedHandler(null,
                                                      new SevenZipExtractionFinishedEventArgs(savePath));
                        };

                    sevenZipExtractor.ExtractArchive(savePath);
                }
                catch (Exception e)
                {
                    throw new AggregateException("FileCompression.UnTgz : Fail sevenZipExtractor", e);
                }
            }
        }

        /// <summary>
        /// ExtractionFinishedHandler의 EventArgs
        /// UnTgz에서 저장할 파일 위치를 내부에 담아서 전달
        /// </summary>
        private class SevenZipExtractionFinishedEventArgs
        {
            public string ExtractPath { get; set; }

            public SevenZipExtractionFinishedEventArgs(string extractPath)
            {
                ExtractPath = extractPath;
            }
        }

        /// <summary>
        /// SevenZupExtractor에서 사용하는 ExtractionFinished Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void ExtractionFinishedHandler(object sender, SevenZipExtractionFinishedEventArgs e)
        {
            try
            {
                FileInfo fi = new FileInfo((sender as SevenZipExtractor).FileName);
                string fileName = fi.Name.Replace(fi.Extension, "");
                string DirName = e.ExtractPath;
                ArrayList ar = GenerateFileList(DirName);
                foreach (string Fil in ar)
                {
                    fi = new FileInfo(Fil);
                    if (fi.Extension.ToUpper() == ".TAR")
                    {
                        UnTar(Fil);
                        fi.Delete();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new AggregateException("FileCompression.ExtractionFinishedHandler : Fail sevenZipExtractor", ex);
            }
        }

        /// <summary>
        /// tar 파일 압축 해제
        /// </summary>
        /// <param name="TarName">tar 파일명</param>
        public static void UnTar(string TarName)
        {
            try
            {
                FileInfo fi = new FileInfo(TarName);

                using (TarInputStream s = new TarInputStream(File.OpenRead(TarName)))
                {
                    TarEntry theEntry;

                    while ((theEntry = s.GetNextEntry()) != null)
                    {
                        string FullName = string.Format("{0}\\{1}", fi.DirectoryName, theEntry.Name);
                        string DirName = Path.GetDirectoryName(FullName);
                        string FileName = Path.GetFileName(FullName);

                        if (!Directory.Exists(DirName)) Directory.CreateDirectory(DirName);

                        if (FileName != string.Empty)
                        {
                            FileStream SW = File.Create(FullName);

                            int Size = 2048;
                            byte[] data = new byte[2048];
                            while (true)
                            {
                                Size = s.Read(data, 0, data.Length);
                                if (Size > 0) SW.Write(data, 0, Size);
                                else break;
                            }
                            SW.Close();
                        }
                    }
                }

            }
            catch (Exception e)
            {
                throw new AggregateException("FileCompression.UnTar : Fail untar", e);
            }
        }
    }
}
```
