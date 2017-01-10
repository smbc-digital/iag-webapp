FROM microsoft/dotnet:1.0.0-preview2-sdk

MAINTAINER Stockport Digital By Design

# Create an app folder to run the application in
RUN mkdir -p /opt/app/src \
    && mkdir -p /opt/app/test

# Install .Net Dependencies
COPY global.json /opt/app/
COPY src/StockportWebapp/project.json /opt/app/src/StockportWebapp/
COPY test/StockportWebappTests/project.json /opt/app/test/StockportWebappTests/

RUN cd /opt/app/src/StockportWebapp \
    && dotnet restore \
    && cd /opt/app/test/${my_test} \
    && dotnet restore

# Copy in code and tests
COPY src/StockportWebapp /opt/app/src/StockportWebapp
COPY test/StockportWebappTests /opt/app/test/StockportWebappTests

# Test
WORKDIR /opt/app/test/StockportWebappTests
RUN dotnet test -parallel none

# Expose port 5000
EXPOSE 5000

# Start the application web target on running container
WORKDIR /opt/app/src/StockportWebapp
ENTRYPOINT [ "dotnet", "run" ]
