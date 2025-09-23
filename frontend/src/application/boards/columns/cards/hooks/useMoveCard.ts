import { useApiMutation } from '@/application/shared/hooks/useApi'
import type { MoveCardCommand } from '@/dataAccess/cards/models'
import { ApiClient } from '@/dataAccess/services/ApiClient'
import type { ApiException } from '@/dataAccess/shared'
import { notification } from 'ant-design-vue'
import type { AxiosError } from 'axios'
import { container } from 'tsyringe'

function useMoveCard() {
  const client = () => container.resolve(ApiClient)
  return useApiMutation(
      async (vars: MoveCardCommand) => {
        try {
          return await client().cards.move(vars)
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

export { useMoveCard }
