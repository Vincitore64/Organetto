import type { ApiClient } from '@/dataAccess/services/ApiClient'
import type { UserDto } from '@/dataAccess/users/models'
import { type UsersStore } from '../stores/usersStore'
import { useAuthStore } from '@/application/authentication/stores/authStore'

/**
 * Higher-order composable for user data operations.
 * @param apiClient - injected ApiClient containing UsersClient
 * @returns an object with API methods bound to store
 */
export function useUsersComposable(apiClient: ApiClient, userStore: UsersStore) {
  const authStore = useAuthStore(apiClient)
  /**
   * Fetch all users from the backend and populate the store.
   */
  async function getAllUsers(): Promise<UserDto[]> {
    throw new Error('Not implemented')
    // const users = await apiClient.users.getAll();
    // store.setUsers(users);
    // return users;
  }

  /**
   * Fetch the current user by and set it in store.
   */
  async function getCurrentUser(): Promise<UserDto> {
    const firebaseUid = authStore.tokens?.uuid
    if (!firebaseUid) throw new Error('Not authenticated')
    if (userStore.currentUser && userStore.currentUser.firebaseUid === firebaseUid)
      return userStore.currentUser
    const user = await apiClient.users.getUserByUid(firebaseUid)
    userStore.setCurrentUser(user)
    return user
  }

  return {
    getAllUsers,
    getCurrentUser,
  }
}
