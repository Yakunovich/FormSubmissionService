<template>
  <div class="submissions-container">
    <h2>Submitted Forms</h2>
    
    <div class="search-panel">
      <div class="search-form">
        <div class="form-row">
          <div class="form-group">
            <label for="formType">Form Type:</label>
            <select id="formType" v-model="searchParams.formType">
              <option value="">All types</option>
              <option value="contactForm">Contact Form</option>
            </select>
          </div>
          
          <div class="form-group">
            <label for="searchTerm">Search:</label>
            <input type="text" id="searchTerm" v-model="searchParams.searchTerm" 
                   placeholder="Search form content">
          </div>
        </div>
        
        <div class="form-row">
          <div class="form-group">
            <label for="fromDate">From date:</label>
            <input type="date" id="fromDate" v-model="searchParams.fromDate">
          </div>
          
          <div class="form-group">
            <label for="toDate">To date:</label>
            <input type="date" id="toDate" v-model="searchParams.toDate">
          </div>
        </div>
        
        <div class="form-actions">
          <button @click="searchSubmissions" class="search-button">Search</button>
          <button @click="resetSearch" class="reset-button">Reset</button>
        </div>
      </div>
    </div>
    
    <div v-if="loading" class="loading">Loading...</div>
    
    <div v-else-if="submissions.length === 0" class="no-results">
      No data to display
    </div>
    
    <div v-else class="table-responsive">
      <table class="submissions-table">
        <thead>
          <tr>
            <th>ID</th>
            <th>Type</th>
            <th>Date</th>
            <th>Data</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(item, index) in submissions" :key="index" 
              @click="toggleDetails(index)" 
              class="submission-row"
              :class="{ 'expanded': expandedRows.includes(index) }">
            <td>{{ item.id || index + 1 }}</td>
            <td>{{ formatFormType(item.formType) }}</td>
            <td>{{ formatDate(item.submittedAt) }}</td>
            <td class="form-highlights">
              {{ getFormDataSummary(item.formData) }}
            </td>
          </tr>
          <tr v-for="(item, index) in submissions" :key="`details-${index}`" v-if="expandedRows.includes(index)" class="details-row">
            <td colspan="4">
              <div class="form-details">
                <div v-for="(value, key) in getFormDataObject(item.formData)" :key="key" class="form-field">
                  <span class="field-name">{{ formatFieldName(key) }}:</span>
                  <span class="field-value">{{ formatFieldValue(value) }}</span>
                </div>
              </div>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
    
    <div class="pagination" v-if="submissions.length > 0">
      <button @click="previousPage" :disabled="searchParams.page <= 1">Previous</button>
      <span>Page {{ searchParams.page }}</span>
      <button @click="nextPage">Next</button>
    </div>
  </div>
</template>

<script>
import api from '../services/api';

export default {
  name: 'FormsList',
  data() {
    return {
      submissions: [],
      expandedRows: [],
      loading: false,
      searchParams: {
        formType: '',
        searchTerm: '',
        fromDate: '',
        toDate: '',
        page: 1,
        pageSize: 10
      }
    };
  },
  created() {
    this.loadSubmissions();
  },
  methods: {
    async loadSubmissions() {
      this.loading = true;
      
      try {
        if (this.hasSearchParams()) {
          const response = await api.searchSubmissions(this.searchParams);
          this.processResponse(response);
        } else {
          const response = await api.getSubmissions();
          this.processResponse(response);
        }
      } catch (error) {
        console.error('Error loading submissions:', error);
      } finally {
        this.loading = false;
      }
    },
    
    processResponse(response) {
      if (response && response.data) {
        if (Array.isArray(response.data)) {
          this.submissions = response.data.map(this.normalizeItem);
        } else if (typeof response.data === 'object') {
          if (Array.isArray(response.data.items)) {
            this.submissions = response.data.items.map(this.normalizeItem);
          } else if (Array.isArray(response.data.results)) {
            this.submissions = response.data.results.map(this.normalizeItem);
          } else if (Array.isArray(response.data.data)) {
            this.submissions = response.data.data.map(this.normalizeItem);
          }
        }
      }
    },
    
    normalizeItem(item) {
      return {
        id: item.id || item.Id,
        formType: item.formType || item.FormType,
        formData: item.formData || item.FormData,
        submittedAt: item.submittedAt || item.SubmittedAt
      };
    },
    
    hasSearchParams() {
      return this.searchParams.formType || 
             this.searchParams.searchTerm || 
             this.searchParams.fromDate || 
             this.searchParams.toDate;
    },
    
    searchSubmissions() {
      this.searchParams.page = 1;
      this.loadSubmissions();
    },
    
    resetSearch() {
      this.searchParams = {
        formType: '',
        searchTerm: '',
        fromDate: '',
        toDate: '',
        page: 1,
        pageSize: 10
      };
      this.loadSubmissions();
    },
    
    previousPage() {
      if (this.searchParams.page > 1) {
        this.searchParams.page--;
        this.loadSubmissions();
      }
    },
    
    nextPage() {
      this.searchParams.page++;
      this.loadSubmissions();
    },
    
    toggleDetails(index) {
      if (this.expandedRows.includes(index)) {
        this.expandedRows = this.expandedRows.filter(i => i !== index);
      } else {
        this.expandedRows.push(index);
      }
    },
    
    formatDate(dateString) {
      if (!dateString) return 'N/A';
      
      try {
        const date = new Date(dateString);
        return date.toLocaleDateString();
      } catch (e) {
        return dateString;
      }
    },
    
    formatFormType(type) {
      if (!type) return 'Unknown';
      
      return type
        .replace(/([A-Z])/g, ' $1')
        .replace(/^./, str => str.toUpperCase())
        .trim();
    },
    
    formatFieldName(key) {
      if (!key) return '';
      
      return key
        .replace(/([A-Z])/g, ' $1')
        .replace(/^./, str => str.toUpperCase())
        .trim();
    },
    
    formatFieldValue(value) {
      if (value === null || value === undefined) {
        return 'N/A';
      }
      
      if (Array.isArray(value)) {
        return value.join(', ');
      }
      
      if (typeof value === 'object') {
        return JSON.stringify(value);
      }
      
      if (typeof value === 'boolean') {
        return value ? 'Yes' : 'No';
      }
      
      if (typeof value === 'string' && value.match(/^\d{4}-\d{2}-\d{2}/) && !isNaN(new Date(value))) {
        return new Date(value).toLocaleDateString();
      }
      
      return String(value);
    },
    
    getFormDataObject(formData) {
      if (!formData) return null;
      
      try {
        return typeof formData === 'string' 
          ? JSON.parse(formData) 
          : formData;
      } catch (e) {
        return null;
      }
    },
    
    getFormDataSummary(formData) {
      const data = this.getFormDataObject(formData);
      if (!data) return 'No data';
      
      const name = data.name || '';
      const email = data.email || '';
      
      if (name && email) {
        return `${name} (${email})`;
      } else if (name) {
        return name;
      } else if (email) {
        return email;
      } else {
        return 'Data available';
      }
    }
  }
};
</script>

