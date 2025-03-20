import { createApp } from 'vue'
import App from './App.vue'
import './style.css'
import axios from 'axios'

axios.defaults.baseURL = '/api'

const app = createApp(App);

app.config.errorHandler = (err, vm, info) => {
  console.error('Vue Error:', err);
};

app.mount('#app'); 