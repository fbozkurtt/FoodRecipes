# create the build instance 
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

WORKDIR /src                                                                    
COPY ./src ./

# restore solution
RUN dotnet restore Internative.FoodRecipes.sln

WORKDIR /src/Presentation/Internative.FoodRecipes.Web

# build project   
RUN dotnet build Internative.FoodRecipes.Web.csproj -c Release

# publish project
WORKDIR /src/Presentation/Internative.FoodRecipes.Web 
RUN dotnet publish Internative.FoodRecipes.Web.csproj -c Release -o /app/published

# create the runtime instance 
FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine AS runtime

# add globalization support
RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# copy entrypoint script
COPY ./entrypoint.sh /entrypoint.sh
RUN chmod 755 /entrypoint.sh

WORKDIR /app        
RUN mkdir bin
RUN mkdir logs

COPY --from=build /app/published .

ENTRYPOINT "/entrypoint.sh"