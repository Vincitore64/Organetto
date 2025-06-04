import { inject } from 'vue'
import { ProvidedService } from './ProvidedService'
import type { DependencyContainer } from 'tsyringe'

export function tryInjectServices() {
  const container = inject<DependencyContainer>(ProvidedService.Container)
  if (!container) throw new Error('Container not found')
  return container
}
