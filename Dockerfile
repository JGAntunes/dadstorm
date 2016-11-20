FROM microsoft/dotnet-framework:4.6.2

# copy project.json and restore as distinct layers
WORKDIR /dadstorm
COPY project.json .
RUN dotnet restore

# copy and build everything else
COPY ./src .
RUN dotnet publish -c Release -o out
ENTRYPOINT ["dotnet", "out/dadstorm.dll"]
