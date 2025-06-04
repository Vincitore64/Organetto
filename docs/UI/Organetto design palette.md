Below is a cohesive **Organetto design palette** that works in both light and dark themes, with semantic mapping ready for CSS variables. (Ниже приведена согласованная **цветовая палитра Organetto**, работающая как в светлой, так и в тёмной теме, с семантическим отображением для CSS-переменных.)

---

### 1. Brand core (Primary & Accent)

(### 1. Основные фирменные цвета (Primary и Accent))

| Token           | Light-Hex | Dark-Hex | Notes                                                           |
| --------------- | --------- | -------- | --------------------------------------------------------------- |
| **Primary 50**  | #EEF4FF   | #0B1633  | very light background tint _(очень светлый оттенок фона)_       |
| **Primary 100** | #D8ECFF   | #13245C  | hover/selected bg _(фон при наведении)_                         |
| **Primary 500** | #315BFF   | #3B78FF  | main indigo / logo ✔ _(основной индиго / логотип)_              |
| **Primary 700** | #2542CC   | #5C8FFF  | high-contrast text on light bg _(контрастный текст на светлом)_ |
| **Accent**      | #9B4DFF   | #B87DFF  | drag handle, FABs _(ручки перетаскивания, плавающие кнопки)_    |

---

### 2. Informational neutrals

(### 2. Информационные нейтралы)

| Token              | Light-Hex | Dark-Hex | Usage                                      |
| ------------------ | --------- | -------- | ------------------------------------------ |
| **Surface**        | #FFFFFF   | #1E1E26  | cards, modals _(карточки, модалки)_        |
| **Background**     | #F6F8FA   | #121219  | app canvas _(фон всего приложения)_        |
| **Stroke**         | #D0D7DE   | #303347  | dividers, borders _(разделители, бордеры)_ |
| **Text-Primary**   | #1C2331   | #ECEFF4  | main copy _(основной текст)_               |
| **Text-Secondary** | #5A667A   | #9BA7C2  | helper labels _(подписи, вторичный текст)_ |

---

### 3. Status colors

(### 3. Цвета статусов)

| Status      | Base    | 20 % Tint | Contrast-Text |
| ----------- | ------- | --------- | ------------- |
| **Success** | #2DBE7F | #E8F9F1   | #0D3B27       |
| **Warning** | #FFB020 | #FFF6E3   | #663C00       |
| **Error**   | #F64F4F | #FEEDEE   | #4D0E0E       |

_(С учётом WCAG AA: контраст текста ≥ 4.5 : 1)_
(_(Контраст рассчитан по WCAG AA: ≥ 4,5 : 1)_)

---

### 4. Gradient presets

(### 4. Градиентные пресеты)

```css
/* Hero background (light) */
--gradient-hero: linear-gradient(120deg, #d8ecff 0%, #f5faff 40%, #fef6e9 100%);

/* Hero background (dark) */
--gradient-hero-dark: linear-gradient(120deg, #13245c 0%, #0b1633 100%);
```

(— используйте на крупных баннерах / splash экранах.)
(— использовать на крупных баннерах или splash-экранах.)

---

### 5. Suggested CSS variable map

(### 5. Предлагаемая карта CSS-переменных)

```scss
:root {
  /* brand */
  --color-primary-50: #eef4ff;
  --color-primary-100: #d8ecff;
  --color-primary-500: #315bff;
  --color-primary-700: #2542cc;
  --color-accent: #9b4dff;

  /* neutrals – light theme */
  --color-bg: #f6f8fa;
  --color-surface: #ffffff;
  --color-stroke: #d0d7de;
  --color-text: #1c2331;
  --color-text-weak: #5a667a;

  /* status */
  --color-success: #2dbe7f;
  --color-warning: #ffb020;
  --color-error: #f64f4f;

  /* tints for badges / chips */
  --color-success-20: #e8f9f1;
  --color-warning-20: #fff6e3;
  --color-error-20: #feedee;
}

[data-theme="dark"] {
  --color-primary-50: #0b1633;
  --color-primary-100: #13245c;
  --color-primary-500: #3b78ff; /* slightly lighter for dark */
  --color-primary-700: #5c8fff;
  --color-accent: #b87dff;

  --color-bg: #121219;
  --color-surface: #1e1e26;
  --color-stroke: #303347;
  --color-text: #eceff4;
  --color-text-weak: #9ba7c2;
}
```

_(Задаём переменные в \:root и переключаемся на тёмную тему через атрибут data-theme.)_
(_(Переменные заданы в \:root; тёмная тема — через атрибут data-theme.))_

---

### 6. Usage cheatsheet

(### 6. Как применять палитру)

| UI элемент         | Цвет / переменная                            | Why?                           |
| ------------------ | -------------------------------------------- | ------------------------------ |
| CTA / Primary btn  | `--color-primary-500` (hover → 700)          | высокое внимание               |
| Secondary btn      | border `--color-stroke`, text `--color-text` | ненавязчиво                    |
| Kanban list header | `--color-primary-50` background              | визуальная группировка списков |
| Card background    | `--color-surface`                            | читаемость контента            |
| Drag-shadow        | rgba(`--color-primary-500`, 0.2)             | подчёркивает бренд             |
| Success toast      | bg `--color-success`, text `#fff`            | instant feedback               |

_(Используйте систему токенов вместо жёстких hex-значений — легче масштабировать темы.)_
_(Рекомендуется обращаться к токенам, а не к статичным hex — это упрощает масштабирование тем.)_

---

With these tokens you can drive **SCSS maps, Tailwind config, or CSS-in-JS themes** from one source of truth. (Эти токены легко подключаются в **SCSS-карты, Tailwind config или CSS-in-JS темы** как единый источник правды.)

Let me know if you need a full Tailwind palette or design-token JSON! (Дайте знать, если нужна готовая палитра для Tailwind или JSON-файл с токенами дизайна!)
