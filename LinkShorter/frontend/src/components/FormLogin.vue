<template>
  <validation-observer ref="observer" v-slot="{ invalid, handleSubmit }">
    <form @submit.prevent="handleSubmit(submit)">
      <validation-provider v-slot="{ errors }" rules="required" name="username">
        <v-text-field type="text" label="Username" :error-messages="errors" v-model="username" required></v-text-field>
      </validation-provider>
      <validation-provider v-slot="{ errors }" rules="required" name="password">
        <v-text-field
          type="password"
          label="Password"
          :error-messages="errors"
          v-model="password"
          required
        ></v-text-field>
      </validation-provider>
      <v-btn text color="primary" type="submit" :disabled="invalid"> Login </v-btn>
    </form>
  </validation-observer>
</template>

<script lang="ts">
import Vue from "vue";
import { ValidationProvider, ValidationObserver } from "vee-validate";
import { mapMutations } from "vuex";
import axios from "axios";

export default Vue.extend({
  components: {
    ValidationProvider,
    ValidationObserver,
  },
  data() {
    return {
      username: "",
      password: "",
      totp: null,
    };
  },
  methods: {
    ...mapMutations(["setIsLoggedIn", "setUserId", "setUsername"]),
    async submit(): Promise<void> {
      try {
        console.log("kekw");

        const response = await axios.post(
          "api/login/login",
          {
            Username: this.username,
            Password: this.password,
          },
          {
            withCredentials: true,
          }
        );

        if (response.status !== 200) {
          throw response;
        }

        const data = await response.data;

        const headers = await response.headers["set-cookie"];
        console.log(`Headers: ${headers}`);

        this.$emit("success", this.username);
        this.setIsLoggedIn(true);
        // this.setUserId(data.id);
        // this.setUsername(data.username);
      } catch (e) {
        this.$emit("error-push", "An error occured");
      }
    },
  },
});
</script>

<style></style>
