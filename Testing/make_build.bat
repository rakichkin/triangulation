@echo off

md .\build
copy .\bin\Debug\net6.0\ .\build\ /y

md .\build\etc\
copy .\bin\Debug\net6.0\etc\* .\build\etc\ /y

md .\build\etc\scripts
copy .\bin\Debug\net6.0\etc\scripts\* .\build\etc\scripts\ /y