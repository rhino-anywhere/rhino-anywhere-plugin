# Build the Rhino Anywhere Plugin from source

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

## Prerequisites

* Git
  ([download](https://git-scm.com/downloads))
* Visual Studio 2022 (For Windows)
  ([download](https://visualstudio.microsoft.com/downloads/))
* .NET Framework (4.8) Developer Pack
  ([download](https://www.microsoft.com/net/download/visual-studio-sdks))
* Rhino
  ([download](https://www.rhino3d.com/download/))

## Getting Source & Build

1. Clone the repository. At the command prompt, enter the following command:

    ```console
    git clone --recursive https://github.com/rhino-anywhere/rhino-anywhere-plugin.git
    ```

2. In Visual Studio, open `RhinoAnywhere.sln`.
3. Press F5 or Debug

## Installing & Uninstalling

Rhino Anywhere will install automatically on build.
To uninstall, close Rhino and delete your bin directory.
