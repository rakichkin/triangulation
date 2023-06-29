@echo off


echo Starting filtration and triangulation...
.\Triangulation\bin\Debug\net6.0\Triangulation.exe
echo Filtration and triangulation success

echo Printing results...

cd .\scripts
python .\points_visualizer.py

pause

