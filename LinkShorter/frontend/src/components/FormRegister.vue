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
import axios from "axios";
import { ValidationObserver, ValidationProvider } from "vee-validate";

export default Vue.extend({
  components: {
    ValidationProvider,
    ValidationObserver,
  },
  data() {
    return {
      username: "",
      password: "",
      passConfirm: "",
    };
  },
  methods: {
    async submit() {
      try {
        // let response = await axios.post("api/login/register", {
        //   username: this.username,
        //   password: this.password,
        // });

        // if (response.status) {
        //   throw response;
        // }

        this.$emit("success", `${this.username}, you successfully registered and can now log in!`);
      } catch (e) {
        if (e.status < 500) {
          this.$emit("error-push", "An undefined error occured, please try again");
        } else {
          this.$emit("error-push", "The server encountered an error, please try again later");
        }
      } finally {
        this.username = "";
        this.password = "";
        this.passConfirm = "";
      }
    },
  },
});
</script>

<style scoped></style>
