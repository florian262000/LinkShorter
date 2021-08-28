<template>
  <validation-observer ref="observer" v-slot="{ invalid, handleSubmit }">
    <form @submit.prevent="handleSubmit(submit)">
      <validation-provider v-slot="{ errors }" rules="required|min:8|confirmed:passConfirm" name="password">
        <v-text-field
          type="password"
          label="Password"
          :error-messages="errors"
          v-model="password"
          required
        ></v-text-field>
      </validation-provider>
      <validation-provider v-slot="{ errors }" vid="passConfirm">
        <v-text-field
          type="password"
          label="Password confirmation"
          :error-messages="errors"
          v-model="passwordConfirm"
        ></v-text-field>
      </validation-provider>
      <v-btn text color="primary" type="submit" :disabled="invalid"> Change password </v-btn>
    </form>
  </validation-observer>
</template>

<script lang="ts">
import Vue from "vue";
import { ValidationObserver, ValidationProvider } from "vee-validate";

export default Vue.extend({
  components: {
    ValidationObserver,
    ValidationProvider,
  },
  data() {
    return {
      password: "",
      passwordConfirm: "",
    };
  },
});
</script>

<style scoped></style>
