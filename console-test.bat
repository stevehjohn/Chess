pushd

cd src

dotnet run --project Engine.ConsoleTests -c Release "%*"

popd