<style scoped>
.submissions-container {
  max-width: 1000px;
  margin: 0 auto;
  padding: 20px;
}

.search-panel {
  background-color: #f2f2f2;
  padding: 15px;
  border-radius: 5px;
  margin-bottom: 20px;
}

.search-form {
  display: flex;
  flex-direction: column;
  gap: 15px;
}

.form-row {
  display: flex;
  gap: 15px;
  flex-wrap: wrap;
}

.form-group {
  flex: 1;
  min-width: 200px;
}

.form-group label {
  display: block;
  margin-bottom: 5px;
  font-weight: bold;
}

.form-group input,
.form-group select {
  width: 100%;
  padding: 8px;
  border: 1px solid #ddd;
  border-radius: 4px;
}

.form-actions {
  display: flex;
  justify-content: flex-end;
  gap: 10px;
  margin-top: 5px;
}

.search-button {
  background-color: #4CAF50;
  color: white;
  border: none;
  padding: 8px 15px;
  border-radius: 4px;
  cursor: pointer;
}

.search-button:hover {
  background-color: #45a049;
}

.reset-button {
  background-color: #f8f8f8;
  border: 1px solid #ddd;
  padding: 8px 15px;
  border-radius: 4px;
  cursor: pointer;
}

.reset-button:hover {
  background-color: #e8e8e8;
}

.table-responsive {
  overflow-x: auto;
}

.submissions-table {
  width: 100%;
  border-collapse: collapse;
  margin-bottom: 20px;
}

.submissions-table th, 
.submissions-table td {
  border: 1px solid #ddd;
  padding: 10px;
  text-align: left;
}

.submissions-table th {
  background-color: #f2f2f2;
}

.submissions-table tr:nth-child(even):not(.details-row) {
  background-color: #f9f9f9;
}

.submissions-table tr:hover:not(.details-row) {
  background-color: #f1f1f1;
  cursor: pointer;
}

.submission-row.expanded {
  background-color: #e6f7ff !important;
}

.details-row {
  background-color: #f8f8f8 !important;
}

.form-details {
  padding: 15px;
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
  gap: 10px;
}

.form-field {
  background-color: white;
  border-radius: 4px;
  padding: 8px 12px;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
  display: flex;
  flex-direction: column;
}

.field-name {
  font-weight: bold;
  margin-bottom: 5px;
  color: #333;
}

.pagination {
  display: flex;
  justify-content: center;
  gap: 15px;
  margin-top: 20px;
  align-items: center;
}

.pagination button {
  padding: 5px 15px;
  border: 1px solid #ddd;
  background-color: #f8f8f8;
  border-radius: 4px;
  cursor: pointer;
}

.pagination button:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.pagination button:hover:not(:disabled) {
  background-color: #e8e8e8;
}

.loading {
  text-align: center;
  padding: 20px;
  font-size: 18px;
  color: #666;
}

.no-results {
  text-align: center;
  padding: 20px;
  font-size: 16px;
  color: #666;
  background-color: #f8f8f8;
  border-radius: 4px;
}
</style> 