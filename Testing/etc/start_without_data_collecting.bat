@echo off


echo Starting filtration and triangulation...
.\Testing.exe
echo Filtration and triangulation success

echo Printing results...

cd .\scripts
python .\points_visualizer.py

pause

