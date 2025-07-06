import type { UserDto } from '@/dataAccess/users/models'
import { defineStore } from 'pinia'

export const useUsersStore = defineStore('users', {
  state: () => ({
    users: [] as UserDto[],
    currentUser: null as UserDto | null,
  }),
  getters: {
    getAll: (state) => state.users,
    getCurrent: (state) => state.currentUser,
  },
  actions: {
    setUsers(users: UserDto[]) {
      this.users = users
    },
    setCurrentUser(user: UserDto) {
      this.currentUser = user
    },
    clear() {
      this.users = []
      this.currentUser = null
    },
  },
})

export type UsersStore = ReturnType<typeof useUsersStore>
