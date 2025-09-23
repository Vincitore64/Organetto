import { useApiMutation } from '@/application/shared/hooks/useApi'
import type { MoveColumnCommand } from '@/dataAccess/columns/models'
import { ApiClient } from '@/dataAccess/services/ApiClient'
import type { ApiException } from '@/dataAccess/shared'
import { notification } from 'ant-design-vue'
import type { AxiosError } from 'axios'
import { container } from 'tsyringe'

function useMoveColumn() {
  const client = () => container.resolve(ApiClient)
  return useApiMutation(
      async (vars: MoveColumnCommand) => {
        try {
          return await client().columns.move(vars)
        } catch (ex) { // TODO: Rework to notification provider
          const error = ex as AxiosError<ApiException>
          if (error.response?.data) {
            notification.error({
              message: error.response.data.title ?? 'Unknown error',
              description: error.response.data.detail,
            })
          }
          throw ex
        }
      },
      []
    )
}

export { useMoveColumn }