# To build our project we need dotnet sdk. But to run dll/exe
# we just need dotnet runtime.
# In docker final image, it only contains last "FROM" base image.
FROM mcr.microsoft.com/dotnet/sdk:5.0 as build-env

WORKDIR /app
COPY  *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

# Now we don't need sdk as we have already build using sdk.
# In our final image we only need runtime not sdk.
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
# Copy output of previous stages.
COPY --from=build-env /app/out .
ENTRYPOINT [ "dotnet", "CommandsService.dll" ]
