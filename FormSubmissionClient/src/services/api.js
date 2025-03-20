import axios from 'axios';

const apiClient = axios.create({
  baseURL: '/api',
  headers: {
    'Content-Type': 'application/json'
  },
  timeout: 15000
});

apiClient.interceptors.response.use(
  response => {
    return response;
  },
  error => {
    return Promise.reject(error);
  }
);

export default {
  submitForm(formData) {
    return apiClient.post('/Forms', formData);
  },
  getSubmissions() {
    return apiClient.get('/Forms');
  },
  getSubmission(id) {
    return apiClient.get(`/Forms/${id}`);
  },
  searchSubmissions(searchParams) {
    return apiClient.post('/Forms/search', searchParams);
  }
}; 