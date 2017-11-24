# usql-split-tiff
U-SQL `Extractor` and `Outputter` to split GeoTIFF rasters into smaller chunks. 

## Get Started
Register the `SplitTiff` assembly together with its managed dependency `BitMiracle.LibTiff.NET40`.

## Run it in U-SQL
The following snippet will split each raster inside `/tiff_inputs` into chunks of `224*224`.
**NOTE**: U-SQL Outputters currently [lack the feature of outputting multiple files with dynamic names](https://stackoverflow.com/questions/42636855/u-sql-output-in-azure-data-lake/42676271#42676271).

```sql
REFERENCE ASSEMBLY [BitMiracle.LibTiff.NET40];
REFERENCE ASSEMBLY SplitTiff;

@tiff_images =
    EXTRACT chunkId int
        , tiff byte[]
        , size int
        , name string
    FROM "/tiff_inputs/{name}.tif"
    USING new SplitTiff.SplitTiffExtractor(224, 224);

OUTPUT @tiff_images
TO "/tiff_outputs/out_{*}.tif"
USING new SplitTiff.TiffOutputter();
```

## Run it in Console
The `SplitTiff.Console` app allows you to split files using CLI.
```
SplitTiff.Console.exe input.tiff output_{0}.tiff
```
>`{0}` will become chunk Id.