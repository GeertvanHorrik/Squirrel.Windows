﻿#pragma warning disable 1699    // 1699 = Use command line option '/keyfile' or appropriate project settings instead of 'AssemblyKeyFile'

using System.Reflection;
using System.Resources;

// Shared assembly info that is common for all assemblies of this project

////[assembly: AssemblyTitle("DEFINED IN ACTUAL ASSEMBLYINFO")]
////[assembly: AssemblyProduct("DEFINED IN ACTUAL ASSEMBLYINFO")]
////[assembly: AssemblyDescription("DEFINED IN ACTUAL ASSEMBLYINFO")]

[assembly: AssemblyCompany("Paul Betts")]
[assembly: AssemblyCopyright("Copyright © Paul Betts 2013 - 2014")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: NeutralResourcesLanguage("en-US")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:

// NOTE: Restamp on build server, I prefer GitVersion but anything can be used. Changing manually is not recommended but is possible

[assembly: AssemblyVersion("0.5.6")]
[assembly: AssemblyInformationalVersion("0.5.6, manually built in Visual Studio")]