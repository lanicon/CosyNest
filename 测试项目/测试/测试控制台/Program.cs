#pragma warning disable

using System;

using OfficeOpenXml;

var path = @"C:\Users\liu\Desktop\新建 Microsoft Excel 工作表.xlsx";
var a = new ExcelPackage(new System.IO.FileInfo(path));
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
var b = a.Workbook.Worksheets[0].Cells.Address;
;
