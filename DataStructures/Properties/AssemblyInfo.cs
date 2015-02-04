using System.Reflection;
#if portable
using System.Resources;
#else
using System.Runtime.InteropServices;
#endif

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Data Structures from MASBicudo")]
[assembly: AssemblyDescription("MASBicudo Data Structures - Immutables, Continuous.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("MASBicudo")]
[assembly: AssemblyProduct("DataStructures")]
[assembly: AssemblyCopyright("Copyright © MASBicudo 2015")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
#if portable
[assembly: NeutralResourcesLanguage("en")]
#endif

#if !portable
// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("d3c97654-b34a-4762-9140-f9ada7bae68e")]
#endif

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// note: ONLY THE MAIN VERSION NUMBER MAY BE CHANGED AND MUST BE THE SAME AS THE NUGET MAIN VERSION
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.2.1.0")] // This line can be the same as the informational

[assembly: AssemblyInformationalVersion("1.2.1")] // NuGet version