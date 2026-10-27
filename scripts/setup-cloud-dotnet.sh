#!/usr/bin/env bash
# Installs the Microsoft-built .NET 8 SDK into ~/.dotnet on a Linux cloud session.
#
# Why not `apt install dotnet-sdk-8.0` from the Ubuntu archive: that source-built SDK
# lacks Microsoft.NET.Sdk.WindowsDesktop, so the WPF project (net8.0-windows) cannot even be
# loaded. dotnet-install.sh is not usable either when builds.dotnet.microsoft.com is blocked
# by the network policy, so the .deb packages from packages.microsoft.com are extracted instead.
set -euo pipefail

SDK_VERSION="${SDK_VERSION:-8.0.427}"
RUNTIME_VERSION="${RUNTIME_VERSION:-8.0.33}"
FEED="https://packages.microsoft.com/ubuntu/22.04/prod"
INSTALL_DIR="${DOTNET_ROOT:-$HOME/.dotnet}"

if [ -x "$INSTALL_DIR/dotnet" ] && "$INSTALL_DIR/dotnet" --list-sdks | grep -q "^$SDK_VERSION "; then
  echo ".NET SDK $SDK_VERSION already installed in $INSTALL_DIR"
  exit 0
fi

work="$(mktemp -d)"
trap 'rm -rf "$work"' EXIT
cd "$work"

curl -fsSL "$FEED/dists/jammy/main/binary-amd64/Packages.gz" | gunzip > Packages

fetch() { # <package> <version-prefix>
  local file
  file="$(awk -v pk="$1" -v ver="$2" '$1=="Package:"{p=($2==pk)} p&&$1=="Version:"{v=$2} p&&$1=="Filename:"&&index(v,ver)==1{print $2}' Packages | tail -1)"
  [ -n "$file" ] || { echo "package $1 $2 not found in feed" >&2; exit 1; }
  curl -fsSLO "$FEED/$file"
}

fetch dotnet-sdk-8.0 "$SDK_VERSION"
for p in dotnet-runtime-8.0 dotnet-hostfxr-8.0 dotnet-runtime-deps-8.0 dotnet-targeting-pack-8.0 \
         dotnet-apphost-pack-8.0 aspnetcore-runtime-8.0 aspnetcore-targeting-pack-8.0; do
  fetch "$p" "$RUNTIME_VERSION"
done
fetch dotnet-host ""
fetch netstandard-targeting-pack-2.1 ""

mkdir root
for deb in *.deb; do dpkg -x "$deb" root; done

rm -rf "$INSTALL_DIR"
mkdir -p "$(dirname "$INSTALL_DIR")"
mv root/usr/share/dotnet "$INSTALL_DIR"
ln -sf "$INSTALL_DIR/dotnet" /usr/local/bin/dotnet 2>/dev/null || echo "Add $INSTALL_DIR to PATH"

"$INSTALL_DIR/dotnet" --list-sdks
