# Creating a Toast notifier for windows

This shiz took me 4 hours to complete with a lot of struggle and a lot of Stackoverflow Driven Development

Oh, I build the app to use command line args for the notification text and header.

## The steps I did to make it work

### Step 1
Create a new project and add unload the project. Then edit the project settings add the following line to the property group
```
<TargetPlatformVersion>8.0</TargetPlatformVersion> 
```
and another reference to the project
```
<Reference Include="System.Runtime" /><Reference 
```
### Step 2:
We need windows reference for the application to work. Lets add them too. 
Note: windows reference only will show after editing the project for TargetPlatformVersion to 8.0.

Add the following reference by choosing 'Add Reference' context menu on the project.
```
Windows.UI
Windows.Data
```
### Step 3:
After that it is critical that we need create a application ID for our application to work. ie, to make notification to the system api.
Without this application id we won't able to make API requests to make toast. Please refer: [Toast not working after windows update](https://stackoverflow.com/questions/46814858/toast-notification-not-working-on-windows-fall-creators-update)

For that 
Install the following nuget package inorder to access the System shell to create the application
```
WindowsAPICodePack-Shell
```
### Step 4:
Other logics are too simple to write it here. Go look the code.
Compile the app and run. It should probably work. :)

	