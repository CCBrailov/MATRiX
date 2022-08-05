#   __  __         _______  _____   _ __   __   _____                               
#  |  \/  |    /\ |__   __||  __ \ (_)\ \ / /  / ____|                              
#  | \  / |   /  \   | |   | |__) | _  \ V /  | (___    ___  _ __ __   __ ___  _ __ 
#  | |\/| |  / /\ \  | |   |  _  / | |  > <    \___ \  / _ \| '__|\ \ / // _ \| '__|
#  | |  | | / ____ \ | |   | | \ \ | | / . \   ____) ||  __/| |    \ V /|  __/| |   
#  |_|  |_|/_/    \_\|_|   |_|  \_\|_|/_/ \_\ |_____/  \___||_|     \_/  \___||_|   
# ==================================================================================
#  Charles Brailovsky
#  Contact: bakerc@lawrence.edu AND/OR chai.baker@gmail.com
# ==================================================================================
#  Instructions:
#       Copy this file into your model's repository
#       Add imports as needed:

import socket

#       Fill in the following required functions:

def unpack(dataString: str):
    # MATRiX will send a single sample for prediction through the server as a string
    # This function needs to unpack the string into a sample your model can predict from
    return sample

def getTrajectories(sample):
    # This function must take the sample unpacked by the previous function and use your model to return trajectory predictions for agents in the scene
    return preds

def pack(preds):
    # This function must take in predictions (in whatever format your model creates) and pack them into a string that can be encoded and passed through the server
    # MATRiX will decode and unpack the string on the other side and pass predictions to MATRiX agents as navigation paths
    return package

#       Run this file before starting MATRiX in Unity

# =================================================================================

HOST = "127.0.0.1"
PORT = 2063

with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
    s.bind((HOST, PORT))
    s.listen()
    conn, addr = s.accept()
    with conn:
        print(f"Connected by {addr}")
        while True:
            data = conn.recv(1024)
            if not data:
                break

            print("Received Sample")

            raw = data.decode() # string
            sample = unpack(raw) # object readable by your model
            preds = getTrajectories(sample) # object produced by your model
            package = pack(preds) # string produced by your packing function   

            print("Sending Predictions")

            conn.sendall(package)