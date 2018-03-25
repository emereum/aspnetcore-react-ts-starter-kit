# Chocolatey
Set-ExecutionPolicy Bypass
iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))

# Build toolchain
cinst git ruby putty netfx-4.7.1-devpack -y
cinst dotnetcore-sdk --version 2.1.4 -y
cinst nuget.commandline --version 4.5.1 -y
cinst nodejs --version 9.8.0 -y