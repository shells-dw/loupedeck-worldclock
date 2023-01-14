 ![GitHub](https://img.shields.io/github/license/shells-dw/loupedeck-worldclock)
 ![GitHub last commit](https://img.shields.io/github/last-commit/shells-dw/loupedeck-worldclock)
  ![GitHub downloads](https://img.shields.io/github/downloads/shells-dw/loupedeck-worldclock/total)
 [![Tip](https://img.shields.io/badge/Donate-PayPal-green.svg)]( https://www.paypal.com/donate?hosted_button_id=8KXD334CCEEC2) / [![ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/Y8Y4CE9LH)

 # Loupedeck World Clock Plugin

![Overview](/docs/overview.png)

 ## What Is This (and what does it do?)

It's a plugin for the [Loupedeck Consoles][Loupedeck] which allows to place time displays for places all around the world on the Loupedeck.
It supports 12h and 24h time displays and analog clock faces which show the time, date, location of places in any time zone - or a mixture of all.

No external connections are made, the database comes with the plugin and works locally.

- [x] **Works on Windows and MacOS**

## Release / Installation

You can find the precompiled plugin lplug4 file in the [Releases][Releases]. Download and open it, your computer should already recognize this as a Loupedeck plugin file and offer to open it with Loupedeck Configuration Console - which will have the plugin available in the list then.

## Usage

![Menu](/docs/menu.png)

Pull what you want to have displayed to a free button slot on your Loupedeck, select the zone and location for which you want to have the time shown, click save - and that's it.

The buttons have no touch action.

## Notes / Restrictions

- Looking for Korean and Japanese speakers for the respective translation, get in touch!
- While the date display will use your locale (_the date format you set or that's automatically set based on your language settings by Windows_) on Windows, the date on MacOS will always be displayed in US format due to how mono works (_that's what makes .NET Framework run on MacOS_). The same is true for the local clock that comes with Loupedeck by the way. In contrary to the default clock, this plugin is at least able to display 24h clock on MacOS :wink: Might be improved at some point in the future.
- Etc/GMT offsets are geographical offsets westerly and easterly of Greenwich, not as commonly used a time offset. That means that GMT-2 is two timezones east of Greenwich (what you would normally refer to as GMT+2). Just mentally flip + and - and you're good.
- If your city is not available in the list, pick one that's in your timezone. You will find a city that's near you. This is for three reasons: 
    - so that users don't have to search through multiple thousand locations
    - because I didn't want your computer to have to perform _and wait for_ external API calls to internet services that offer this kind of service
    - because that's what the timezone database I use offers. 

# I have an issue or miss a feature

You can submit an issue or request a feature with [GitHub issues] or start a [discussion](https://github.com/shells-dw/loupedeck-worldclock/discussions). Please describe as good as possible what went wrong or doesn't act like you'd expect it to when you open an issue. 

# Support

If you'd like to drop me a coffee for the hours I've spent on this:
[![Tip](https://img.shields.io/badge/Donate-PayPal-green.svg)]( https://www.paypal.com/donate?hosted_button_id=8KXD334CCEEC2), sponsor me on [GitHub](https://github.com/sponsors/shells-dw) 
or use Ko-Fi [![ko-fi](https://ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/Y8Y4CE9LH)

# Changelog
## [1.2.0] - 2023-01-14
### Added
- Digital watch faces with additional day of the week display
### Improved
- Removed the need for Newtonsoft.Json and went with JSON functions that come with the SDK to avoid potential mismatch issues between the bundled and the NuGet Newtonsoft.Json version in the future

<details><summary>Changelog History</summary><p>

## [1.1.0] - 2023-01-10
### Added
- UI Translation for German and French
## [1.0.0] - 2023-01-02
### Added
- Time + date displays without location
- Time + seconds without location
## Changed
- Streamlined action file names (_Note: existing time displays on the [Loupedeck] made with v0.2.0 might need to be placed again_)
- Removed checks for non-nominal plugin stati (currently not seeing a reason for setting the status as this plugin doesn't depend on anything external, so why check for it)
- General code cleanup
- #beautified icons
## [0.2.0] - 2023-01-01
### Added
- Analog watch faces
## [0.1.0] - 2022-12-31
- initial release

</p></details>


<!-- Reference Links -->

[Loupedeck]: https://loupedeck.com "Loupedeck.com"
[Releases]: https://github.com/shells-dw/loupedeck-worldclock/releases "Releases"
[GitHub issues]: https://github.com/shells-dw/loupedeck-worldclock/issues "GitHub issues link"

