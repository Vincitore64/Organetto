<template>
  <main class="registration-form-wrapper">
    <h2>{{ t('registration.page.title') }}</h2>
    <a-form layout="vertical" :disabled="registerState.isLoading.value">
      <a-form-item label="Email" name="email" v-bind="validateInfos.email" :rules="rules.email">
        <a-input v-model:value="formState.email" placeholder="you@example.com" />
      </a-form-item>
      <a-form-item :label="t('registration.page.passwordLabel')" name="password" v-bind="validateInfos.password"
        :rules="rules.password">
        <a-input-password v-model:value="formState.password"
          :placeholder="t('registration.page.passwordPlaceholder')" />
      </a-form-item>
      <a-form-item :label="t('registration.page.confirmLabel')" name="confirm" v-bind="validateInfos.confirm"
        :rules="rules.confirm">
        <a-input-password v-model:value="formState.confirm" :placeholder="t('registration.page.confirmPlaceholder')" />
      </a-form-item>

      <p class="note">
        {{ t('registration.page.updatesText.before') }}&nbsp;
        <a href="#" target="_blank">{{ t('registration.page.updatesText.link') }}</a>.
      </p>

      <a-form-item v-bind="validateInfos.agree" :rules="rules.agree">
        <a-checkbox v-model:checked="formState.agree">
          {{ t('registration.page.agreeText.before') }}
          <a href="#" target="_blank">{{ t('registration.page.agreeText.terms') }}</a>
          {{ t('registration.page.agreeText.middle') }}
          <a href="#" target="_blank">{{
            t('registration.page.agreeText.policy') }}</a>.
        </a-checkbox>
      </a-form-item>

      <a-form-item>
        <a-button type="primary" block :disabled="!formState.email || !formState.password || !formState.agree"
          html-type="submit" @click="onSubmit" :loading="registerState.isLoading.value">
          {{ t('registration.page.continueButton') }}
        </a-button>
      </a-form-item>

      <div class="or-separator">
        <span>{{ t('registration.page.or') }}</span>
      </div>

      <div class="social-buttons">
        <a-button block :icon="h(GithubOutlined)" class="social-btn">
          <span class="social-buttons__text">
            {{ t('registration.page.continueWith.github') }}
          </span>
        </a-button>
        <a-button block :icon="h(GoogleOutlined)" class="social-btn">
          <span class="social-buttons__text">
            {{ t('registration.page.continueWith.google') }}
          </span>
        </a-button>
        <a-button block :icon="h(WindowsOutlined)" class="social-btn">
          <span class="social-buttons__text">
            {{ t('registration.page.continueWith.microsoft') }}
          </span>
        </a-button>
      </div>
    </a-form>
  </main>
</template>

<script setup lang="ts">
import { reactive, h } from 'vue'
import { Form as AForm, Input as AInput, Button as AButton, Checkbox as ACheckbox } from 'ant-design-vue'
import { GithubOutlined, WindowsOutlined, GoogleOutlined } from '@ant-design/icons-vue'
import { useI18n } from 'vue-i18n'
import type { Rule } from 'ant-design-vue/lib/form/interface'
import { useRegister } from '@/application/authentication/hooks/useAuth'
import { tryInjectServices } from '@/shared'
import { ApiClient } from '@/dataAccess/services/ApiClient'

const { t } = useI18n()

const apiClient = tryInjectServices().resolve(ApiClient)
const registerState = useRegister(apiClient)

const formState = reactive({
  email: '',
  password: '',
  confirm: '',
  agree: false
})

const rules: Record<string, Rule[]> = reactive({
  email: [
    { required: true, message: t('registration.validation.emailRequired'), trigger: 'blur' },
    { type: 'email', message: t('registration.validation.emailInvalid'), trigger: ['blur', 'change'] }
  ],
  password: [
    { required: true, message: t('registration.validation.passwordRequired'), trigger: 'blur' },
    { min: 6, message: t('registration.validation.passwordMin'), trigger: 'blur' }
  ],
  confirm: [
    { required: true, message: t('registration.validation.confirmRequired'), trigger: 'blur' },
    { validator: validateConfirm, trigger: 'blur' }
  ],
  agree: [
    {
      validator: validateAgree,
      trigger: 'change'
    }
  ]
})

function validateAgree(_: unknown, value: boolean) {
  if (value) {
    return Promise.resolve()
  } else {
    return Promise.reject(new Error(t('registration.validation.agreeRequired')))
  }
}

function validateConfirm(_: unknown, value: string) { // callback: (error?: string) => void
  // debugger
  if (value !== formState.password) {
    return Promise.reject(new Error(t('registration.validation.passwordMismatch')))
  }
  return Promise.resolve()
}


const { resetFields, validate, validateInfos } = AForm.useForm(formState, rules, { immediate: false, validateOnRuleChange: true })

async function onSubmit() {
  await validate()
  // TODO: hook into your auth service
  console.log('register with', formState)
  await registerState.execute(0, {
    email: formState.email,
    password: formState.password,
    displayName: formState.email,
  }).then((success) => {
    if (success) {
      resetFields()
      // TODO: redirect to login page
    }
  })
}
</script>

<style scoped>
.registration-form-wrapper {
  padding: 2rem;
  background: white;
  border-radius: 12px;
  box-shadow: 0 4px 16px rgba(0, 0, 0, 0.1);
  display: grid;
  grid-template-rows: auto 1fr;
  gap: 1rem;
}

.registration-form-wrapper h2 {
  margin: 0 0 1rem;
  font-size: 1.75rem;
  text-align: center;
}

.note {
  font-size: 0.875rem;
  color: #666;
  margin: 0 0 1rem;
}

.or-separator {
  display: flex;
  align-items: center;
  text-align: center;
  margin: 1rem 0;
}

.or-separator::before,
.or-separator::after {
  content: '';
  flex: 1;
  height: 1px;
  background: #ddd;
}

.or-separator span {
  padding: 0 0.5rem;
  color: #888;
  font-size: 0.875rem;
}

.social-buttons {
  display: grid;
  gap: 0.5rem;
}

.social-buttons__text {
  max-width: 80%;
  width: 50%;
}

.social-btn {
  display: flex;
  align-items: center;
  justify-content: center;
}

.social-btn :deep() {
  margin-right: 0.5rem;
}
</style>
