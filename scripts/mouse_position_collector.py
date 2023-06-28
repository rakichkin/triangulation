from win32 import win32gui
import atexit
import json
import time
import math

def main():
    static_points = read_static_points()
    
    movements_detailed = list()
    movements_short = list()
    atexit.register(on_exit, movements_detailed, movements_short, static_points)

    while(True):
        _, _, (x, y) = win32gui.GetCursorInfo()

        distances = list()
        for p in static_points:
            distances.append(calculate_distance(p, (x, y)))
        
        append_mov_detailed(movements_detailed, static_points, distances, (x, y))
        movements_short.append(distances)
        time.sleep(0.01)
        
        
def read_static_points():
    with open('config.json', 'r') as file:
        points_str = file.read()
        return json.loads(points_str)["static_points"]
   

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

def on_exit(movements_detailed:list,  movements_short:list, static_points: list):
    with open('data\movements_detailed.json', 'w') as outfile:
        outfile.write(json.dumps(movements_detailed))
    with open('data\movements_short.json', 'w') as outfile:
        outfile.write(json.dumps(movements_short))
        
    real_points = list()
    for movement in movements_detailed:
        moving_point = movement[len(movement)-1]['moving_point']
        real_points.append(moving_point)
        
    with open('data\\real_moving_point_coords.json', 'w') as outfile:
        outfile.write(json.dumps(real_points))

if(__name__ == '__main__'):
    main()