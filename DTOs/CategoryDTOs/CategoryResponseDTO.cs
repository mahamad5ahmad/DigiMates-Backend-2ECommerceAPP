namespace ECommerceApp.DTOs.CategoryDTOs
{
    // DTO for returning category details.
    public class CategoryResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}