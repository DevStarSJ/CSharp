## FileCompression.NET

## Abstract

File Compression utility in `C#`

- Zip, Unzip
- Decompress unix type file : Z, Tgz, Tar

##Notice

Please add a comment below on the following `Project Properties` -> `Build Event` -> `Post-build event command line:`
```
COPY /Y "$(SolutionDir)$(ProjectName)\7z.dll" "$(SolutionDir)$(SolutionName)\$(OutDir)\7z.dll"
```

##Decription

I used SevenZip, SharpZipLib, 7zip for each file types.
- Z file : `7z.dll` and `LzwInputStream.cs`
- Zip file : SharpZipLib
- Tgz file : SevenZip
- Tar file : SharpZipLib
