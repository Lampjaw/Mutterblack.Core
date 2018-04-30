pushd %~dp0\src\Mutterblack.Data
dotnet ef migrations add mutterblack.release.1 -v ^
    -c Mutterblack.Data.MutterblackContext ^
    -s ./../Mutterblack.Core/Voidwell.Core.csproj ^
    -o ./Migrations ^
    --msbuildprojectextensionspath ./../../build/Mutterblack.Core/Debug/obj
popd