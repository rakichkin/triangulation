import random
import matplotlib.path
import matplotlib.pyplot as plt

def generate(count, init_x, init_y):
    plt.xlim(0, 100)
    plt.ylim(0, 100)
    plt.grid()

    axes = plt.gca()
    axes.set_aspect("equal")

    plt.scatter(x, y, color='red')

    plt.show()

    points = []
    distance = random.uniform(-100, 100)
    x = distance + random.uniform(-100, 100)
    y = distance + random.uniform(-100, 100)
    
