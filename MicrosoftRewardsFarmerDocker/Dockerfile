FROM ghcr.io/haq/microsoft-rewards-bot:latest
ENV shm_size=4gb
WORKDIR /app
COPY ./autoRun.py /app/autoRun.py
COPY ./utils.py /app/src/utils.py
COPY ./requirements.txt /app/requirements.txt
COPY ./entrypoint.sh /app/entrypoint.sh
RUN pip install -r requirements.txt

# No extra arguments
CMD ["python", "autoRun.py"]

# With telegram bot
# CMD ["python", "autoRun.py", "--telegram", "123456789:ABCdefGHIjklMNoPQrsTUVWxyZ", "123456789"]

# With discord webhook
# CMD ["python", "autoRun.py", "--discord", "https://discord.com/api/webhooks/123456789/ABCdefGHIjklMNoPQrsTUVWxyZ"]

# With telegram bot and discord webhook
# CMD ["python", "autoRun.py", "--telegram", "123456789:ABCdefGHIjklMNoPQrsTUVWxyZ", "123456789", "--discord", "https://discord.com/api/webhooks/123456789/ABCdefGHIjklMNoPQrsTUVWxyZ"]

# !!Important!!: Only use one of the above commands, not multiple at the same time