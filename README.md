# AUT Workshop 2020 

This is a repository for a workshop on ALA (https://abstractionlayeredarchitecture.com)

The code is a Hello world program in ALA and C# for Visual Studio on Windows.

It contains the ALA layer folders:

* Application
* Domain Abstractions
* Programming Paradigms
* Library.

Inside the Application folder is the Hello world program, which we will modify for various exercises.

Inside the Domain Abstractions folder are a few abstractions for us to use in the exercises.


## Getting started

1. If you don't already have Visual Studio, install Visual Studio Community 2019 from https://visualstudio.microsoft.com/vs for C# development on a Windows PC.
    1. Once the installer loads select the ".NET desktop development" from the options and then click Install
    ![Install_VisualStudio](/images/Install_VisualStudio.png)

2. If you don't already have Git installed, install Git from https://git-scm.com/download/win. It is worth making the effort to use Git to clone the repository because then you can get the latest version easily. We may be adding new material up until the workshop starts. It will also allow you to contribute your changes back to the repository for others to use.
    1. In the Select Components window, leave all default options checked and check any other additional components you want installed.
    1. Next, in the Choosing the default editor, used by Git unless you're familiar with Vim we highly recommend using a text editor you're comfortable using. If Notepad++ is installed, we suggest using it as your editor. If Notepad++ is not installed, you can cancel the install and [install Notepad++](https://notepad-plus-plus.org/) and then restart the GitHub install.
    1. Leave all other options as the recommended defaults

    If you can't make Git work, you can download a zip file from this repository.

3. Create a folder on your PC to clone the project.
3. Inside the folder right click and select "Git Bash" from the context menu
    ![Open_GitBash](/images/Open_GitBash.png)
3. Inside the Git Bash terminal clone the repository with the command:
    ![Git_Clone](/images/Git_Clone.png)
```
$ git clone https://github.com/john-spray/AUTWorkshop2020.git
```
Alternatively, unzip the downloaded zip file into the folder.

4. Double click the file ALASandbox.sln, which will open in Visual Studio.
    1. If you get a pop up asking "How do you want to open this file?" select either "Microsoft Visual Studio Version Selector" Or "Visual Studio 2019"
    ![Version_Selector](/images/Version_Selector.png)

5. Press F5 to run the default application which is Hello world.

