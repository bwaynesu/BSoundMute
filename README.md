# bSoundMute
Quickly toggle mute for any open and controllable program with volume control.

> _Author: bwaynesu_  
> _Created: August 15, 2015_  
> _Tags: C#, Windows, .Net Framework 4.5_

[[巴哈討論串]](https://forum.gamer.com.tw/Co.php?bsn=60030&sn=1868316)

It looks like this:  
<img src="https://truth.bahamut.com.tw/s01/201508/e0f0c61736dcfb07f3c13e49d5df7c9b.JPG" width="180"></img>

## Development Reasons
I find it inconvenient to adjust the volume of a specific software from the volume mixer (as shown in the figure below) every time I want to mute it. I wish there was a quick switch similar to the maximize and minimize buttons.  

<img src="https://truth.bahamut.com.tw/s01/201508/3f8a6f58adb74d009262ffbe0154fcc2.JPG" width="300"></img>

## Installation
It will be only one exe file, which can be executed directly.
(If it does not work properly, you may need to download `.net framework 4.5` from Microsoft)

## Instructions
After executing the exe file, there will be a main window for displaying some information. It can be minimized (or will automatically minimize after 3 seconds).

Then, the mute icon will appear on any window that controls the volume (such as the first picture). If it is not used, it will automatically hide after a while, and it will appear again when the cursor is moved to the icon position.

Then, just press the button to mute/restore the volume.  
Shortcut keys: `LCtrl + B` (mainly used in full-screen games and videos)

If this tool cannot mute full-screen games:  
Please press `Alt + Tab` to return to the desktop and open the main window of bSoundMute. Press `Refresh` and try again.

## Known Issues
1. Windows Media Player cannot be muted (but if you have opened the media player, you probably don't need my tool)
2. Flash videos in browsers cannot be muted (this should not be a big deal)