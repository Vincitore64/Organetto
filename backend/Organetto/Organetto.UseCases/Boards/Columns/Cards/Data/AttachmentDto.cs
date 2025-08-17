namespace Organetto.UseCases.Boards.Columns.Cards.Data
{
    public record AttachmentDto(long Id, string FileName, string FileKey, DateTime CreatedAt, long SizeBytes, string ContentType, long UploaderId)
    {
        public AttachmentDto() : this(0, string.Empty, string.Empty, DateTime.UnixEpoch, 0, string.Empty, 0)
        {
            
        }
    }
}
