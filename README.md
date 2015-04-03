## Flatten Packages for Visual Studio

[![Build status](https://ci.appveyor.com/api/projects/status/7l4iyu356ci5yy2u?svg=true)](https://ci.appveyor.com/project/madskristensen/reactsnippetpack)

Flattens `node-modules` folder to avoid long path issues on Windows.

Download this extension from the [VS Gallery](https://visualstudiogallery.msdn.microsoft.com/234d79e9-f0fd-41e1-a926-850da8e8c7d7)
or get the [nightly build](http://vsixgallery.com/extension/a7dff10f-3592-429c-9dc1-622fe517921d/).

### Features

The extension adds a button in the context-menu of Solution Explorer.

![React Snippet Pack](art/context-menu.png)

It shows up when you right-click `package.json` or any folder containing
a subfolder called `node_modules`.

The first time you run this extension it will download the npm package
[flatten-packages](https://www.npmjs.com/package/flatten-packages) if
you don't already have it installed.

![React Snippet Pack](art/first-run.png)

When the flatten-packages modules is installed (it takes ~5 seconds),
the extension will run the command to flatten the package hierarchy
in the `node_modules` folder.

![React Snippet Pack](art/normal-run.png)

Since the extension uses a Node module behind the scenes, the package
flattening will work with long paths that Windows and Visual Studio 
normally can't handle.