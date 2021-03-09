# Project One

## Notes
- `BitmapConvolutionalFiltersExtensions.cs` file contains detailed explanation of working with `Marshalling` in `C#` and more efficient application of operations on `System.Drawing.Bitmap`.
- Please, use `#region` block in `MainViewModel`. I used them for grouping logically related methods and properties.

## Technology stack 
* Languages: `C#`
* Runtime: `.net5`
* Framework: `AvaloniaUI`
## Tasks
* Image uploading and saving.
* Image resetting without loading original image.
* Applying functional filters:


-[x] Contrast
 

-[x] Invert
 

-[x] Gamma
 

-[x] Brightness correction


* Applying convolutional filters:

-[x] Identity (just to check if it works).
 

-[x] Blur
 

-[x] Gaussian Blur
 

-[x] Edge detection
 

-[x] Emboss
 

-[x] Sharpen


## Additional tasks
-[x] Canvas with graph 
 

-[x] Chaining and storing custom functional filters in the order applied.
 

-[x] Applying custom functional filters to the image (see custom filters list on the left).
 

-[ ] Adding, moving, deleting points on the graph.
 

-[ ] Applying function presented on the graph to the image (and storing it as filter).
 

-[ ] Creating new functional filter from the identity filter(two movable only on axis points).
 

-[ ] Storing it as a function filter.

## Lab part 
```
In the application from Project 1 add new kind of filter with 9 parameters [a1, a2, ..., a9] editable from the UI. The filter converts each pixel [sr, sg, sb] to the pixel [dr, dg, db] according to the equations:
dr = a1 * sr + a2 * sg + a3 * sb
dg = a4 * sr + a5 * sg + a6 * sb
db = a7 * sr + a8 * sg + a9 * sb
Arrange parameters in the UI in 3x3 grid.
```
**Mark** : 5/5.
