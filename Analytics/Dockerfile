# Dockerfile za analytics projekt
FROM python:3.9-slim

WORKDIR /app

COPY Analytics_script.py .

RUN pip install paho-mqtt

CMD ["python", "Analytics_script.py"]
