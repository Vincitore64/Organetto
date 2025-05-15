<template>
  <main class="registration-form-wrapper">
    <h2>Sign up</h2>
    <a-form :model="form" layout="vertical" @submit.prevent="onSubmit">
      <a-form-item label="Email" name="email">
        <a-input v-model:value="form.email" placeholder="you@example.com" />
      </a-form-item>

      <p class="note">
        Organetto may send me updates.&nbsp;
        <a href="#" target="_blank">Privacy Policy</a>.
      </p>

      <a-form-item>
        <a-checkbox v-model:checked="form.agree">
          I agree to the <a href="#" target="_blank">Terms of Service</a> &amp; <a href="#" target="_blank">Privacy
            Policy</a>.
        </a-checkbox>
      </a-form-item>

      <a-form-item>
        <a-button type="primary" block :disabled="!form.email || !form.agree" html-type="submit">
          Continue
        </a-button>
      </a-form-item>

      <div class="or-separator">
        <span>OR</span>
      </div>

      <div class="social-buttons">
        <a-button block :icon="h(GithubOutlined)" class="social-btn">
          <span class="social-buttons__text">
            Continue with GitHub
          </span>
        </a-button>
        <a-button block :icon="h(GoogleOutlined)" class="social-btn">
          <span class="social-buttons__text">
            Continue with Google
          </span>
        </a-button>
        <a-button block :icon="h(WindowsOutlined)" class="social-btn">
          <span class="social-buttons__text">
            Continue with Microsoft
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

const form = reactive({
  email: '',
  agree: false
})

function onSubmit() {
  // TODO: hook into your auth service
  console.log('register with', form)
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

.social-btn ::v-deep .anticon {
  margin-right: 0.5rem;
}
</style>
