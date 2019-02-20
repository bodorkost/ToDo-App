FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /app

COPY *.sln .
COPY . .
WORKDIR /app/ToDo-App
RUN dotnet restore

RUN dotnet publish -c Release -o out

FROM microsoft/dotnet:2.2-aspnetcore-runtime AS runtime
WORKDIR /app
COPY --from=build /app/ToDo-App/out ./

# Optional: Set this here if not setting it from docker-compose.yml
# ENV ASPNETCORE_ENVIRONMENT Development

ENTRYPOINT ["dotnet", "ToDo-App.dll"]