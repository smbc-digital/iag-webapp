FROM microsoft/dotnet:1.0.0-preview2-sdk

MAINTAINER Stockport Digital By Design

ARG my_app=StockportWebapp
ARG my_test=${my_app}Tests

ARG user=app
ARG group=app
RUN groupadd -g 2101 ${group} \
    && useradd -u 2101 -g ${group} -m -s /bin/bash ${user}

# Create an app folder to run the application in
RUN mkdir -p /opt/app/src \
    && mkdir -p /opt/app/test

# Install .Net Dependencies
COPY global.json /opt/app/
COPY src/${my_app}/project.json /opt/app/src/${my_app}/
COPY test/${my_test}/project.json /opt/app/test/${my_test}/

# Use app user for all further actions
#RUN chown -R ${user}:${group} /opt/app
#USER ${user}

RUN cd /opt/app/src/${my_app} \
    && dotnet restore \
    && cd /opt/app/test/${my_test} \
    && dotnet restore

# Copy in code and tests
COPY src/${my_app} /opt/app/src/${my_app}
COPY test/${my_test} /opt/app/test/${my_test}

# Test
WORKDIR /opt/app/test/${my_test}
RUN dotnet test

# Expose port 5000
EXPOSE 5000

# Start the application web target on running container
WORKDIR /opt/app/src/${my_app}
ENTRYPOINT [ "dotnet", "run" ]
