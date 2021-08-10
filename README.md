# Virtual Avatars

What is it?

## Features


## Project structure

The project contains three folders. Assets, Packages, and ProjectSettings.

### TODO

1. This README.md
2. Installation guide
3. Project structure explanation
---
# 1. Installation

This virtualisation project is a Unity project which requires the Unity software to be installed.

To create virtual avatars several different software is required (see section 2.1)

## 1.1 Unity software

To install Unity follow their instructions: https://store.unity.com/download?ref=personal

Once downloaded you must login and then activate a new license in Unity Hub.
The license option should be `Unity Personal` and `I don't use Unity in a professional capacity`. 

## 1.2 Additional software

### 1.2.1 Git Large File Storage

Unity uses some large binary assets for 3D models, sounds, images, etc. (https://thoughtbot.com/blog/how-to-git-with-unity) Git LFS must be installed. This replaces large files such as audio samples, videos, datasets, and graphics with text pointers inside Git, while storing the file contents on a remote server like GitHub.com or GitHub Enterprise (https://git-lfs.github.com/).

Git Large File Storage can be installed from https://git-lfs.github.com/.

## 1.3 Adding the virtualisation project to unity

Once Unity (Unity Hub) is installed adding the project is straightforward. More details can be found here (https://docs.unity3d.com/2018.3/Documentation/Manual/GettingStartedOpeningProjects.html)

---
# 2. Creating Avatars

## 2.1 Installation of additional software
To create avatars a number of additional software must be installed first.

### 2.1.1 Makehuman Project
To install Makehuman follow the instructions on the Makehuman wiki: http://www.makehumancommunity.org/content/makehuman_120.html

#### 2.1.2 Makehuman required plugins
This project requires the use of the FACSHuman plugin for makehuman. Detailed installation of this plugin is found on their Github page: https://github.com/montybot/FACSHuman#facshuman-installation

### 2.1.3 Blender
To install Blender follow the instructions on their website: https://www.blender.org/download/
