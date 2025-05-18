<template>
  <section class="kanban-preview">
    <div class="board">
      <div class="columns">
        <div class="column" v-for="col in columns" :key="col.id">
          <header class="column-header">{{ col.name }}</header>
          <div class="tasks">
            <a-card v-for="task in col.tasks" :key="task.id" :hoverable="false" class="task-card">
              {{ task.title }}
            </a-card>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">

interface Task {
  id: number
  title: string
}

interface Column {
  id: number
  name: string
  tasks: Task[]
}

const columns: Column[] = [
  {
    id: 1,
    name: 'To Do',
    tasks: [
      { id: 1, title: 'Write documentation' },
      { id: 2, title: 'Design new logo' },
      { id: 3, title: 'Set up CI/CD pipeline' }
    ]
  },
  {
    id: 2,
    name: 'In Progress',
    tasks: [
      { id: 4, title: 'Implement authentication' },
      { id: 5, title: 'Update dependencies' }
    ]
  },
  {
    id: 3,
    name: 'Done',
    tasks: [
      { id: 6, title: 'Code review' },
      { id: 7, title: 'Fix bugs in database' },
      { id: 8, title: 'Write unit tests' }
    ]
  }
]
</script>

<style scoped lang="scss">
.kanban-preview {
  background-color: #ffffff;
  border-radius: 8px;
  padding: 16px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

.columns {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 16px;
}

.column {
  display: flex;
  flex-direction: column;
}

.column-header {
  font-size: 16px;
  font-weight: 600;
  margin-bottom: 8px;
}

.tasks {
  display: flex;
  flex-direction: column;
  gap: 8px;
}

.task-card {
  /* Card styling comes from Ant Design Vue */
}
</style>