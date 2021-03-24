#pragma warning disable

using System;
using System.Linq;
using System.Mapping;
using System.Mapping.Settlement;
using System.Maths;



var a = CreateMapping.SettlementPointRoot("BM1", 21);
var B = a.Add(11111).Add("1", 22222).Add(33333).Add("2", 44444).Add(55555).
    Add("3", 66666).Add(77777);
B.Add("BM1", 88888);
B.Father.RemoveOffspring();
var x = a.SonAll.OfType<ISettlementPoint>().ToArray();
foreach (var item in x)
{
    Console.WriteLine($"{item.Name}:{item.High}:{item.ClosedDifference}");
}

Console.ReadLine();
;
