<template>
  <div class="app-container">
    <header>
      <h1>Form Management System</h1>
      <nav>
        <button @click="activeTab = 'form'" :class="{ active: activeTab === 'form' }">
          Submit Form
        </button>
        <button @click="activeTab = 'list'" :class="{ active: activeTab === 'list' }">
          Forms List
        </button>
      </nav>
    </header>
    
    <main>
      <ContactForm v-if="activeTab === 'form'" />
      <FormsList v-if="activeTab === 'list'" />
    </main>
    
    <footer>
      <p>&copy; 2023 Form Management System</p>
    </footer>
  </div>
</template>

<script>
import ContactForm from './components/ContactForm.vue';
import FormsList from './components/FormsList.vue';

export default {
  name: 'App',
  components: {
    ContactForm,
    FormsList
  },
  data() {
    return {
      activeTab: 'form'
    };
  }
};
</script>

<style>
* {
  box-sizing: border-box;
  margin: 0;
  padding: 0;
}

body {
  font-family: Arial, sans-serif;
  line-height: 1.6;
  color: #333;
  background-color: #f8f8f8;
}

.app-container {
  min-height: 100vh;
  display: flex;
  flex-direction: column;
}

header {
  background-color: #4CAF50;
  color: white;
  padding: 1rem;
  text-align: center;
}

h1 {
  margin-bottom: 1rem;
}

nav {
  display: flex;
  justify-content: center;
  gap: 1rem;
}

nav button {
  background-color: transparent;
  border: 2px solid white;
  color: white;
  padding: 0.5rem 1rem;
  border-radius: 4px;
  cursor: pointer;
  font-size: 1rem;
  transition: background-color 0.3s;
}

nav button:hover {
  background-color: rgba(255, 255, 255, 0.2);
}

nav button.active {
  background-color: white;
  color: #4CAF50;
}

main {
  flex: 1;
  padding: 2rem;
}

footer {
  background-color: #333;
  color: white;
  text-align: center;
  padding: 1rem;
  margin-top: auto;
}
</style> 