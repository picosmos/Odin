#!/bin/bash

# Odin Library Build and Pack Script
# This script builds the Odin library and creates a local NuGet package

set -e  # Exit on any error

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[0;33m'
BLUE='\033[0;34m'
NC='\033[0m' # No Color

# Configuration
PROJECT_PATH="./src/Odin.csproj"
OUTPUT_DIR="./packages"
CONFIGURATION="Release"

echo -e "${BLUE}🔨 Building Odin Library NuGet Package${NC}"
echo "========================================"

# Create output directory if it doesn't exist
mkdir -p "$OUTPUT_DIR"

# Clean previous builds
echo -e "${YELLOW}🧹 Cleaning previous builds...${NC}"
dotnet clean "$PROJECT_PATH" --configuration "$CONFIGURATION" --verbosity quiet

# Restore packages
echo -e "${YELLOW}📦 Restoring packages...${NC}"
dotnet restore "$PROJECT_PATH" --verbosity quiet

# Build the project
echo -e "${YELLOW}🏗️  Building project...${NC}"
dotnet build "$PROJECT_PATH" --configuration "$CONFIGURATION" --no-restore --verbosity quiet

# Pack the project
echo -e "${YELLOW}📦 Creating NuGet package...${NC}"
dotnet pack "$PROJECT_PATH" \
    --configuration "$CONFIGURATION" \
    --no-build \
    --output "$OUTPUT_DIR" \
    --verbosity normal

# List created packages
echo -e "${GREEN}✅ Package creation completed!${NC}"
echo ""
echo -e "${BLUE}📦 Created packages:${NC}"
ls -la "$OUTPUT_DIR"/*.nupkg 2>/dev/null || echo "No packages found in $OUTPUT_DIR"

echo ""
echo -e "${GREEN}🎉 Build and pack completed successfully!${NC}"
echo ""
echo -e "${BLUE}📝 To use this package locally:${NC}"
echo "1. Add the local package source to your project:"
echo "   dotnet nuget add source $(pwd)/packages --name \"Odin Local\""
echo ""
echo "2. Install the package in your target project:"
echo "   dotnet add package Odin --source \"Odin Local\""
echo ""
echo "3. Or reference it directly in your .csproj:"
echo '   <PackageReference Include="Odin" Version="1.0.0" />'