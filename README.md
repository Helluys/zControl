# zControl
A command and control package for Unity.

Provides generic command and control components that integrate naturally in a Unity environment. The package is made of a core C# command and control library, as well as Unity bindings for common use cases.

## How to use
Download and unpack the package to your desired location, then add as a local package from Unity Package Manager. You can import the samples from there to have example use cases.

Once the package is imported in your project, you will have access to the MonoBehaviours for your game objects. You can also create your own behaviour relying on the pure C# library directly from your code.

## How to help
This small package welcomes any improvement. If you want to add functionality, add the package to your project as above and add it as "testables" to your Unity project (so that the Test Runner sees the package tests). You can do so by adding the following to your [project manifest](https://docs.unity3d.com/Manual/upm-manifestPrj.html).
```
  "testables": [
    "syulleh.zcontrol"
  ]
```
## License
The package is currently under MIT license (see [LICENSE.md](LICENSE.md) file).
