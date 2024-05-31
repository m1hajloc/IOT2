# Dockerfile za sensor projekt
FROM python:3.9-slim

WORKDIR /app

COPY sensor_service.py .

RUN pip install pymongo paho-mqtt 

CMD ["python", "sensor_service.py"]
