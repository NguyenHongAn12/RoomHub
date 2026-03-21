namespace Application.DTOs.RoomPosts
{
    public class RoomPhotoViewModel
    {
        public int Id { get; set; }
        public string Url { get; set; } = null!;
        public bool IsMain { get; set; }
    }
}
