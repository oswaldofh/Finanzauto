namespace Finanzauto.Domain.DTOs
{
    public class DataVehicleDto
    {
        public string Plate { get; set; }

        public string Color { get; set; }

        public int BrandId { get; set; }

        public int PhaseId { get; set; }

        public string Line { get; set; }

        public int Year { get; set; }

        public string Mileage { get; set; }

        public decimal Price { get; set; }

        public string? Observation { get; set; }

    }
}
