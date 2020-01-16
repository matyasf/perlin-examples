# Perlin

Perlin (named from a [bird](https://en.wikipedia.org/wiki/Perlin_(falconry))) is a lightweight 2D graphics engine for .NET Core using [Veldrid](https://veldrid.dev/). It's well documented, has a simple architecture and is more meant to be as a base code for your engine than something that does everything out-of-the-box. Currently its Windows-only, in the near future I plan to get it work on OSX and maybe Linux, Android, iOS.

## Features

- Thanks for Veldrid it automatically chooses the ideal rendering backend for your PC (Vulkan/Metal/OpenGL/DirectX..)
- Loads images via ImageSharp, so common formats (jpg, png, bmp, gif) are supported.
- Uses the display tree concept (see below) to render your scene.
- Mouse and keyboard handling code included.
- Desktop rendering using Veldrid and SDL2.

## The display tree

![painter's algorithm in steps](Painters_algorithm.png)

Perlin puts the objects to render in a display tree similar to other rendering engines (HTML rendering, Android'd UI, Microsoft's WPF,..). The root of the display tree (called Stage) has children, these can also have children and so on creating a tree data structure. Every object in the display tree is a subclass of DisplayObject, here lie the common properties, like width, height, rotation,... Transformations in the display tree are cumulative, e.g. if you set the transparency/rotation/position/scale of an object its every descendant (child, grandchild,..) will be affected as well.

The Z-order (what is rendered in front of what) is determined based on the position of the display tree. An object will be always behind its children; the order between the children is that the first child will be in the back and the last one in the front.
