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
    movements = list()
    atexit.register(on_exit, movements)
    while(True):
        _, _, (x, y) = win32gui.GetCursorInfo()
        distance1 = calculate_distance(Point(0, 0), Point(x, y))
        distance2 = calculate_distance(Point(1919, 0), Point(x, y))
        distance3 = calculate_distance(Point(0, 1079), Point(x, y))
        distance4 = calculate_distance(Point(1919, 1079), Point(x, y))
        
        movement = list()
        movement.append({
            "point": {
                "x": 0,
                "y": 0
            },
            "distance": distance1
        })
        movement.append({
            "point": {
                "x": 1919,
                "y": 0
            }, 
            "distance": distance2
        })
        movement.append({
            "point": {
                "x": 0,
                "y": 1079,
            },
            "distance": distance3
        })
        movement.append({
            "point": {
                "x": 1919,
                "y": 1079,
            },
            "distance": distance4
        })
        movement.append({
            "moving_point": {
                "x": x,
                "y": y,
            }
        })
        
        movements.append(movement)

        time.sleep(0.1)
        
def calculate_distance(point1:Point, point2:Point) -> float:
    distance = math.sqrt((point2.x - point1.x) ** 2 + (point2.y - point1.y) ** 2)
    return distance

def on_exit(movements:list):
    with open('mouse_position_collector\movements.json', 'w') as outfile:
        outfile.write(json.dumps(movements))

if(__name__ == '__main__'):
    main()