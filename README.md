<h1 align=center>ptm</h1>

An CLI application with the purpose of matching given patterns to any given texts.

Built by: antonio harger (ahr) and vinicius moreira (vvm).

## Instructions

### Installations

* Install Mono Runtime Platform
```
    sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF
    echo "deb http://download.mono-project.com/repo/ubuntu trusty main" | sudo tee /etc/apt/sources.list.d/mono-official.list
    sudo apt-get update
    sudo apt-get install mono-complete
```

### Building

Run

    msbuild /p:Configuration=Release pmt.csproj

Change directory to ```/pmt/src/bin/Release/```

### Running

    mono pmt.exe [option] pattern textfile [textfile]



