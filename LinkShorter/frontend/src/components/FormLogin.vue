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
        // session cookie is automatically saved in browser's cookie storage
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

        // TODO: not showing anything as the component update supresses the snackbar (probably just remove this tbh)
        this.$emit("success", this.username);
        this.setIsLoggedIn(true);
        this.setUsername(this.username);
      } catch (e) {
        if (e.status < 500) {
          this.$emit("error-push", "An undefined error occured, please try again (file a bug report if this persists)");
        } else {
          this.$emit("error-push", "The server encountered an error, please try again later");
        }
      }
    },
  },
});
</script>

<style></style>
