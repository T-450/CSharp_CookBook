// See https://aka.ms/new-console-template for more information

using EnumerationRecords;

var cheeseburger = Hamburger.Cheeseburger;
var bigMac = Hamburger.BigTasty;
var bigTasty = Hamburger.BigMac;

var allItems = Hamburger.GetAll();
foreach (var item in allItems)
{
    Console.WriteLine(item.ToString());
}
// Console.WriteLine(Hamburger.GetAll());
// Console.WriteLine(bigMac.ToString());
// Console.WriteLine(bigTasty.ToString());

// Hamburguer Enumeration
public record Hamburger : Enumeration<Hamburger>
{
    public static readonly Hamburger Cheeseburger = new Hamburger(1, "Cheeseburger");
    public static readonly Hamburger BigMac = new Hamburger(2, "Big Mac");
    public static readonly Hamburger BigTasty = new Hamburger(3, "Big Tasty");

    private Hamburger(int id, string description) : base(id, description) { }
}
