import matplotlib.patches
import matplotlib.path
import matplotlib.pyplot as plt

import json

def main():
    data = deserialize_json()

    plt.xlim(-200, 200)
    plt.ylim(-200, 200)
    plt.grid()

    axes = plt.gca()
    axes.set_aspect("equal")

    for item in data:
        x = item['point']['x']
        y = item['point']['y']
        radius = item['distance']
        matplotlib.patches.Circle((x, y), radius=1, fill=True)
        axes.add_patch(
            matplotlib.patches.Circle((x, y), radius=radius, fill=False)
        )
        axes.add_patch(
            matplotlib.patches.Circle((x, y), radius=1, fill=True)
        )
    plt.show()

def deserialize_json():
    data_str = open('data.json')
    data = json.load(data_str)
    return data

if __name__ == "__main__":
    main()