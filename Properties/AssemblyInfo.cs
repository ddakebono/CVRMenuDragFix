using System;
using System.Reflection;
using System.Runtime.InteropServices;
using MelonLoader;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("MenuDragFix")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("MenuDragFix")]
[assembly: AssemblyCopyright("Copyright ©  2023")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("1F553218-EA85-4124-A7B1-6E24114AFF96")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: MelonInfo(typeof(MenuDragFix.MenuDragFix), MenuDragFix.BuildInfo.Name, MenuDragFix.BuildInfo.Version, MenuDragFix.BuildInfo.Author, MenuDragFix.BuildInfo.DownloadLink)]
[assembly: MelonGame("Alpha Blend Interactive", "ChilloutVR")]
[assembly: MelonColor(ConsoleColor.DarkMagenta)]
[assembly: MelonAuthorColor(ConsoleColor.DarkMagenta)]
[assembly: HarmonyDontPatchAll]