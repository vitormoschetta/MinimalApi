ARG DOTNET_VERSION=6.0
FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS build

COPY TodoApi/ /app/
RUN dotnet publish /app/TodoApi.csproj -c Release -o /public

FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION}
WORKDIR /public
COPY --from=build /public .

ENTRYPOINT ["/usr/bin/dotnet", "/public/TodoApi.dll"]
