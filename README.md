# 1. Virtual Avatars

Avatar virtualisation can be used in therapy or coaching sessions to help the patients gain insight into their behaviour and understand their functioning better.

This repository contains a virtualisation project utilising the Unity game engine. It includes several human-like avatars with the ability to express several human emotions.
Several programming libraries are included to facilitate the easy application of simulation data from causal models to the virtual avatars.
The libraries are easy to expand and can be used in several different virtualisation scenarios. A guide is included to create new avatars as well.

## 1.1 Project structure

The project contains three folders. Assets, Packages, and ProjectSettings.

* Assets
  * Additional Assets
    * This folder contains any additional assets that are used in many different scenes. This includes prefabs for scenes, materials, etc.
  * Libraries
    * The Libraries folder contains several different namespaces and functions used in the virtualisation of avatars. This includes the `Avatar Actions`, `Avatar Emotion Controller`, `Avatar Facial Expressions`, `Camera Controller`, and `Model Data`.
  * Meshes
    * The Meshes folder contains assets used in the virtualisation.
    * Avatars
      * This folder contains the avatars. Two avatars are included in the base project.
  * Scenes
    * This folder contains the different virtualisation scenes. A scene can contains its own level, avatars, and any other assets.
    * Example
      * This scene is an example virtualisation.
    * Template
      * This scene is a template for creating new scenes.

## 1.2 TODO

This project can be improved by adding several features

1. Make an automation software to make it easy to create new avatars.
2. Expand Avatar Action library to include more actions.
3. Create a main menu where a user can select which scene is loaded.
4. Expand documentation further to make it easier to pick up the project.
5. Think of more things to do.

---
# 2. Installation

This virtualisation project is a Unity project which requires the Unity software to be installed.

To create virtual avatars several different software is required (see section 5.1)

## 2.1 Unity software

To install Unity follow their instructions: https://store.unity.com/download?ref=personal

Once downloaded you must login and then activate a new license in Unity Hub.
The license option should be `Unity Personal` and `I don't use Unity in a professional capacity`.

## 2.2 Additional software

### 2.2.1 Git Large File Storage

