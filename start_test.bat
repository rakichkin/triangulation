@echo off

echo Running mouse movements collector script...
cd scripts
python .\mouse_position_collector.py
cls
echo Movements collected successfull

echo Starting filtration and triangulation...
.\..\Triangulation\bin\Debug\net6.0\Triangulation.exe
echo Filtration and triangulation success

echo Printing results...
python .\points_visualizer.py

pause

