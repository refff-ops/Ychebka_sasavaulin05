namespace vaulin_up.Dtos;

public sealed class ProductDto
{

    public long ProductId { get; set; }

    public long Article { get; set; }

    public string Name { get; set; }

    public int ProductTypeId { get; set; }

    public decimal MinPartnerPrice { get; set; }

    public int MainMaterialTypeId { get; set; }

}
