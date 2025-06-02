# XR Navigation Aid – APK Builds

This repository contains experimental builds of an XR-based navigation aid system using multimodal cues.

## APK Versions

- **`in-view.apk`**
  - Initial implementation of the in-view and out-view indicator.

- **`transition+inview3.apk`**
  - Adds a **transition sidebar** indicator to the system.

- **`all2.apk`**
  - Integrates transition lock-on audio and ripple attention guide in the system.
    
- **`test1.apk`**
    - Added Settings Panel
 
- **`plz5.apk`**
    - Added simulations with Settings, can only create new spatial anchors
 
- **`plz6.apk`**
    - peripheral not working, spatial anchors all working but didn't attach cube
      
- **`plz8.apk`**
    - Final Working Version

APKs can be found here:
https://drive.google.com/file/d/1A-x4xMufU7dVWERhA-Fwv00LMmyzhwrn/view?usp=sharing

## Angular Zones

- **In-view zone:** `0°–30°`  
- **Transition zone:** `30°–65°`  
- Targets outside of 65° can be extended with out-of-view indicators if needed.

## Active Development Branch

- `in-view`

## Notes
Spatial Anchors only works when it's roomscale boundary, and spatial data permission is given. 
Vibration is related to glow and Pulse sound is related to out of view arrow.
If reloading failed, erase all and try create and reload again then it will work.

SpatialAnchorCoreBuildingBlock.cs should be modified. 

## Demo Video
[https://drive.google.com/file/d/1d0TTOIerimKHI2FOQhDo-rZHOZ-YaTG-/view?usp=drive_link](https://drive.google.com/file/d/1d0TTOIerimKHI2FOQhDo-rZHOZ-YaTG-/view?usp=sharing)

