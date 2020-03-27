To manually load the dll you use AutoCAD netload command.

For AutoCAD to automatically load the dll it needs to be in a specific path.<br>
Example: `C:\Users\AppData\Autodesk\ApplicationPlugins\AcacdIO.bundle`

Within AcacdIO.bundle you would place<br>
* AcadIO.dll
* PackageContents.xml
* any other necessary files
