import time
import json
import paho.mqtt.client as mqtt

mqtt_client = mqtt.Client()

received_count = 0
published_count = 0
processed_ids = set()

def on_connect(client, userdata, flags, rc):
    if rc == 0:
        print("Connected to MQTT broker")
        mqtt_client.subscribe("sensor_data", qos=1)
    else:
        print(f"Failed to connect to MQTT broker, return code {rc}")

def on_message(client, userdata, msg):
    global received_count, published_count
    received_count += 1
    print(f"Received message {received_count}: {msg.payload.decode()}")

    data = json.loads(msg.payload)
    if data["_id"] not in processed_ids:
        processed_ids.add(data["_id"])
        if data["temp"] < 27 and data["out_in"] == "Out":
            mqtt_client.publish("analyzed_data", json.dumps(data), qos=1)
            published_count += 1
            print(f"Published analyzed data {published_count}: {data}")
    else:
        print(f"Duplicate message received: {data['_id']}")

def on_disconnect(client, userdata, rc):
    print(f"Disconnected from MQTT broker with code {rc}")
    print(f"Total received messages: {received_count}")
    print(f"Total published messages: {published_count}")

mqtt_client.on_connect = on_connect
mqtt_client.on_message = on_message
mqtt_client.on_disconnect = on_disconnect

while True:
    try:
        mqtt_client.connect("localhost", 1883, 60)
        break
    except Exception as e:
        print(f"Connection failed: {e}")
        time.sleep(5)

mqtt_client.loop_forever()
