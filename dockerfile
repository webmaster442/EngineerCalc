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
		--self-contained true \
		/p:InvariantGlobalization=true \
		/p:PublishSingleFile=true \
		/p:IncludeNativeLibrariesForSelfExtract=true \
		--output /out/publish

# Keep the requested executable name for container entrypoint.
RUN mv /out/publish/EngineerCalc /out/publish/EngineerCalc.exe && chmod +x /out/publish/EngineerCalc.exe

# Build a minimal rootfs for scratch by copying only required native dependencies.
RUN set -eux; \
		mkdir -p /out/rootfs; \
		cp -a /out/publish /out/rootfs/app; \
		ldd /out/rootfs/app/EngineerCalc.exe \
			| awk '/=> \/.*/ { print $3 } /^\/lib.*/ { print $1 }' \
			| sort -u \
			| xargs -I '{}' cp --parents '{}' /out/rootfs; \
		cp --parents /lib64/ld-linux-x86-64.so.2 /out/rootfs || true; \
		cp --parents /etc/ssl/certs/ca-certificates.crt /out/rootfs || true

FROM scratch AS runtime
WORKDIR /app

COPY --from=build /out/rootfs/ /

ENTRYPOINT ["/app/EngineerCalc.exe"]
