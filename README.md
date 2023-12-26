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
### With the UI
1. Download the latest release from [here](https://github.com/Maikeruwu/MicrosoftRewardsFarmerUI/releases/latest)
2. Install the program by running the `MicrosoftRewardsFarmerInstaller.msi` or the `setup.exe`  file
3. Enjoy!

### With Docker
1. Install Docker
2. Open the Dockerfile and add telegram or discord credentials (optional)
3. Open the docker-compose.yml and edit the volumes if you don't want to use the default ones
4. Edit the wg0.conf file:
    - Add an interface private key (wg genkey)
    - Add an interface address (e.g. 192.168.2.1/24)
    - Add a peer public key (echo "your private key here" | wg pubkey)
    - Add a peer preshared key (wg genpsk) (optional)
    - Add a peer endpoint (e.g. 192.168.1.2:51820)
5. Run `docker-compose up -d` to start the container
6. Enjoy!

Please note that the docker runs the farm script every day between 03:00 and 04:00.

## Troubleshooting
### There is an issue with undetected_chromedriver
You can do two things:
- Update your Chrome (Navigate to the [settings](chrome://settings/help), it will auto update)
- Remove the `undetected_chromedriver.exe` file from your system (should be located at `%localappdata%\Packages\PythonSoftwareFoundation...\LocalCache\Roaming\undetected_chromedriver`)
  - If you can't delete the file, chromedriver is still running. You can kill the process in task manager (search for `undetected_chromedriver`)
 
 ### I cannot pull the docker image
 To have access to the docker image, you need to be logged in to the GitHub Container Registry. You can do this by running `docker login ghcr.io` and entering your GitHub username and a [personal access token](https://docs.github.com/packages/working-with-a-github-packages-registry/working-with-the-container-registry#authentifizierung-mit-personal-access-token-classic). After that, you should be able to pull the image.

## I have another problem
Make a new issue [here](https://github.com/Maikeruwu/MicrosoftRewardsFarmerUI/issues/new?assignees=Maikeruwu&labels=&projects=&template=bug_report.md&title=%5BBUG%5D) and describe your problem!
