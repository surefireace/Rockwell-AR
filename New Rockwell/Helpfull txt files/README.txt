how to build on hololens

1. build unity as a Universal windows platform. take note of build location "Test_Builds" folder is ideal location.
2. open the .sln in VS 2019. right click the solution on the right and re-target solution to 10.0, 10.0.17134.0, v142.
3. make sure its on release x86 and remote machine or plug in the headset and choose device. 
4. debug > Rockwell properties > debugging > Machine Name put it as the hololens IP address.
5. debug > start without debugging. wait till its done. it should auto launch on the hololens when its compiled.

P.S. see useful links.txt for help if still having issues.

how to build for PC (only do this if no access to hololens and need to do simple tests, due to the build being run through emulator)

0. make sure that the PC is in Developer mode. Settings> update and security > for Developers click the developer box.  
1. build unity as a Universal windows platform. take note of build location "Test_Builds" folder is ideal location.
2. open the .sln in VS 2019. right click the solution on the right and re-target solution to 10.0, 10.0.17134.0, v142.
3. make sure its on release x86 local machine. 
5. debug > start without debugging. wait till its done. it should auto launch when its compiled.

how to build for hololens 2 (https://docs.microsoft.com/en-us/windows/mixed-reality/mrlearning-base-ch1)

1. build unity as a Universal windows platform. take note of build location "Test_Builds" folder is ideal location.
2. open the .sln in VS 2019. right click the solution on the right and re-target solution to 10.0, 10.0.17134.0, v142.
3. make sure its on release, ARM, and remote machine or plug in the headset and choose device. 
4. debug > Rockwell properties > debugging > Machine Name put it as the hololens IP address.
5. debug > start without debugging. wait till its done. it should auto launch on the hololens when its compiled.

how to build on hololens 2 emulator (https://docs.microsoft.com/en-us/windows/mixed-reality/using-the-hololens-emulator)

1. build unity as a Universal windows platform. take note of build location "Test_Builds" folder is ideal location.
2. open the .sln in VS 2019. right click the solution on the right and re-target solution to 10.0, 10.0.17134.0, v142.
3. make sure its on release x86 with the hololens emulator is selected. 
4. select the hololens 2 emulator as the target device. wait till its done. it will auto launch a new emulator instance and auto
launch the app once it finishes loading.

NOTES:
Adjusting book and button placement is done through their Orbital (script) World Offset values.

hololens 2 emulator will throw a exception when the app tries to read its battery also Vuforia doesn't support it.
also the emulator cant be used to get a good grasp of the experience since a good portion of the gestures aren't supported.

any function with [PunRPC] above it, is called by the sever with another client invoking the call with photonView.RPC(). Remote Procedural Call (RPC) is the main way the
clients communicate. one non obvious bit of code that uses RPC is how the play/pause and close buttons work. when one of the buttons are pressed they call a ButtonClient.PlayPause()
that calls ClientManager.PlayPauseInteraction() which RPC's ClientManager.PlayContent() or ClientManager.PauseContent() to tell the other group members to plays or pauses the interactions for the group respectively.

Trouble shooting connection to server issues:
1. Make sure the server is running. 
	A. in Photon-OnPremise-Server-SDK_v4-0-29-11263 > deploy > bin_Win64 run "PhotonControl.exe" as administrator.
	B. right click the new task bar icon, under "Game Server IP Config" make sure one of the IP's is selected.
		i. public IP's is for remote connection from a different network. private IP's are for local connections from within the same network.
	C. under LoadBalancing(MyCloud) in PhotonControl left click "Start as Application". the server should now be running.

2. Make sure the correct IP is entered for the application and server.
	A. right click the PhotonControl icon, under "Game Server IP Config" make sure one of the IP's is selected.
		i. public IP's is for remote connection from a different network. private IP's are for local connections from within the same network.
	B. make sure that is the same IP address entered into the application Ctrl+Shift+Alt+P to access "PhotonServerSettings" the IP address in Server should match.
	C. try restarting the application sometimes it doesn't connect of first start up.
	D. if it is a Hololens that is having issues connecting try connecting the Hololens to a hotspot from the computer the server is running on and use a local IP address.
	
3. the firewall might be blocking the connection. this is UNLIKELY do this only if other options have been ruled out as it can compromise computer security.
	A. turn off firewall. this can be different for most PC's so the only instructions I have is to search firewall in settings.
		i. if turning it off didn't help then its a different issue and the firewall should be turned back on.
	B. if it did help then port 4530-4540 would need to be opened through firewall settings so the firewall can still be active. use Google to find out how to do this for the specific computer.
	


