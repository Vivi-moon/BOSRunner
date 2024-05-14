# BOS PC Sample

Sample project for using PC BOS with the NeuroMD app (via BiofeedbackNetwork.dll)

# Build and Run
Running NeuroMD is required to run the game. Otherwise, it will automatically close after a while. Running separately from the exe (as in the Unity editor), is not recommended. 

### Build:
 - (in the context menu) *CI ->Build Window*;
 - select the build type in *Behaviour type*"*;
 - select simulation on/off in *"*Is Control App Simulate*"*;
 - click *"*Save Settings*"*;
 - click *"*Build*"*;
 - for quick navigation you can open the build folder (click "open build path").

NOTE: An archive (*{name}_{version}.zip*) will be created in the build folder. Version - build date and time (*yyyyyy_mm_dd_hh_mm*).

### Run in editor:
 - run Unity editor;
 - open *InitScene*;
 - enter play mode;

NOTE: There may be some problems in the NeuroMD app (not sending pause command to unity game, etc), but it doesn't interfere with the tests.

### Run windows build
 - NeuroMD app is required;
 - copy game build to the NeuroMD folder (*"{app_folder}/Neurotech/Devices/BOS_Games/Games/"*);
 - (if not done before) change the "*.xml" config to display in the NeuroMD game list. For xml config, the parameters in "Game" and "RelPath" are mandatory;
 - Run the game in NeuroMD.

NOTE: Build from the archive is ready for import. 

# Using

###  Main scripts:
 - Game logic start - *GameEntryPoint.cs*;
 - UI of the game - *GameHUD.cs*;
 - UI menu - *GameScreen.cs*;
 - API for the application - *IBFBGameProvider.cs*;
 - API for sensor usage - *IDevice.cs* (get it via *IBFBGameProvider.GetDeviceFor*);
 - See also: *EFrameStatus*, *EFrameZoneType*, *PlayerController.UpdateData()*.

### Simulation
The simulation mode is selected in the builder window (see Build). Simulation replaces the NeuroMD app, allowing to control game commands (pause, shutdown) and other data (frame, sensor value).

NOTE: Data control keys can be found out in *BFBSimulation.GetSimulateControlInstruction()* (output to Unity log at initialization).

# Game in the example

After the initialization scene and waiting for connection, the training begins. For each sensor (channel) a bar (value from the sensor), a frame (required zone for the sensor value) and information fields (channel, frame/column values and frame readiness indicator) of the player are added to the screen.

If player 1, ui is placed in the center, otherwise distributed evenly by number.

If the frame is not static (changes according to a periodic rule), the frame indicator will change. At this time it is not possible to get frame values into the future (i.e. IDevice.ReadFrame(gameTime+5, ...) call).