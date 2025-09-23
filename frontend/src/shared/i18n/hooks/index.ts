import { useI18n } from 'vue-i18n'

// ---------- i18n safe hook ----------
function tryI18n() {
  try { return useI18n() } catch { return { t: (_k: string, fallback: string) => fallback } as any }
}

export {
  tryI18n
}