namespace Organetto.UseCases.Boards.Columns.Cards.Data
{
    public record CardDetailDto(long Id, string Title, string Description, long Position, DueDateDto[] DueDates, AttachmentDto[] Attachments)
    {
        public CardDetailDto() : this(0, string.Empty, string.Empty, 0, Array.Empty<DueDateDto>(), Array.Empty<AttachmentDto>())
        {
        }
    }
}
