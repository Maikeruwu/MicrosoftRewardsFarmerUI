# MicrosoftRewardsFarmerUI
This is a UI for the Microsoft Rewards Farmer from [@charlesbel](https://github.com/charlesbel) on GitHub.
The original project can be found [here](https://github.com/charlesbel/Microsoft-Rewards-Farmer).

## Prerequisites
- Chrome ~~and Chromedriver~~ installed
    - Selenium >= 4.10.0 now includes a webdriver manager, just update selenium with `pip install selenium --upgrade`
- Python3 installed
- (On Windows) Microsoft Visual C++ Redistributable installed
    - If not, install the current "vc_redist.exe" from [this](https://learn.microsoft.com/en-GB/cpp/windows/latest-supported-vc-redist?view=msvc-170) and reboot

## How to use
1. Download the latest release from [here](https://github.com/Maikeruwu/MicrosoftRewardsFarmerUI/releases/latest)
2. Install the program by running the `MicrosoftRewardsFarmerInstaller.msi` or the `setup.exe`  file
3. Enjoy!

## Troubleshooting
### The program doesn't go further than "Writing password..."
This is because Microsoft asks you if you want to stay signed in, which the script currently doesn't auto accept.
An easy workaround is to check the "Show Window" Checkbox and manually clicking the button.

### There is an issue with undetected_chromedriver
You can do two things:
- Update your Chrome (Navigate to the [settings](chrome://settings/help), it will auto update)
- Remove the `undetected_chromedriver.exe` file from your system (should be located at `%localappdata%\Packages\PythonSoftwareFoundation...\LocalCache\Roaming\undetected_chromedriver`)
  - If you can't delete the file, chromedriver is still running. You can kill the process in task manager (search for `undetected_chromedriver`)
 
## I have another problem
Make a new issue [here](https://github.com/Maikeruwu/MicrosoftRewardsFarmerUI/issues/new?assignees=Maikeruwu&labels=&projects=&template=bug_report.md&title=%5BBUG%5D) and describe your problem!
