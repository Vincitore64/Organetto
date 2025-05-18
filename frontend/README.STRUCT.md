### Архитектура уровня кода для проекта Organetto (Русская версия)

---

#### **Общая структура**

Проект разделен на четыре уровня, образующих **многоуровневую архитектуру**:

1. **Web (Сборка приложения)**: Точка входа, сборка, маршрутизация и глобальная конфигурация.
2. **Presentation (UI)**: Отвечает за отображение данных и взаимодействие с пользователем.
3. **App (Бизнес-логика)**: Содержит сценарии использования, управление состоянием и координацию между слоями.
4. **Data Access (Работа с данными)**: Взаимодействие с API, базами данных и внешними сервисами.
5. **Shared (Общие ресурсы)**: Переиспользуемые компоненты, утилиты и инфраструктурный код.

#### **Организация внутри уровней**

Каждый уровень структурирован по **доменам** (например, `transactions`, `settings`), а внутри доменов — по **функционалу**:

```
src/
├── web/                    # Уровень Web (Сборка)
│   ├── pages/              # Основные страницы приложения
│   │   ├── Home/           # Главная страница
│   │   └── Auth/           # Страницы авторизации
│   ├── router/             # Маршрутизация (Vue Router)
│   └── entry/              # Точки входа (main.ts, App.vue)
│
├── presentation/           # Уровень UI
│   ├── transactions/       # Домен: Транзакции
│   │   ├── components/     # Vue-компоненты
│   │   ├── views/          # Страницы/макеты
│   │   └── styles/         # Стили (CSS/SCSS)
│   ├── settings/           # Домен: Настройки
│   └── shared/             # Общие ресурсы уровня
│       ├── components/     # Глобальные компоненты (кнопки, формы)
│       └── utils/          # Утилиты для UI (валидация, форматирование)
│
├── app/                    # Уровень бизнес-логики
│   ├── transactions/       # Домен: Транзакции
│   │   ├── store/          # Pinia/Vuex-сторы
│   │   ├── hooks/          # Хуки (логика компонентов)
│   │   └── services/       # Сервисы (координация данных)
│   └── shared/             # Общие ресурсы уровня
│       ├── plugins/        # Плагины (инициализация)
│       └── types/          # Глобальные типы данных
│
├── dataAccess/            # Уровень работы с данными
│   ├── transactions/       # Домен: Транзакции
│   │   ├── api/            # API-клиенты (Axios)
│   │   └── repositories/   # Репозитории (CRUD-операции)
│   └── shared/             # Общие ресурсы уровня
│       ├── http/           # HTTP-клиент (интерсепторы)
│       └── cache/          # Кэширование данных
│
└── shared/                 # Глобальные общие ресурсы
    ├── lib/                # Внешние библиотеки
    ├── constants/          # Константы (роуты, настройки)
    └── assets/             # Статика (шрифты, иконки)
```

#### **Ключевые правила**

1. **Вертикальная изоляция**: Домены не зависят друг от друга.
2. **Горизонтальное разделение**: Уровни зависят только от нижележащих (например, `app` → `dataAccess`).
3. **Shared-папки**: Для кода, используемого внутри уровня несколькими доменами.
4. **Типизация**: Все модели данных описаны в `models` внутри доменов.

### **Преимущества**

- **Модульность**: Изменения в одном домене/уровне не затрагивают другие.
- **Тестируемость**: Четкое разделение упрощает unit- и E2E-тесты.
- **Переиспользуемость**: Общий код вынесен в `shared`.
- **Масштабируемость**: Новая функциональность добавляется через домены.

---

### **Принципы взаимодействия / Layer Interaction**

1. **Web → Presentation → App → Data Access**: Прямой поток данных.
2. **Shared**: Используется всеми уровнями.
3. **Запрещено**:
   - Data Access → App → Presentation → Web
   - Кросс-доменные зависимости.

### **Пример запроса / Flow Example**

```
Пользователь →
Web (страница) →
Presentation (компонент) →
App (хук) →
Data Access (API-клиент) →
Сервер
```

---

---

### Code Layer Architecture for Organetto Project (English Version)

---

#### **General Structure**

The project follows a **multi-layered architecture** with four levels:

1. **Web (Build)**: Entry point, build configuration, routing.
2. **Presentation (UI)**: Handles data display and user interaction.
3. **App (Business Logic)**: Contains use cases, state management, and layer coordination.
4. **Data Access (Data Layer)**: Manages API calls, databases, and external services.
5. **Shared (Common Resources)**: Reusable components, utilities, and infrastructure code.

#### **Internal Organization**

Each layer is structured by **domains** (e.g., `transactions`, `settings`), with domain-specific subfolders:

```
src/
├── web/                    # Web Layer (Build)
│   ├── pages/              # Application pages
│   │   ├── Home/           # Home page
│   │   └── Auth/           # Authentication pages
│   ├── router/             # Vue Router setup
│   └── entry/              # Entry points
│
├── presentation/           # UI Layer
│   ├── transactions/       # Domain: Transactions
│   │   ├── components/     # Vue components
│   │   ├── views/          # Pages/layouts
│   │   └── styles/         # CSS/SCSS styles
│   ├── settings/           # Domain: Settings
│   └── shared/             # Layer-wide shared resources
│       ├── components/     # Global components (buttons, forms)
│       └── utils/          # UI utilities (validation, formatting)
│
├── app/                    # Business Logic Layer
│   ├── transactions/       # Domain: Transactions
│   │   ├── store/          # Pinia/Vuex stores
│   │   ├── hooks/          # Custom hooks
│   │   └── services/       # Business services
│   └── shared/             # Layer-wide shared resources
│       ├── plugins/        # Initialization plugins
│       └── types/          # Global data types
│
├── dataAccess/            # Data Layer
│   ├── transactions/       # Domain: Transactions
│   │   ├── api/            # API clients (Axios)
│   │   └── repositories/   # CRUD operations
│   └── shared/             # Layer-wide shared resources
│       ├── http/           # HTTP client (interceptors)
│       └── cache/          # Data caching
│
└── shared/                 # Global Shared Resources
    ├── lib/                # External libraries
    ├── constants/          # Constants (routes, configs)
    └── assets/             # Static files (fonts, icons)
```

#### **Key Principles**

1. **Vertical Isolation**: Domains are independent of each other.
2. **Horizontal Separation**: Layers depend only on lower layers (e.g., `app` → `dataAccess`).
3. **Shared Folders**: For code reused across domains within a layer.
4. **Typing**: All data models are defined in domain-specific `models`.

### **Benefits**

- **Modularity**: Changes in one domain/layer don’t affect others.
- **Testability**: Clear separation simplifies unit/E2E testing.
- **Reusability**: Shared code is centralized in `shared`.
- **Scalability**: New features are added via domains.

---

### **Принципы взаимодействия / Layer Interaction**

1. **Web → Presentation → App → Data Access**: Прямой поток данных.
2. **Shared**: Используется всеми уровнями.
3. **Запрещено**:
   - Data Access → App → Presentation → Web
   - Кросс-доменные зависимости.

### **Пример запроса / Flow Example**

```
Пользователь →
Web (страница) →
Presentation (компонент) →
App (хук) →
Data Access (API-клиент) →
Сервер
```
