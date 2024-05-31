import json
import paho.mqtt.client as mqtt
import pymongo

mongo_client = pymongo.MongoClient("mongodb://localhost:27017/")
db = mongo_client["database"]
collection = db["tempdata"]

mqtt_client = mqtt.Client()

def on_connect(client, userdata, flags, rc):
    if rc == 0:
        print("Connected to MQTT broker")
        sensor_data_list = collection.find()
        for sensor_data in sensor_data_list:
            sensor_data['_id'] = str(sensor_data['_id'])
            payload = {
                "_id": sensor_data['_id'],
                "room_id/id": sensor_data["room_id/id"],
                "noted_date": sensor_data["noted_date"],
                "temp": sensor_data["temp"],
                "out_in": sensor_data["out/in"]
            }
            mqtt_client.publish("sensor_data", json.dumps(payload))
            print("Published sensor data to MQTT broker:", payload)
    else:
        print(f"Failed to connect to MQTT broker, return code {rc}")

mqtt_client.on_connect = on_connect
mqtt_client.connect("localhost", 1883, 60)
mqtt_client.loop_start()
mqtt_client.loop_forever()
