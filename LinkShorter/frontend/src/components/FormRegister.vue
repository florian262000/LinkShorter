<template>
  <validation-observer ref="observer" v-slot="{ invalid, handleSubmit }">
    <form @submit.prevent="handleSubmit(submit)">
      <validation-provider v-slot="{ errors }" rules="required" name="username">
        <v-text-field label="Username" v-model="username" :error-messages="errors"></v-text-field>
      </validation-provider>
      <validation-provider v-slot="{ errors }" rules="required|min:8|confirmed:passConfirm" name="password">
        <v-text-field label="Password" type="password" :error-messages="errors" v-model="password"></v-text-field>
      </validation-provider>
      <validation-provider v-slot="{ errors }" vid="passConfirm">
        <v-text-field
          label="Password confirmation"
          type="password"
          :error-messages="errors"
          v-model="passConfirm"
        ></v-text-field>
      </validation-provider>
      <v-btn text color="primary" type="submit" :disabled="invalid"> Sign Up </v-btn>
    </form>
  </validation-observer>
</template>

<script lang="ts">
import Vue from "vue";
import { ValidationObserver, ValidationProvider } from "vee-validate";

export default Vue.extend({
  components: {
    ValidationProvider,
    ValidationObserver,
  },
  data() {
    return {
      username: "",
      email: "",
      password: "",
    };
  },
  methods: {
    async submit() {
      try {
        const response = await fetch("/api/user/register/", {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({
            username: this.username,
            password: this.password,
          }),
        });

        if (!response.ok) {
          throw response;
        }

        const data = await response.json();

        this.$emit("oauth-push", data.oauthUrl);
      } catch (e) {
        const data = await e.json();
        this.$emit("error-push", data.errorMessage);
      }
    },
  },
});
</script>

<style scoped></style>
