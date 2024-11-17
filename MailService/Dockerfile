FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5100

ENV ASPNETCORE_URLS=http://+:5096

USER app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["MailService.csproj", "./"]
RUN dotnet restore "MailService.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "MailService.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "MailService.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MailService.dll"]