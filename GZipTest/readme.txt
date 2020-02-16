1) The help display.
GZipTest.exe
GZipTest.exe /?
GZipTest.exe -?
GZipTest.exe /h
GZipTest.exe -h

2) The file compression.

GZipTest.exe compress <path to the source file> [<path to the result file>]

If <path to the result file> is not specified, the resulting file is created 
in the same directory as the source file with adding the extension ".gz".

3) The file decompression.

GZipTest.exe decompress <path to the source file> <path to the result file>

If successful, the program returns 0, otherwise 1.

4) Additional parameters
Additional parameters contains in GZipTest.exe.config:

4.1) ReadBlockMaxSize - max size of bytes block for reading source file.
Default value is 1 [MB] = 1 << 20 [B] = 1024 * 1024 [B] = 1048576 [B]

4.2) RewriteExistsFile - flag to rewriting exists result file.
Default value - False
If value True  existed result file will be rewtited.
If value False to file name of existed result file will be add the suffix with 
free index in parenheses.

4.3) MaxBufferLength
Default value is 10
Allows you to set a limit on the maximum length of each of buffers to save RAM.

4.4) OutputState
Default value is OutputStateEnum.Default
Allows you to set a choice one of values from "Debug" or "Default".
If "Default" is selected, then when the program  is executed, the percentage
of completion, elapsed time, and the expected and remaining time are displayed.
If "Debug" is selected, then  when  the  program is executed, the current and 
maximum value of the buffer queues lenngths are displayed, as well as the maximum 
time for the conversion step to be executed when adding an item to the queue.


