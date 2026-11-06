namespace backend.services;

public sealed class BrandHasBeersException : Exception
{
    public BrandHasBeersException(int brandId)
        : base($"No se puede eliminar la marca {brandId} porque tiene cervezas asociadas.")
    {
    }
}
