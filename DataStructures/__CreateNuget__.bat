echo off
set PATH=%PATH%;C:\Program Files\MSBuild\14.0\Bin
set PATH=%PATH%;C:\Program Files\MSBuild\12.0\Bin
set PATH=%PATH%;C:\Program Files (x86)\MSBuild\14.0\Bin
set PATH=%PATH%;C:\Program Files (x86)\MSBuild\12.0\Bin
echo on

for %%f in (..\*.sln) do (

            echo BUILDING SOLUTION: %%~nf
            msbuild.exe "..\%%~nf.sln" /t:rebuild /p:Configuration=Release /p:Platform="Any CPU" /verbosity:quiet
    )

nuget pack DataStructures.portable-net45+win+wpa81+wp80.csproj -Prop Configuration=Release -Prop Platform=AnyCPU
nuget pack DataStructures.portable-net45+win+wpa81+wp80.csproj -Prop Configuration=Release -Prop Platform=AnyCPU -Symbols
