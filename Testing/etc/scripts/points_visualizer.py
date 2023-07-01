import matplotlib.patches
import matplotlib.path
import matplotlib.pyplot as plt
import json
import datetime
import os
import ctypes
import platform
from PIL import Image
from PIL import ImageDraw
from PIL import ImageFont

def draw(subplot, points, title):
    subplot.set_xlim(0, 1920)
    subplot.set_ylim(0, 1080)
    
    static_points = config['static_points']
    for point in static_points:
        subplot.plot(point[0], point[1], marker="o", markersize=6, markeredgecolor="blue", markerfacecolor="blue")
    
    for point in points:
        subplot.plot(point['x'], point['y'], marker="o", markersize=4, markeredgecolor="red", markerfacecolor="red")
    subplot.set_title(title)
    subplot.grid(which='both')
    subplot.minorticks_on()

def print_config_values_and_save(img_name):
    with open(os.path.join('..', 'config.json'), 'r') as file:
        text = file.read()
    
    img = Image.open(img_name)
    i1 = ImageDraw.Draw(img)
    
    if platform.system() == 'Windows':
        font_path = os.path.join(os.environ['WINDIR'], 'fonts', 'arial.ttf')
    elif platform.system() == 'Linux':
        font_path = os.path.join('/urs', 'share', 'fonts', 'arial.ttf')
    
    font = ImageFont.truetype(font_path, 20)
    i1.text((-10, -10), text, font=font, fill=(255, 0, 0))
    img.show()
    img.save(img_name)

def deserialize_json(file_path:str):
    with open(file_path, 'r') as file:
        data_str = file.read()
    data = json.loads(data_str)
    return data

def read_config():
    with open(os.path.join('..', 'config.json'), 'r') as file:
        config_str = file.read()
        return json.loads(config_str)
    
config = read_config()

def main():
    # src_point_coords = deserialize_json('.\\data\\real_moving_point_coords.json')
    # unfiltered_triang_point_coord = deserialize_json('.\\data\\unfiltered_triangulation.json')
    # kalman_triang_point_coord = deserialize_json('.\\data\\kalman_triangulation.json')
    # mkarlo_triang_point_coord = deserialize_json('.\\data\\monte_carlo_triangulation.json')
    
    src_point_coords = deserialize_json(os.path.join('..', 'data', 'real_moving_point_coords.json'))
    unfiltered_triang_point_coord = deserialize_json(os.path.join('..', 'data', 'unfiltered_triangulation.json'))
    kalman_triang_point_coord = deserialize_json(os.path.join('..', 'data', 'kalman_triangulation.json'))
    mkarlo_triang_point_coord = deserialize_json(os.path.join('..', 'data', 'monte_carlo_triangulation.json'))

    screensize = ctypes.windll.user32.GetSystemMetrics(0), ctypes.windll.user32.GetSystemMetrics(1)
    figure, subplots = plt.subplots(2, 2, figsize=(screensize[0] / 100, screensize[1] / 100), dpi=100)
    
    draw(subplots[0, 0], src_point_coords, 'Raw points')
    draw(subplots[0, 1], unfiltered_triang_point_coord, 'Triangulated points')
    draw(subplots[1, 0], kalman_triang_point_coord, 'Kalman Filter')
    draw(subplots[1, 1], mkarlo_triang_point_coord, 'Monte-Carlo filter')

    if not os.path.exists('output_imgs'): 
        os.mkdir('output_imgs')
    img_name = os.path.join('output_imgs', f'{datetime.datetime.now().strftime("%Y_%m_%d_%H_%M_%S")}.png')
    figure.savefig(img_name)

    print_config_values_and_save(img_name)

if __name__ == "__main__":
    main()