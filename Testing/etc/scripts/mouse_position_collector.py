from win32 import win32gui
import ctypes
import atexit
import json
import time
import math
import os

def main():
    config = read_config()
    
    static_points = config['static_points']
    
    movements_detailed = list()
    movements_short = list()
    atexit.register(on_exit, movements_detailed, movements_short)
    
    while(True):
        _, _, (x, y) = win32gui.GetCursorInfo()

        scaleFactor = ctypes.windll.shcore.GetScaleFactorForDevice(0) / 100
        x = x * scaleFactor
        y = y * scaleFactor
        
        distances = list()
        for p in static_points:
            distances.append(calculate_distance(p, (x, y)))
        
        append_mov_detailed(movements_detailed, static_points, distances, (x, y))
        movements_short.append(distances)
        time.sleep(config['mouse_pos_collecting_freq'])
    
def read_config():
    with open(os.path.join('..', 'config.json'), 'r') as file:
        config_str = file.read()
        return json.loads(config_str)
   
def append_mov_detailed(mov_detailed: list, static_points: list(), distances: list(), moving_point: tuple):
    movement = list()
        
    for (point, distance) in zip(static_points, distances):
        movement.append({
            "point": {
                "x": point[0],
                "y": point[1]
            },
            "distance": distance
        })
        
    movement.append({
        "moving_point": {
            "x": moving_point[0],
            "y": moving_point[1]
        }
    })
        
    mov_detailed.append(movement)
    
def calculate_distance(point1, point2) -> float:
    distance = math.sqrt((point2[0] - point1[0]) ** 2 + (point2[1] - point1[1]) ** 2)
    return distance

def on_exit(movements_detailed:list,  movements_short:list):
    if not os.path.exists(os.path.join('..', 'data')):
        os.mkdir(os.path.join('..', 'data'))
    
    with open(os.path.join('..', 'data', 'movements_detailed.json'), 'w') as outfile:
        outfile.write(json.dumps(movements_detailed))
    with open(os.path.join('..', 'data', 'movements_short.json'), 'w') as outfile:
        outfile.write(json.dumps(movements_short))
        
    real_points = list()
    for movement in movements_detailed:
        moving_point = movement[len(movement)-1]['moving_point']
        real_points.append(moving_point)
        
    with open(os.path.join('..', 'data', 'real_moving_point_coords.json'), 'w') as outfile:
        outfile.write(json.dumps(real_points))

if(__name__ == '__main__'):
    main()