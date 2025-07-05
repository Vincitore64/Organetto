# Organetto Project Rules

This document outlines the coding standards, architectural principles, and development workflow for the Organetto project. Adhering to these rules ensures consistency, maintainability, and collaboration across the development team.

## 1. Architecture

The project follows a multi-layered architecture designed for modularity, scalability, and separation of concerns. The layers are organized as follows:

- **Web**: Entry point, build configuration, routing, and global setup.
- **Presentation (UI)**: User interface components, views, and styles.
- **App (Business Logic)**: Use cases, state management, and coordination between layers.
- **Data Access**: API clients, repositories, and interaction with external services.
- **Shared**: Reusable utilities, components, and infrastructure code.

### Key Principles:

- **Vertical Isolation**: Domains (e.g., `transactions`, `settings`) are independent of each other.
- **Horizontal Separation**: Layers can only depend on lower-level layers (e.g., `app` can depend on `dataAccess`, but not the other way around).
- **Shared Resources**: Code shared within a layer is placed in a `shared` directory at that level. Globally shared code resides in the root `src/shared` directory.
- **Data Models**: All data models and types are defined within their respective domains.

For a detailed breakdown, refer to [README.STRUCT.md](README.STRUCT.md).

## 2. Coding Style & Formatting

We enforce a consistent coding style using **Prettier** and **EditorConfig**. All code should be automatically formatted according to these configurations.

### EditorConfig (`.editorconfig`)

- **Charset**: `utf-8`
- **Indent Style**: `space`, size `2`
- **Line Endings**: `lf`
- **Max Line Length**: `100` characters
- **Whitespace**: Final newline required, trailing whitespace trimmed.

### Prettier (`.prettierrc.json`)

- **Semicolons**: `false` (disabled)
- **Quotes**: `singleQuote` (`true`)
- **Print Width**: `100` characters

## 3. Linting

We use **ESLint** to identify and fix problems in JavaScript and TypeScript code. The configuration is defined in `eslint.config.ts`.

- **Configuration**: Based on `@vue/eslint-config-typescript`.
- **Plugins**: Includes rules for Vue, Vitest, and Cypress.
- **Files to Lint**: `**/*.{ts,mts,tsx,vue}`.
- **Ignored Directories**: `dist`, `dist-ssr`, `coverage`.

To run the linter, use the command:

```sh
npm run lint
```

## 4. Development Workflow

### Setup

1.  **Install Dependencies**:

    ```sh
    npm install
    ```

2.  **IDE Configuration**:
    - **VSCode** is the recommended IDE.
    - Install the **Volar** extension for Vue 3 and TypeScript support.

### Running the Application

- **Development Server** (with hot-reload):

  ```sh
  npm run dev
  ```

- **Production Build**:

  ```sh
  npm run build
  ```

### Testing

- **Unit Tests** (with Vitest):

  ```sh
  npm run test:unit
  ```

- **End-to-End Tests** (with Cypress):
  - Against dev server: `npm run test:e2e:dev`
  - Against production build: `npm run test:e2e`

## 5. Version Control

- **Branching**: Follow a feature-branching workflow (e.g., `feature/`, `bugfix/`, `chore/`).
- **Commits**: Write clear, concise, and descriptive commit messages.
- **Pull Requests**: All new code must be submitted through a pull request and reviewed before merging.

By following these rules, we can maintain a high-quality, organized, and collaborative development environment.
