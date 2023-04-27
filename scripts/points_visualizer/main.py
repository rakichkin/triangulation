import matplotlib.patches
import matplotlib.path
import matplotlib.pyplot as plt

import json

def main():
    src_points = deserialize_json('points_visualizer\gps_data.json')
    filtered_points = deserialize_json('points_visualizer\\f.json')

    min_lim = src_points[0]['x']
    max_lim = src_points[0]['x']
    for point in src_points:
        if point['x'] < min_lim:
            min_lim = point['x']
        if point['y'] < min_lim:
            min_lim = point['y']
        if point['x'] > max_lim:
            max_lim = point['x']
        if point['y'] > max_lim:
            max_lim = point['y']

    plt.xlim(0, max_lim)
    plt.ylim(0, max_lim)
    plt.grid()

    axes = plt.gca()
    axes.set_aspect("equal")

    draw_points(src_points, 'red')
    draw_points(filtered_points, 'blue')

    plt.show()



def draw_points(points, color:str):
    for point in points:
        x = point['x']
        y = point['y']
        plt.scatter(x, y, color=color)

def deserialize_json(file_path:str):
    data_str = open(file_path)
    data = json.load(data_str)
    return data

if __name__ == "__main__":
    main()