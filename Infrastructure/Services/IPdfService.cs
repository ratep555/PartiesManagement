namespace Infrastructure.Services
{
    public interface IPdfService
    {
        void GeneratePdf(int orderNo);
        void GeneratePdf1(int orderNo);
    }
}