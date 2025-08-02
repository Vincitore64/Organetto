import { createArrayMapper, createMapper } from '@/application/shared/mappers'
import type { BoardMemberDto } from '@/dataAccess/boards/members/models'
import type { BoardMemberVm } from '../models'

const mapBoardMember = createMapper<BoardMemberDto, BoardMemberVm>({
  id: 'id',
  email: 'email',
  name: 'name',
  role: 'role',
  avatar: () => null,
  roleLabel: 'role',
})

const mapBoardMembers = createArrayMapper(mapBoardMember)

export { mapBoardMember, mapBoardMembers }
