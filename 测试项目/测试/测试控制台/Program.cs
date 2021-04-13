#pragma warning disable

using System;
using System.Office;

using var a = CreateEPPlusOffice.ExcelBook(@"C:\Users\liu\Desktop\新建 Microsoft Excel 工作表.xlsx");
a[0][1, 1].Value = "aaa";
;
