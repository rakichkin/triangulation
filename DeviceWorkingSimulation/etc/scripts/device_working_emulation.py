import math
import sqlite3
import json
import os
import ctypes
from win32 import win32gui
import atexit
import time

def read_config():
    with open(os.path.join('..', 'config.json'), 'r') as file:
        config_str = file.read()
        return json.loads(config_str)

def calculate_distance(point1, point2) -> float:
    distance = math.sqrt((point2[0] - point1[0]) ** 2 + (point2[1] - point1[1]) ** 2)
    return distance

config = read_config()

def main():   
    conn = sqlite3.connect('distances.db')

    conn.execute('DROP TABLE IF EXISTS DISTANCES')
    conn.execute('''CREATE TABLE IF NOT EXISTS DISTANCES
        (id INTEGER PRIMARY KEY AUTOINCREMENT,
        distance1 REAL NOT NULL,
        distance2 REAL NOT NULL,
        distance3 REAL NOT NULL);''')
    
    atexit.register(on_exit, conn)
    
    while(True):
        _, _, (x, y) = win32gui.GetCursorInfo()
        scaleFactor = ctypes.windll.shcore.GetScaleFactorForDevice(0) / 100
        x = x * scaleFactor
        y = y * scaleFactor
        
        distance1 = calculate_distance((x, y), config['static_points'][0])
        distance2 = calculate_distance((x, y), config['static_points'][1])
        distance3 = calculate_distance((x, y), config['static_points'][2])
        
        conn.execute('INSERT INTO distances (distance1, distance2, distance3)' 
                        + f'VALUES ({distance1}, {distance2}, {distance3})')
        conn.commit()
        print(f'Values ({distance1}, {distance2}, {distance3}) inserted into database')
        
        time.sleep(config['mouse_pos_collecting_freq'])
        

def on_exit(connection):
    connection.commit()
    connection.close()

if __name__ == '__main__':
    main()