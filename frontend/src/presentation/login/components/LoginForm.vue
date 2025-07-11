<template>
  <a-card :bordered="false" class="card" :body-style="{ padding: '24px' }">
    <h2 class="panel__title">{{ t('login.page.title') }}</h2>

    <a-form :model="form" :rules="rules" layout="vertical" @submit.prevent="handleSubmit">
      <a-form-item :label="t('login.page.email')" name="email">
        <a-input v-model:value="form.email" type="email" placeholder="you@example.com" />
      </a-form-item>

      <a-form-item :label="t('login.page.password')" name="password">
        <a-input-password v-model:value="form.password" placeholder="••••••••" />
      </a-form-item>

      <div class="forgot">
        <router-link to="/forgot" class="link">{{ t('login.page.forgot') }}</router-link>
      </div>

      <a-button type="primary" html-type="submit" :loading="loading" block class="btn-primary">
        {{ t('login.page.continue') }}
      </a-button>
    </a-form>

    <a-divider plain>{{ t('login.page.or') }}</a-divider>

    <a-button block class="oauth github" @click="oauth('github')">
      <template #icon>
        <GithubOutlined />
      </template>
      {{ t('login.page.github') }}
    </a-button>

    <a-button block class="oauth google" @click="oauth('google')">
      <template #icon>
        <GoogleOutlined />
      </template>
      {{ t('login.page.google') }}
    </a-button>

    <a-button block class="oauth microsoft" @click="oauth('microsoft')">
      <template #icon>
        <WindowsOutlined />
      </template>
      {{ t('login.page.microsoft') }}
    </a-button>

    <p class="footer">
      {{ t('login.page.noAccount') }}
      <router-link to="/signup">{{ t('login.page.signup') }}</router-link>
    </p>
  </a-card>
</template>

<script setup lang="ts">
import { reactive } from 'vue'
import { useLogin } from '@/application/authentication/hooks/useAuth'
import { message } from 'ant-design-vue'
import { GithubOutlined, GoogleOutlined, WindowsOutlined } from '@ant-design/icons-vue'
import { useI18n } from 'vue-i18n'
import { tryInjectServices } from '@/shared'
import { ApiClient } from '@/dataAccess/services/ApiClient'

const emits = defineEmits<{
  (e: 'success'): void
}>()

const { t } = useI18n()
const apiClient = tryInjectServices().resolve(ApiClient)
const { execute: login, isLoading: loading, error } = useLogin(apiClient)

const form = reactive({ email: '', password: '' })

const rules = {
  email: [
    { required: true, message: t('login.validation.emailRequired'), type: 'email', trigger: 'blur' },
  ],
  password: [{ required: true, message: t('login.validation.passwordRequired'), trigger: 'blur' }],
}

async function handleSubmit() {
  const ok = await login(0, { ...form })
  if (ok) {
    emits('success')
  } else if (error.value) {
    message.error((error.value as Error).message ?? 'Unknown error')
  }
}

function oauth(provider: 'github' | 'google' | 'microsoft') {
  message.info(t('login.page.soon', { provider }))
}
</script>

<style scoped lang="scss">
.card {
  width: 100%;
  padding: 1rem;
  background: var(--color-surface);
  border-radius: 16px;
  box-shadow: 0 8px 24px rgba(0, 0, 0, 0.07);
}

.panel__title {
  text-align: center;
  font-size: 2rem;
  font-weight: 500;
  letter-spacing: 0.2px;
  margin-bottom: 1rem;
  color: var(--color-text);
}

.forgot {
  text-align: right;
  margin-bottom: 1rem;

  .link {
    font-size: 0.85rem;
    color: var(--color-primary-500);
  }
}

.btn-primary {
  background: var(--color-primary-500);
  border: none;

  &:hover {
    background: var(--color-primary-700);
  }
}

.oauth {
  margin-top: 0.5rem;
}

.footer {
  margin-top: 1.25rem;
  text-align: center;
  font-size: 1rem;
  letter-spacing: 0.2px;
  color: var(--color-text-weak);

  a {
    color: var(--color-primary-500);
    font-weight: 600;
  }
}
</style>
