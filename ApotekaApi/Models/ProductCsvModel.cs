using CsvHelper.Configuration.Attributes;

public class ProductCsvModel
{
    [Name("Name")]
    public string Name { get; set; }

    [Name("Description")]
    public string? Description { get; set; }

    [Name("Baseprice")]
    public decimal Baseprice { get; set; }

    [Name("Quantity")]
    public int Quantity { get; set; }

    [Name("Categoryid")]
    public int? Categoryid { get; set; }

    [Name("Recipetypeid")]
    public int? Recipetypeid { get; set; }
}

