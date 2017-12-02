# usql-split-tiff
U-SQL `Extractor` and `Outputter` to split GeoTIFF rasters into smaller chunks. 

## Get Started
Register the `SplitTiff` assembly together with its managed dependencies `BitMiracle.LibTiff.NET40` and `System.IO.Compression`. [Here are some sample GeoTIFFs](http://www.terracolor.net/sample_imagery.html) that can be used as inputs.

## Run it in U-SQL
Following snippet will split the input `@in` into chunks of `1000 x 1000` and create a zip output `@out`.

**NOTE**: U-SQL Outputters currently [lack the feature of outputting multiple files with dynamic names](https://stackoverflow.com/questions/42636855/u-sql-output-in-azure-data-lake/42676271#42676271) hence we have to use `ZipOutputter`.

```sql
REFERENCE ASSEMBLY [BitMiracle.LibTiff.NET40];
REFERENCE ASSEMBLY [System.IO.Compression];
REFERENCE ASSEMBLY SplitTiff;

DECLARE @in string = "/input.tif";
DECLARE @out string = "/output.zip";

@tiff_chunks =
    EXTRACT tiff byte[]
    FROM @in
    USING new SplitTiff.SplitTiffExtractor(1000, 1000);

OUTPUT @tiff_chunks
TO @out
USING new SplitTiff.ZipOutputter("out_{0}.tiff");
```
>`{0}` will become chunk Id inside the zip archive.

## Run it in Console
The `SplitTiff.Console` app allows you to split files using CLI.
```
SplitTiff.Console.exe input.tiff output_{0}.tiff
```