# Dockerfile

FROM ghcr.io/railwayapp/nixpacks:ubuntu-1684957838@sha256:eb26a5ad60faad269d01ac896a4eefb5e7987040d39549dbe5d8cdfd1a830b75

WORKDIR /app

# Copy the .nixpacks directory
COPY .nixpacks/nixpkgs-293a28df6d7ff3dec1e61e37cc4ee6e6c0fb0847.nix .nixpacks/nixpkgs-293a28df6d7ff3dec1e61e37cc4ee6e6c0fb0847.nix

# Install Nix packages and collect garbage
RUN nix-env -if .nixpacks/nixpkgs-293a28df6d7ff3dec1e61e37cc4ee6e6c0fb0847.nix && nix-collect-garbage -d

# Copy the application files
COPY . .

# Build and publish the application
RUN dotnet restore --runtime linux-x64
RUN dotnet publish --no-restore -c Release -o out

# Set the startup command
CMD dotnet ./out/BusinessObjectLayer.dll
