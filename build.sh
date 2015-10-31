#! /bin/sh

if which nuget > /dev/null; then
    nuget restore
else
    echo "Failed to find nuget in the system path."
    exit 1
fi

if which xbuild > /dev/null; then
    xbuild c4fsharp.sln
else
    echo "Failed to find xbuild in the system path."
    exit 1
fi
