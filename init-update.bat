pushd %~dp0\src\Mutterblack.Data
dotnet ef database update -v ^
    -c Mutterblack.Data.MutterblackContext ^
    -s ./../Mutterblack.Core/Mutterblack.Core.csproj ^
    --msbuildprojectextensionspath ./../../build/Mutterblack.Core/Debug/obj
popd