md new
icacls ".\new" /grant Everyone:(OI)(CI)F

cd new 
move ..\*.avi .
for %%a in ("*.avi") do ffmpeg -i "%%a" -c:v libx264 -preset slow -crf 25 "%%~na.mp4"
copy *.mp4 .\..
del /F /Q *.avi
del /F /Q *.mp4

cd ..
rd /s /q  ".\new"