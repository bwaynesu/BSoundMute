# BSoundMute
A lightweight utility to quickly mute any application with a single click or hotkey.

> _Author: bwaynesu_  
> _Created: August 15, 2015_  
> _Updated: May 2025_  
> _Tags: C#, Windows, .NET 8_

🔗 [巴哈討論串](https://forum.gamer.com.tw/Co.php?bsn=60030&sn=1868316)

It looks like this:  
<img src="https://truth.bahamut.com.tw/s01/201508/e0f0c61736dcfb07f3c13e49d5df7c9b.JPG" width="180"></img>

## Why BSoundMute?
It's inconvenient to open the Windows volume mixer (shown below) every time you want to mute a specific application. BSoundMute adds a mute button directly to application windows, similar to the minimize and maximize buttons.

<img src="https://truth.bahamut.com.tw/s01/201508/3f8a6f58adb74d009262ffbe0154fcc2.JPG" width="300"></img>

## Installation
Simply go to [Releases](https://github.com/bwaynesu/BSoundMute/releases) page and download the latest standalone executable file.
(For v1, if it does not work properly, you may need to download `.net framework 4.5` from Microsoft)

## How It Works
After running BSoundMute:

1. A main information window appears (which you can minimize to tray)
2. A mute button will appear near the top-right corner of any window that produces sound
3. The button automatically hides when not in use and reappears when you move your cursor to that area
4. Click the button to instantly toggle mute for that specific application

**Hotkey**: Use `LCtrl + B` (or `B + LCtrl` if it conflicts with other application) to mute/unmute the current foreground application. This is especially useful for fullscreen applications.

## Troubleshooting
- If the mute button doesn't appear or works incorrectly: Click the BSoundMute tray icon, press "Refresh" and try again

## Limitations
- Some apps may not be compatible
- Applications running with administrator privileges may require BSoundMute to also run as administrator
