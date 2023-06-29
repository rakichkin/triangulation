import matplotlib.patches
import matplotlib.path
import matplotlib.pyplot as plt
import json
import datetime
from PIL import Image
from PIL import ImageDraw
from PIL import ImageFont


def main():
    src_point_coords = deserialize_json('.\\data\\real_moving_point_coords.json')
    unfiltered_triang_point_coord = deserialize_json('.\\data\\unfiltered_triangulation.json')
    kalman_triang_point_coord = deserialize_json('.\\data\\kalman_triangulation.json')
    mkarlo_triang_point_coord = deserialize_json('.\\data\\monte_carlo_triangulation.json')
    
    figure, subplots = plt.subplots(2, 2, figsize=(19.2, 10.8), dpi=100)
    
    subplots[0, 0].set_xlim(0, 1920)
    subplots[0, 0].set_ylim(0, 1080)
    for point in src_point_coords:
        subplots[0, 0].plot(point['x'], point['y'], marker="o", markersize=4, markeredgecolor="red", markerfacecolor="red")
    subplots[0, 0].set_title("Raw points")
    subplots[0,0].grid(True)
    
    subplots[0, 1].set_xlim(0, 1920)
    subplots[0, 1].set_ylim(0, 1080)
    for point in unfiltered_triang_point_coord:
        subplots[0, 1].plot(point['x'], point['y'], marker="o", markersize=4, markeredgecolor="red", markerfacecolor="red")
    subplots[0, 1].set_title("Triangulated points")
    subplots[0,1].grid(True)
    
    subplots[1, 0].set_xlim(0, 1920)
    subplots[1, 0].set_ylim(0, 1080)
    for point in kalman_triang_point_coord:
        subplots[1, 0].plot(point['x'], point['y'], marker="o", markersize=4, markeredgecolor="red", markerfacecolor="red")
    subplots[1, 0].set_title("Kalman Filter")
    subplots[1, 0].grid(True)

    for point in mkarlo_triang_point_coord:
        subplots[1, 1].plot(point['x'], point['y'], marker="o", markersize=4, markeredgecolor="red", markerfacecolor="red")
    subplots[1, 1].set_title("Monte-Carlo filter")
    subplots[1,1].grid(True)


    # figure.show()
    # plt.waitforbuttonpress()
    
    img_name = f'data\\imgs\\{datetime.datetime.now().strftime("%Y_%m_%d_%H_%M_%S")}.png'
    figure.savefig(img_name)

    print_config_values_and_save(img_name)

def print_config_values_and_save(img_name):
    with open('config.json', 'r') as file:
        text = file.read()
    
    img = Image.open(img_name)
    i1 = ImageDraw.Draw(img)
    font = ImageFont.truetype('Arial.ttf', 20)
    i1.text((-10, -10), text, font=font, fill=(255, 0, 0))
    img.show()
    img.save(img_name)

def deserialize_json(file_path:str):
    with open(file_path, 'r') as file:
        data_str = file.read()
    data = json.loads(data_str)
    return data

if __name__ == "__main__":
    main()