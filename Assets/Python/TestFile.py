import random as r

class Points:
    def __init__(self, int: size):
        self.size = size
    
    def makePoints(self):
        points = []
        for i in range(8):
            x = r.randint(-9, 9)
            y = r.randint(-9, 9)
            points.append([x, y])
        return points