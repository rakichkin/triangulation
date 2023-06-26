from win32 import win32gui
import atexit
import json
import time
import math

class Point:
    def __init__(self, x, y):
        self.x = x
        self.y = y

def main():
    static_points = [ (0, 0), (1919, 0), (0,1079), (1919, 1079) ]
    
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
        
        time.sleep(0.1)
        
        
def append_mov_detailed(mov_detailed: list, static_points: list(), distances: list(), moving_point: tuple):
    movement = list()
    
    # for i in range(0, len(static_points)):
    #     movement.append({
    #         "point": {
    #             "x": static_points[i][0],
    #             "y": static_points[i][1]
    #         },
    #         "distance": distances[i]
    #     })
        
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
    with open('mouse_position_collector\movements_detailed.json', 'w') as outfile:
        outfile.write(json.dumps(movements_detailed))
    with open('mouse_position_collector\movements_short.json', 'w') as outfile:
        outfile.write(json.dumps(movements_short))
    with open('mouse_position_collector\static_points.json', 'w') as outfile:
        outfile.write(json.dumps(static_points))

if(__name__ == '__main__'):
    main()