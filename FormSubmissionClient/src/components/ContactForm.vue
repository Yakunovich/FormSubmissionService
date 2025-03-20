<template>
  <div class="form-container">
    <h2>Contact Form</h2>
    <form @submit.prevent="submitForm">
      <div class="form-group">
        <label for="name">Name:</label>
        <input 
          id="name" 
          v-model="form.name" 
          type="text" 
          :class="{ 'error': errors.name }"
          required
        />
        <span v-if="errors.name" class="error-message">{{ errors.name }}</span>
      </div>
      
      <div class="form-group">
        <label for="email">Email:</label>
        <input 
          id="email" 
          v-model="form.email" 
          type="email" 
          :class="{ 'error': errors.email }"
          required
        />
        <span v-if="errors.email" class="error-message">{{ errors.email }}</span>
      </div>
      
      <div class="form-group">
        <label for="subject">Subject:</label>
        <select 
          id="subject" 
          v-model="form.subject" 
          :class="{ 'error': errors.subject }"
          required
        >
          <option value="">Select a subject</option>
          <option value="question">Question</option>
          <option value="feedback">Feedback</option>
          <option value="support">Support</option>
        </select>
        <span v-if="errors.subject" class="error-message">{{ errors.subject }}</span>
      </div>
      
      <div class="form-group">
        <label for="date">Preferred response date:</label>
        <input 
          id="date" 
          v-model="form.preferredDate" 
          type="date" 
          :class="{ 'error': errors.preferredDate }"
        />
        <span v-if="errors.preferredDate" class="error-message">{{ errors.preferredDate }}</span>
      </div>
      
      <div class="form-group">
        <label>Preferred contact method:</label>
        <div class="radio-group">
          <label>
            <input type="radio" v-model="form.contactMethod" value="email" />
            Email
          </label>
          <label>
            <input type="radio" v-model="form.contactMethod" value="phone" />
            Phone
          </label>
        </div>
        <span v-if="errors.contactMethod" class="error-message">{{ errors.contactMethod }}</span>
      </div>
      
      <div class="form-group">
        <label>Services of interest:</label>
        <div class="checkbox-group">
          <label>
            <input type="checkbox" v-model="form.services" value="consulting" />
            Consulting
          </label>
          <label>
            <input type="checkbox" v-model="form.services" value="development" />
            Development
          </label>
          <label>
            <input type="checkbox" v-model="form.services" value="support" />
            Technical Support
          </label>
        </div>
        <span v-if="errors.services" class="error-message">{{ errors.services }}</span>
      </div>
      
      <button type="submit" class="submit-button" :disabled="isSubmitting">
        {{ isSubmitting ? 'Submitting...' : 'Submit' }}
      </button>
    </form>
  </div>
</template>

<script>
import api from '../services/api';

export default {
  name: 'ContactForm',
  data() {
    return {
      form: {
        name: '',
        email: '',
        subject: '',
        preferredDate: '',
        contactMethod: 'email',
        services: []
      },
      errors: {},
      isSubmitting: false
    };
  },
  methods: {
    validateForm() {
      this.errors = {};
      
      if (!this.form.name) {
        this.errors.name = 'Name is required';
      }
      
      if (!this.form.email) {
        this.errors.email = 'Email is required';
      } else if (!/\S+@\S+\.\S+/.test(this.form.email)) {
        this.errors.email = 'Please enter a valid email';
      }
      
      if (!this.form.subject) {
        this.errors.subject = 'Please select a subject';
      }
      
      if (this.form.services.length === 0) {
        this.errors.services = 'Please select at least one service';
      }
      
      return Object.keys(this.errors).length === 0;
    },
    
    async submitForm() {
      if (!this.validateForm()) {
        return;
      }
      
      this.isSubmitting = true;
      
      try {
        await api.submitForm({
          formType: 'contactForm',
          formData: this.form
        });
        
        alert('Form submitted successfully!');
        this.resetForm();
      } catch (error) {
        console.error('Error submitting form:', error);
        alert('An error occurred while submitting the form');
      } finally {
        this.isSubmitting = false;
      }
    },
    
    resetForm() {
      this.form = {
        name: '',
        email: '',
        subject: '',
        preferredDate: '',
        contactMethod: 'email',
        services: []
      };
    }
  }
};
</script>

<style scoped>
.form-container {
  max-width: 600px;
  margin: 0 auto;
  padding: 20px;
  background-color: #f9f9f9;
  border-radius: 8px;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

.form-group {
  margin-bottom: 20px;
}

label {
  display: block;
  margin-bottom: 5px;
  font-weight: bold;
}

input[type="text"],
input[type="email"],
select,
input[type="date"] {
  width: 100%;
  padding: 10px;
  border: 1px solid #ddd;
  border-radius: 4px;
}

.radio-group,
.checkbox-group {
  display: flex;
  flex-direction: column;
  gap: 10px;
}

.radio-group label,
.checkbox-group label {
  display: flex;
  align-items: center;
  font-weight: normal;
}

.error {
  border-color: #ff0000;
}

.error-message {
  color: #ff0000;
  font-size: 12px;
  margin-top: 5px;
}

.submit-button {
  background-color: #4CAF50;
  color: white;
  padding: 12px 20px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 16px;
}

.submit-button:hover:not(:disabled) {
  background-color: #45a049;
}

.submit-button:disabled {
  background-color: #cccccc;
  cursor: not-allowed;
}
</style> 