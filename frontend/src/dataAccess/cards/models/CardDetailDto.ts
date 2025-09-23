import type { AttachmentDto } from './Attachment'

interface CardDetailDto {
  id: number;                     // long in C#
  title: string;
  description: string | null;
  position: number;
  dueDates: DueDateDto[];
  attachments: AttachmentDto[];
}

interface DueDateDto {
  dueAt: string;                   // ISO datetime string
  isComplete: boolean;
}

interface CardDetailPayload {
  columnId: number,
  id: number,
}

export type { CardDetailDto, DueDateDto, CardDetailPayload }


