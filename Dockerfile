# https://hub.docker.com/_/microsoft-dotnet
# https://medium.com/@oluwabukunmi.aluko/dockerize-asp-net-core-web-app-with-multiple-layers-projects-part1-2256aa1b0511
# transact/Transact.APII/Dockerfile
# To create an image, run the next command in the root of project.
# cd transact
# docker build -f Transact.API/Dockerfile -t transact-backend .
# Both API and Data must be present here as per the sln file

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app 

# copy csproj and restore as distinct layers
COPY *.sln .
COPY Transact.API/*.csproj ./Transact.API/
COPY Transact.Data/*.csproj ./Transact.Data/
RUN dotnet restore 

# copy everything else and build app
COPY Transact.API/. ./Transact.API/
COPY Transact.Data/. ./Transact.Data/

WORKDIR /app/Transact.API
RUN dotnet publish -c release -o out 

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
WORKDIR /app 

COPY --from=build /app/Transact.API/out ./
EXPOSE 5000
ENTRYPOINT ["dotnet", "Transact.API.dll"]
