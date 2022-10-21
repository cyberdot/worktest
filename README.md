This is a Windows service implementation for Petroineos Development Challenge.

To deploy the code as a Windows service:

- Build the solution
- Run dotnet publish --output "C:\custom\publish\directory"
- Run sc.exe create "Power Trades Report Generator" binpath="C:\Path\To\ReportGenerator.Service.exe"
- Make sure that appsettings.json configuration is present in the same directory as .exe file

