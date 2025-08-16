namespace Organetto.UseCases.Boards.Columns.Cards.Data
{
    public record AttachmentDto(long Id, string Filename, string FileUrl, DateTime UploadedAt, long UploaderId)
    {
        public AttachmentDto() : this(0, string.Empty, string.Empty, DateTime.UnixEpoch, 0)
        {
            
        }
    }
}
