# computer-graphics-projects
Repo for projects from Computer Graphics 1 subject at MiNI, WUT.

## How to run 
- install `.net5`
- run `dotnet restore`
- run `dotnet run`

## How to run in case you don't want to have `.net5` on your PC
- install `docker` for your platform.
- run: `docker build . -t kirylvolkau/cg1avalonia`
- run: `docker run --name avaloniaui kirylvolkau/cg1avalonia`
- run: `docker cp avaloniaui:/app/ComputerGrahics.exe` `$PWD/build/`


**IN THEORY** it should have created self-containing application in the `.exe` file, but, for some reason, it doesnt. Maybe I am getting wrong this self-contained flag or maybe `Avalonia.Markdown` dependency is forcing to use some other standard - I am still working on it.


## Notes
**If you are on linux or macOS** 
- please add `osx64.CoreData.System.Drawing` Nuget  for MacOS.
- install `mongo-libgdiplus` using `homebrew` (or any other linux package manager you are familiar with).
