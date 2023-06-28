import matplotlib.patches
import matplotlib.path
import matplotlib.pyplot as plt

import json

def main():
    src_positions = deserialize_json('points_visualizer\gps_data.json')
    kalman_filter_points = deserialize_json('points_visualizer\\kalman_results.json')
    monte_carlo_points = deserialize_json('points_visualizer\\monte_carlo_results.json')

    min_lim = src_positions[0]['point']['x']
    max_lim = src_positions[0]['point']['x']
    for pos in src_positions:
        if pos['point']['x'] < min_lim:
            min_lim = pos['point']['x']
        if pos['point']['y'] < min_lim:
            min_lim = pos['point']['y']
        if pos['point']['x'] > max_lim:
            max_lim = pos['point']['x']
        if pos['point']['y'] > max_lim:
            max_lim = pos['point']['y']

    plt.xlim(37.96, 38.01)
    plt.ylim(59.11, 59.13)
    plt.grid()

    axes = plt.gca()
    axes.set_aspect("equal")

    draw_points(src_positions, 'red')
    draw_points(kalman_filter_points, 'blue')
    # draw_points(monte_carlo_points, 'green')

    plt.show()



def draw_points(positions, color:str):
    for pos in positions:
        x = pos['point']['x']
        y = pos['point']['y']
        plt.scatter(x, y, color=color)

def deserialize_json(file_path:str):
    data_str = open(file_path)
    data = json.load(data_str)
    return data

if __name__ == "__main__":
    main()