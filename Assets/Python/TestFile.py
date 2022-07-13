import random

class Points:
    def __init__(self, size):
        self.size = size
    
    def makePoints(self):
        points = []
        for i in range(self.size):
            x = random.randint(-9, 9)
            y = random.randint(-9, 9)
            points.append([x, y])
        return points

# p = Points(2)
# pts = p.makePoints()
# for pt in pts:
#     print(pt)