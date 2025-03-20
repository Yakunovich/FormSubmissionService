<template>
  <div class="submissions-container">
    <h2>Submitted Forms</h2>
    
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
      loading: false
    };
  },
  created() {
    this.loadSubmissions();
  },
  methods: {
    async loadSubmissions() {
      this.loading = true;
      
      try {
        const response = await api.getSubmissions();
        
        if (response && response.data) {
          if (Array.isArray(response.data)) {
            this.submissions = response.data;
          } else if (typeof response.data === 'object') {
            if (Array.isArray(response.data.items)) {
              this.submissions = response.data.items;
            } else if (Array.isArray(response.data.results)) {
              this.submissions = response.data.results;
            } else if (Array.isArray(response.data.data)) {
              this.submissions = response.data.data;
            }
          }
        }
      } catch (error) {
        console.error('Error loading submissions:', error);
      } finally {
        this.loading = false;
      }
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

.field-value {
  color: #555;
  word-break: break-word;
}

.data-json {
  margin: 0;
  padding: 10px;
  white-space: pre-wrap;
  font-size: 12px;
  background-color: #f5f5f5;
  border-radius: 4px;
  max-height: 300px;
  overflow-y: auto;
  font-family: monospace;
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
  background-color: #f9f9f9;
  border-radius: 4px;
}
</style> 