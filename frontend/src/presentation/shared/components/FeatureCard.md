# FeatureCard Component

A universal, reusable Vue component for creating feature cards with customizable title, description, icon, cover, and footer sections.

## Features

- 🎨 **Flexible Design**: Supports multiple visual variants and customization options
- 🖱️ **Interactive**: Optional click handling with hover effects
- 📱 **Responsive**: Mobile-first design with responsive breakpoints
- 🎭 **Slot-based**: Extensive slot support for maximum customization
- 🎪 **Animated**: Optional floating icons and smooth transitions
- 🎯 **Accessible**: Built with accessibility best practices

## Props

| Prop | Type | Default | Description |
|------|------|---------|-------------|
| `title` | `string` | `undefined` | Card title text |
| `description` | `string` | `undefined` | Card description text |
| `icon` | `Component` | `undefined` | Icon component to display |
| `clickable` | `boolean` | `false` | Whether the card is clickable |
| `showCover` | `boolean` | `false` | Whether to show the cover section |
| `showFloatingIcons` | `boolean` | `false` | Whether to show animated floating icons in cover |
| `coverVariant` | `'default' \| 'primary' \| 'success' \| 'warning' \| 'danger'` | `'default'` | Cover background variant |

## Events

| Event | Payload | Description |
|-------|---------|-------------|
| `click` | `void` | Emitted when the card is clicked (only if `clickable` is true) |

## Slots

| Slot | Description |
|------|-------------|
| `icon` | Custom icon content |
| `title` | Custom title content |
| `description` | Custom description content |
| `cover` | Custom cover content |
| `footer` | Footer content (features, buttons, etc.) |

## Basic Usage

### Simple Card
```vue
<FeatureCard
  title="My Feature"
  description="This is a description of the feature"
/>
```

### Clickable Card with Icon
```vue
<FeatureCard
  title="Interactive Feature"
  description="Click me to see the action"
  :icon="RocketOutlined"
  :clickable="true"
  @click="handleClick"
/>
```

### Card with Cover and Floating Icons
```vue
<FeatureCard
  title="Animated Feature"
  description="Features animated cover with floating icons"
  :icon="StarOutlined"
  :show-cover="true"
  :show-floating-icons="true"
  cover-variant="primary"
  :clickable="true"
  @click="handleClick"
/>
```

### Card with Footer Slot
```vue
<FeatureCard
  title="Feature with Actions"
  description="This card has custom footer content"
  :icon="HeartOutlined"
  :clickable="true"
  @click="handleClick"
>
  <template #footer>
    <div class="actions">
      <a-button type="primary" size="small">Learn More</a-button>
      <a-button size="small">Try Now</a-button>
    </div>
  </template>
</FeatureCard>
```

## Advanced Usage

### Fully Customized Card
```vue
<FeatureCard
  :show-cover="true"
  cover-variant="success"
  :clickable="true"
  @click="handleClick"
>
  <template #icon>
    <CrownOutlined style="color: #faad14; font-size: 32px;" />
  </template>
  
  <template #title>
    <span style="color: #722ed1;">Premium Feature</span>
  </template>
  
  <template #description>
    <span style="font-style: italic;">Exclusive premium content</span>
  </template>
  
  <template #cover>
    <div class="premium-cover">
      <h4>Premium</h4>
      <p>Exclusive Access</p>
    </div>
  </template>
  
  <template #footer>
    <div class="premium-badges">
      <a-tag color="gold">Premium</a-tag>
      <a-tag color="purple">Exclusive</a-tag>
    </div>
  </template>
</FeatureCard>
```

## Cover Variants

The component supports different cover background variants:

- **default**: Light gray gradient
- **primary**: Blue gradient
- **success**: Green gradient
- **warning**: Orange gradient
- **danger**: Red gradient

## Styling

The component uses CSS custom properties for theming:

```scss
:root {
  --color-primary-300: #91d5ff;
  --color-primary-400: #69c0ff;
  --color-primary-500: #1890ff;
  --color-surface: #ffffff;
  --color-text: #262626;
  --color-text-weak: #8c8c8c;
  --color-border: #d9d9d9;
  --color-blue-gradient: linear-gradient(135deg, #1890ff, #096dd9);
}
```

## Responsive Behavior

- **Desktop**: Full feature set with optimal spacing
- **Tablet** (≤768px): Adjusted icon sizes and padding
- **Mobile** (≤480px): Compact layout with smaller icons

## Accessibility

- Proper ARIA attributes for interactive elements
- Keyboard navigation support for clickable cards
- Screen reader friendly content structure
- High contrast support

## Examples

See `FeatureCardExample.vue` for comprehensive usage examples demonstrating all features and customization options.

## Migration from CreateBoardCard

If you're migrating from the old `CreateBoardCard` component:

```vue
<!-- Old -->
<CreateBoardCard @create="handleCreate" />

<!-- New -->
<FeatureCard
  :title="t('boards.page.create')"
  :description="t('boards.createCard.description')"
  :icon="PlusOutlined"
  :clickable="true"
  :show-cover="true"
  :show-floating-icons="true"
  cover-variant="primary"
  @click="handleCreate"
>
  <template #footer>
    <!-- Your custom footer content -->
  </template>
</FeatureCard>
```