Unity uses some large binary assets for 3D models, sounds, images, etc. (https://thoughtbot.com/blog/how-to-git-with-unity) Git LFS must be installed. This replaces large files such as audio samples, videos, datasets, and graphics with text pointers inside Git, while storing the file contents on a remote server like GitHub.com or GitHub Enterprise (https://git-lfs.github.com/).

Git Large File Storage can be installed from https://git-lfs.github.com/. Make sure to follow the installation instructions.

## 2.3 Adding the virtualisation project to unity

Once Unity (Unity Hub) is installed adding the project is straightforward.
1. First clone this repository to your device.
2. Then open Unity Hub and in the projects tab click `Add`. More details can be found here (https://docs.unity3d.com/2018.3/Documentation/Manual/GettingStartedOpeningProjects.html).
3. Next go to the `Installs` tab in Unity Hub and add Unity version `Unity 2020.3.15f2 (LTS)4`.
4. Finally go back to the projects tab and open the project.

---
# 3. Usage
Once the project is open the Project window will contain several folders with all the necessary assets. The important folders are: `Libraries`, `Meshes`, and `Scenes`.
The libraries folder contains all the script and libraries to control avatars. The meshes folder contains the avatars. The scenes folder contains the different scenes.

The scenes folder contains an example and a template. To create a new virtualisation scenario duplicate the template folder and rename all the items. Open the scene by double clicking the scene asset (template scene called Template Scene). The template scene contains several game objects. Cameras, Level, avatar 1, avatar 2, and Controller. The Controller object links the scene to the virtualisation script called Virtualise Template. You can add anything into the scene, to build the scene. To control the scene you will need to modify the script. The template script contains detailed explanations how the avatars are controlled using the simulation data from the model.

The other scene is an example virtualisation of two avatars arguing. To open the scene double click the Scene Asset, and to play the scene click the Play button at the top of the editor.

Each scene can contain anything part of the virtualisation. This makes it easy to separate several different virtualisation scenarios. To make it easy to reuse assets across different scenes Unity has a concept called prefabs. See Unity's manual for more details https://docs.unity3d.com/Manual/Prefabs.html.

How to connect simulation data from a model to the avatars is explained in the Example scene and the template. The libraries also contain many details on how to trigger actions and animations.

# 4. Useful resources

1. Unity Manual: https://docs.unity3d.com/Manual/UnityManual.html
2. Blender Manual: https://docs.blender.org/manual/en/latest/

# 5. Creating Avatars

## 5.1 Installation of additional software
To create avatars a number of additional software must be installed first.

### 5.1.1 Makehuman Project
To install Makehuman follow the instructions on the Makehuman wiki: http://www.makehumancommunity.org/content/makehuman_120.html

#### 5.1.2 Makehuman required plugins
This project requires the use of the FACSHuman plugin for makehuman. Detailed installation of this plugin is found on their Github page: https://github.com/montybot/FACSHuman#facshuman-installation

### 5.1.3 Blender
To install Blender follow the instructions on their website: https://www.blender.org/download/


## 5.2 Creation process

### 5.2.1 Video tutorial (recommended)
[![Link to video tutorial](http://img.youtube.com/vi/Gy6HW2XWAws/0.jpg)](http://www.youtube.com/watch?v=Gy6HW2XWAws)

In the video all the steps from start to finish shown.

### 5.2.2 makehuman
First create an avatar in the makehuman software.

* Pose/Animate > Skeleton > Cmu mb
* Pose/Animate > Pose > Tpose

Export this with Mesh format `Filmbox (fbx)`

### 5.2.3 blender
Create a new blender project and remove the default light, camera, and cube.
* File > Import > FBX (.fbx) the avatar you created in make human.
  * Make sure to untick the `Manual Orientation`, and `Animation` settings.


Next the blend shapes must be imported for facial expression. This step involves going back and forth from makehuman to blender.
* Open the base avatar in makehuman.
* Remove the skeleton by going to Pose/Animate > Skeleton and selecting None.
* Next go to the Modelling > FACSHuman 0.1 tab. If this is not here then you have not installed the required plugins correctly (section 5.1.2).
* In the right panel you should see a section called `Action Units coding` and a button called `Load FACS Code`. Click this and select the code for the emotion you need to encode.
* Now export this as an FBX.
* In blender import the FBX you just exported in makehuman by going to File > Import > FBX (.fbx)
  * Make sure to untick the `Manual Orientation`, and `Animation` settings.
* The blender project should now contain two avatars, the base avatar and the avatar with the active FACS codes. There will be a mesh for every part of the avatar. Depending on what FACS codes were activated only some parts of the mesh are important. Any part of the newly imported mesh that is clearly not activated can be deleted. For example the clothing and hair aren't triggered. Select, first the newly import mesh, and then the corresponding mesh on the base avatar.
* In the properties window select `Object Data Properties`, and under the `Shape Keyes` tab select the `Shape Key Specials` button on the right side. Click the `Join as Shapes` button to create the blend shape.
* A new Shape Key will be added. Make sure to rename the added blend shape to: `emotion_name-area_activated`. For example, activating the FACS codes for expressing the eyebrows while angry would be named `angry-eyebrows`, or for the mouth `angry-mouth`.
* To preview the blend shape set the weight to 1.
* Now delete the newly imported mesh because it has been joined as a blend shape and is no longer needed. Now select the next imported mesh and the corresponding existing mesh and repeat this process of joining them as blend shapes.
* After completing this for one set of FACS codes you must go back to makehuman, load the next FACS codes, export the FBX, import the FBX into blender and repeat the process of creating blend shapes.

### 5.2.4 Animations

To add animations to the avatar head over to mixamo.com. You must log in with an adobe account to use this service. Once logged in, select the option to `UPLOAD CHARACTER`. Select the base avatar that was exported from makehuman. It will take a bit to upload but after it is uploaded you can search for an animation.
* Find an animation you want to add.
* Configure the animation making sure to set the option `In-place`.
* Download the animation:
  * The format should be FBX and Skin should be set to "without skin".
  * Frames per second can be set to 30 and keyframe reduction to "none".
  * TIP: give the downloaded FBX a good name as you may be downloading several animations.
* In blender you can now import the animation by going to File > Import > FBX (.fbx)
  * Make sure to set the Manual Orientation: Forward: Y Forward, Up: Z Up. Also enable the Animation.
* Once imported, rename the imported skeleton to the name of your avatar.
* Open a new window in blender and select the `Dope sheet`, and the `Action Editor`
* In the Scene select the base avatars skeleton, and in the Action Editor click the button next to the button labeled "New" called "Browse Action to be linked".
* Select the imported animation. This adds the animation to the avatar's skeleton.
* The imported skeleton with the animation can be removed.
* Repeat this process for as many animations you want to add to the avatar.

### 5.2.5 Importing the avatar to Unity

* In the scene select all the avatars meshes, skeleton, and anything else that belongs to the avatar.
* Next go to File > Export > FBX, modify the following settings
  * In the Include header set "Limit to" "Selected Objects"
  * Set "Object Types" to "Armature" and "Mesh"
  * In the Transform header set "Forward" to "Z Forward"
  * Set "Up" to "Y Up"
  * In the Geometry header untick the "Apply Modifiers" option
  * In the Armature header untick "Add Leaf Bones"
  * Enable Bake Animations but untick "NLA Strips" and "Force Start/End Keying"
* Head over to Unity and add a new folder in Meshes > Avatars with the name of your avatar.
* Right click in this folder and select "Import New Asset"
* Next you need to add a folder called "textures" as well. This folder is created when you export the avatar from makehuman. Simply draw and drop this folder into next to the imported avatar. There might be a pop up about a NormalMap settings. In this case just click the Fix now button.
* Select the imported avatar and in the Inspector you should see the avatar.
* In the Inspector click the Materials tab and click "Extract Materials". Create a new folder called materials and tell Unity to extract the materials there.
* The Inspector should now say that the Remapped Materials are no longer none.
* Enter this materials folder and select the material for the eyes. In the inspector right below the name you see an option called "Shader". Click on this and change the shader to "Legacy Shaders/Transparent/Diffuse".
* Next head back to the imported avatar. In the Inspector head over to the Animations tab.
* Here you must configure the animations.
  * Rename each animation to the name of the avatar + the name of the animation.
  * For animations that should loop, e.g. walking, running, etc, tick the button called "Loop Time". This makes sure that the animation loops.
* The final part of importing is to go to the Rig tab. Change the Animation Type from Generic to Humanoid. Click Apply.
* Next we must create a prefab variant which allows the avatar to be used in more than one scene. Right click the avatar in the project window and select "create" and click "Prefab variant".
* Select the prefab variant you just created and click "Add Component" in the Inspector. Look for a component called "Rigidbody" and add it to the prefab.
  * In the inspector you can configure the newly added component. Under the "Constraints" settings select "Freeze Rotation" for X, Y, and Z.
  * Next we must add a Box Collider to the feet of the avatar. This ensures that the avatar can collide with other objects in the scene. It also prevents the avatar from clipping through other objects and allows gravity to function on the avatar.
  * In the project window open the avatar prefab variant. The scene window should now show the different meshes part of the avatar. Select the skeleton, Hips > LHipJoint > LeftUpLeg > LeftLeg > LeftFoot > LeftToeBase. In the Inspector add a new component called Box Collider. You must now transform it so it does not protrudes underneath the avatar.
* We also need to add an Animator component so we can control the animations of the avatar. Select the prefab variant and in the Inspector click Add Component. Add a component called Animator.
  * The Animator component requires two settings to be configured. First set the Avatar to avatar that you imported.
  * Make sure to untick `Apply Root Motion`
  * Now in the project window right click in the same folder as the avatar and prefab and click create "Animator Controller".
* We now add the Avatar Emotion Controller script which connects the library to the avatar. Click on the prefab variant and navigate to the "Libraries" folder in the project window. Drag the script called AvatarEmotionController to the "Add Component" button. This should add the script to the avatar.
