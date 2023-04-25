FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
COPY ["Lazuli/Lazuli.csproj" ,"Lazuli/"]
COPY ["Lazuli/Lazuli.csproj" ,"LazuliLibrary/"]

RUN dotnet restore "Lazuli/Lazuli.csproj"

COPY Lazuli Lazuli/
COPY LazuliLibrary LazuliLibrary/

RUN dotnet build "Lazuli/Lazuli.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Lazuli/Lazuli.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT [ "dotnet", "Lazuli.dll" ]