#pragma warning disable

using System;
using System.Linq;
using System.Mapping;
using System.Mapping.Settlement;
using System.Maths;

static IUnit<IUTLength> Get(Num num)
    => CreateBaseMathObj.Unit(num, CreateMapping.UTSettlement);

var a = CreateMapping.SettlementPointRoot("BM1", 21);
var B = a.Add(Get(11111)).Add("1", Get(22222)).Add(Get(33333)).Add("2", Get(44444)).Add(Get(55555)).
    Add("3", Get(66666)).Add(Get(77777));
B.Add("BM1", Get(88888));
var x = a.SonAll.OfType<ISettlementPoint>().ToArray();
foreach (var item in x)
{
    Console.WriteLine($"{item.Name}:{item.High}:{item.ClosedDifference}");
}

Console.ReadLine();
;
