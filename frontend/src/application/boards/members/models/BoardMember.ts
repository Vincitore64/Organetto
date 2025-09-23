import type { BoardMemberDto } from '@/dataAccess/boards/members/models'

/** Board member enriched for avatars & roles. */
export interface BoardMemberVm extends BoardMemberDto {
  /** Absolute URL to 64×64 avatar image (or null). */
  avatar: string | null
  /** Localised role label (“Owner”, “Editor”…). */
  roleLabel: string
}