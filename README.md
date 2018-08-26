# Switcheroo.OrchardCore
Custom module for Orchard Core CMS that allows the administrator to change the active theme for the current session by specifying the theme name using a query string parameter. This allows the administrator to view a theme other than the currently active website theme.

## Status
[![Build status](https://ci.appveyor.com/api/projects/status/6u3a9gvb1gy43ds1?svg=true)](https://ci.appveyor.com/project/davidhayden/switcheroo) [![Status](https://img.shields.io/myget/davidhayden-ci/v/Switcheroo.OrchardCore.svg)](https://www.myget.org/feed/davidhayden-ci/package/nuget/Switcheroo.OrchardCore)

## Getting Started

* Add the NuGet package, **Switcheroo.OrchardCore**, to the Orchard Core CMS Website.
* Launch the application and sign in as an *administrator*.
* Enable the Switcheroo Module from the admin dashboard.
* Enable any themes you wish to view from your browser session.
* Browse to the homepage of the website.
* Append *?theme={themeName}* to the URL of your website and hit RETURN, where *{themeName}* is the name of an **enabled** theme on the website. The theme on the website should change to that theme after the request.
* Switch as necessary to other themes using the same technique.

```
?theme=TheAgencyTheme

?theme=TheBlogTheme

?theme=SafeMode
```

## Troubleshooting

If you are not signed in as an *administrator*, Switcheroo will ignore your request.

Switcheroo does not validate the theme name. Make sure you choose a theme that is **enabled** for your website. It's not enough for the theme to be installed. The theme must be enabled or you will get an error.

If you choose a theme that does not exist, Orchard Core will ignore Switcheroo.

If you choose a theme that has been installed, but has not been enabled, Orchard Core will display an error. Either enable the theme or choose another to avoid the error.

Switching to a theme that does not support your custom website modifications could provide undesirable results. This is normal.

Switcheroo adds a cookie with the theme name you supplied in the query string. You can delete this cookie during your session. If the query string parameter and cookie do not exist during a request, Switcheroo will do nothing to override the currently active theme.

## Road map

There are no plans to add any additional features to the code.

## Credits
Created and maintained by David Hayden.
