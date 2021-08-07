FROM mcr.microsoft.com/dotnet/aspnet:3.1
COPY LinkShorter/bin/Release/netcoreapp3.1/publish App/
WORKDIR /App
EXPOSE 80
ENTRYPOINT ["dotnet", "LinkShorter.dll"]
