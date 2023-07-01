@echo off

echo Running mouse movements collector script...
cd scripts
python .\mouse_position_collector.py
cls
echo Movements collected successfull

echo Starting filtration and triangulation...
cd ..\..\
.\Testing.exe
echo Filtration and triangulation success

echo Printing results...
cd etc\scripts
python .\points_visualizer.py

pause

