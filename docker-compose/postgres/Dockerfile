# Use the official PostgreSQL image
FROM postgres:13

# Set environment variables for PostgreSQL
ENV POSTGRES_USER=auction_user
ENV POSTGRES_PASSWORD=auction_password
ENV POSTGRES_DB=auction_db

# Expose the PostgreSQL port
EXPOSE 5432

# Copy custom initialization scripts if needed
# COPY init.sql /docker-entrypoint-initdb.d/
