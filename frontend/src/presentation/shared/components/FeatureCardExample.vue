<template>
  <div class="feature-card-examples">
    <h2>FeatureCard Component Examples</h2>
    
    <div class="examples-grid">
      <!-- Basic Card -->
      <FeatureCard
        title="Basic Card"
        description="A simple card with title and description"
        :clickable="true"
        @click="handleClick('basic')"
      />
      
      <!-- Card with Icon -->
      <FeatureCard
        title="With Icon"
        description="Card featuring a custom icon"
        :icon="RocketOutlined"
        :clickable="true"
        @click="handleClick('icon')"
      />
      
      <!-- Card with Cover -->
      <FeatureCard
        title="With Cover"
        description="Card with animated cover and floating icons"
        :icon="StarOutlined"
        :show-cover="true"
        :show-floating-icons="true"
        cover-variant="success"
        :clickable="true"
        @click="handleClick('cover')"
      />
      
      <!-- Card with Footer Slot -->
      <FeatureCard
        title="With Footer"
        description="Card featuring custom footer content"
        :icon="HeartOutlined"
        cover-variant="warning"
        :show-cover="true"
        :clickable="true"
        @click="handleClick('footer')"
      >
        <template #footer>
          <div class="custom-footer">
            <a-button type="primary" size="small">Action</a-button>
            <a-button size="small">Cancel</a-button>
          </div>
        </template>
      </FeatureCard>
      
      <!-- Card with Custom Slots -->
      <FeatureCard
        :show-cover="true"
        cover-variant="danger"
        :clickable="true"
        @click="handleClick('custom')"
      >
        <template #icon>
          <CrownOutlined style="color: #faad14; font-size: 32px;" />
        </template>
        
        <template #title>
          <span style="color: #722ed1;">Custom Title</span>
        </template>
        
        <template #description>
          <span style="font-style: italic;">Custom description with styling</span>
        </template>
        
        <template #cover>
          <div class="custom-cover">
            <h4>Custom Cover Content</h4>
          </div>
        </template>
        
        <template #footer>
          <div class="badge-footer">
            <a-tag color="blue">Feature 1</a-tag>
            <a-tag color="green">Feature 2</a-tag>
            <a-tag color="orange">Feature 3</a-tag>
          </div>
        </template>
      </FeatureCard>
      
      <!-- Non-clickable Card -->
      <FeatureCard
        title="Information Card"
        description="This card is not clickable and serves as information display"
        :icon="BulbOutlined"
        :show-cover="true"
        cover-variant="primary"
      >
        <template #footer>
          <div class="info-footer">
            <small style="color: #8c8c8c;">Read-only information</small>
          </div>
        </template>
      </FeatureCard>
    </div>
  </div>
</template>

<script setup lang="ts">
import { message } from 'ant-design-vue'
import {
  RocketOutlined,
  StarOutlined,
  HeartOutlined,
  CrownOutlined,
  BulbOutlined
} from '@ant-design/icons-vue'
import FeatureCard from './FeatureCard.vue'

const handleClick = (cardType: string) => {
  message.success(`Clicked ${cardType} card!`)
}
</script>

<style scoped lang="scss">
.feature-card-examples {
  padding: 2rem;
  max-width: 1200px;
  margin: 0 auto;
  
  h2 {
    text-align: center;
    margin-bottom: 2rem;
    color: var(--color-text, #262626);
  }
}

.examples-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));
  gap: 1.5rem;
  
  @media (max-width: 768px) {
    grid-template-columns: 1fr;
    gap: 1rem;
  }
}

.custom-footer {
  display: flex;
  gap: 8px;
  justify-content: center;
}

.badge-footer {
  display: flex;
  gap: 4px;
  justify-content: center;
  flex-wrap: wrap;
}

.info-footer {
  text-align: center;
}

.custom-cover {
  display: flex;
  align-items: center;
  justify-content: center;
  height: 100%;
  background: linear-gradient(135deg, #ff7875, #ff9c6e);
  color: white;
  
  h4 {
    margin: 0;
    font-weight: 600;
  }
}
</style>