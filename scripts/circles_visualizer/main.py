import matplotlib.patches
import matplotlib.path
import matplotlib.pyplot as plt

import json

def main():
    arrowprops = {
        'arrowstyle': '->',
    }

    triang_points = deserialize_points()
    desired_point = deserialize_desired_point()

    plt.xlim(0, 500)
    plt.ylim(0, 500)
    plt.grid()

    axes = plt.gca()
    axes.set_aspect("equal")

    flag = False

    plt.plot([0, 0], [20, 20], '-b')
    for item in triang_points:
        x = item['point']['x']
        y = item['point']['y']
        radius = item['distance']

        axes.add_patch(
            matplotlib.patches.Circle((x, y), radius=radius, fill=False)
        )
        plt.scatter(x, y, color='blue')

        if flag:
            plt.annotate('',
                xy=(x, y),
                xytext=(x, y+radius),
                arrowprops=arrowprops)
        else: 
            plt.annotate('',
                    xy=(x, y),
                    xytext=(x-radius, y),
                    arrowprops=arrowprops)
        flag = not flag

    plt.scatter(desired_point['x'], desired_point['y'], color='red')
    plt.show()

def deserialize_points():
    data_str = open('circles_visualizer/trilateration_points3.json')
    data = json.load(data_str)
    return data

def deserialize_desired_point():
    data_str = open('circles_visualizer/desired_point.json')
    data = json.load(data_str)
    return data

if __name__ == "__main__":
    main()