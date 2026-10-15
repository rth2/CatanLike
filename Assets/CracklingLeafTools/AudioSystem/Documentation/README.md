# CracklingLeafTools – Audio System
A lightweight, reusable, high-performance audio management system designed for Unity projects.  
This package provides central sound playback, pooling, and organized sound libraries with minimal setup.

## 📦 Installation
1. Import the `CracklingLeafTools/AudioSystem` folder into your Unity project.  
2. Inside `AudioSystem/Prefabs`, locate the **AudioManager.prefab**.

## 🚀 Getting Started
### 1. Add the Audio Manager to Your Game
1. Drag **AudioManager.prefab** into your first scene.  
2. That’s it — the prefab already includes:
   - `AudioManager` script  
   - `AudioPool` component  

The Audio Manager will automatically persist across scenes.
  - Remove 'DontDestroyOnLoad from Awake to remove persistence.
  - There are default 2d and 3d sound config files in `CracklingLeafTools/AudioSystem/Configs` which are attached to the AudioManager.

## 🔊 Audio Pool Configuration
The **AudioPool** is responsible for:
- Reusing AudioSources (performance-friendly)
- Handling playback lifetimes
- Preventing unnecessary allocations

Select the AudioManager prefab or instance in your scene, then adjust:

### AudioPool Serialized Fields
| Field | Description |
|-------|-------------|
| **initialPoolSize** | How many AudioSources are pre-created on startup. Doubles as the pool max size if expanding is not allowed. |
| **expandIfNeeded** | If enabled, the pool creates new sources if none are free. |

Adjust these values based on your game’s needs.

## 📚 Sound Libraries & Definitions
The audio system organizes sounds using **Sound Libraries** (collections of sound definitions).

### 2. Create a Sound Library
1. Right-click in the Project window  
2. **Create → CracklingLeafTools → Audio System → Sound Library**

Give the library a clear name such as:
- `BackgroundMusic`
- `SFX`
- `AllSounds`
If you don't name the library it will be called:
- `_UnknownKey`

### 3. Add Sound Definitions
1. Right-click in the Project window  
2. **Create → CracklingLeafTools → Audio System → Sound Definition**
3. Fill in each SoundDefinition:
   - **Sound Key** This is the name used to identify this sound. 
   - **Clip** (AudioClip)
   - **Volume**
   - **Pitch**
   - **Loop** (optional)
   - **Mixer Group** (optional, for later mixer integration)

## Creating SoundKey Constants
There are two ways to to automatically generate a static class of constants for clean, typo-safe usage.
1. On the audio manager, click the `Generate SoundKey Constants` button.
2. Use **CracklingLeafTools → Audio System → Generate SoundKey Constants**  
   - Creates the Generated -> SoundKey.cs
   - Fields generate like: `public const string LibraryName_SoundDefinition.SoundKey = "LibraryName_SoundDefinition.SoundKey";`
	-Example: public const string AllSounds_Goblin = "AllSounds:Goblin";

## 🎧 Playing Sounds

Recommended way to play sounds:
public void PlaySound(string soundKey);
The string is the auto generated soundkey constant.
- Example Call:
  AudioManager.Instance.PlaySound(SoundKey.AllSounds_Goblin);


## 📁 Persistent vs Scene Libraries
### Persistent Libraries
Assigned directly on the AudioManager prefab.  
These are always available across all scenes.
These will not be removed with ClearSceneLibraries();

### Scene Libraries
Can be assigned directly on startup and can be added or removed at runtime:
```csharp
AudioManager.Instance.AddSceneLibrary(library);
AudioManager.Instance.RemoveSceneLibrary(library);
AudioManager.Instance.ClearSceneLibraries();
```

Useful for level-specific SFX, biome audio, or temporary groups.

## ⏳ Future Features
Mixer support will be expanded later, including:
- Global volume categories (Music/SFX/UI)
- Exposed mixer parameters
- Easy slider binding for settings menus

(Documentation for mixers will be added once integrated.)

## 📄 License
This package is intended for internal CracklingLeafTools use but may be shared or extended as needed.
