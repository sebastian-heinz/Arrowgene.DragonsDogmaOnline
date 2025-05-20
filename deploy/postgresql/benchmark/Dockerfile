FROM postgres:17

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
       python3 python3-pip python3-venv python3-dev gcc libpq-dev \
    && rm -rf /var/lib/apt/lists/*

RUN ln -s /usr/bin/python3 /usr/local/bin/python
RUN pip3 install --break-system-packages --no-cache-dir --upgrade pip wheel setuptools
RUN pip3 install --break-system-packages psycopg2 faker numpy

EXPOSE 5432
