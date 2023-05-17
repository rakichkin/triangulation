import json
import random
import datetime
import math

def generate_gps_data_linear(num_points):
    data = []
    x = 100.67834
    y = 100.34789
    for i in range(num_points):
        date = datetime.datetime.now().isoformat()
        if(math.ceil(random.random() * 1000) == 5):
            data.append({
                "point": {
                    "x": random.random() * 3000,
                    "y": random.random() * 3000
                },
                "date": date})
        else:
            x += random.uniform(-30, random.uniform(30, 80))
            y += random.uniform(-30, random.uniform(30, 80))
            data.append({
                "point": {
                    "x": x,
                    "y": y
                },
                "date": date})
            
        # data.append({"x": 100, "y": 100, "date": date})
        # data.append({"x": 110, "y": 110, "date": date})
        # data.append({"x": 120, "y": 120, "date": date})
        # data.append({"x": 130, "y": 130, "date": date})
        # data.append({"x": 140, "y": 140, "date": date})
        # data.append({"x": 150, "y": 150, "date": date})
        # data.append({"x": 160, "y": 160, "date": date})
        # data.append({"x": 170, "y": 170, "date": date})
    return data

def generate_gps_data_random(num_points):
    data = []
    x = 100.67834
    y = 100.34789
    for i in range(num_points):
        date = datetime.datetime.now().isoformat()
        # if(math.ceil(random.random() * 100000) == 5):
        #     data.append({"x": random.random() * 3000, "y": random.random() * 3000, "date": date})
        # else:
        x += random.uniform(-10, 11)
        y += random.uniform(-10, 11)
        data.append({
            "point": {
                "x": x,
                "y": y
            },
            "date": date})
    return data

# gps_data = generate_gps_data_linear(1000)
gps_data = generate_gps_data_random(1000)
with open("points_visualizer\gps_data.json", "w") as outfile:
    json.dump(gps_data, outfile)
    