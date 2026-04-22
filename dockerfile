FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files first to maximize layer cache usage.
COPY EngineerCalc/EngineerCalc.csproj EngineerCalc/
COPY DynamicEvaluator/DynamicEvaluator.csproj DynamicEvaluator/

RUN dotnet restore EngineerCalc/EngineerCalc.csproj

COPY . .

RUN dotnet publish EngineerCalc/EngineerCalc.csproj \
		--configuration Release \
		--runtime linux-x64 \
		/p:InvariantGlobalization=true \
		--output /out/publish

FROM mcr.microsoft.com/dotnet/runtime:10.0 AS runtime
WORKDIR /app

COPY --from=build /out/publish .

ENTRYPOINT ["dotnet", "EngineerCalc.dll"]
