REM set myKey=
set ver=0.0.5

nuget push bin\Packages\Release\NuGet\NotRemoting.%ver%.nupkg -src https://api.nuget.org/v3/index.json -ApiKey %myKey%
nuget push bin\Packages\Release\NuGet\NotRemoting.Compression.Lz4.%ver%.nupkg -src https://api.nuget.org/v3/index.json -ApiKey %myKey%
nuget push bin\Packages\Release\NuGet\NotRemoting.Serialization.BinaryFormatter.%ver%.nupkg -src https://api.nuget.org/v3/index.json -ApiKey %myKey%
nuget push bin\Packages\Release\NuGet\NotRemoting.Serialization.Json.%ver%.nupkg -src https://api.nuget.org/v3/index.json -ApiKey %myKey%
nuget push bin\Packages\Release\NuGet\NotRemoting.Serialization.MemoryPack.%ver%.nupkg -src https://api.nuget.org/v3/index.json -ApiKey %myKey%
nuget push bin\Packages\Release\NuGet\NotRemoting.Serialization.MessagePack.%ver%.nupkg -src https://api.nuget.org/v3/index.json -ApiKey %myKey%
nuget push bin\Packages\Release\NuGet\NotRemoting.Serialization.Protobuf.%ver%.nupkg -src https://api.nuget.org/v3/index.json -ApiKey %myKey%
